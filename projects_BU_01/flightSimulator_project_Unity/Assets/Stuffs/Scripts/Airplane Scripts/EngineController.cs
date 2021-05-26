using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FlightSimulator
{
    public class EngineController : MonoBehaviour
    {
        public float maxForce = 2500f;
        public float maxRPM = 2500f;
        public AnimationCurve powerCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public PropellerController propeller;



        public Vector3 CalculateForce(float throttle)
        {
            float finalThrottle = Mathf.Clamp01(throttle);
            float finalPower = finalThrottle * maxForce;
            float currRPM = finalThrottle * maxRPM;

            

            finalThrottle = powerCurve.Evaluate(finalThrottle);
            Vector3 finalForce = transform.forward * finalPower;
            propeller.HandlePropeller(currRPM);


            return finalForce;
        }
    }
}
