using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public static SaveLoad instance;

    private Vector3 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveGame() {
        PlayerPrefs.SetInt("ContinueAvailable", 1);
        PlayerPrefs.SetFloat("_PlayerX", PlayerControllerFirstPerson.instance.transform.position.x);
        PlayerPrefs.SetFloat("_PlayerY", PlayerControllerFirstPerson.instance.transform.position.y);
        PlayerPrefs.SetFloat("_PlayerZ", PlayerControllerFirstPerson.instance.transform.position.z);
        PlayerPrefs.SetFloat("_PlayerRotY", PlayerControllerFirstPerson.instance.transform.rotation.y);
    }

    public void LoadGame() {
        Vector3 oldLocation = new Vector3(PlayerPrefs.GetFloat("_PlayerX"), PlayerPrefs.GetFloat("_PlayerY"), PlayerPrefs.GetFloat("_PlayerZ"));
        PlayerControllerFirstPerson.instance.transform.position = oldLocation;
        PlayerControllerFirstPerson.instance.transform.Rotate(new Vector3(0, PlayerPrefs.GetFloat("_PlayerRotY"), 0));
    }
}
