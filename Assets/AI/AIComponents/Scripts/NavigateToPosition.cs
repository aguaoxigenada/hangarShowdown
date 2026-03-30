using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavigateToPosition : MonoBehaviour
{
    public Vector3 position;
    NavMeshAgent agent;
    public bool goToPosition = true;

    void Awake()
    {
      //  lifeAmount = GetComponent<TargetWithLife>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {

        if (goToPosition)
        {
            agent.SetDestination(position);
        }

    }
}
