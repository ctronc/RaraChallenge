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

    void Awake()
    {
        // Sets spawn y position for transform
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _startingPosition = transform.position;
    }
    
    private void OnEnable()
    {
        GameStateManager.OnGameReset += ResetState;
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
        // Gets a random point in the navmesh
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;
        NavMesh.SamplePosition(randDirection, out var navHit, dist, 1);

        return navHit.position;
    }
    
    public int GetDamage()
    {
        return wandererDamage;
    }

    private void ResetState()
    {
        _navMeshAgent.isStopped = true;
        _navMeshAgent.enabled = false;
        transform.position = _startingPosition;
        _navMeshAgent.enabled = true;
    }
    
    private void OnDisable()
    {
        GameStateManager.OnGameReset -= ResetState;
    }   
}
