using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FlightSimulator
{
    public class GroundEffectController : MonoBehaviour
    {
        private Rigidbody rb;
        public float maxDistance = 3f;



        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }


        // Update is called once per frame
        void FixedUpdate()
        {
            if (rb)
            {

            }
        }


        protected virtual void HandleGroundEffect()
        {
            RaycastHit hit;



            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                if(hit.transform.tag == "Ground" && hit.distance < maxDistance)
                {
                    Debug.Log("Hitting ground...");
                }
            }
        }
    }
}
