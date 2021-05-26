using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MusicController : MonoBehaviour
{
    public float timer;

    public AudioSource sound;
    public AudioClip music;

    public bool startMusic;



    // Start is called before the first frame update
    void Start()
    {
        startMusic = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Invoke("playMusic", 3.0f);


        if (timer > 0.0f)
            timer -= Time.deltaTime;
        else
        {
            if (!sound.isPlaying)
                sound.PlayOneShot(music, 0.15f);
        }
    }


    void playMusic()
    {
        Debug.Log("Playing Music");
        if (!sound.isPlaying)
            sound.PlayOneShot(music, 0.25f);
        //sound.Play();
    }
}
