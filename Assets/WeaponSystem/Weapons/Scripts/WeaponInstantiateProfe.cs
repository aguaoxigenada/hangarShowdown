using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponInstantiateProfe : WeaponBase
{

    [SerializeField] GameObject prefabProyectil;
    [SerializeField] Transform shootPoint;
    [SerializeField] float forceToApplyOnShot = 300f;

    public override void Shot()
    {
       GameObject proyectil = Instantiate(prefabProyectil, shootPoint.transform.position, shootPoint.rotation);
       prefabProyectil.GetComponent<Rigidbody>()?.AddForce(shootPoint.forward * forceToApplyOnShot);
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
