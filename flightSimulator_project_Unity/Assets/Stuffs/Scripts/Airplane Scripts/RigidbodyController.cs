using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FlightSimulator
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public class RigidbodyController : MonoBehaviour
    {
        public Rigidbody rb;
        public AudioSource sound;



        public void Start()
        {
            rb = GetComponent<Rigidbody>();
            sound = GetComponent<AudioSource>();
            if (sound)
            {
                sound.playOnAwake = false;
            }
        }


        void FixedUpdate()
        {
            if (rb)
            {
                HandlePhysics();
            }
        }



        protected virtual void HandlePhysics()
        {

        }
    }
}
