using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private int playerHealth;
    private CharacterController _characterController;

    // replace with scriptableobject!!
    private int _projectileDamage = 20;
    private int _wandererDamage = 100;
    private int _chaserDamage = 100;
    
    private Vector3 _startingPosition;
    private int _startingHealth;

    private bool _playerDisplayed;
    private bool _hasDied;
    
    public delegate void PlayerAction();
    public static event PlayerAction OnPlayerDeath;
    public static event PlayerAction OnPlayerWin;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _startingPosition = transform.position;
        _startingHealth = playerHealth;
        _hasDied = false;
    }
    
    private void OnEnable()
    {
        GameStateManager.OnGameReset += ResetState;
        GameStateManager.OnLevelClear += ClearPlayer;
    }

    void Update()
    {
        if (playerHealth <= 0 && _hasDied == false)
        {
            Debug.Log("Game Over");
            OnPlayerDeath?.Invoke();
            _hasDied = true;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (playerHealth > 0)
        {
            if (other.CompareTag("Projectile"))
            {
                playerHealth -= _projectileDamage;
                Debug.Log("Projectile hit -> player health: " + playerHealth);
            }

            if (other.CompareTag("Wanderer"))
            {
                playerHealth -= _wandererDamage;
                Debug.Log("Wanderer hit -> player health: " + playerHealth);
            }

            if (other.CompareTag("Chaser"))
            {
                playerHealth -= _chaserDamage;
                Debug.Log("Chaser hit -> player health: " + playerHealth);
            }

            if (other.CompareTag("Flag"))
            {
                OnPlayerWin?.Invoke();
                Debug.Log("You win!");
            }
        }
    }

    public void DisplayPlayer()
    {
        _playerDisplayed = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }
    private void ClearPlayer()
    {
        _playerDisplayed = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public bool GetPlayerDisplayedState()
    {
        return _playerDisplayed;
    }

    public void SetPlayerPosition(Vector3 newPos)
    {
        Vector3 playerPosition = new Vector3(newPos.x, transform.position.y, newPos.z);
        
        _characterController.enabled = false;
        transform.position = playerPosition;
        _startingPosition = playerPosition;
        _characterController.enabled = true;
    }

    private void ResetState()
    {
        _characterController.enabled = false;
        transform.position = _startingPosition;
        _characterController.enabled = true;
        
        playerHealth = _startingHealth;
        _hasDied = false;
    }
    
    private void OnDisable()
    {
        GameStateManager.OnGameReset -= ResetState;
        GameStateManager.OnLevelClear -= ClearPlayer;
    }
}