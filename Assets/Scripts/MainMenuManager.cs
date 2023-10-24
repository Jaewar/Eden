using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button continueButton;
    [SerializeField] GameObject optionsPanel;

    bool optionsPanelOpen = false;
    bool saveDataExists = false;

    private void Awake() {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    // Start is called before the first frame update
    void Start() {
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
}
