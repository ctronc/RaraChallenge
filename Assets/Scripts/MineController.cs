using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour
{
    private void Awake()
    {
        // sets spawn y position for transform
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
    
    private void OnEnable()
    {
        GameStateManager.OnGameReset += ResetState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void ResetState()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    
    private void OnDisable()
    {
        GameStateManager.OnGameReset -= ResetState;
    }
}
