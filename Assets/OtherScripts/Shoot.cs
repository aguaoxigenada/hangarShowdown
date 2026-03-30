using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    
    public Transform bulletReference;
    public GameObject bulletPrefab;
    public float bulletVelocity;
    public float timeToDestroy;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            GameObject bulletTemporal = Instantiate(bulletPrefab, bulletReference.transform.position, bulletReference.transform.rotation) as GameObject;
            // Rigidbody rig = bulletTemporal.GetComponent<Rigidbody>();
            Rigidbody rig = bulletTemporal.AddComponent<Rigidbody>();

            rig.useGravity = false;

            rig.AddForce(bulletTemporal.transform.forward * bulletVelocity);
            Destroy(bulletTemporal, timeToDestroy);


        }        
    }
}
