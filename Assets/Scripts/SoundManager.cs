using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    const string musicVolPP = "_MusicVolume";
    const string sfxVolPP = "_SFXVolume";

    [SerializeField] Slider musicVolumeSlider, sfxVolumeSlider;
    [SerializeField] AudioSource musicSource, sfxSource;
    [SerializeField] TextMeshProUGUI musicValueText, sfxValueText;

    [SerializeField] AudioClip[] musicClips, sfxClips;

    void Start()
    {
        if (instance == null) {
            instance = this;
        }

        LoadSoundSettings();

        // Listener for slider change (music)
        musicVolumeSlider.onValueChanged.AddListener(delegate { MusicValueChangeCheck(); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { SFXValueChangeCheck(); });

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SaveSoundSettings() {
        PlayerPrefs.SetFloat(musicVolPP, musicVolumeSlider.value);
        PlayerPrefs.SetFloat(sfxVolPP, sfxVolumeSlider.value);

    }

    // getting floats and assigning options panel values.
    void LoadSoundSettings() {
        musicSource.volume = PlayerPrefs.GetFloat(musicVolPP);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(musicVolPP);

        sfxSource.volume = PlayerPrefs.GetFloat (sfxVolPP);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(sfxVolPP);
    }

    void SetMusicVolume(float volume) {
        musicSource.volume = musicVolumeSlider.value;
    }

    void SetSFXVolume(float volume) {
        sfxSource.volume = sfxVolumeSlider.value;
    }

    void MusicValueChangeCheck() {
        SetMusicVolume(musicVolumeSlider.value);
        SaveSoundSettings();
    }

    void SFXValueChangeCheck() {
        SetSFXVolume(sfxVolumeSlider.value);
        SaveSoundSettings();
        sfxSource.clip = sfxClips[0];
        sfxSource.Play();
    }

    // For MainMenuManager, while Options Panel active.
    public void setValueTexts() {
        musicValueText.text = normalizeValue(PlayerPrefs.GetFloat(musicVolPP)).ToString();
        sfxValueText.text = normalizeValue(PlayerPrefs.GetFloat(sfxVolPP)).ToString();
    }
    // Convert 0 to 1 into a value of 100
    int normalizeValue(float value) {
        if (value == 0) {
            return 0;
        } else return Mathf.RoundToInt(value * 100);
        
    }

    public void PlaySFX(int index) {
        sfxSource.clip = sfxClips[index];
        sfxSource.Play();
    }
}
