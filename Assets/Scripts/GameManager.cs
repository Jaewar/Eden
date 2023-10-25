using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] GameObject optionsPanel;
    public PlayableDirector director;

    public int curSceneIndex;
    public int nextSceneIndex;

    bool optionsPanelOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if (PlayerPrefs.GetInt("ContinueAvailable") != 1) {
            director.Play();
        }

        curSceneIndex = SceneManager.GetActiveScene().buildIndex;
        nextSceneIndex = curSceneIndex + 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (director.state != PlayState.Playing) {
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

    public PlayableDirector getDirector() {
        return director;
    }
}
