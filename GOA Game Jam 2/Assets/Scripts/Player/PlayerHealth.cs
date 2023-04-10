using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
        PlayerMovement.instance.enabled = false;
        PlayerAttack.instance.enabled = false;
        StartCoroutine(MainMenu());
    }

    IEnumerator MainMenu()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("TitleScreen");
    }

    public void Heal()
    {
        currentHealth = maxHealth;
    }
}
