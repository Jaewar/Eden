using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject optionsPanel;

    bool optionsPanelOpen = false;


    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("ContinueAvailable") == 1) {
            SaveLoad.instance.LoadGame();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            ToggleOptionsMenu();
        }

        if (optionsPanel.gameObject.activeInHierarchy) {
            PlayerControllerFirstPerson.instance.LockCursor(false);
            SoundManager.instance.setValueTexts();
        } else {
            PlayerControllerFirstPerson.instance.LockCursor(true);
        }
    }

    public void ToggleOptionsMenu() {
        optionsPanelOpen = !optionsPanelOpen;
        optionsPanel.gameObject.SetActive(optionsPanelOpen);
        SoundManager.instance.PlaySFX(0);
    }

    public void MainMenuButton() {
        SaveLoad.instance.SaveGame();
        SceneManager.LoadScene(0);
    }

    public void QuitGame() {
        SaveLoad.instance.SaveGame();
        Application.Quit();
    }
}
