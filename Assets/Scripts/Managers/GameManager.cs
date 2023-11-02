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

    [SerializeField] GameObject optionsPanel, crossHair, inventoryPanel;
    [SerializeField] TextMeshProUGUI OpenText, CloseText, pickupText, curApples, InteractText;
    public PlayableDirector director;

    public int curSceneIndex;
    public int nextSceneIndex;
    public int applesCollected = 0;
    public int applesRequired = 5;

    bool optionsPanelOpen = false;
    bool inventoryPanelOpen = false;
    bool doorSelected;
    bool itemSelected;
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

        if (inventoryPanelOpen == false) {
            inventoryPanel.SetActive(false);
        }

        curApples.text = "Collected: " + applesCollected.ToString();
        if (director.state != PlayState.Playing) {
            if (Input.GetKeyDown(KeyCode.Escape) && inventoryPanelOpen == false) {
                ToggleOptionsMenu();
            }
            if (Input.GetKeyDown(KeyCode.Tab) && optionsPanelOpen == false) {
                ToggleInventory();
            }

            if (optionsPanel.gameObject.activeInHierarchy || inventoryPanel.gameObject.activeInHierarchy) {
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
                    if (hit.collider.tag == "Item") {
                        itemSelected = true;
                        TogglePickupText(true);
                    }
                    if (itemSelected) {
                        if (Input.GetKeyDown(KeyCode.E)) {
                            if (hit.collider.gameObject.tag == "Item") {
                                hit.collider.gameObject.SetActive(false);
                                Inventory.Instance.AddItem(hit.collider.gameObject.GetComponent<Item>());
                            }
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
                    itemSelected = false;
                    kitchenSelected = false;
                }
                if (!doorSelected) {
                    ToggleOpenText(false);
                    ToggleCloseText(false);
                }
                if (!itemSelected) {
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

    public void ToggleInventory() {
        inventoryPanelOpen = !inventoryPanelOpen;
        if (inventoryPanelOpen == false) {
            Inventory.Instance.PutItemBack();
        }
        inventoryPanel.gameObject.SetActive(inventoryPanelOpen);
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

    public bool InventoryPanelActive() {
        return inventoryPanel.gameObject.activeInHierarchy;
    }
}
