using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    static public Vector3 lastExplosionPosition;
    static public float lastExplosionRadius;

    [SerializeField] GameObject prefabExplosionVisual;
    [SerializeField] float radius = 1f;
    [SerializeField] AudioClip explosionAudioclip;
    [SerializeField] float explosionDamage = 5f;

    void Start()
    {
        Destroy(gameObject);

        // Hack ! HACk!!!
        // Asumimos que solo el player tiene
        // un arma que genera explosiones y 
        // asumimos que en un fotograma unicamente
        // puede ocurrir una explosion
        lastExplosionPosition = transform.position;
        lastExplosionRadius = radius;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider c in colliders)
        {
            TargetBase target = c.GetComponent<TargetBase>();
            target?.NotifyExplosion(explosionDamage);
        }
        AudioSource.PlayClipAtPoint(explosionAudioclip, transform.position); // cheere leerlo
        Instantiate(prefabExplosionVisual, transform.position, Quaternion.identity);
    }
}
