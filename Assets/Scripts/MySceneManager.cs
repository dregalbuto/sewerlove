using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D ChangeScene)
    {
        if (ChangeScene.gameObject.tag == "Player")
        {
            Debug.Log("Got player");
            if (SceneManager.GetActiveScene().name == "Town")
            {
                Debug.Log("Loading Fishing scene");
                SceneManager.LoadSceneAsync("Fishing");
            }
            else if (SceneManager.GetActiveScene().name == "Fishing")
            {
                SceneManager.LoadSceneAsync("Town");
                Debug.Log("Loading Town scene");
            }
        }
    }
}
