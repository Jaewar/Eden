using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button continueButton;
    [SerializeField] GameObject optionsPanel;

    bool optionsPanelOpen = false;
    bool saveDataExists;

    private void Awake() {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    // Start is called before the first frame update
    void Start() {
        saveDataExists = PlayerPrefs.GetInt("ContinueAvailable") == 1;
        if (!saveDataExists) {
            continueButton.GetComponent<CanvasRenderer>().SetColor(new Color(0, 0, 0, 0.5f));
            continueButton.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (optionsPanel.gameObject.activeInHierarchy) {
            SoundManager.instance.setValueTexts();
        }
    }

    public void ToggleOptionsMenu() {
        optionsPanelOpen = !optionsPanelOpen;
        optionsPanel.gameObject.SetActive(optionsPanelOpen);
        SoundManager.instance.PlaySFX(0);
    }

    public void QuitButton() {
        SoundManager.instance.SaveSoundSettings();
        SoundManager.instance.PlaySFX(0);
        Application.Quit();
    }

    public void NewGameButton() {
        PlayerPrefs.SetInt("ContinueAvailable", 0);
        SceneManager.LoadScene(1);
    }

    public void ContinueButton() {
        SceneManager.LoadScene(1);
    }
}
