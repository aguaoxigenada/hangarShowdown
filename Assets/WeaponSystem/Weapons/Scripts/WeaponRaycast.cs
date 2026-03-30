using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponRaycast : WeaponBase // heredaba de weaponmelee por si acaso.
{
    [Header("Weapon info")]
    [SerializeField] Transform shootPoint;
    [SerializeField] Transform rifleMuzzle;
    [SerializeField] float scatterAngle = 0;
    [SerializeField] float shotCadence = 5f;
    [SerializeField] float projectilesPerShot = 1f;
    [FormerlySerializedAs("canShoot")]
    [SerializeField] bool canShootOnce;
    [SerializeField] bool canShootContinuosly;

    [Header("Debug")]
    [SerializeField] bool debugShoot;

    [Header("Sounds")]
    [SerializeField] AudioSource shotSound;
    [SerializeField] AudioSource reloadSound;
    [SerializeField] AudioSource outOfAmmoSound;

    bool isShootingContinuosly;

    NoiseMaker noiseMaker;

    public GameObject weaponMuzzle;

    Vector3 movementOfInput;

    private void OnValidate()  // afectara a lo que se toque en el inspector
    {
        if (debugShoot)
        {
            Shot();
            debugShoot = false;
        }
    }

    void Awake()
    {
        noiseMaker = GetComponentInChildren<NoiseMaker>();
    }

    void Start()
    {
        if (thisIsPlayerInWeaponBase)
        {
            GameUI.instance.isKnife = false;
            GameUI.instance.AmmoText(currentAmmo, maxAmmo);
            GameUI.instance.AmmoInMagazine(ammoInCurrentMagazine);
        }
    }

    public override WeaponUseType GetUseType()
    {
        return WeaponUseType.Shot;
    }

    float timeForNextShot = 0f;
    private void Update()
    {
        timeForNextShot -= Time.deltaTime;
        timeForNextShot = timeForNextShot > 0f ? timeForNextShot : 0f;
        if (isShootingContinuosly)
        {
            InternalShot();
        }
    }

    public override void Shot()
    {
        if (canShootOnce)
        {
            InternalShot();
        }
    }

    protected void InternalShot()
    {
        if (!isReloading && timeForNextShot <= 0f)
        {
            timeForNextShot += 1 / shotCadence;

            shotSound?.Play();  //TODO: Agregarlo al final del juego!

            noiseMaker?.MakeNoise();

            RaycastHit hit;

            if (UseAmmo() == UseAmmoResult.ShotMade)
            {
                for (int i = 0; i < projectilesPerShot; i++)
                {
                    var playerMuzzleEffect = Instantiate(weaponMuzzle, rifleMuzzle.position, transform.rotation);
                    playerMuzzleEffect.transform.parent = gameObject.transform;

                    float horizontalScatterAngle = Random.Range(-scatterAngle, scatterAngle);
                    Quaternion horizontalScatter = Quaternion.AngleAxis(horizontalScatterAngle, shootPoint.up);
                    float verticalScatterAngle = Random.Range(-scatterAngle, scatterAngle);
                    Quaternion verticalScatter = Quaternion.AngleAxis(verticalScatterAngle, shootPoint.right);

                    Vector3 shotForward = verticalScatter * (horizontalScatter * shootPoint.forward);  // no sabia que podia multiplicar contra un quaternion  // ver mas ejemp0lo en el futuro

                    if (Physics.Raycast(shootPoint.position, shotForward, out hit, Mathf.Infinity, targetLayers, QueryTriggerInteraction.Ignore))  /// para que ignore el trigger
                    {
                        //   Debug.Log("Le he dado a " + hit.collider, hit.collider); // cuando en debug.log le pones algo despues de la coma, en los coments al apretarlo te permselecciona el objecto en la herarquia.
                        Debug.DrawLine(shootPoint.position, hit.point, Color.cyan, 10f);
                        Debug.DrawRay(hit.point, hit.normal, Color.red, 10f);

                        TargetBase targetBase = hit.collider.GetComponent<TargetBase>();
                        targetBase?.NotifyShot(CalcDamage(targetBase.transform.position));
                    }
                }
            }
        }
    }

    public override void StartShooting()
    {
        isShootingContinuosly = canShootContinuosly;
    }

    public override void StopShooting()
    {
        isShootingContinuosly = false;
    }
}

/*
Es importante volver a mesh colider como convexos porque los rigidbodys no funcionan bien.
https://www.youtube.com/watch?v=glx4qolDnHg
*/