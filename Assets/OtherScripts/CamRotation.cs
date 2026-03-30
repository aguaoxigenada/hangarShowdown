using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotation : MonoBehaviour
{
    float changeY;
    public Camera cam;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        { 
            // no parece necesario
            changeY = cam.transform.rotation.y;
            transform.eulerAngles += Vector3.up * changeY;
            Debug.Log(Vector3.up * changeY);
        } // modificar el eje x de camara

        if (Input.GetKey(KeyCode.E))
        {
            changeY = cam.transform.rotation.y;
            transform.eulerAngles += Vector3.up * changeY;
            Debug.Log(Vector3.up * changeY);
        }  // modificar el eje x
    }
}
