using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionColorice : MonoBehaviour
{
    public Color colorToChange;

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Obstacle")
        {
            ChangeColor changerOfColor = other.gameObject.GetComponent<ChangeColor>();
            changerOfColor.colorObject = colorToChange;
        }
    }
}
