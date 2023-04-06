using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public bool isEnemy;

    public void TakeDamage()
    {
        if (isEnemy)
        {
            //Enemy damage
        }
        if (!isEnemy)
        {
            //Destroy object
        }
    }
}
