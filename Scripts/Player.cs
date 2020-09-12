using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public float currentHealth = 100;
    public float maxHealth = 100;
    public HealthBar healthBar;
    public Animator animator;
    public float deathTime;
    public LevelLoader levelLoader;
    private float healTime;
    public float yellowHealth = 100;
    public Slider yellow;
    public GameObject endScreen;
    public UiController ui;

    public void Start()
    {
        animator.GetComponent<Animator>();
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (yellowHealth > currentHealth)
        {
            StartCoroutine("YellowHealth");
        }
        if (yellowHealth <= currentHealth)
        {
            SetYellowHealth();
        }
        animator.SetFloat("Health", currentHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth >= 100)
        {
            currentHealth = 100;
        }


        if (currentHealth <= 0)
        {
            deathTime -= Time.unscaledDeltaTime;
            if (deathTime <= 0)
            {
                ui.EndScreen(false);
                endScreen.SetActive(true);
            }

        }
    }

    public void SetYellowHealth()
    {
        yellowHealth = currentHealth;
        SetYellow();
    }

    public void DamagePlayer (float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    IEnumerator YellowHealth()
    {
        yield return new WaitForSeconds(2);
        yellowHealth -= 4 * Time.fixedDeltaTime;
        SetYellow();
        yield return new WaitForSeconds(2);
        
    }

    public void HealPlayer (int heal)
    {
        currentHealth += heal;
        healthBar.SetHealth (currentHealth);
    }

    void SetYellow()
    {
        yellow.value = yellowHealth;
    }

}
