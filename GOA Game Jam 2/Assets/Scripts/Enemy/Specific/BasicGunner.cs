using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicGunner : MonoBehaviour
{
    NavMeshAgent agent;
    Transform Player;
    public float knockbackStrength;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("PlayerFinder").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(new Vector2(Player.position.x, -3));
        if (Player.position.x < transform.position.x) GetComponentInChildren<SpriteRenderer>().flipX = true;
        else GetComponentInChildren<SpriteRenderer>().flipX = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("Meow");
            collision.gameObject.GetComponentInChildren<PlayerHealth>().currentHealth -= 200;
        }
    }
}
