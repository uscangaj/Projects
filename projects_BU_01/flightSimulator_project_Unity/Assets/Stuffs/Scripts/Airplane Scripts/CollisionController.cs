using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FlightSimulator
{
    public class CollisionController : MonoBehaviour
    {
        public GameManager gm;
        public CharacteristicsController cc;

        public AudioSource sound1;
        public AudioSource sound2;
        public AudioClip crash;
        public AudioClip houston;
        public AudioClip noink;

        public float timer = 1f;
        public bool hasCrashed = false;



        // Start is called before the first frame update
        void Start()
        {
            timer = 1f;
            hasCrashed = false;
        }


        // Update is called once per frame
        void Update()
        {
            if (hasCrashed == true)
            {
                timer -= Time.deltaTime;
                
                if (timer <= 0)
                {
                    gm.gameOver = true;
                }
            }
        }


        void OnCollisionEnter(Collision other)
        {
            Debug.Log("Crashed | fwdSpeed:" + cc.fwdSpeed + " | MPH: " + cc.mph);

            

            if (cc.fwdSpeed > 12.5f && cc.mph > 50f)
            {
                sound1.PlayOneShot(crash, 0.7f);
                sound2.PlayOneShot(houston, 0.5f);

                hasCrashed = true;
            }
            else
            {
                sound1.PlayOneShot(noink, 0.75f);
            }
        }
    }
}
