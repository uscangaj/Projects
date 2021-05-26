using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class InstructionController : MonoBehaviour
{
    public GameObject main;
    public GameObject instructions;

    public AudioSource sound;
    public AudioClip beep;



    // Start is called before the first frame update
    private void Start()
    {
        main.SetActive(true);
        instructions.SetActive(false);
    }



    public void gotoMain()
    {
        sound.PlayOneShot(beep, 0.25f);
        main.SetActive(true);
        instructions.SetActive(false);
    }



    public void gotoInsructions()
    {
        sound.PlayOneShot(beep, 0.25f);
        main.SetActive(false);
        instructions.SetActive(true);
    }
}
