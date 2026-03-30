using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerShooting : MonoBehaviour
{
    WeaponBase[] availableWeapons;
    public WeaponBase currentWeapon;

    [SerializeField] int currentWeaponIndex = 0;
    float towards;

    void Awake()
    {                                                       // true hace referencia a que esten activos
        availableWeapons = GetComponentsInChildren<WeaponBase>(true); // como es clase base tiene a todos los hijos >D
                                                                      // weaponAnimator = GetComponentsInChildren<Animator>(true);
    }

    void Start()
    {
        SelectCurrentWeapon(currentWeaponIndex);
    }

    // Update is called once per frame
    void Update()
    {
        // Inicio / Final de disparo
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (currentWeapon.IsMelee())
            {
                currentWeapon.Swing();
            }

            else
            {
                currentWeapon.Shot();
                currentWeapon.StartShooting();
            }
            currentWeapon.animator.SetBool("TimeToShoot", true);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            currentWeapon.animator.SetBool("TimeToShoot", false);
            currentWeapon.StopShooting();
        }

        // Recarga
        if (Input.GetKeyDown(KeyCode.R))
            currentWeapon.Reload();

        // Usar arma al estilo Melee
        if (Input.GetKeyDown(KeyCode.Mouse1))
            currentWeapon.Swing();

        // Cambio de armma
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon.animator.SetTrigger("TimeToChangeWeapon");
            towards = 0f;
            Invoke(nameof(ChangeNextWeapon), 1f);  // time of anim

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon.animator.SetTrigger("TimeToChangeWeapon");
            towards = 1f;
            Invoke(nameof(ChangeNextWeapon), 0.8f);
        }
    }

    void ChangeNextWeapon()
    {
        if (towards == 0)
        {
            currentWeaponIndex--;

            SelectCurrentWeapon(currentWeaponIndex);
        }
        else if (towards == 1f)
        {
            currentWeaponIndex++;

            SelectCurrentWeapon(currentWeaponIndex);
        }

        if (!currentWeapon.IsMelee())
        {
            GameUI.instance.isKnife = false;
            GameUI.instance.AmmoText(currentWeapon.currentAmmo, currentWeapon.maxAmmo);
            GameUI.instance.AmmoInMagazine(currentWeapon.ammoInCurrentMagazine);
        }

        towards = 2f;
    }

    void SelectCurrentWeapon(int weaponIndex)
    {
        if (weaponIndex < 0) { currentWeaponIndex = availableWeapons.Length - 1; } // se pasa a la ultima de la lista
        else if (weaponIndex >= availableWeapons.Length) { currentWeaponIndex = 0; }

        currentWeapon?.StopShooting();
        currentWeapon = availableWeapons[currentWeaponIndex];

        foreach (WeaponBase w in availableWeapons)
        {
            w.gameObject.SetActive(w == currentWeapon);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammo"))
        {
            // TODO: disallow recovering more
            // life than the orifinal life value
            currentWeapon.AddAmmo();
            Destroy(other.gameObject);
        }
    }

    public bool CurrentWeapon()
    {
        if (currentWeapon.IsMelee())
            return true;
        else
            return false;
    }

}
