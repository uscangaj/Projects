using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FlightSimulator
{
    public class PropellerController : MonoBehaviour
    {
        public float minRot = 30f;



        public void HandlePropeller(float currRPM)
        {
            float degPerSec = (((currRPM * 360f) / 60f) + minRot) * Time.deltaTime;
            degPerSec = Mathf.Clamp(degPerSec, 0f, minRot);

            transform.Rotate(Vector3.forward, degPerSec);
        }
    }
}