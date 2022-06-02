using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public delegate void CoinAction();
    public static event CoinAction OnCoinTouch;

    private GameObject _coinObject;
    
    private void OnEnable()
    {
        GameStateManager.OnGameReset += ResetState;
    }

    private void OnDisable()
    {
        GameStateManager.OnGameReset -= ResetState;
    }

    private void Awake()
    {
        // gets child GameObject that has the coin mesh
        _coinObject = gameObject.transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCoinTouch?.Invoke();
            _coinObject.SetActive(false);
        }
    }

    private void ResetState()
    {
        _coinObject.SetActive(true);
    }
}
