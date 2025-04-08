using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        Paused
    }

    public static PauseManager instance;
    public GameState currentState = GameState.Playing;
    [SerializeField] private GameObject pauseMenuUI;
    [HideInInspector] public Button pauseButton;

    public Sprite pauseImage;
    public Sprite exitImage;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        pauseButton?.onClick.AddListener(TogglePause);
        pauseButton.image.sprite = pauseImage;
        Resume();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!InventoryUI.instance.onInventoryInspector)
            {
                TogglePause();
            }
            else if (InventoryUI.instance.onInventoryInspector && !ExorcismManager.instance.onExorcismProgress)
            {
                InventoryUI.instance.HideClueDetails();
            }
        }
    }

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

    public void Resume()
    {
        pauseMenuUI?.SetActive(false);
        Time.timeScale = 1f;
        currentState = GameState.Playing;
    }

    public void Pause()
    {
        pauseMenuUI?.SetActive(true);
        Time.timeScale = 0f;
        currentState = GameState.Paused;
    }

    public void BackTitleScreen()
    {
        ScenesManager.instance.LoadMenu();
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
