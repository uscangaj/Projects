using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FlightSimulator
{
    public class CharacteristicsController : MonoBehaviour
    {
        private Rigidbody rb;
        private float startDrag;
        private float startAngDrag;
        public float fwdSpeed;
        public float maxLiftPower = 850f;
        public float mph;
        public float maxMPS;
        public float maxMPH = 150f;
        public float mphConversion = 2.24f;
        public float normalizedMPH;
        public AnimationCurve liftCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public float dragFactor = 0.015f;
        public float flapDragFactor = 0.0075f;
        public float flapLift = 75f;
        public float attackAngle;
        public float pitchAngle;
        public float rollAngle;
        public float yawAngle;
        public float pitchSpeed = 1500f;
        public float rollSpeed = 1500f;
        public float yawSpeed = 1500f;
        private InputController input;
        public float rbSpeed = 0.05f;
        private float torqueFactor;
        public AnimationCurve torqueCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public Vector3 finalLiftForce;
        public float dragFinal;



        public void InitCharacteristics(Rigidbody currRB, InputController currInp)
        {
            input = currInp;
            rb = currRB;

            startDrag = rb.drag;
            startAngDrag = rb.angularDrag;

            maxMPS = maxMPH / mphConversion;
        }


        public void UpdateCharacteristics()
        {
            if (rb)
            {
                CalcFwdSpeed();
                HandleTorque();
                CalcLift();
                CalcDrag();
                HandlePitch();
                HandleRoll();
                HandleYaw();
                HandleBank();
                HandleRB();
            }
        }


        void CalcFwdSpeed()
        {
            Vector3 localVel = transform.InverseTransformDirection(rb.velocity);
            fwdSpeed = (Mathf.Max(0f, localVel.z));
            fwdSpeed = Mathf.Clamp(fwdSpeed, 0, maxMPS);

            
            mph = fwdSpeed * mphConversion * 1.5f;
            mph = Mathf.Clamp(mph, 0f, maxMPH);
            normalizedMPH = Mathf.InverseLerp(0f, maxMPH, mph);
        }


        void HandleTorque()
        {
            torqueFactor = torqueCurve.Evaluate(normalizedMPH);
        }


        void CalcLift()
        {
            if (fwdSpeed > 22.5f)
            {
                attackAngle = Vector3.Dot(rb.velocity.normalized, transform.forward);
                attackAngle *= attackAngle;

                float flapLiftPower = (float)input.flaps * flapLift;
                Vector3 liftDir = transform.up;
                float liftPower = liftCurve.Evaluate(normalizedMPH) * maxLiftPower * 0.25f;

                finalLiftForce = liftDir * ((liftPower + flapLiftPower) * 0.75f) * attackAngle;
                rb.AddForce(finalLiftForce);

                //Debug.Log("FlapLiftPower: " + flapLiftPower + " | FinalLiftForce: " + finalLiftForce);
                //Debug.Log("Lift" + finalLiftForce);
            }
        }


        void CalcDrag()
        {
            float dragSpeed = fwdSpeed * dragFactor;
            float flapDrag = input.flaps * flapDragFactor;
            
            dragFinal = startDrag + dragSpeed + flapDrag;

            rb.drag = dragFinal;
            rb.angularDrag = startAngDrag * fwdSpeed;
        }


        void HandleRB()
        {
            if (rb.velocity.magnitude > 1f)
            {
                Vector3 updateVel = Vector3.Lerp(rb.velocity, transform.forward * fwdSpeed, fwdSpeed * attackAngle * Time.deltaTime * rbSpeed);
                rb.velocity = updateVel;

                Quaternion updateRotation = Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(rb.velocity.normalized, transform.up), Time.deltaTime * rbSpeed);
                rb.MoveRotation(updateRotation);
            }
        }


        void HandlePitch()
        {
            Vector3 flatFwd = transform.forward;
            flatFwd.y = 0f;
            pitchAngle = Vector3.Angle(transform.forward, flatFwd);

            Vector3 pitchTorque = input.pitch * pitchSpeed * transform.right * torqueFactor;
            rb.AddTorque(pitchTorque);
        }


        void HandleRoll()
        {
            Vector3 flatRight = transform.right;
            flatRight.y = 0f;
            rollAngle = Vector3.Angle(transform.right, flatRight);

            Vector3 rollTorque = -input.roll * rollSpeed * transform.forward * torqueFactor * 1.5f;
            rb.AddTorque(rollTorque);
        }


        void HandleYaw()
        {
            Vector3 yawTorque = input.yaw * yawSpeed * transform.up * torqueFactor;
            rb.AddTorque(yawTorque);
        }


        void HandleBank()
        {
            float sideBank = Mathf.InverseLerp(-90f, 90f, rollAngle * 1.25f);
            float bank = Mathf.Lerp(-1f, 1f, sideBank);

            Vector3 bankTorque = bank * rollSpeed * transform.up;
            rb.AddTorque(bankTorque);
        }
    }
}
