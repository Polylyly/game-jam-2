using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public bool isEnemy;

    public void TakeDamage(int damage)
    {
        if (isEnemy)
        {
            GetComponent<EnemyHealth>().TakeDamage(damage);
        }
        if (!isEnemy)
        {
            //Destroy object
        }
    }
}
