using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace FlightSimulator
{
    public class GameManager : MonoBehaviour
    {
        public bool isPaused;
        public bool gameOver;

        public Text textGO;

        public GameObject menu;
        public AudioSource sound;
        public AudioClip noFuel;
        public AudioClip addFuel;

        public float fuel = 100f;
        public bool specialEvent = false;
        public Text fuelGauge;

        public CharacteristicsController cc;
        public Text speed;
        public Text lift;
        public Text drag;



        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = 1.0f;
            textGO.text = "";

            isPaused = false;
            gameOver = false;

            menu.SetActive(false);

            specialEvent = false;
            fuelGauge.text = "Fuel:  " + fuel.ToString() + " %";

            speed.text = "Speed:  " + cc.mph.ToString() + " mph";
            lift.text = "Lift  " + cc.finalLiftForce.ToString();
            drag.text = "Drag: " + cc.dragFinal.ToString("F2");
        }



        // Update is called once per frame
        void Update()
        {
            speed.text = "Speed:  " + cc.mph.ToString("#.00") + " mph";
            lift.text = "Lift  " + cc.finalLiftForce.ToString("#.00");
            drag.text = "Drag: " + cc.dragFinal.ToString("F2");

            fuel -= Time.deltaTime * 0.15f;
            fuelGauge.text = "Fuel:  " + fuel.ToString("#.00") + " %";
            if (fuel <= 1.35f && fuel > 0.5f)
            {
                if (!sound.isPlaying)
                    sound.PlayOneShot(noFuel, 0.85f);
            }
            else if (fuel <= 0f)
            {
                fuel = 0f;
                textGO.text = "Out of fuel!";
                specialEvent = true;
                gameOver = true;
            }
            else if (fuel > 100f)
            {
                fuel = 100f;
            }


            if (Input.GetKey(KeyCode.Escape))
            {
                if (isPaused == true)
                    isPaused = true;
                else
                    isPaused = false;
            }
            else if (Input.GetKeyUp(KeyCode.Minus))
            {
                sound.PlayOneShot(addFuel, 0.1f);


                fuel -= 10f;
            }
            else if (Input.GetKeyUp(KeyCode.Equals))
            {
               sound.PlayOneShot(addFuel, 0.1f);


                fuel += 10f;
            }


            if (isPaused == true)
            {
                Time.timeScale = 0.0f;
                menu.SetActive(true);

                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    isPaused = false;
                }
            }
            else
            {
                Time.timeScale = 1.0f;
                menu.SetActive(false);

                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    isPaused = true;
                }
            }


            if (gameOver == true)
            {
                //sound.PlayOneShot(KO, 0.75f);
                isPaused = true;
                endGame();
            }
        }



        public void endGame()
        {
            Debug.Log("Game Over");
            Time.timeScale = 0.0f;
            if (specialEvent == false)
                textGO.text = "You crashed!";
            menu.SetActive(true);
        }
    }
}