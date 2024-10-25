using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private Coroutine triggerCoroutine;
    public Material startGameColour; // Reference to the material to change
    public Material loadInfoColour; // Reference to the material to change
    public AudioSource loadSFX;
    public GameObject infoScreen;// Reference to the load sound effect
    public AudioSource btnSFX;
    private void Start()
    {
        Time.timeScale = 1f;
        infoScreen.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        btnSFX.Play();
        loadSFX.Play();
        if (other.CompareTag("StartButton"))
        {
            triggerCoroutine = StartCoroutine(LoadGame());
            Debug.Log(triggerCoroutine);
        }
        if(other.CompareTag("InfoButton"))
        {
            triggerCoroutine = StartCoroutine(LoadInfo());
            Debug.Log(triggerCoroutine);
        }
    }

    void OnTriggerExit(Collider other)
    {
        loadSFX.Stop();
        if (other.CompareTag("StartButton"))
        {
            Debug.Log(triggerCoroutine);
            if (triggerCoroutine != null)
            {
                StopCoroutine(triggerCoroutine);
                triggerCoroutine = null;
                startGameColour.color = Color.black; // Reset color if exited early
            }
        }

        if (other.CompareTag("InfoButton"))
        {
            Debug.Log(triggerCoroutine);

            if (triggerCoroutine != null)
            {
                StopCoroutine(triggerCoroutine);
                triggerCoroutine = null;
                loadInfoColour.color = Color.black; // Reset color if exited early
            }
        }
    }

    IEnumerator LoadGame()
    {
        float duration = 3f;
        float elapsedTime = 0f;
        Color startColor = Color.black;
        Color endColor = Color.green;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            startGameColour.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            yield return null;
        }

        startGameColour.color = startColor; // Reset color after the loop
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Game");
    }

    IEnumerator LoadInfo()
    {
        float duration = 3f;
        float elapsedTime = 0f;
        Color startColor = Color.black;
        Color endColor = Color.green;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            loadInfoColour.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            yield return null;
        }

        loadInfoColour.color = startColor; // Reset color after the loop
        yield return new WaitForSeconds(0.1f);
        infoScreen.SetActive(true);
    }
}