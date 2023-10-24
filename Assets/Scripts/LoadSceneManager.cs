using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{

    IEnumerator WaitAndLoadNewScene() {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene(2);
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return StartCoroutine("WaitAndLoadNewScene");
    }

}
