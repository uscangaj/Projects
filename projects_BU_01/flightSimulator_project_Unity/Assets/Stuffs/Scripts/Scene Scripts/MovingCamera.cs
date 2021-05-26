using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MovingCamera : MonoBehaviour
{
    public GameObject camera;
    public float amount = 1.0f;
    public float rotate = 0.25f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0.0f, 0.0f, transform.position.z + amount);
        transform.Rotate(Vector3.right * Time.deltaTime * rotate);
    }
}
