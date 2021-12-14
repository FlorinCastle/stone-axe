using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;

    [Header("UI")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private void Start()
    {
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
    }

    public void SetMusicLevel(float sliderValue) { SetLevel(sliderValue, _musicSlider); }

    public void SetLevel(float sliderValue, Slider sliderRef)
    {
        if (sliderRef == _musicSlider)
        {
            _mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
            PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        }
        if (sliderRef == _sfxSlider)
        {
            _mixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
            PlayerPrefs.SetFloat("SFXVolume", sliderValue);
        }
    }
}
