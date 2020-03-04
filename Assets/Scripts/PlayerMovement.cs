using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float MoveSpeed = 7;
    [SerializeField]
    private float SmoothMoveTime = .1f;
    [SerializeField]
    private float TurnSpeed = 8;

    private bool dead;

    private float angle;
    private float smoothInputMagnitude;
    private float smoothMoveVelocity;
    private float inputMagnitude;

    private Vector3 velocity;
    private Vector3 inputDirection;

    private bool isDashing;
    private bool canDash = true;

    [SerializeField]
    private float dashTime;
    [SerializeField]
    private float dashLength;
    [SerializeField]
    private float dashCooldow;
    private float dashReady;

    private int noOfClicks = 0;
    private float lastClickedTime = 0;
    private float deadTime = 0;
    private float respawnTime = 5.0f;
    [SerializeField]
    private float maxComboDelay = 1.0f;

    [SerializeField]
    private ParticleSystem dashParticles;
    [SerializeField]
    private ParticleSystem eletricParticles;
    [SerializeField]
    private ParticleSystem bloodParticles;

    public GameObject attackCollider;
    public GameObject levelUpStats;
    public GameObject DamageTextPrefab;

    private new Rigidbody rigidbody;
    private LayerMask scenarioMask;
    private Animator animator;

    private CharState charState;
    private enum CharState
    {
        idle, moving, dashing, attacking, stuned, dead, menu, cs
    }

    public Text attackTxt, defenceTxt, lifeTxt, levelTxt, pointsTxt;
    
    public Image healthbar, hbg;
    public Image xpbar;

    [SerializeField]
    AudioManager am;
       
    private float cutsceneTime = 5.0f, startCutscene;
    private bool cutsceneDone = false;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        dashReady = dashCooldow;
        charState = CharState.cs;
        scenarioMask = 1 << 8;
        startCutscene = Time.time;
    }

    void Update()
    {

        switch (charState)
        {
            
            case CharState.idle:
                checkMovement();
                if (inputMagnitude!= 0)
                {
                    charState = CharState.moving;
                }                
                if (Input.GetMouseButtonDown(0))
                {
                    lastClickedTime = Time.time;
                    noOfClicks++;
                    Attack();
                    animator.SetBool("Attack1", true);
                    charState = CharState.attacking;
                }
                if (Input.GetButtonDown("DashKey") && canDash)
                {
                    StartDash();
                }
                if (Input.GetButtonDown("Stats"))
                {
                    openStats();
                    charState = CharState.menu;
                }
                break;
            case CharState.moving:
                checkMovement();
                velocity = transform.forward * MoveSpeed * smoothInputMagnitude;
                if (inputMagnitude == 0)
                {
                    charState = CharState.idle;     
                }
                if (Input.GetMouseButtonDown(0))
                {
                    lastClickedTime = Time.time;
                    noOfClicks++;
                    Attack();
                    animator.SetBool("Attack1", true);
                    charState = CharState.attacking;
                }
                if (Input.GetButtonDown("DashKey") && canDash)
                {
                    StartDash();
                }
                if (Input.GetButtonDown("Stats"))
                {
                    openStats();
                    charState = CharState.menu;
                }
                break;
            case CharState.dashing:                    
                break;
            case CharState.attacking:  
                if (Input.GetMouseButtonDown(0))
                {
                    lastClickedTime = Time.time;
                    noOfClicks++;
                }
                break;
            case CharState.stuned:                 
                break;
            case CharState.dead:
                if (Time.time - deadTime > respawnTime)
                    Respawn();
                break;
            case CharState.menu:
                if (Input.GetButtonDown("Stats"))
                {
                    openStats();
                }
                animator.SetFloat("Speed", 0);      
                if(levelUpStats.activeInHierarchy == false)
                {
                    charState = CharState.idle;
                }

                break;
            case CharState.cs:
                if (!cutsceneDone && Time.time - startCutscene > cutsceneTime)
                {
                    charState = CharState.idle;
                    cutsceneDone = true;
                    GameObject.Find("TimeLine").SetActive(false);
                }
                break;
        }
        
        if (Input.GetButtonDown("God") && canDash)
        {
            God();
        }
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        if(levelUpStats.activeInHierarchy && charState != CharState.menu)
        {
            levelUpStats.SetActive(false);
        }

        UpdateDashCooldown();

        if (dead)
        {
            charState = CharState.dead;
        }
        float maxLifePlayer = gameObject.GetComponent<CharacterStats>().maxLifePlayer;
        float Health = gameObject.GetComponent<CharacterStats>().Life;       
        healthbar.fillAmount = (Health / maxLifePlayer);

        float xp = gameObject.GetComponent<CharacterStats>().experiencePlayer;
        float level = gameObject.GetComponent<CharacterStats>().level;
        xpbar.fillAmount = xp / (level * 10);

        levelTxt.text = "" + gameObject.GetComponent<CharacterStats>().level;
        attackTxt.text = (gameObject.GetComponent<CharacterStats>().attackPoint ) + "/50";
        defenceTxt.text = "" + (gameObject.GetComponent<CharacterStats>().defencePoint) + "/50";
        lifeTxt.text = "" + (gameObject.GetComponent<CharacterStats>().lifePoint) + "/50";
        pointsTxt.text = "" + (gameObject.GetComponent<CharacterStats>().pointsXp);
    }

    void FixedUpdate()
    {
        switch (charState)
        {
            case CharState.idle:
                rigidbody.MoveRotation(transform.rotation);
                break;
            case CharState.moving:
                rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
                rigidbody.MovePosition(rigidbody.position + velocity * Time.deltaTime);
                break;
            case CharState.dashing:
                if (!isDashing)
                {
                    Dash();
                }      
                break;
            case CharState.attacking:

                break;
            case CharState.dead:
                break;
            case CharState.menu:
                break;
            case CharState.cs:
                break;

        }
    }

    void checkMovement()
    {
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        inputMagnitude = inputDirection.magnitude;
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, SmoothMoveTime);

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * TurnSpeed * inputMagnitude);

        animator.SetFloat("Speed", inputMagnitude);
    }

    void StartDash()
    {
        dashReady = 0;
        canDash = false;
        dashParticles.Play();
        charState = CharState.dashing;
    }

    void Dash()
    {
        returnAttack0();
        RaycastHit hit;
        Vector3 direction = (inputMagnitude > 0) ? (inputDirection * dashLength * Time.deltaTime) :(transform.forward * dashLength * Time.deltaTime);
        angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));        
        Ray ray = new Ray(rigidbody.position, direction);

        PlaySound("Dash");

        if (!Physics.Raycast(ray, out hit, direction.magnitude, scenarioMask))
        {
            rigidbody.MovePosition(rigidbody.position + direction);
        }
        else
        {
            rigidbody.MovePosition(hit.point);
        }

        isDashing = true;
    }

    void UpdateDashCooldown()
    {
        if (dashReady <= dashCooldow)
        {
            dashReady += Time.deltaTime;
        }
        else if (dashReady >= dashCooldow && canDash == false)
        {
            canDash = true;
            eletricParticles.Play();
        }

        if (isDashing)
        {
            if (dashReady >= dashTime)
            {
                charState = CharState.idle;
                isDashing = false;
            }
        }
    }

    void Attack()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        
        if (Physics.Raycast(ray, out hit))
        {            
            angle = Mathf.Atan2(hit.point.x - transform.position.x, hit.point.z - transform.position.z) * Mathf.Rad2Deg;
            rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));            
        }
    }
   
    void Die()
    {
        dead = true;
        animator.SetBool("isDead", true);
        deadTime = Time.time;
    }

    public void openStats()
    {
       levelUpStats.SetActive(!levelUpStats.activeSelf);       
    }

    public void TakeHit(int damage)
    {
        gameObject.GetComponent<CharacterStats>().Life -= damage;
        if (DamageTextPrefab)
            ShowDamageText(damage);
        Debug.Log(gameObject.GetComponent<CharacterStats>().Life);
       
        if (gameObject.GetComponent<CharacterStats>().Life <= 0 && !dead)
        {
            Die();
            PlaySound("Death");
        }
        else
        {
            PlaySound("Hit");
        }
        animator.SetBool("Attack1", false);
        animator.SetBool("isStuned", true);
        charState = CharState.stuned;
        animator.Play("getHit");
        bloodParticles.Play();
    }

    public void returnAttack0()
    {
       
        animator.SetBool("Attack1", false);
        noOfClicks = 0;
        charState = CharState.idle;
    }

    public void returStun()
    {

        animator.SetBool("isStuned", false);
        charState = CharState.idle;
    }

    public void PlaySound(string sound)
    {
        am.Play(sound);
    }
    void Respawn()
    {
        Debug.Log("Respawn");
        gameObject.transform.position = new Vector3(22.9f, 0.07319021f, 2f);
        gameObject.GetComponent<CharacterStats>().Life = gameObject.GetComponent<CharacterStats>().maxLifePlayer;
        charState = CharState.idle;
        animator.SetBool("isDead", false);
        dead = false;

    }

    void God()
    {
        gameObject.GetComponent<CharacterStats>().level = 38;
        gameObject.GetComponent<CharacterStats>().pointsXp = 120;
    }

    void ShowDamageText(int _damage)
    {
        var text = Instantiate(DamageTextPrefab, transform.position, Quaternion.identity, transform);
        text.GetComponent<TextMesh>().text = _damage.ToString();
        text.GetComponent<TextMesh>().color = new Color(255,0,0);
    }

    public void pauseState(int state)
    {
        if (state == 1)
        {
            charState = CharState.menu;
        }
        else
        {
            charState = CharState.idle;
        }
    }
}