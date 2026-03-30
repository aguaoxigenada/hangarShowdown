using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFirstPersonCamera : MonoBehaviour
{
    [SerializeField][Range(0f, 180f)] float maxLookAngle = 60f;
    [SerializeField] float mouseSensitivityY = 0.005f;

    public Texture2D cursorArrow;  // mover esto a game manager
    public Texture2D cursorNone;

    PlayerShooting currentWeapon;

    void Awake()
    {
        currentWeapon = GetComponentInParent<PlayerShooting>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
    }

    void Update() // repasar toda esta parte
    {

        if (!currentWeapon.CurrentWeapon()) // parece que no es grave
            ChangeCursor("cursorArrow");
        else
            ChangeCursor("cursorNone");

        float mouseDelta = Input.GetAxis("Mouse Y");
        if (mouseDelta != 0f)  // quiere decir que solo ocurre si esta en movimiento el mouse.
        {
            float mouseSpeed = (mouseDelta / Screen.height) / Time.deltaTime;

            Vector3 forwardOnPlane = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            float angle = Vector3.SignedAngle(forwardOnPlane, transform.forward, transform.right);
            float angleToApply = mouseSpeed * mouseSensitivityY;
            // Debug.Log(angle);

            if ((angle + angleToApply) > maxLookAngle)
                angleToApply += (maxLookAngle - (angle + angleToApply));

            else if ((angle + angleToApply) < -maxLookAngle)
                angleToApply += (-maxLookAngle - (angle + angleToApply));

            Quaternion rotationToApply = Quaternion.AngleAxis(angleToApply, transform.right);
            transform.rotation = rotationToApply * transform.rotation;
        }
    }

    void ChangeCursor(string weaponType)  // tiene que ser con un unity event?  no me gusta pero buebno
    {
        //Debug.Log(currentWeapon.CurrentWeapon());
        if (weaponType == "cursorArrow")
        {
            Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
        }
        else if (weaponType == "cursorNone")
        {
            GameUI.instance.isKnife = true;
            GameUI.instance.AmmoText(0, 0);
            GameUI.instance.AmmoInMagazine(0);
            Cursor.SetCursor(cursorNone, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}
