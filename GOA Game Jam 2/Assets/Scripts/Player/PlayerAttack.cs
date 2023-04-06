using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Inputs")]
    public KeyCode parryKey;
    public KeyCode attackKey;

    [Header("AttackSphere")]
    public Transform attackPosition;
    public LayerMask damageable;

    [Header("Parry")]
    public float parryLength;
    public bool isParrying;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    void MyInput()
    {
        if (Input.GetKeyDown(parryKey)) StartParry();
    }

    void StartParry()
    {
        //Parry animation
    }

    IEnumerator Parry()
    {
        isParrying = true;
        yield return new WaitForSeconds(parryLength);
        isParrying = false;
    }

    //Animation event
    public void DoDamage(int damage, float range)
    {
        Collider[] colliders = Physics.OverlapSphere(attackPosition.position, range);
        foreach(Collider col in colliders)
        {
            if (col.GetComponent<Damageable>()) col.GetComponent<Damageable>().TakeDamage();
        }
    }
}
