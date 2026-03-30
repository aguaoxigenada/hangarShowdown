using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float horizontalMove;
    public float verticalMove;

    public float playerSpeed;

    private Vector3 playerInput;

    private CharacterController player;

    public Camera cam;

    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 movePlayer;

    public float gravity = 9.8f;
    public float fallVelocity;

    public float jumpForce;

    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        // Debug.Log("Vector NOT clamped " + playerInput);

        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        //   playerInput = Vector3.ClampMagnitude(new Vector3(1, 0, 1), 1);

       //  Debug.Log("Vector clamp " + playerInput);
        // para que poner esto..si el maximo del getAxis es 1 

        CameraDirection();
      //  Debug.Log("camRight " + camRight);
      //  Debug.Log("camLeft " + camForward);
        movePlayer = playerInput.x * camRight + playerInput.z * camForward;  // te da la posiocion hacia donde debe mirar el personaje
        movePlayer = movePlayer * playerSpeed;
       // Debug.Log("move player " + movePlayer);

        player.transform.LookAt(player.transform.position + movePlayer);

        setGravity();
        playerSkills();
       
       // player.Move(playerInput * Time.deltaTime/* * playerSpeed*/);
       player.Move(movePlayer * Time.deltaTime);
    }

    void CameraDirection()
    {
        camForward = cam.transform.forward;   // Aca se normaliza. 
        camRight = cam.transform.right;       // Aca se normaliza.

        camForward.y = 0;
        camRight.y = 0;

        //  Debug.Log("Cam Forward " + camForward);
        //  Debug.Log("Cam Right " + camRight);

        camForward = camForward.normalized;   // para que lo normaliza de nuevo si arriba ya se normaliza?
        camRight = camRight.normalized;

        //  Debug.Log("Cam Forward Normalized " + camForward);
        //  Debug.Log("Cam Right Normalized" + camRight);
    }

    void setGravity()
    {
        if (player.isGrounded)
        {
            Debug.Log("it is grounded");
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        else
        {
            Debug.Log("this happenerd");
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }

    }
    void playerSkills()
    {
        if (player.isGrounded && Input.GetButton("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
        }
    }
}
