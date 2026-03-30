using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour
{
    Rigidbody rig;
    public float force;

    void Start()
    {
        rig = GetComponent<Rigidbody>();

    }

    void Update()
    {  
        if (Input.GetKey(KeyCode.UpArrow))
            rig.AddForce(Vector3.forward * force * Time.deltaTime, ForceMode.Impulse);
        else if (Input.GetKey(KeyCode.DownArrow))
            rig.AddForce(Vector3.forward * -force * Time.deltaTime, ForceMode.Impulse);
        else if (Input.GetKey(KeyCode.RightArrow))
            rig.AddForce(Vector3.right * force * Time.deltaTime, ForceMode.Impulse);
        else if (Input.GetKey(KeyCode.LeftArrow))
            rig.AddForce(Vector3.right * -force * Time.deltaTime, ForceMode.Impulse);
    }
}
