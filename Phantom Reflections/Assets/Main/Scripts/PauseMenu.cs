using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        Paused
    }

    public GameState currentState = GameState.Playing;
    public GameObject pauseMenuUI;
    public Button pauseButton;

    void Awake()
    {
        pauseButton.onClick.AddListener(TogglePause);
        Resume();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (currentState)
            {
                case GameState.Playing:
                    Pause();
                    break;

                case GameState.Paused:
                    Resume();
                    break;
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;           
        currentState = GameState.Playing;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        currentState = GameState.Paused;
    }

    // 切換暫停狀態
    public void TogglePause()
    {
        switch (currentState)
        {
            case GameState.Playing:
                Pause();
                break;

            case GameState.Paused:
                Resume();
                break;
        }

    }
}
