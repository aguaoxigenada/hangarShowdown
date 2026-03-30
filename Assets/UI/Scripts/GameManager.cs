using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Interfaz
    public bool gamePaused;

    // Player
    public bool playerIsDead;

    public AudioClip deathSfx;
    public AudioSource audioSource;

    // Instancia de GameManager
    public static GameManager instance;

    void Awake()
    {
        // Singleton
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        gamePaused = !gamePaused;
//        Debug.Log(gamePaused);
        Time.timeScale = gamePaused == true ? 0.0f : 1.0f;
        Cursor.lockState = gamePaused == true ? CursorLockMode.None : CursorLockMode.Locked;

        // Menu de Pausa
        GameUI.instance.TogglePauseMenu(gamePaused);
    }

    public void CheckIfWon(bool won)
    {
        if (won)
            WinGame();
        else
            LoseGame();
    }

    void WinGame()
    {
        Cursor.lockState = CursorLockMode.None;
        GameUI.instance.SetEndGameScreen(true);
        Time.timeScale = 0.0f;
    }

    public void PlayerDied()
    {
        playerIsDead = true;
    }

    public void LoseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        GameUI.instance.SetEndGameScreen(false);
        Time.timeScale = 0.0f;
    }

}
