using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu; // Reference to the pause menu GameObject
    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        pauseMenu.SetActive(false); // Hide the pause menu
    }

    public void QuitToMenu()
    {
        Debug.Log("clicked");
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }
}
