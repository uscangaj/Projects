using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class ButtonController : MonoBehaviour
{
    public AudioSource sound;
    public AudioClip beep;



    public void sceneLoad (string sceneName)
    {
        sound.PlayOneShot(beep, 0.35f);
        Debug.Log("Loading Scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }


    public void quitGame()
    {
        sound.PlayOneShot(beep, 0.25f);
        Debug.Log("Game Exited");
        Application.Quit();
    }
}
