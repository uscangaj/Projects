using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FlightSimulator
{
    public class CameraController : MonoBehaviour
    {
        public int camSel = 1;

        public GameObject plane;
        public GameObject cockpit;
        public GameObject FPVcam;
        public GameObject RVcam;
        public GameObject TPVcam;
        public GameObject GVcam;



        // Start is called before the first frame update
        void Start()
        {
            camSel = 3;

            plane.SetActive(true);
            cockpit.SetActive(false);

            FPVcam.SetActive(false);
            RVcam.SetActive(false);
            TPVcam.SetActive(true);
            GVcam.SetActive(false);
        }


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                camSel = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                camSel = 2;
            }
            else if (Input.GetKeyUp(KeyCode.Tab))
            {
                camSel = 1;
            }
            else if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                camSel = 3;
            }
            else if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                camSel = 4;
            }


            if (camSel == 1) //first person
            {
                plane.SetActive(false);
                cockpit.SetActive(true);

                FPVcam.SetActive(true);
                RVcam.SetActive(false);
                TPVcam.SetActive(false);
                GVcam.SetActive(false);
            }
            else if (camSel == 2) //rear view
            {
                plane.SetActive(true);
                cockpit.SetActive(false);

                FPVcam.SetActive(false);
                RVcam.SetActive(true);
                TPVcam.SetActive(false);
                GVcam.SetActive(false);
            }
            else if (camSel == 3) //third person
            {
                plane.SetActive(true);
                cockpit.SetActive(false);

                FPVcam.SetActive(false);
                RVcam.SetActive(false);
                TPVcam.SetActive(true);
                GVcam.SetActive(false);
            }
            else if (camSel == 4) //ground view
            {
                plane.SetActive(true);
                cockpit.SetActive(false);

                FPVcam.SetActive(false);
                RVcam.SetActive(false);
                TPVcam.SetActive(false);
                GVcam.SetActive(true);
            }
        }
    }
}
