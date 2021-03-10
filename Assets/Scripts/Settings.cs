using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    [SerializeField]
    private AudioMixer AudioMixer;
    [SerializeField]
    private string SliderName;

    private void Start()
    {
        float volume = 0f;
        AudioMixer.GetFloat(SliderName, out volume);
        gameObject.GetComponent<Slider>().value = volume;
    }

    public void SetVolume(float volume)
    {
        AudioMixer.SetFloat(SliderName, volume);
    }

}
