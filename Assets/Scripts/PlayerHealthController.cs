using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    private void Awake()
    {
        instance = this;
    }

    public float currentHealth, maxHealth;

    public Slider healthSlider;

    public GameObject deathEffect;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = PlayerStatController.instance.health[0].value;

        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void Heal(int pointsToHeal)
    {
        if (currentHealth + pointsToHeal <= maxHealth)
        {
            currentHealth += pointsToHeal;
        } else
        {
            currentHealth = maxHealth;
        }
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(float damageToTake)
    {
        currentHealth -= damageToTake;

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);

            LevelManager.instance.EndLevel();

            Instantiate(deathEffect, transform.position, transform.rotation);

            SFXManager.instance.PlaySFX(3);
        } else
        {
            PlayerController.instance.BlinkSprite();

            if (currentHealth < 50 && PlayerController.instance.luck >= Random.Range(0f, 1f))
            {
                ObjectSpawner.instance.SpawnObjectOutsideViewport();
            }
        }

        healthSlider.value = currentHealth;
    }
}
