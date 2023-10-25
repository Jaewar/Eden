using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    System.Random rnd = new System.Random();

    IEnumerator WaitAndLoadNewScene() {
        int num = rnd.Next(6);
        yield return new WaitForSeconds(num);
        SceneManager.LoadScene(2);
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return StartCoroutine("WaitAndLoadNewScene");
    }

}
