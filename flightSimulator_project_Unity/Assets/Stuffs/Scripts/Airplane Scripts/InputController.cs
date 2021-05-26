using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FlightSimulator
{
    public class InputController : MonoBehaviour
    {
        public float pitch = 0f;
        public float roll = 0f;
        public float yaw = 0f;
        public float throttle = 0f;
        public float brake = 0f;
        public int flaps = 0;
        public int fMax = 3;
        public KeyCode brakeKey = KeyCode.Space;
        public float throttleSpeed = 0.1f;
        public float stickyThrottle;
        


        void Start()
        {

        }


        void Update()
        {
            HandleInput();
        }


        void HandleInput()
        {
            pitch = Input.GetAxis("Vertical");
            roll = Input.GetAxis("Horizontal");

            yaw = Input.GetAxis("Yaw");
            throttle = Input.GetAxis("Throttle");
            /*
            if (!Input.GetKey(KeyCode.W) || !Input.GetKey(KeyCode.S))
            {
                stickyThrottle -= throttle * throttleSpeed * Time.deltaTime;
                stickyThrottle = Mathf.Clamp01(stickyThrottle);
            }
            */

            StickyThrottleControl();

            brake = Input.GetKey(KeyCode.Space) ? 1f : 0f;

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                flaps -= 1;
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                flaps += 1;
            }
            flaps = Mathf.Clamp(flaps, 0, fMax);
        }


        void StickyThrottleControl()
        {
            stickyThrottle = stickyThrottle + (throttle * throttleSpeed * Time.deltaTime);
            stickyThrottle = Mathf.Clamp01(stickyThrottle);
        }
    }
}