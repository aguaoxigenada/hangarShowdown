using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollController : MonoBehaviour
{
    [SerializeField] float force = 10f;
    [SerializeField] float explosionForce = 2000f;
    [SerializeField] float explosionUpforce = 100f;

    Collider[] colliders;
    Rigidbody[] rigidbodies;
    TargetWithLife targetWithLife;

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        targetWithLife = GetComponentInParent<TargetWithLife>();
    }

    void OnEnable()
    {
        foreach (Collider c in colliders) { c.enabled = false; }
        foreach (Rigidbody rb in rigidbodies) { rb.isKinematic = true; }

        targetWithLife.onDeath.AddListener(NotifyDeath);
    }

    void OnDisable()
    {
        targetWithLife.onDeath.RemoveListener(NotifyDeath);
    }

    void NotifyDeath(TargetWithLife target, TargetWithLife.DeathInfo deathInfo)
    {

        foreach (Collider c in colliders) { c.enabled = true; }
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
            if (deathInfo.type == TargetWithLife.DamageType.Explosion)
            {
                rb.AddExplosionForce(explosionForce, deathInfo.explosionPosition, deathInfo.explosionRadius, explosionUpforce);
            }
            else
                { rb.AddForce(deathInfo.direction * force, ForceMode.Impulse); }  // esto parece que nunca lo usa porque no hay un deathInfo.direction por defecto
        }

        // Info???

        // 1) Aplicar una fuerza por explosion a todos
        //    los rigidbodies



        // 2) aplicar una fuerza por empuje al rigidbody
        //    que habria recibido el impacto
    }

}
