using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public delegate void CoinAction();
    public static event CoinAction onCoinTouch;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onCoinTouch?.Invoke();
            Destroy(gameObject);
        }
    }
}
