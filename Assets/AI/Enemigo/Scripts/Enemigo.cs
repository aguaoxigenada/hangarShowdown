using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavigateToTransform))]
[RequireComponent(typeof(NavigateRoute))]
[RequireComponent(typeof(NavigateToPosition))]
public class Enemigo : MonoBehaviour, TargetWithLifeThatNotifies.IDeathNotifiable, NoiseMaker.INoiseListener
{
    public enum BehaviourType
    {
        Cautious,
        Valiant,
        Sneaky,
        Guardian,
    };
    [SerializeField] BehaviourType behaviourType = BehaviourType.Valiant;

    NavigateToTransform navigateToTransform;
    NavigateRoute navigateRoute;
    NavigateToPosition navigateToPosition;

    WeaponBase currentWeapon;
    [SerializeField] float attacksPerSecond = 0.5f;
    [SerializeField] float timeToForgetNoiseMaker = 1f;

    [Header("Cover Parameters")]
    [SerializeField] float fearDistance = 15f;
    [SerializeField] float coverSearchRadius = 20f;
    [SerializeField] LayerMask coverSearchLayerMask;
    [SerializeField] LayerMask occludingLayerMask = Physics.DefaultRaycastLayers;

    Transform currentTarget = null;

    NoiseMaker lastHeardNoiseMaker = null;
    Sight sight;

    public Animator anim;  // quitar despues

    public bool isDead;

    enum State
    {
        Patrol,
        Idle,
        Seek,
        Attack,
        TakeCover,
        Die,
        CheckLastPosition,
    }
    [SerializeField] State state = State.Seek;
    [SerializeField] float checkPositionThreshold = 1.5f;

    private void Awake()
    {
        navigateToTransform = GetComponent<NavigateToTransform>();
        navigateRoute = GetComponent<NavigateRoute>();
        navigateToPosition = GetComponent<NavigateToPosition>();
        currentWeapon = GetComponentInChildren<WeaponBase>();
        sight = GetComponent<Sight>();
    }

    void Start()
    {
        navigateToTransform.enabled = false;
        navigateRoute.enabled = false;
        navigateToPosition.enabled = false;

        state = State.Idle;
        if (behaviourType == BehaviourType.Guardian)
        { 
            GetComponent<NavMeshAgent>().speed = 0f; 
            anim.SetBool("IsGuardian", true);
        }
        else if (navigateRoute.route != null)
        { state = State.Patrol; }
    }

    float timeForNextAttack = 0f;
    float timeLeftToForgetNoiseMaker;
    private Vector3 lastNoticedPosition;
    bool locatedFirstTarget = false;

    private void Update()
    {
        UpdateNoiseMaker();
        UpdateCurrentTarget();

        /// IMPLEMENTAR: Patrol e Idle
        switch (state)
        {
            case State.Idle:
                if (currentTarget != null)
                { state = State.Seek; }
                else
                { GoTo(transform.position); }
                break;

            case State.Patrol:
                if (currentTarget != null)
                { state = State.Seek; }
                else
                { Patrol(); }
                break;

            case State.Seek:
                if (currentTarget == null)
                {
                    navigateRoute.enabled = false;
                    state = State.CheckLastPosition;
                }
                else
                {
                    GoTo(currentTarget);
                    if (IsInAttackRange())
                    {
                        timeForNextAttack = 1f / attacksPerSecond;
                        navigateToTransform.transformGoTo = null;
                        state = State.Attack;
                    }
                }
                break;

            case State.Attack:
                UpdateAttack();
                break;

            case State.TakeCover:
                UpdateTakeCover();
                break;

            case State.CheckLastPosition:
                if (currentTarget != null)
                {
                    state = State.Seek;
                }
                else
                {
                    GoTo(lastNoticedPosition);
                    if (Vector3.Distance(navigateToPosition.position, transform.position) < checkPositionThreshold)
                    {
                        if (navigateRoute.route != null)
                        {
                            Patrol();
                            state = State.Patrol;
                        }
                        else
                        { state = State.Idle; }
                    }
                }
                break;

            case State.Die:
                break;
        }
    }

    void UpdateNoiseMaker()
    {
        timeLeftToForgetNoiseMaker -= Time.deltaTime;
        if (timeLeftToForgetNoiseMaker <= 0f) { lastHeardNoiseMaker = null; }
    }

    void UpdateCurrentTarget()
    {
        currentTarget = null;
        if (sight.collidersInsight.Count > 0)
        { currentTarget = sight.collidersInsight[0].transform; }
        else
        {
            // Complex, but it's the same
            if ((behaviourType != BehaviourType.Sneaky) || locatedFirstTarget)  // hacer que no le afecte el ruido al Cautious?
            {
                if (lastHeardNoiseMaker != null)
                {
                    currentTarget = lastHeardNoiseMaker.transform;
                 //  Debug.Log(currentTarget);
                }
            }
        }

        // Complex, but it's the same
        locatedFirstTarget |= currentTarget != null;

        // locatedFirstTarget = locatedFirsTarget | currentTarget!= null
        //          cambia el bool a true o false |  true  
        /* 
        x |= y;
        into

        x = x | y;
        */

        if (currentTarget != null) { lastNoticedPosition = currentTarget.position; }
        //Debug.Log(currentTarget);
    }

    bool currentSideStepDirection = false;
    bool oldIsInMinRange = false;
    void UpdateAttack()
    {
        if (currentTarget == null)
        { state = State.CheckLastPosition; }
        else
        {
            bool advanceWhileAttacking = behaviourType == BehaviourType.Valiant || behaviourType == BehaviourType.Sneaky || behaviourType == BehaviourType.Cautious;
            bool isInMinRange = Vector3.Distance(currentTarget.position, transform.position) < currentWeapon.GetMinRange();

            if ((behaviourType == BehaviourType.Cautious) && (Vector3.Distance(currentTarget.position, transform.position) < fearDistance))  // si es menor a 15 es porque estoy mas cerca
            {
                
                selectedCover = FindBestCover();
                if (selectedCover)
                {
                    Debug.Log("Brou!");
                     state = State.TakeCover; 
                }
                else
                { GoTo(transform.position); }
            }
            else if (advanceWhileAttacking)
            {
                if (!isInMinRange)
                {
                    // Debug.Log("!isInMinRange");
                    GoTo(currentTarget);
                    anim.SetBool("ShootRifle", false);
                }
                else
                {
                    // Debug.Log("Happening");
                    if (oldIsInMinRange != isInMinRange)
                    {
                        currentSideStepDirection = Random.Range(0f, 100f) < 50f;
                        Debug.Log("Step to the right: " + currentSideStepDirection);
                        anim.SetBool("ShootRifle", false);
                    }

                    SideStep(currentSideStepDirection);
                }
            }
           
            else
            { GoTo(transform.position); }


            // Aiming / Shooting
            LookAt(currentTarget);

            // TODO: chequear el tipo de arma, y 
            // utilizar las llamadas correctas
            // para disparar

            if (currentWeapon.NeedsReload())                  // hagamos una trampa.  siempre tiene mucha bala siempre tendra por ahora pa disparar
            {
                currentWeapon.Reload();
                anim.SetBool("ShootRifle", false);
            }
            else
            {
                currentWeapon.Shot();
                anim.SetBool("ShootRifle", true);
            }

            if (!IsInAttackRange())
            {
                anim.SetBool("ShootRifle", false);
                state = State.Seek;
                GoTo(currentTarget);
            }

            oldIsInMinRange = isInMinRange;
        }
    }

    Transform selectedCover;
    float timeCovering;
    [SerializeField] float thresholdCover = 0.5f;
    void UpdateTakeCover()
    {
        if (Vector3.Distance(selectedCover.position, transform.position) > thresholdCover)
        {
            // Yendo a cubrirse
            GoTo(selectedCover);
        }
        else
        {
            // Estamos a cubierto
            if (currentTarget != null)
            {
                selectedCover = FindBestCover();
                if (!selectedCover)  // checar
                { state = State.Attack; }
            }
            else
            {
                timeCovering -= Time.deltaTime;
                if (timeCovering < 0f)
                { state = State.CheckLastPosition; }
            }
        }
    }

    void GoTo(Vector3 position) // ir hacia un lugar
    {
        navigateRoute.enabled = false;
        navigateToTransform.enabled = false;
        navigateToPosition.enabled = true;
        navigateToPosition.position = position;
    }

    void Patrol()
    {
        navigateRoute.enabled = true;
        navigateToTransform.enabled = false;
        navigateToPosition.enabled = false;
    }

    void GoTo(Transform targetTransform) // ir hacia el player
    {
        navigateRoute.enabled = false;
        navigateToTransform.enabled = true;
        navigateToTransform.transformGoTo = targetTransform;
        navigateToPosition.enabled = false;
    }

    void LookAt(Transform lookTarget)
    {
        Vector3 positionOnSameHeight = lookTarget.position;
        positionOnSameHeight.y = transform.position.y;
        transform.LookAt(positionOnSameHeight);
    }

    void SideStep(bool toRight)
    {
        Vector3 destination = transform.position + (toRight ? transform.right : -transform.right);
        GoTo(destination);
    }

    bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, PlayerMovement.instance.transform.position) < currentWeapon.GetMaxRange();
    }

    void TargetWithLifeThatNotifies.IDeathNotifiable.NotifyDeath()
    {
        if (state != State.Die)
        {
            state = State.Die;
            navigateToTransform.transformGoTo = null;

            Collider collider = GetComponent<Collider>();
            if (collider) { collider.enabled = false; }

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb) { rb.isKinematic = true; }

            Animator animator = GetComponentInChildren<Animator>();
            animator.enabled = false;

            Invoke(nameof(TimeToDie), 1f);
        }
    }

    void TimeToDie()
    {
        // Lanzar animaci�n
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        navigateToPosition.enabled = false;
        isDead = true;
        gameObject.SetActive(false);
        this.enabled = false;
        //Destroy(gameObject);
    }

    void NoiseMaker.INoiseListener.OnHeard(NoiseMaker noiseMaker)  // revisar esta interfaz
    {
      //  Debug.Log("This is the noise maker " + noiseMaker);
        lastHeardNoiseMaker = noiseMaker;
        timeLeftToForgetNoiseMaker = timeToForgetNoiseMaker;
    }

    Transform FindBestCover()
    {
        Collider[] potentialCovers = Physics.OverlapSphere(transform.position, coverSearchRadius, coverSearchLayerMask, QueryTriggerInteraction.Ignore);

        // TODO: discard covers that are closer to
        //    the currentTarget than this entity
        // TODO: sort potential covers


        foreach (Collider c in potentialCovers)  // tiene que tener un collider
        {
            RaycastHit hit;
            Vector3 direction = c.transform.position - currentTarget.position;
            if (Physics.Raycast(currentTarget.position, direction, out hit, direction.magnitude, occludingLayerMask, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(c.transform.position, Vector3.up * 5f, Color.red);
                return c.transform;
            }
        }

        return null;
    }
}
