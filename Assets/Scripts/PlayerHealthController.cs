using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    private int currentHealth;
    private float currentSanity;
    private float currentHunger;
    public int maxHealth = 5;
    public float hungerSpeed = 1.5f;
    public float maxHunger = 100.0f;
    public float maxSanity = 100.0f;
    public float invinciblityLength = 1.7f;
    private float invinceCounter;

    public GameObject[] modelDisplay;
    private float flashCounter;
    public float flashTimeInterval = .1f;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        FillAllStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale <= 0.00001f || LevelManager.instance.levelComplete) return;

        currentHunger -= hungerSpeed * Time.deltaTime;
        UIController.instance.UpdateHungerDisplay(currentHunger);
        if (currentHunger <= 0)
        {
            LevelManager.instance.Respawn();
        }

        if (invinceCounter > 0)
        {
            invinceCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                flashCounter = flashTimeInterval;
                foreach (GameObject piece in modelDisplay)
                {
                    piece.SetActive(!piece.activeSelf);
                }
            }

            // last frame
            if (invinceCounter <= 0)
            {
                foreach (GameObject piece in modelDisplay)
                {
                    piece.SetActive(true);
                }
            }
        }


    }

    public void DamagePlayer()
    {
        if (invinceCounter > 0) return;

        invinceCounter = invinciblityLength;

        currentHealth--;
        UIController.instance.UpdateHealthDisplay(currentHealth);
        if (currentHealth <= 0)
        {
            LevelManager.instance.Respawn();
        }
    }
    public void FillAllStats()
    {
        FillHealth();
        FillHunger();
        FillSanity();
    }

    public void FillHealth()
    {
        currentHealth = maxHealth;
        UIController.instance.UpdateHealthDisplay(currentHealth);
    }

    public void FillHunger()
    {
        currentHunger = maxHunger;
        UIController.instance.UpdateHungerDisplay(currentHunger);
    }

    public void FillSanity()
    {
        currentSanity = maxSanity;
        UIController.instance.UpdateSanityDisplay(currentSanity);
    }

    public void TryToEatCoin()
    {
        bool success = LevelManager.instance.UseCoin();
        if (success)
        {
            currentHunger = Math.Min(maxHunger, currentHunger + 10);  // TODO: 10
            UIController.instance.UpdateHungerDisplay(currentHunger);
        }
    }

    public void TryToEatCrystal()
    {
        bool success = LevelManager.instance.UseCrystal();
        if (success)
        {
            currentHealth = Math.Min(maxHealth, currentHealth + 1);  // TODO: 40
            UIController.instance.UpdateHealthDisplay(currentHealth);
        }
    }

    public bool TryToUseSanity(int sanity)
    {
        if (currentSanity < sanity) return false;

        currentSanity -= sanity;
        UIController.instance.UpdateSanityDisplay(currentSanity);
        return true;
    }

    public void AddSanity(float sanity)
    {
        currentSanity = Math.Min(maxSanity, currentSanity + sanity);
        UIController.instance.UpdateSanityDisplay(currentSanity);
    }
}