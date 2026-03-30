using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("This component is obsolete. Please use WEaponRaycast with projectilesPershot > 1")]
public class WeaponRaycastMultiShot : WeaponRaycast
{
    [SerializeField] int numShots = 10;
    public override void Shot()
    {
        for (int i = 0; i < numShots; i++)
        {
            base.InternalShot();
        }
    }
}
