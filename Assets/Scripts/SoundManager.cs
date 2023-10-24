using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    const string musicVolPP = "_MusicVolume";

    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] AudioSource musicSource;
    [SerializeField] TextMeshProUGUI musicValueText;

    void Start()
    {
        if (instance == null) {
            instance = this;
        }

        LoadSoundSettings();

        // Listener for slider change (music)
        musicVolumeSlider.onValueChanged.AddListener(delegate { MusicValueChangeCheck(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SaveSoundSettings() {
        PlayerPrefs.SetFloat(musicVolPP, musicVolumeSlider.value);

    }

    // getting floats and assigning options panel values.
    void LoadSoundSettings() {
        musicSource.volume = PlayerPrefs.GetFloat(musicVolPP);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(musicVolPP);
    }

    void SetMusicVolume(float volume) {
        musicSource.volume = musicVolumeSlider.value;
    }

    void MusicValueChangeCheck() {
        SetMusicVolume(musicVolumeSlider.value);
        SaveSoundSettings();
    }

    // For MainMenuManager, while Options Panel active.
    public void setValueTexts() {
        musicValueText.text = normalizeValue(PlayerPrefs.GetFloat(musicVolPP)).ToString();
    }
    // Convert 0 to 1 into a value of 100
    int normalizeValue(float value) {
        if (value == 0) {
            return 0;
        } else return Mathf.RoundToInt(value * 100);
        
    }
}
