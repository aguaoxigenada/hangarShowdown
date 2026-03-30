using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavigateToTransform : MonoBehaviour
{
    public Transform transformGoTo;
    NavMeshAgent navMeshAgent;

    void Awake()
    {
        // se cachea el componente desde antes para no utilizar recursos despues
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

  
    void Update()
    {
        if (transformGoTo)
        {
            navMeshAgent.SetDestination(transformGoTo.position);
        }
        else
        {
            navMeshAgent.SetDestination(transform.position);
        }
    }
}
