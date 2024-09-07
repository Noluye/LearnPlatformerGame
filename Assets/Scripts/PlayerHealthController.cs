using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    private int currentHealth;
    public int maxHealth = 5;
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
        FillHealth();
    }

    // Update is called once per frame
    void Update()
    {
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

    public void FillHealth()
    {
        currentHealth = maxHealth;
        UIController.instance.UpdateHealthDisplay(currentHealth);
    }
}
