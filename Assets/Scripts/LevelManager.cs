using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private CameraController cam;

    private void Awake()
    {
        instance = this;

        currentCrystals = PlayerPrefs.GetInt("Crystals");
        currentCoins = PlayerPrefs.GetInt("Coins");
    }

    public float waitBeforeRespawning = 1;
    [HideInInspector]
    public bool respawning;

    private PlayerController player;

    [HideInInspector]
    public Vector3 respawnPoint;

    [HideInInspector]
    public float levelTimer;

    public int coinThreshold = 100;
    public int currentCoins = 0;
    public int currentCrystals = 0;

    public float waitToEndLevel = 2f;
    public bool levelComplete;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        respawnPoint = player.transform.position + Vector3.up;
        cam = FindObjectOfType<CameraController>();

        UIController.instance.coinText.text = currentCoins.ToString();
        UIController.instance.crystalText.text = currentCrystals.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelComplete) {
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation,
                Quaternion.Euler(0f, 180f, 0f), 10f * Time.deltaTime);
            return;
        }

        levelTimer += Time.deltaTime;
        UIController.instance.timeText.text = levelTimer.ToString("0");

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
        }
#endif
    }

    public void Respawn()
    {
        if (!respawning)
        {
            respawning = true;

            StartCoroutine(RespawnCo());
        }
    }

    public IEnumerator RespawnCo()
    {
        player.gameObject.SetActive(false);
        UIController.instance.FadeToBlack();

        yield return new WaitForSeconds(waitBeforeRespawning);
        player.transform.position = respawnPoint;
        cam.snapToTaget();

        player.gameObject.SetActive(true);

        respawning = false;

        UIController.instance.FadeFromBlack();

        PlayerHealthController.instance.FillAllStats();
        player.DestroyShield();
    }

    public void GetCoin()
    {
        currentCoins++;
        //if (currentCoins >= coinThreshold)
        //{
        //    currentCoins -= coinThreshold;
        //    GetCrytal();
        //}
        UIController.instance.coinText.text = currentCoins.ToString();
        PlayerPrefs.SetInt("Coins", currentCoins);
    }

    public bool UseCoin()
    {
        if (currentCoins == 0) return false;
        currentCoins--;
        UIController.instance.coinText.text = currentCoins.ToString();
        PlayerPrefs.SetInt("Coins", currentCoins);
        return true;
    }

    public bool UseCrystal()
    {
        if (currentCrystals == 0) return false;
        currentCrystals--;
        UIController.instance.crystalText.text = currentCrystals.ToString();
        PlayerPrefs.SetInt("Crystals", currentCrystals);
        return true;
    }

    public void GetCrytal()
    {
        currentCrystals++;
        UIController.instance.crystalText.text = currentCrystals.ToString();

        PlayerPrefs.SetInt("Crystals", currentCrystals);
    }

    public void EndLevel(string nextLevel)
    {
        StartCoroutine(EndLevelCo(nextLevel));
    }

    IEnumerator EndLevelCo(string nextLevel)
    {
        levelComplete = true;

        //player.anim.SetTrigger("endLevel");

        yield return new WaitForSeconds(waitToEndLevel - 1f);
        UIController.instance.FadeToBlack();
        yield return new WaitForSeconds(1f);

        if (nextLevel == "MainMenu")
        {
            Cursor.lockState = CursorLockMode.None;
        }
        SceneManager.LoadScene(nextLevel);
    }
}
