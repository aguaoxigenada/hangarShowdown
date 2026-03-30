using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraTarget : MonoBehaviour
{
    public Vector3 distance;
    public Vector3 rotationTwo;

    public Camera cam;

    public GameObject target;

    private float rotX;
    
    private Vector3 velocity = Vector3.zero;
    // private float smoothTime = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.LookAt(target.transform.position);  // solo esto es residen Evil!
        cam.transform.position = target.transform.position + distance;
       
        //Vector3  targetPosition = target.transform.position + distance;
       // transform.position = Vector3.SmoothDamp(transform.position, target.transform.position + distance, ref velocity, smoothTime);  // le da un toque interesanton
        //cam.transform.rotation = target.transform.rotation + rotationTwo;

//        float y = Input.GetAxis("Vertical") * 5; // Mouse X / Y
  //      rotX += Input.GetAxis("Horizontal") * 0.1f;


       // cam.transform.localRotation = Quaternion.Euler(0, rotX, 0); // Rotacion de la camara de arriba a abajo.  // aprender Quaternions
        //transform.eulerAngles += Vector3.up * y;  // Movimiento de camara de izq a der
        
    }
}
