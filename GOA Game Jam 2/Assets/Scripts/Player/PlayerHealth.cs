using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    public int maxHealth, currentHealth;
    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentHealth = maxHealth;
        HealthBar.instance.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth--;
        HealthBar.instance.SetHealth(currentHealth);

        if(currentHealth <= 0 && !dead)
        {
            dead = true;
            Debug.Log("Meow");
            GetComponent<Animator>().SetLayerWeight(0, 0);
            GetComponent<Animator>().SetLayerWeight(1, 1);
            GetComponent<Animator>().Play("Player-Death");
        }
    }

    void Die()
    {

    }

    public void Heal()
    {
        currentHealth = maxHealth;
    }
}
