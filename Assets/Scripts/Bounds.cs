using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "CameraController")
        {
            if (other.GetComponent<CameraController>() != null)
            {
                CameraController camera = other.GetComponent<CameraController>();
                camera.canFollow = false;
            }
        }
    }
}
