using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimation : MonoBehaviour
{
    Animator animator;
    Vector3 oldPosition = Vector3.zero;
    float smoothFactor = 20f;
    TargetWithLife target;

    private void Awake()
    {
        oldPosition = transform.position;
        animator = GetComponentInChildren<Animator>();

        target = GetComponent<TargetWithLife>();
    }

    private void OnEnable()  // se llama cada vez que se habilita el componente
    {
        target.onLifeLost.AddListener(NotifyHit);
        target.onDeath.AddListener(NotifyDeath);
    }

    private void OnDisable()
    {
        target.onLifeLost.RemoveListener(NotifyHit);
        target.onDeath.RemoveListener(NotifyDeath);
    }


    Vector3 smoothedVelocity = Vector3.zero;
    void Update()
    {
        Vector3 currentWorldVelocity = (transform.position - oldPosition) / Time.deltaTime;
        Vector3 currentLocalVelocity = transform.InverseTransformDirection(currentWorldVelocity);

        smoothedVelocity += (currentLocalVelocity - smoothedVelocity).normalized * smoothFactor * Time.deltaTime;
        if (smoothedVelocity.magnitude > 0.5f)
        {
            animator.SetFloat("ForwardVelocity", smoothedVelocity.z);
            animator.SetFloat("HorizontalVelocity", smoothedVelocity.x);
        }
        else
        {
            animator.SetFloat("ForwardVelocity", 0f);
            animator.SetFloat("HorizontalVelocity", 0f);
        }

        oldPosition = transform.position;
    }

    public void NotifyHit(TargetWithLife target, float lifeLeft)
    {
        //animator.SetTrigger("Hit");
    }

    public void NotifyDeath(TargetWithLife target, TargetWithLife.DeathInfo deathInfo)
    {
        //animator.SetTrigger("Death");
    }
}
