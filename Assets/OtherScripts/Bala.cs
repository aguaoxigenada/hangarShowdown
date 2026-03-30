using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float timeToDestroy;

    void Start()
    {
        transform.Rotate(90, 0, 0, Space.Self);
        Destroy(gameObject, timeToDestroy);
    }
}
