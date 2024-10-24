using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeController : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource
    public Slider volumeSlider; // Reference to the UI Slider

    void Start()
    {
        // Ensure the slider's value matches the current volume
        if (audioSource != null && volumeSlider != null)
        {
            volumeSlider.value = audioSource.volume;
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
        }
    }

    // Method to change the volume
    public void ChangeVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}
