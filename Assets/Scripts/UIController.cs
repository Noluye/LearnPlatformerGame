using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public Image fadeScreen;
    private bool isFadingToBlack = false;
    private bool isFadingFromBlack = false;
    public float fadeSpeed = 2f;

    public Slider healthSlider, hungerSlider, sanitySlider;
    public TMP_Text healthText, hungerText, sanityText, timeText;

    public TMP_Text coinText, crystalText;

    public GameObject pauseScreen;

    public string mainMenu, levelSelect;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadingToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
        }
        if (isFadingFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void FadeToBlack()
    {
        isFadingToBlack = true;
        isFadingFromBlack = false;
    }

    public void FadeFromBlack()
    {
        isFadingToBlack = false;
        isFadingFromBlack = true;
    }

    public void UpdateHealthDisplay(int health)
    {
        healthText.text = "HEALTH: " + health + "/" + PlayerHealthController.instance.maxHealth;
        healthSlider.maxValue = PlayerHealthController.instance.maxHealth;
        healthSlider.value = health;
    }

    public void UpdateHungerDisplay(float hunger)
    {
        hungerText.text = "HUNGER: " + (hunger / PlayerHealthController.instance.maxHunger * 100.0f).ToString("0");
        hungerSlider.maxValue = PlayerHealthController.instance.maxHunger;
        hungerSlider.value = hunger;
    }

    public void UpdateSanityDisplay(float sanity)
    {
        sanityText.text = "SANITY: " + (sanity / PlayerHealthController.instance.maxSanity * 100.0f).ToString("0");
        sanitySlider.maxValue = PlayerHealthController.instance.maxSanity;
        sanitySlider.value = sanity;
    }

    public void PauseUnpause()
    {
        pauseScreen.SetActive(!pauseScreen.activeSelf);
        if (pauseScreen.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
        
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenu);
    }

    public void GoToLevelSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelSelect);
    }
}
