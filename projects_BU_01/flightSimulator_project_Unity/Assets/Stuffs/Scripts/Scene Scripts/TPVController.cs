using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FlightSimulator
{
    public class TPVController : MonoBehaviour
    {
        public float yOff = 2.5f;
        public float zOff = -10f;
        public Camera cam;
        public GameObject target;
        public Vector3 moveCam;



        // Use this for initialization
        void Start()
        {
            //cam.transform.position = target.transform.position - transform.forward * zOff + Vector3.up * yOff;
        }



        // Update is called once per frame
        void Update()
        {
            moveCam = target.transform.position - target.transform.forward * zOff + Vector3.up * yOff;
            cam.transform.position = moveCam;
            cam.transform.LookAt(target.transform.position);
        }
    }
}