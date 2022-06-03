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
        // sets spawn y position for transform
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        
        _targetObject = GameObject.Find("Player");
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _isChasing = false;
        _startingPosition = transform.position;
    }
    
    private void OnEnable()
    {
        GameStateManager.OnGameReset += ResetState;
    }

    void Update()
    {
        if (_targetObject != null)
        {
            float distance = Vector3.Distance(transform.position, _targetObject.transform.position);

            if (distance < chaseStartDistance)
            {
                _isChasing = true;
                _navMeshAgent.isStopped = false;
            }

            if ((distance < chaseStopDistance) && _isChasing)
            {
                _navMeshAgent.destination = _targetObject.transform.position;
            }
            else if ((distance >= chaseStopDistance) && _isChasing)
            {
                _isChasing = false;
                _navMeshAgent.isStopped = true;
            }
        }
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
