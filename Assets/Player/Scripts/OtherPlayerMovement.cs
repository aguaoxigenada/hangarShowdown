using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class OtherPlayerMovement : MonoBehaviour
{
    //arriba de public class..

    //[SerializeField] private float moveSpeed = 4f;
    [SerializeField] float speed = 4f;
    CharacterController characterController;
    Animator animator;
    Vector3 movementFromCamera;
    private float speedY = 0f;
    private float gravity = -9.8f;


    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 movementFromInput = Vector3.zero;
        if (Input.GetKey(KeyCode.A)) { movementFromInput += Vector3.left; }
        if (Input.GetKey(KeyCode.W)) { movementFromInput += Vector3.forward; }
        if (Input.GetKey(KeyCode.S)) { movementFromInput += Vector3.back; }
        if (Input.GetKey(KeyCode.D)) { movementFromInput += Vector3.right; }


        movementFromCamera = Camera.main.transform.TransformDirection(movementFromInput);   // va a ir hacia donde apunta la camara :D hasta aca
                                                                                            // Debug.Log("movement from camera " + movementFromCamera);

        //  Debug.DrawRay(transform.position, movementFromCamera, Color.red, 0.1f);
        movementFromCamera = Vector3.ProjectOnPlane(movementFromCamera, Vector3.up);  // sin esto el coment de arriba no QUEDA //   es global la direction
                                                                                      // Debug.DrawRay(transform.position, movementFromCamera, Color.blue, 0.1f);
        movementFromCamera.Normalize();

        speedY += gravity * Time.deltaTime;
        movementFromCamera.y = speedY;

        characterController.Move(movementFromCamera * speed * Time.deltaTime);

        if (characterController.isGrounded) { speedY = 0f; }

        Vector3 localMovement = transform.InverseTransformDirection(movementFromCamera);
        animator.SetFloat("ForwardVelocity", localMovement.z);
        animator.SetFloat("HorizontalVelocity", localMovement.x);

        Vector3 desiredForward = Vector3.zero;

        if (movementFromInput.sqrMagnitude > (0.01f * 0.01f)) // para saber que el jugador se esta moviendo sino no se mueve.
        {
            desiredForward = Camera.main.transform.forward;
            desiredForward = Vector3.ProjectOnPlane(desiredForward, Vector3.up);

            desiredForward = Vector3.ProjectOnPlane(desiredForward, Vector3.up);
            desiredForward.Normalize();

            Quaternion desiredRotation = Quaternion.LookRotation(desiredForward, Vector3.up);
            Quaternion currentRotation = transform.rotation;
            transform.rotation = Quaternion.Lerp(currentRotation, desiredRotation, 0.04f);
        }
    }
}
