using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FlightSimulator
{
    public enum sensorType
    {
        Above,
        Below,
        Front,
        Rear,
        Left,
        Right
    }



    public class SensorController : MonoBehaviour
    {
        public float timer = 1.0f;
        public float delay = 2.0f;
        public Text sensor;
        public string message;
        public sensorType direction;



        void Start()
        {
            sensor.text = "";
            message = "";
        }

        void Update()
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                sensor.text = "";
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (direction == sensorType.Above)
                sensor.text = "Above";
            else if (direction == sensorType.Below)
                sensor.text = "Below";
            else if (direction == sensorType.Front)
                sensor.text = "Front";
            else if (direction == sensorType.Rear)
                sensor.text = "Rear";
            else if (direction == sensorType.Left)
                sensor.text = "Left";
            else if (direction == sensorType.Right)
                sensor.text = "Right";


            timer = 1.0f;
        }
    }
}