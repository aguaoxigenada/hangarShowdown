using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderUp : MonoBehaviour
{
    private bool stairUp;
    private Vector3 velocity;

    void Update()
    {
        if (stairUp)
        {
            if (Input.GetKey(KeyCode.W))
            {
                velocity.y = 2f;
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                velocity.y = 0f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                velocity.y = -2f;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                velocity.y = 0f;
            }
        }
        else
        {
            transform.localPosition = new Vector3(0, 0, 0);
        }

        float posY = transform.localPosition.y;
        posY += velocity.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("happened");
            stairUp = true;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("happenedfffdsfas");
        stairUp = false;
    }
}
