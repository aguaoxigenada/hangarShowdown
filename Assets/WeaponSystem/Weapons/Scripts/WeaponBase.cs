using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("Layer Info")]
    [SerializeField] protected LayerMask targetLayers = Physics.DefaultRaycastLayers;

    [Header("Min Range delivers max Range")]
    [SerializeField] float minRange = 1f;
    [SerializeField] float maxDamage = 1f;

    [Header("Max Range delivers min Range")]
    [SerializeField] float maxRange = 25f;
    [SerializeField] float minDamage = 0.25f;

    protected bool isUsable = true;
    public Animator animator;
    public bool thisIsPlayerInWeaponBase;

    public enum WeaponUseType
    {
        Swing,
        Shot,
        ContinuosShoot,
        Undefined
    }

    public virtual WeaponUseType GetUseType() { return WeaponUseType.Undefined; }

    public virtual bool IsMelee()
    {
        return false;
    }

    public virtual void Swing()
    {

    }

    public virtual void Shot()
    {
        //  Debug.Log("Shot called in WeaponBase");
    }

    public virtual void StartShooting()
    {

    }

    public virtual void StopShooting()
    {

    }

    protected float CalcDamage(Vector3 hitPosition)
    {
        return CalcDamage(Vector3.Distance(transform.position, hitPosition));  // este llama al otro CalcDamage
    }

    protected float CalcDamage(float distance)
    {
        if (distance < minRange) { return maxDamage; }
        if (distance > maxRange) { return 0f; }
        return Mathf.Lerp(maxDamage, minDamage, (distance - minRange) / (maxRange - minRange));  // ver desmos
        // da un valor entre maxDamage y minDamage //  
    }

    public float GetMaxRange() { return maxRange; }
    public float GetMinRange() { return minRange; }


    [Header("Ammo and Magazine, reload")]
    [SerializeField] public int maxAmmo = 100;
    [SerializeField] public int currentAmmo = 24;
    [SerializeField] public int ammoInCurrentMagazine = 12;
    [SerializeField] int magazineCapacity = 12;
    [SerializeField] float reloadTime = 5f;
    [SerializeField] bool consumesAmmo = true;
    protected bool isReloading;

    protected enum UseAmmoResult
    {
        ShotMade,
        NeedsReload,
        NoAmmo,
    };

    protected UseAmmoResult UseAmmo()
    {
        if (currentAmmo == 0) return UseAmmoResult.NoAmmo;
        if (ammoInCurrentMagazine == 0) return UseAmmoResult.NeedsReload;

        if (consumesAmmo)
        {
            currentAmmo--;
            if (thisIsPlayerInWeaponBase)
                GameUI.instance.AmmoText(currentAmmo, maxAmmo);
        }

        ammoInCurrentMagazine--;

        if (thisIsPlayerInWeaponBase)
            GameUI.instance.AmmoInMagazine(ammoInCurrentMagazine);

        return UseAmmoResult.ShotMade;
    }

    internal void AddAmmo()
    {
        currentAmmo += magazineCapacity;
        currentAmmo = Mathf.Clamp(currentAmmo, 1, maxAmmo);

        if (thisIsPlayerInWeaponBase)
            GameUI.instance.AmmoText(currentAmmo, maxAmmo);
    }

    public bool HasAmmo() { return currentAmmo > 0; }
    public bool NeedsReload() { return HasAmmo() && (ammoInCurrentMagazine == 0); }

    public void Reload()
    {
        if (isUsable)
        {
            animator.SetBool("TimeToReload", true);
            //          Debug.Log("started reload");
            isUsable = false;
            isReloading = true;
            Invoke(nameof(ReloadAfterSeconds), reloadTime);
        }
    }

    void ReloadAfterSeconds()
    {
        animator.SetBool("TimeToReload", false);
        //        Debug.Log("finished reload");
        isReloading = false;
        isUsable = true;
        ammoInCurrentMagazine = Mathf.Min(magazineCapacity, currentAmmo); // revisar

        if (thisIsPlayerInWeaponBase)
        {
            GameUI.instance.AmmoText(currentAmmo, maxAmmo);
            GameUI.instance.AmmoInMagazine(ammoInCurrentMagazine);
        }
    }


}
