using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    [SerializeField]
    private AudioMixer AudioMixer;

    private void Start()
    {
        float volume = 0f;
        AudioMixer.GetFloat("Music", out volume);
        gameObject.GetComponent<Slider>().value = volume;
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
