using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserController : MonoBehaviour
{
    private GameObject _targetObject;
    [SerializeField] private int chaserDamage;
    [SerializeField] private float chaseStartDistance;
    [SerializeField] private float chaseStopDistance;

    private Vector3 _startingPosition;
    private NavMeshAgent _navMeshAgent;
    private bool _isChasing;
    
    private void Awake()
    {
        // Sets spawn Y position for transform
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        
        // Find player GameObject to set as follow target
        _targetObject = GameObject.Find("Player");
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _isChasing = false;
        _startingPosition = transform.position;
    }
    
    private void OnEnable()
    {
        // Listen for reset event
        GameStateManager.OnGameReset += ResetState;
    }

    void Update()
    {
        if (_targetObject != null)
        {
            // distance between player and chaser
            float distance = Vector3.Distance(transform.position, _targetObject.transform.position);

            // start the chase
            if (distance < chaseStartDistance)
            {
                _isChasing = true;
                _navMeshAgent.isStopped = false;
            }

            // continue the chase if far away enough
            if ((distance < chaseStopDistance) && _isChasing)
            {
                _navMeshAgent.destination = _targetObject.transform.position;
            }
            
            // stop the chase is player gets far away enough
            else if ((distance >= chaseStopDistance) && _isChasing)
            {
                _isChasing = false;
                _navMeshAgent.isStopped = true;
            }
        }
    }
    
    public int GetDamage()
    {
        return chaserDamage;
    }

    private void ResetState()
    {
        _isChasing = false;
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
