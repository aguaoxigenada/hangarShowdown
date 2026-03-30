using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    private bool timeToZoomOut;
    private bool timeToZoomIn;
    private bool timeToStart = true;

    public Transform zoomOutEnd;
    public Transform zoomInEnd;
    public float moveSpeed = 3f;

    void Update()
    {
        if (timeToStart)
        {
            transform.position = Vector3.MoveTowards(transform.position, zoomOutEnd.position, moveSpeed * Time.deltaTime);
            Invoke(nameof(TimeToCloseStart), 8f);
        }

        if (timeToZoomOut)
        {
            transform.position = Vector3.MoveTowards(transform.position, zoomOutEnd.position, moveSpeed * Time.deltaTime);
            Invoke(nameof(TimeToZoomIn), 12f);
        }

        if (timeToZoomIn)
        {
            transform.position = Vector3.MoveTowards(transform.position, zoomInEnd.position, moveSpeed * Time.deltaTime);
            Invoke(nameof(TimeToZoomOut), 12f);
        }

    }

    void TimeToCloseStart()
    {
        timeToStart = false;
        Invoke(nameof(TimeToZoomIn), 12f);
    }

    void TimeToZoomIn()
    {
        timeToZoomIn = true;
        timeToZoomOut = false;
    }

    void TimeToZoomOut()
    {
        timeToZoomIn = false;
        timeToZoomOut = true;
    }
}
