using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public delegate void CoinAction();
    public static event CoinAction OnCoinTouch;

    private GameObject _coinObject;
    
    private void Awake()
    {
        // Sets spawn y position for transform
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        
        // Gets child GameObject that has the coin mesh
        _coinObject = gameObject.transform.GetChild(0).gameObject;
    }
    
    private void OnEnable()
    {
        GameStateManager.OnGameReset += ResetState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Emit OnCoinTouch event to increase score and
            // disable the coin mesh GameObject
            OnCoinTouch?.Invoke();
            _coinObject.SetActive(false);
        }
    }

    private void ResetState()
    {
        _coinObject.SetActive(true);
    }
    
    private void OnDisable()
    {
        GameStateManager.OnGameReset -= ResetState;
    }
}
