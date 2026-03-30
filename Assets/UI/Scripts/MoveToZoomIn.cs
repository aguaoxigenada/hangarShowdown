using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToZoomIn : MonoBehaviour
{
    public RectTransform zoomInEnd;
    public float moveSpeed = 3f;

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, zoomInEnd.position, moveSpeed);
    }

}
