using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class WarningController : MonoBehaviour
{
    public float timer = 2.0f;
    //public Text warning;
    public AudioSource sound;
    public AudioClip warn;
    


    // Start is called before the first frame update
    void Start()
    {
        //warning.text = "";
    }


    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            //warning.text = "";
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (!sound.isPlaying)
            sound.PlayOneShot(warn, 0.75f);
        //warning.text = "Warning";
        timer = 2.0f;
    }
}
