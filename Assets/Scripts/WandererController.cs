using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WandererController : MonoBehaviour
{
    [SerializeField] private int wandererDamage;
    [SerializeField] private float wanderRadius;
    
    private NavMeshAgent _navMeshAgent;
    private Vector3 _startingPosition;

    private void OnEnable()
    {
        GameStateManager.OnGameReset += ResetState;
    }

    private void OnDisable()
    {
        GameStateManager.OnGameReset -= ResetState;
    }    
    
    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _startingPosition = transform.position;
    }

    void Start()
    {
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
        _navMeshAgent.SetDestination(newPos);  
    }

    void Update()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
            _navMeshAgent.SetDestination(newPos);
        }
    }
    
    public static Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;
        NavMesh.SamplePosition(randDirection, out var navHit, dist, 1);

        return navHit.position;
    }

    private void ResetState()
    {
        transform.position = _startingPosition;
    }
}
