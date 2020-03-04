using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    int damage;
    Collider swordCollider;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        swordCollider = gameObject.GetComponent<BoxCollider>();
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Attack(player.GetComponent<CharacterStats>().Attack, collider);
    }

    public void SetCollider(bool enable)
    {
        swordCollider.enabled = enable;
    }

   

    void Attack(int Damage, Collider collider)
    {
        IDamageable damageableObject = collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(Damage);
        }
    }
}
