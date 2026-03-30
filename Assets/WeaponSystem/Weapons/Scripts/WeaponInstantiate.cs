using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponInstantiate : WeaponBase
{
    public Transform bulletReference;
    public GameObject bulletPrefab;
    private GameObject instance;

    public float bulletSpeed;
    private float rotX;
    public float lookSensitivity = 8f;

    public float minLookX = -80f;
    public float maxLookX = 80f;

    void Update()
    {
        CamLook();

        if (Input.GetButtonDown("Fire1"))
        {
            Shot();
        }
    }

    public override void Shot()
    {
        instance = Instantiate(bulletPrefab, bulletReference.transform.position, bulletPrefab.transform.rotation);

        instance.transform.position = bulletReference.position;
        instance.transform.rotation = transform.localRotation = Quaternion.Euler(-rotX, 0, 0);
        instance.GetComponent<Rigidbody>().velocity = bulletReference.forward * bulletSpeed;
    }

    void CamLook()
    {
        //float y = Input.GetAxis("Mouse X") * lookSensitivity;
        rotX += Input.GetAxis("Mouse Y") * lookSensitivity;
        //transform.eulerAngles += Vector3.up * y;  // Movimiento de camara de izq a der   pa despues

        rotX = Mathf.Clamp(rotX, minLookX, maxLookX);
    }

    public override void Swing()
    {
        base.Swing();
    }

    public override void StartShooting()
    {
        base.StartShooting();
    }

    public override void StopShooting()
    {
        base.StopShooting();
    }

}













// targetbase para todo lo que sea recibir da'p
// weapon bse disparar cosas.
