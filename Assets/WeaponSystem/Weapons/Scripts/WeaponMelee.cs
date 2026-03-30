using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : WeaponBase
{
    [SerializeField] Transform meleePoint;
    [SerializeField] float forwardRange = 1f;
    [SerializeField] float horizontalRange = 1f;
    [SerializeField] float verticalRange = 1f;

    [SerializeField] float meleeDamage = 0.25f;  // raro
    [SerializeField] float meleeCadence = 1f;

    void Start()
    {
        meleePoint = meleePoint ? meleePoint : transform;
    }

    public override void Swing()
    {
        if (isUsable)
        {
            isUsable = false;
            Invoke(nameof(SwingEnd), 1f / meleeCadence);

            Vector3 halfExtents = new Vector3(horizontalRange / 2f, verticalRange / 2f, forwardRange / 2f);

            Collider[] colliders = Physics.OverlapSphere(meleePoint.position, 0.5f, targetLayers);
            foreach (Collider c in colliders)
            {
                TargetWithLifeThatNotifies targetWithLifeThatNotifies = c.GetComponent<TargetWithLifeThatNotifies>();
                targetWithLifeThatNotifies?.NotifySwing(meleeDamage);
            }

        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(meleePoint.position, 0.5f);
    }

    void SwingEnd()
    {
        isUsable = true;
    }

    public override bool IsMelee()
    {
        return true;
    }
}