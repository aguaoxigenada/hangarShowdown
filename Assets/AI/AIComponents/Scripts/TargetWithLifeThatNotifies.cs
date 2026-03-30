using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetWithLifeThatNotifies : TargetWithLife
{
    public Animator anim; // podriavolverse privada y buscara.

    public interface IDeathNotifiable  // tiene que ser publica para poder ser implementada
    {
        public void NotifyDeath();
    }

    protected override void CheckStillAlive()
    {
        base.CheckStillAlive();

        if (life <= 0f)
        {
            GetComponent<IDeathNotifiable>()?.NotifyDeath();
        }
    }

    protected override bool DestroyOnAllLifeLost()
    {
        return false; // para los enemigos no ocurre la muerte desde aca.
    }

    protected override void LoseLife(DamageType damageType, float howMuch)
    {
       // Debug.Log("Got Hit");
        anim.SetTrigger("GotHit");
        base.LoseLife(damageType, howMuch);
    }

}




