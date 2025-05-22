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
    private const float MIN_VOLUME = -80f;
    
    [SerializeField] private AudioMixer mixer;
    public float maxMasterVolume = 0f;
    public float maxSfxVolume = 10f; 
    public float maxMusicVolume = 0f;
    
    public float masterVolume, sfxVolume, musicVolume;
    
    public Slider masterSlider, sfxSlider, musicSlider;
    
    private TextMeshProUGUI masterText, sfxText, musicText;
    
    private void Start()
    {
        masterText = masterSlider.GetComponentInChildren<TextMeshProUGUI>();
        musicText = musicSlider.GetComponentInChildren<TextMeshProUGUI>();
        sfxText = sfxSlider.GetComponentInChildren<TextMeshProUGUI>();
        
        masterVolume = GetLevel("MasterVolume");
        sfxVolume = GetLevel("SFXVolume");
        musicVolume = GetLevel("MusicVolume");
        
        masterSlider.value = (masterVolume - MIN_VOLUME) / (maxMasterVolume - MIN_VOLUME) * masterSlider.maxValue;
        sfxSlider.value = (sfxVolume - MIN_VOLUME) / (maxSfxVolume - MIN_VOLUME) * sfxSlider.maxValue;
        musicSlider.value = (musicVolume - MIN_VOLUME) / (maxMusicVolume - MIN_VOLUME) * musicSlider.maxValue;
    }
    
    private float GetLevel(string parameter)
    {
        bool result =  mixer.GetFloat(parameter, out float value);
        
        if (result) return value;
        return 0f;
    }

    private void Update()
    {
        musicText.text = ((int)musicSlider.value).ToString();
        masterText.text = ((int)masterSlider.value).ToString();
        sfxText.text = ((int)sfxSlider.value).ToString();
        
        float scaledMasterVolume = masterSlider.value / masterSlider.maxValue * (maxMasterVolume - MIN_VOLUME) + MIN_VOLUME;
        float scaledSfxVolume =  sfxSlider.value / sfxSlider.maxValue * (maxSfxVolume - MIN_VOLUME) + MIN_VOLUME;
        float scaledMusicVolume = musicSlider.value / musicSlider.maxValue * (maxMusicVolume - MIN_VOLUME) + MIN_VOLUME;
        
        mixer.SetFloat("MasterVolume", scaledMasterVolume);
        mixer.SetFloat("SFXVolume", scaledSfxVolume);
        mixer.SetFloat("MusicVolume", scaledMusicVolume);
    }
}
