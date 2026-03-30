using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [Header("HUD")]
    public TextMeshProUGUI ammountOfAmmoInMagazine;
    public TextMeshProUGUI totalAmountOfAmmo;
    public Image healthBarFill;
    public Image staminaFill;

    [Header("Pause Menu")]
    public GameObject pauseMenu;

    [Header("End Game Screen")]
    public GameObject endGameScreen;
    public TextMeshProUGUI endGameHeaderText;
    public TextMeshProUGUI endgameText;

    public int intOfLevel;
    public bool isKnife;

    public GameObject panel;

    // instance
    public static GameUI instance;

    void Awake()
    {
        // Instancia del singleton
        instance = this;
    }

    public void UpdateHealthBar(float curHp, float maxHp)  // hay que hacer un clam
    {
        healthBarFill.fillAmount = (float)curHp / (float)maxHp;
    }

    public void UpdateStaminaBar(float curStamina, float maxStamina)
    {
        staminaFill.fillAmount = curStamina / maxStamina;
    }

    public void AmmoText(int curAmmo, int maxAmmo)
    {
        if (!isKnife)
            totalAmountOfAmmo.text = "Ammo: " + curAmmo + "/" + maxAmmo;
        else if(isKnife)
        {
            Debug.Log("shit happened");
            totalAmountOfAmmo.text = "   ";
        }
    }

    public void AmmoInMagazine(int ammoInCurrentMagazine)
    {
        if (!isKnife)
            ammountOfAmmoInMagazine.text = "Current Ammo In Magazine: " + ammoInCurrentMagazine;
        else
            ammountOfAmmoInMagazine.text = "   ";
    }

    public void TogglePauseMenu(bool paused)
    {
        panel.SetActive(paused);
        pauseMenu.SetActive(paused);
    }

    public void CamPulse()
    {
        GetComponentInChildren<VignettePulse>().DoPulse();
    }

    public void SetEndGameScreen(bool won)
    {
        panel.SetActive(true);
        endGameScreen.SetActive(true);
        endGameHeaderText.text = (won == true ? "You Won" : "You Lose");
        endGameHeaderText.color = (won == true ? Color.green : Color.red);
        endgameText.text = (won == true ? "Congratulations Soldier!" : "Try Again Soldier!");
    }

    public void OnResumeButton()
    {
        Debug.Log("OnREsumeButton");
        GameManager.instance.TogglePauseGame();
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene(intOfLevel); // se puede poner el indice que aparece en el build tambien 0
    }

    public void OnMenuButton()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }
}
