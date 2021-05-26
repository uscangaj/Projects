using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FlightSimulator
{
    [RequireComponent(typeof(CharacteristicsController))]
    public class AirplaneController : RigidbodyController
    {
        public InputController inp;
        public CharacteristicsController charac;
        public float weight = 850f;

        public Transform centerOfMass;
        public List<EngineController> engines = new List<EngineController>();
        public List<WheelController> wheels = new List<WheelController>();


        public List<WingsNShitController> wings = new List<WingsNShitController>();



        public void Start()
        {
            base.Start();


            if (wheels.Count > 0)
            {
                foreach (WheelController wheel in wheels)
                {
                    wheel.InitWheel();
                }
            }


            charac = GetComponent<CharacteristicsController>();
            if (charac)
            {
                charac.InitCharacteristics(rb, inp);
            }
        }



        protected override void HandlePhysics()
        {
            if (inp)
            {
                HandleEngines();
                HandleCharacteristics();
                HandleWingStuff();
                HandleWheels();
                HandAltitude();
            }
        }



        void HandleEngines()
        {
            if (engines.Count > 0)
            {
                foreach (EngineController engine in engines)
                {
                    rb.AddForce(engine.CalculateForce(inp.throttle));
                }
            }
        }


        void HandleCharacteristics()
        {
            charac.UpdateCharacteristics();
        }


        void HandleWheels()
        {
            if (wheels.Count > 0)
            {
                foreach(WheelController wheel in wheels)
                {
                    wheel.HandleWheel(inp);
                }
            }
        }


        void HandAltitude()
        {

        }


        void HandleWingStuff()
        {
            if (wings.Count > 0)
            {
                foreach(WingsNShitController wing in wings)
                {
                    wing.HandleStuff(inp);
                }
            }
        }
    }
}