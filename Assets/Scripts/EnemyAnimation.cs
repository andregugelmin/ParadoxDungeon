using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour, IDamageable
{
    ObjectPooler objectPooler;

    private bool dead;
    private bool isHit;
    public bool isAttacking = false;
    public bool attackedPlayer = false;
    public float dist;
    public float distSpawn;
    private float isHitCooldown;
    public float health;
    public float startingHealth;
    public float stunCooldown = 0.03f;
    public string tag;
    private float attackCooldown = 2f;
    private float attackTime = 0f;
    private float playerLife;

    public GameObject Explosion;
    public GameObject player;
    private Transform playerTarget;
    private Animator animator;
    public NavMeshAgent navMesh;

    public ParticleSystem blood;

    public AudioManager am;

    public GameObject DamageTextPrefab;

    public Vector3 spawnPosition;

    private void OnEnable()
    {
        gameObject.GetComponent<CharacterStats>().Life = gameObject.GetComponent<CharacterStats>().maxLife[gameObject.GetComponent<CharacterStats>().level];
        player = GameObject.Find("Player");
        playerTarget = player.transform;
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        objectPooler = ObjectPooler.Instance;
        StartCoroutine("CheckDistanceFromPlayer");
        StartCoroutine("CheckDistanceFromSpawn");
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (Time.time > isHitCooldown && isHit)
        {
            isHit = false;
            animator.SetBool("isStuned", false);
        }
    }

    IEnumerator CheckDistanceFromPlayer()
    {
        float refreshRate = 0.1f;
        while (true)
        {
            playerTarget = player.transform;
            dist = Vector3.Distance(playerTarget.position, transform.position);
            animator.SetFloat("distance", dist);
            playerLife = player.GetComponent<CharacterStats>().Life;
            if (playerLife <= 0)
            {
                animator.SetBool("playerIsDead", true);
            }
            else
            {
                animator.SetBool("playerIsDead", false);
            }
            yield return new WaitForSeconds(refreshRate);
        }
        
    }
    IEnumerator CheckDistanceFromSpawn()
    {
        float refreshRate = 1f;
        while (true)
        {
            distSpawn = Vector3.Distance(new Vector3(spawnPosition.x, transform.position.y, spawnPosition.z), transform.position);
            animator.SetFloat("distanceSpawn", distSpawn);

            yield return new WaitForSeconds(refreshRate);
        }

    }

    IEnumerator AttackPlayer()
    {
        float refreshRate = 0.25f;
        while (true)
        {      
            if (Time.time - attackTime > attackCooldown && player.GetComponent<CharacterStats>().Life > 0)
            {
                attackTime = Time.time;
                animator.SetBool("isAttacking", true);                
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    IEnumerator ChasePlayer()
    {
        float refreshRate = 0.25f;
        if (gameObject.GetComponent<NavMeshAgent>().enabled == false)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
        }
        while (playerTarget != null)
        {
            if (!dead)
                navMesh.SetDestination(playerTarget.position);
            yield return new WaitForSeconds(refreshRate);
        }
    }
    IEnumerator BackPosition()
    {
        float refreshRate = 1f;
        if (gameObject.GetComponent<NavMeshAgent>().enabled == false)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
        }
        
        navMesh.SetDestination(spawnPosition);
        yield return new WaitForSeconds(refreshRate);
        
    }

    public void Attack(GameObject opponent)
    {  
        float damage = gameObject.GetComponent<CharacterStats>().Attack - opponent.GetComponent<CharacterStats>().Defence;
        
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    private void OnTriggerStay(Collider collider)
    {
        if (isAttacking && collider.gameObject.tag == "Player" && !attackedPlayer)
        {
            Debug.Log("Attacked player");
            int damage = gameObject.GetComponent<CharacterStats>().Attack - collider.gameObject.GetComponent<CharacterStats>().Defence;
            if (damage >= 1)            
                collider.gameObject.GetComponent<PlayerMovement>().TakeHit(damage);            
            else
                collider.gameObject.GetComponent<PlayerMovement>().TakeHit(1);
            attackedPlayer = true;            

        }
    }

    public void TakeHit(int damage)
    {
        
        int damageTaken = damage - this.gameObject.GetComponent<CharacterStats>().Defence;
        if (!isHit)
        {
            blood.Play();
            isHit = true;
            isHitCooldown = Time.time + stunCooldown;            
            if (damageTaken >= 1)
            {
                gameObject.GetComponent<CharacterStats>().Life -= damageTaken;
                if (DamageTextPrefab && gameObject.GetComponent<CharacterStats>().Life > 0)
                    ShowDamageText(damageTaken);
            }

            else
            {
                gameObject.GetComponent<CharacterStats>().Life -= 1;
                if (DamageTextPrefab && gameObject.GetComponent<CharacterStats>().Life>0)
                    ShowDamageText(1);
            }
                
            animator.SetBool("isStuned", true);
            PlaySound("Sword");
            
        }

        if (gameObject.GetComponent<CharacterStats>().Life <= 0 && !dead)
        {
            Die();
        }
    }

    void Die()
    {
        player.GetComponent<CharacterStats>().experiencePlayer += GetComponent<CharacterStats>().Experience;
        Instantiate(Explosion, transform.position + new Vector3(0, 1, 0) , Quaternion.identity);
        objectPooler.poolDictionary[this.tag].Enqueue(this.gameObject);
        gameObject.SetActive(false);
    }

    public void PlaySound(string sound)
    {
        am.Play(sound);
    }

    void ShowDamageText(int _damage)
    {
        var text = Instantiate(DamageTextPrefab, transform.position, Quaternion.identity, transform);
        text.GetComponent<TextMesh>().text = _damage.ToString();
        text.GetComponent<TextMesh>().color = new Color(255, 255, 0);
    }
}
