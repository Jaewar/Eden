using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    public static SaveLoad instance;

    private void Start() {
        if (instance != null && instance != this) {
            Destroy(this);
        } else {
            instance = this;
        }
    }

    public void SaveGame() {
        PlayerPrefs.SetInt("ContinueAvailable", 1);
        PlayerPrefs.SetInt("ScenesSaved", GameManager.instance.curSceneIndex);
        PlayerPrefs.SetFloat("_PlayerX", PlayerControllerFirstPerson.instance.transform.position.x);
        PlayerPrefs.SetFloat("_PlayerY", PlayerControllerFirstPerson.instance.transform.position.y);
        PlayerPrefs.SetFloat("_PlayerZ", PlayerControllerFirstPerson.instance.transform.position.z);
        PlayerPrefs.SetFloat("_PlayerRotY", PlayerControllerFirstPerson.instance.transform.rotation.y);
        PlayerPrefs.SetInt("SceneIndex", SceneManager.GetActiveScene().buildIndex);
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextScene() {
        PlayerPrefs.SetInt("SceneIndex", GameManager.instance.nextSceneIndex);
        SceneManager.LoadScene(GameManager.instance.nextSceneIndex);
        
    }

}
