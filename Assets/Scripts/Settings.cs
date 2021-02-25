using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class Settings : MonoBehaviour
{
    public GameObject box;
    public AudioMixer AudioMixer;

    public void ButtonToggle()
    {
        if (box.activeInHierarchy)
            box.SetActive(false);
        else box.SetActive(true);
    }

    public void SetMusic(float volume)
    {
        AudioMixer.SetFloat("Music", volume);
    }
    public void SetSounds(float volume)
    {
        AudioMixer.SetFloat("Sounds", volume);
    }

}
