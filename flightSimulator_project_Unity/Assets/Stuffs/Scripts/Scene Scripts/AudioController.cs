using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FlightSimulator
{
    public class AudioController : MonoBehaviour
    {
        public AudioSource idleSound;
        public AudioSource throttleSound;
        public InputController input;
        public float finalVolume;
        public float finalPitch;
        public float maxPitch = 1.25f;



        // Start is called before the first frame update
        void Start()
        {
            if (throttleSound)
            {
                throttleSound.volume = 0f;
            }
        }


        // Update is called once per frame
        void Update()
        {
            if(input)
            {
                HandleAudio();
            }
        }


        protected virtual void HandleAudio()
        {
            finalVolume = Mathf.Lerp(0f, 1f, input.stickyThrottle * 1.5f);
            finalPitch = Mathf.Lerp(0.85f, maxPitch, input.stickyThrottle * 1.25f);

            throttleSound.volume = finalVolume;
            throttleSound.pitch = finalPitch;
        }
    }
}
