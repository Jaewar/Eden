using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] GameObject optionsPanel, crossHair;
    [SerializeField] TextMeshProUGUI OpenText, CloseText, pickupText, curApples, InteractText;
    public PlayableDirector director;

    public int curSceneIndex;
    public int nextSceneIndex;
    public int applesCollected = 0;
    public int applesRequired = 5;

    bool optionsPanelOpen = false;
    bool doorSelected;
    bool appleSelected;
    bool kitchenSelected;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if (PlayerPrefs.GetInt("ContinueAvailable") != 1) {
            director.Play();
        }

        curSceneIndex = SceneManager.GetActiveScene().buildIndex;
        nextSceneIndex = curSceneIndex + 1;

        OpenText.gameObject.SetActive(false);
        CloseText.gameObject.SetActive(false);
        pickupText.gameObject.SetActive(false);
        InteractText.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update() {
        curApples.text = "Collected: " + applesCollected.ToString();
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

            if (!optionsPanel.gameObject.activeInHierarchy) {
                if (Physics.Raycast(RayCastDetector.instance.getRay(), out RaycastHit hit, RayCastDetector.instance.range)) {
                    if (hit.collider.tag == "Door") {
                        doorSelected = true;
                        if (!hit.collider.gameObject.GetComponent<Animator>().GetBool("isOpen")) {
                            ToggleOpenText(true);
                            ToggleCloseText(false);
                        } else {
                            ToggleCloseText(true);
                            ToggleOpenText(false);
                        }
                    }
                    if (doorSelected) {
                        if (Input.GetKeyDown(KeyCode.E)) {
                            hit.collider.gameObject.GetComponent<Animator>().SetBool("isOpen", !hit.collider.gameObject.GetComponent<Animator>().GetBool("isOpen"));
                            hit.collider.gameObject.GetComponent<Animator>().SetBool("OpenDoor", !hit.collider.gameObject.GetComponent<Animator>().GetBool("OpenDoor"));
                        }
                    }
                    if (hit.collider.tag == "Apple") {
                        appleSelected = true;
                        TogglePickupText(true);
                    }
                    if (appleSelected) {
                        if (Input.GetKeyDown(KeyCode.E)) {
                            hit.collider.gameObject.SetActive(false);
                            applesCollected++;
                        }
                    }
                    if (hit.collider.tag == "Kitchen") {
                        kitchenSelected = true;
                        ToggleInteractText(true);
                    }
                    if (kitchenSelected) {
                        if (Input.GetKeyDown(KeyCode.E)) {
                            if (applesCollected >= 5) {
                                Debug.Log("stage complete");
                            } else {
                                Debug.Log(applesCollected + " must be " + applesRequired);
                            }
                        }
                    }
                } else {
                    doorSelected = false;
                    appleSelected = false;
                    //kitchenSelected = false;
                }
                if (!doorSelected) {
                    ToggleOpenText(false);
                    ToggleCloseText(false);
                }
                if (!appleSelected) {
                    TogglePickupText(false);
                }
                if (!kitchenSelected) {
                    ToggleInteractText(false);
                }
            }
            }
        }

        public void TogglePickupText(bool toggle) {
        pickupText.gameObject.SetActive(toggle);
    }

    public void ToggleOpenText(bool toggle) {
        OpenText.gameObject.SetActive(toggle);
    }

    public void ToggleInteractText(bool toggle) {
        InteractText.gameObject.SetActive(toggle);
    }

    public void ToggleCloseText(bool toggle) {
        CloseText.gameObject.SetActive(toggle);
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

    public void DisableCrosshair(bool toggle) {
        crossHair.gameObject.SetActive(!toggle);
    }

    public bool OptionsPanelActive() {
        return optionsPanel.gameObject.activeInHierarchy;
    }
}
