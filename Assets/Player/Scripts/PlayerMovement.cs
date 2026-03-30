using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [SerializeField] LayerMask layermaskAimingDetection;
    [SerializeField] float jumpForce = 3f;

    [Header("First Person Rotation")]
    [SerializeField] bool rotateWithMouse = false;
    [SerializeField] float mouseSensitivityX = 0.005f;

    CharacterController characterController;
    Animator animator;
    PlayerShooting currentWeaponAnim;

    float oldMousePositionX;

    public float amount;

    private Vector3 velocity;
    private float posY;

    private bool stairUp;

    // new run stuf and stamina
    public float curStamina = 100;
    public float maxStamina = 100;
    public float lostStamina = 0.1f;
    public float moveSpeed = 3f;
    public float extraSpeed = 0.5f;
    public float limitSpeed = 7.0f;
    private bool decreaseSpeed = false;
    public float normalizedSpeed = 4f;
    public float staminaAddFactor = 0.1f;

    public GameObject camara;

    private void Awake()
    {
        instance = this;

        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        currentWeaponAnim = GetComponent<PlayerShooting>();

        oldMousePositionX = Input.mousePosition.x;

    }

    float speedY = 0f;
    private float gravity = -9.8f;

    private bool increaseCrouch;
    public GameObject LadderUp;
    //https://www.youtube.com/watch?v=oWlts9YGojY

    public float upMovement = 1f;

    private void Update()
    {
        UpdateMovement();
        UpdateOrientation();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryJump();
        }
        else if (characterController.isGrounded && !stairUp)  // esto tiene que cambiarse para otras alturas  creo que tiene que usarse el is grounded
        {
            velocity.y = 0;
        }
        else if(!stairUp)
            velocity.y += gravity * Time.deltaTime;  // llega hasta 0 pero con rebotes raro por eso el final arriba

       if(!stairUp)
            characterController.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift))
            Run();

        else if (!Input.GetKey(KeyCode.LeftShift))
            UnRun();


        if (decreaseSpeed)
            NormalizeSpeed();


        if (Input.GetKey(KeyCode.LeftAlt))
        {
            float posY = camara.transform.position.y;
            posY -= 0.05f;

            if (camara.transform.localPosition.y <= 0.75f && camara.transform.localPosition.y > 0f)
                camara.transform.position = new Vector3(camara.transform.position.x, posY, camara.transform.position.z);
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            increaseCrouch = true;
        }

        if (increaseCrouch)
            NormalizeCrouch();

        if (stairUp)
        {
            if (Input.GetKey(KeyCode.W))
            {
                posY = camara.transform.position.y;
                posY += upMovement /** Time.deltaTime*/;

                //  if (camara.transform.localPosition.y <= 0.75f && camara.transform.localPosition.y > 0f)
            }
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
            // else if (Input.GetKey(KeyCode.S))
            // {
            //     float posY = camara.transform.position.y;
            //     posY -= 0.01f;

            //     //  if (camara.transform.localPosition.y <= 0.75f && camara.transform.localPosition.y > 0f)
            //     transform.position = new Vector3(camara.transform.position.x, posY, camara.transform.position.z);

            // }
        }
        

    }

    Vector3 movementFromInput;
    Vector3 movementFromCamera;

    void TryJump()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, amount))
        {
            //            Debug.Log("Jumping");
            velocity.y = jumpForce;
        }
        // else   // esto sirve para hacer que algun lugar sea rebotable
        // {
        //     velocity.y += gravity * Time.deltaTime;
        // }
    }

    void Run()
    {

        if (curStamina >= 0.01f) // convertir en min value for running, luego
        {
            //Debug.Log("running");
            decreaseSpeed = false;

            if (curStamina <= 0)
            {
                UnRun();
                return;
            }

            if (moveSpeed < limitSpeed)
            {
                currentWeaponAnim.currentWeapon.animator.SetBool("RunningFast", true);
                moveSpeed += extraSpeed;
            }
            else
                moveSpeed = limitSpeed;


            curStamina -= lostStamina;
            GameUI.instance.UpdateStaminaBar(curStamina, maxStamina);
        }
        else
            NormalizeSpeed();
    }

    void UnRun()
    {
        if (curStamina <= maxStamina)
        {
            currentWeaponAnim.currentWeapon.animator.SetBool("RunningFast", false);
            UpdateStamina();
        }
        else
            return;

        if (moveSpeed != normalizedSpeed)
        {
            decreaseSpeed = true;
        }
    }

    void UpdateStamina()
    {
        curStamina += lostStamina * staminaAddFactor;
        GameUI.instance.UpdateStaminaBar(curStamina, maxStamina);
    }

    void NormalizeSpeed()
    {
        if (moveSpeed > normalizedSpeed)
        {
            moveSpeed -= extraSpeed;
            currentWeaponAnim.currentWeapon.animator.SetBool("RunningFast", false);
        }
        else if (moveSpeed <= normalizedSpeed)
        {
            moveSpeed = normalizedSpeed;
            decreaseSpeed = false;
        }
    }

    void NormalizeCrouch()
    {
        float posY = camara.transform.position.y;
        posY += 0.05f;

        if (camara.transform.localPosition.y < 0.75f)
            camara.transform.position = new Vector3(camara.transform.position.x, posY, camara.transform.position.z);
        else
            increaseCrouch = false;
    }



    private void UpdateMovement()
    {
        movementFromInput = Vector3.zero;
        if (!stairUp)
        {
            if (Input.GetKey(KeyCode.W))
            {
                currentWeaponAnim.currentWeapon.animator.SetBool("TimeToWalk", true);
                movementFromInput += Vector3.forward;
            }

            if (Input.GetKey(KeyCode.S))
            {
                currentWeaponAnim.currentWeapon.animator.SetBool("TimeToWalk", true);
                movementFromInput += Vector3.back;
            }
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.W))
            {
                currentWeaponAnim.currentWeapon.animator.SetTrigger("TimeToIdle");
                currentWeaponAnim.currentWeapon.animator.SetBool("TimeToWalk", false);
            }
        }

        if (Input.GetKey(KeyCode.A)) { movementFromInput += Vector3.left; }


        if (Input.GetKey(KeyCode.D)) { movementFromInput += Vector3.right; }


        //  Debug.DrawRay(transform.position, movementFromCamera, Color.red, 0.1f);
        movementFromCamera = Camera.main.transform.TransformDirection(movementFromInput);
        // Debug.DrawRay(transform.position, movementFromCamera, Color.blue, 0.1f);
        movementFromCamera = Vector3.ProjectOnPlane(movementFromCamera, Vector3.up);
        movementFromCamera.Normalize();

        speedY += gravity * Time.deltaTime;
        movementFromCamera.y = speedY;
        characterController.Move(movementFromCamera * moveSpeed * Time.deltaTime);

        if (characterController.isGrounded) { speedY = 0f; }

    }


    [SerializeField] bool orientateToCamera;

    private void UpdateOrientation()
    {
        if (rotateWithMouse)
            UpdateOrientationWithMouse();

        else if (orientateToCamera)
            UpdateOrientationToCamera();

        else
            UpdateOrientateToMouse();
    }

    private void UpdateOrientateToMouse()
    {
        Vector3 desiredForward = Vector3.zero;

        // Asumimos orientaci�n hacia cursor
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermaskAimingDetection))
        {
            desiredForward = hit.point - transform.position;
            PerformOrientation(desiredForward);
        }
    }

    private void UpdateOrientationToCamera()
    {
        Vector3 desiredForward = Vector3.zero;


        if (movementFromInput.sqrMagnitude > (0.01f * 0.01f))
        {
            desiredForward = Camera.main.transform.forward;
            PerformOrientation(desiredForward);
        }
    }

    private void UpdateOrientationWithMouse() // repasar esta que es la que se usa
    {
        float mouseDelta = Input.GetAxis("Mouse X");
        float mouseSpeed = (mouseDelta / Screen.width) / Time.deltaTime;

        Quaternion rotationToApply = Quaternion.AngleAxis(mouseSpeed * mouseSensitivityX, Vector3.up);
        transform.rotation = rotationToApply * transform.rotation;

        oldMousePositionX = Input.mousePosition.x;

    }

    void PerformOrientation(Vector3 desiredForward) // este codigo hay que estudiarlo
    {
        desiredForward = Vector3.ProjectOnPlane(desiredForward, Vector3.up);
        desiredForward.Normalize();

        Quaternion desiredRotation = Quaternion.LookRotation(desiredForward, Vector3.up);
        Quaternion currentRotation = transform.rotation;
        transform.rotation = Quaternion.Lerp(currentRotation, desiredRotation, 0.04f);
    }

    public Vector3 MovementInput()
    {
        return movementFromInput;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stair"))
        {
            Debug.Log("Should happen");
            stairUp = true;
        }
        // else
        //     stairUp = false;
    }

    void OnTriggerExit(Collider other)
    {
        // if (other.CompareTag("Stair"))
        // {
            stairUp = false;
       // }
    }

    // IEnumerator UpLadder()
    // {
    //     stairUp = true;
    //     LadderUp.GetComponent<Animator>().Play("LadderUp");
    //     yield return new WaitForSeconds(0.05f);
    //     yield return new WaitForSeconds(5f);
    //     LadderUp.GetComponent<Animator>().Play("New State");
    //     stairUp = false;
    // }
}


/*
A quaternion can represent a 3D rotation and is defined by 4 real numbers. x, y and z represent a vector. 
w is a scalar that stores the rotation around the vector. 
https://scriptinghelpers.org/blog/how-to-think-about-quaternions
https://eater.net/quaternions
*/
