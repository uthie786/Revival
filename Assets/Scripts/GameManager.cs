using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to the pause menu GameObject
    public AudioSource btnClickSound; // Reference to the button click sound
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            btnClickSound.Play();
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game
        pauseMenu.SetActive(true); // Show the pause menu
        isPaused = true;
    }

    void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        pauseMenu.SetActive(false); // Hide the pause menu
        isPaused = false;
    }
}
