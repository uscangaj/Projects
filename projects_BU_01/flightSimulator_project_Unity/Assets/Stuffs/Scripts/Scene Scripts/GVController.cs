using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GVController : MonoBehaviour
{
    public Transform player;



    void Update()
    {
        Vector3 lookAtObject = player.position - transform.position;
        lookAtObject = player.position - transform.position;
        transform.forward = lookAtObject.normalized;
    }
}
