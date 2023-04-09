using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Inputs")]
    public KeyCode parryKey;
    public KeyCode attackKey;

    [Header("Attack")]
    public int damage;
    public float range, attackCooldown;
    bool readyToAttack;

    [Header("AttackSphere")]
    public Transform attackPosition;
    public LayerMask damageable;

    [Header("Parry")]
    public float parryLength;
    public bool isParrying;
    bool readyToParry;
    public float parryCooldown;

    // Start is called before the first frame update
    void Start()
    {
        readyToAttack = true;
        readyToParry = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    void MyInput()
    {
        if (Input.GetKeyDown(parryKey) && readyToParry) StartParry();
        if (Input.GetKeyDown(attackKey) && readyToAttack)
        {
            GetComponent<Animator>().Play("Player-Punch");
            readyToAttack = false;
            StartCoroutine(AttackGetReady());
        }
    }

    IEnumerator AttackGetReady()
    {
        yield return new WaitForSeconds(attackCooldown);
        readyToAttack = true;
    }

    void StartParry()
    {
        //Parry animation
        readyToParry = false;
        GetComponent<Animator>().Play("Player-Parry");
        GetComponent<Animator>().SetBool("Parrying", true);
        StartCoroutine(Parry());
    }

    IEnumerator Parry()
    {
        isParrying = true;
        yield return new WaitForSeconds(parryLength);
        GetComponent<Animator>().SetBool("Parrying", false);
        isParrying = false;
        StartCoroutine(ParryGetReady());
    }

    IEnumerator ParryGetReady()
    {
        yield return new WaitForSeconds(parryCooldown);
        readyToParry = true;
    }

    //Animation event
    void DoDamage()
    {
        Debug.Log("Attack");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPosition.position, range, damageable);
        foreach(Collider2D col in colliders)
        {
            Debug.Log("Hit");
            if (col.GetComponentInChildren<Damageable>())
            {
                col.GetComponent<Damageable>().TakeDamage(damage);
                Debug.Log("Damaged");
            }
        }
    }
}
