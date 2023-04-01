using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animator : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            gameObject.GetComponent<Animator>().Play("Player-Dash");
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            gameObject.GetComponent<Animator>().Play("Player-Walk");
        }
        if (!Input.anyKey)
        {
            gameObject.GetComponent<Animator>().Play("Player-Idle");
        }


    }
}
