using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private const float MIN_VOLUME = -40f;
    private const float MIN_MUSIC_VOLUME = -36f;
    
    [SerializeField] private AudioMixer mixer;
    public float maxMasterVolume = 0f;
    public float maxSfxVolume = 10f; 
    public float maxMusicVolume = 0f;
    
    private float _masterVolume, _sfxVolume, _musicVolume;
    
    public Slider masterSlider, sfxSlider, musicSlider;
    
    private TextMeshProUGUI _masterText, _sfxText, _musicText;
    
    private void Start()
    {
        _masterText = masterSlider.GetComponentInChildren<TextMeshProUGUI>();
        _musicText = musicSlider.GetComponentInChildren<TextMeshProUGUI>();
        _sfxText = sfxSlider.GetComponentInChildren<TextMeshProUGUI>();
        
        _masterVolume = GetLevel("MasterVolume");
        _sfxVolume = GetLevel("SFXVolume");
        _musicVolume = GetLevel("MusicVolume");

        masterSlider.value = (_masterVolume - MIN_VOLUME) / (maxMasterVolume - MIN_VOLUME) * masterSlider.maxValue;
        sfxSlider.value = (_sfxVolume - MIN_VOLUME) / (maxSfxVolume - MIN_VOLUME) * sfxSlider.maxValue;
        musicSlider.value = (_musicVolume - MIN_MUSIC_VOLUME) / (maxMusicVolume - MIN_MUSIC_VOLUME) * musicSlider.maxValue;
    }
    
    private float GetLevel(string parameter)
    {
        bool result =  mixer.GetFloat(parameter, out float value);
        
        if (result) return value;
        return 0f;
    }

    private void Update()
    {
        _musicText.text = ((int)musicSlider.value).ToString();
        _masterText.text = ((int)masterSlider.value).ToString();
        _sfxText.text = ((int)sfxSlider.value).ToString();
        
        float scaledMasterVolume = masterSlider.value / masterSlider.maxValue * (maxMasterVolume - MIN_VOLUME) + MIN_VOLUME;
        float scaledSfxVolume =  sfxSlider.value / sfxSlider.maxValue * (maxSfxVolume - MIN_VOLUME) + MIN_VOLUME;
        float scaledMusicVolume = musicSlider.value / musicSlider.maxValue * (maxMusicVolume - MIN_MUSIC_VOLUME) + MIN_MUSIC_VOLUME;
        
        mixer.SetFloat("MasterVolume", scaledMasterVolume);
        mixer.SetFloat("SFXVolume", scaledSfxVolume);
        mixer.SetFloat("MusicVolume", scaledMusicVolume);
    }
}
