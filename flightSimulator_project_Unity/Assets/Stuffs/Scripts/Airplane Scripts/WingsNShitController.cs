using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FlightSimulator
{
    public enum wingType
    {
        Aileron,
        Flap,
        Elevator,
        Rudder
    }


    public class WingsNShitController : MonoBehaviour
    {
        public Transform thing;
        public Vector3 rotAxis = Vector3.right;
        public wingType type = wingType.Rudder;
        public float rotAngle = 45f;
        public float wantedAngle;
        public float smoothness = 5f;
        


        // Start is called before the first frame update
        void Start()
        {

        }


        // Update is called once per frame
        void Update()
        {
            Vector3 axisAngle = rotAxis * wantedAngle;
            thing.localRotation = Quaternion.Slerp(thing.localRotation, Quaternion.Euler(axisAngle), Time.deltaTime * smoothness);
        }


        public void HandleStuff(InputController input)
        {
            float inpValue = 0f;
            switch(type)
            {
                case wingType.Aileron:
                    inpValue = input.roll;
                    break;
                case wingType.Flap:
                    inpValue = input.flaps;
                    break;
                case wingType.Elevator:
                    inpValue = input.pitch;
                    break;
                case wingType.Rudder:
                    inpValue = input.yaw;
                    break;
                default:
                    break;
            }

            wantedAngle = rotAngle * inpValue;
        }
    }
}
