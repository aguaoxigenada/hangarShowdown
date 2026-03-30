using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetWithLife : TargetBase
{
    public enum DamageType
    {
        Shot,
        Swing,
        Explosion,
        Particle,
    }

    public struct DeathInfo
    {
        public DamageType type;

        public Vector3 direction;
        public Vector3 explosionPosition;


        public float explosionRadius;
    }

    [SerializeField] protected float life = 1f;
    [SerializeField] protected float maxLife = 1f;
    [SerializeField] protected float medikitLifeRecovery = 5f;
    [SerializeField] public UnityEvent<TargetWithLife, float> onLifeLost;
    [SerializeField] public UnityEvent<TargetWithLife, DeathInfo> onDeath;
    [SerializeField] private bool thisIsPlayer;


    DeathInfo deathInfo = new DeathInfo();

    public override void NotifyShot(float damage)
    {
        LoseLife(DamageType.Shot, damage);
    }

    public override void NotifySwing(float damage)
    {
        LoseLife(DamageType.Swing, damage);
    }

    public override void NotifyExplosion(float damage)
    {
        LoseLife(DamageType.Explosion, damage);
    }

    public override void NotifyParticle(float damage)
    {
        LoseLife(DamageType.Particle, damage);
    }

    protected virtual void LoseLife(DamageType damageType, float howMuch)
    {
        deathInfo.type = damageType;
        switch (deathInfo.type)
        {
            case DamageType.Shot:
            case DamageType.Swing:
            case DamageType.Particle:
                deathInfo.direction = transform.position - PlayerMovement.instance.transform.position;
                break;
            case DamageType.Explosion:
                deathInfo.explosionPosition = Explosion.lastExplosionPosition; // puede chapar este valor dado que es static
                deathInfo.explosionRadius = Explosion.lastExplosionRadius;
                break;

        }
        life -= howMuch;

        if (thisIsPlayer)
        {
            GameUI.instance.UpdateHealthBar(life, maxLife);
            GameUI.instance.CamPulse();
        }

        onLifeLost.Invoke(this, life);
        CheckStillAlive();
    }

    protected virtual void CheckStillAlive()
    {
        if (life <= 0f)
        {
            if (DestroyOnAllLifeLost())  // se podria agregar una animacion de muerte si hay tiempo.
            {
                GameManager.instance.CheckIfWon(false);
                gameObject.SetActive(false);
            }
            onDeath.Invoke(this, deathInfo);  // se llama todo el tiempo al event listener de entity animation
        }
    }

    protected virtual bool DestroyOnAllLifeLost()  // no sirve de mucho solo para el override
    {
        return true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Medikit"))
        {
            // TODO: disallow recovering more
            // life than the orifinal life value
            life += medikitLifeRecovery;
            life = Mathf.Clamp(life, 1, maxLife);

            if (thisIsPlayer)
                GameUI.instance.UpdateHealthBar(life, maxLife);

            Destroy(other.gameObject);
        }
    }
}
