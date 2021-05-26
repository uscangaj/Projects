using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FlightSimulator
{
    [RequireComponent(typeof(WheelCollider))]
    public class WheelController : MonoBehaviour
    {
        private WheelCollider wcol;
        private Vector3 pos;
        private Quaternion rot;
        public Transform wheelT;
        public bool isBrake = false;
        public bool isSteer = false;
        public float brakePower = 10f;
        public float steerAngle = 25f;
        public float smoothness = 5f;
        private float finalSteering;



        void Start()
        {
            wcol = GetComponent<WheelCollider>();
        }

        
        void Update()
        {

        }


        public void InitWheel()
        {
            if (wcol)
            {
                wcol.motorTorque = 0.0000000001f;
            }
        }

        
        public void HandleWheel(InputController input)
        {
            if(wcol)
            {
                wcol.GetWorldPose(out pos, out rot);

                if (wheelT)
                {
                    wheelT.rotation = rot;
                    wheelT.position = pos;
                }


                if (isBrake && input.brake > 0.1f)
                {
                    wcol.brakeTorque = input.brake * brakePower;
                }
                else
                {
                    wcol.brakeTorque = 0f;
                    wcol.motorTorque = 0.000000001f;
                }


                if(isSteer)
                {
                    finalSteering = Mathf.Lerp(finalSteering, -input.yaw * steerAngle, Time.deltaTime * smoothness);
                    wcol.steerAngle = finalSteering;
                }
            }
        }
    }
}