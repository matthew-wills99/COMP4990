using System.Collections;
using System.Collections.Generic;
using static EUtils;
using UnityEngine;

/*
Weapon should keep track of:
    Damage for that specific weapon
    Attack type of that weapon as per the AttackType enum in Utils.cs
    Cooldown of the attack
*/

public class Weapon : MonoBehaviour
{
    public float damage = 15f;
    [SerializeField]
    private EAttackType attackType;
    public float attackCooldown = 0.5f;

    public EAttackType GetAttackType()
    {
        return attackType;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy");
            other.GetComponent<Animal>().Hit(damage, transform.parent.transform);
        }
    }
}
