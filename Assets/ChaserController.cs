using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserController : MonoBehaviour
{
    private GameObject _targetObject;
    [SerializeField] private float chaseStartDistance;
    [SerializeField] private float chaseStopDistance;

    private NavMeshAgent _navMeshAgent;
    private bool _isChasing;

    private void Awake()
    {
        _targetObject = GameObject.Find("Player");
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _isChasing = false;
    }
    
    void Update()
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
            Debug.Log("Stopped with distance: " + distance);
            _isChasing = false;
            _navMeshAgent.isStopped = true;
        }
    }
}
