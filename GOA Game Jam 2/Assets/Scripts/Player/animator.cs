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
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            gameObject.GetComponent<Animator>().Play("Player-Walk");
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            gameObject.GetComponent<Animator>().Play("Player-Crouch");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponent<Animator>().Play("Player-Jump");
        }
        if (!Input.anyKey)
        {
            gameObject.GetComponent<Animator>().Play("Player-Idle");
        }
    }
}
