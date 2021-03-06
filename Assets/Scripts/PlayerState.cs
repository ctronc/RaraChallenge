using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private int playerHealth;
    private CharacterController _characterController;

    private Vector3 _startingPosition;
    private int _startingHealth;
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
        // Listen for reset gameplay state clear level events
        GameStateManager.OnGameReset += ResetState;
        GameStateManager.OnLevelClear += ClearPlayer;
    }

    void Update()
    {
        if (playerHealth <= 0 && _hasDied == false)
        {
            // Player death handling. Emits player death event.
            Debug.Log("Game Over");
            OnPlayerDeath?.Invoke();
            _hasDied = true;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Handles damage to the player and win condition when touching the flag
        if (playerHealth > 0)
        {
            int enemyDamage;
            
            if (other.CompareTag("Projectile"))
            {
                enemyDamage = other.transform.parent.gameObject.GetComponent<ProjectileController>().GetDamage();
                playerHealth -= enemyDamage;
                Debug.Log("Projectile hit, damage: " + enemyDamage + " -> player health: " + playerHealth);
            }

            if (other.CompareTag("Wanderer"))
            {
                enemyDamage = other.transform.parent.gameObject.GetComponent<WandererController>().GetDamage();
                playerHealth -= enemyDamage;
                Debug.Log("Wanderer hit, damage: " + enemyDamage + " -> player health: " + playerHealth);
            }

            if (other.CompareTag("Mine"))
            {
                enemyDamage = other.transform.parent.gameObject.GetComponent<MineController>().GetDamage();
                playerHealth -= enemyDamage;
                Debug.Log("Mine hit, damage: " + enemyDamage + " -> player health: " + playerHealth);
            }

            if (other.CompareTag("Chaser"))
            {
                enemyDamage = other.transform.parent.gameObject.GetComponent<ChaserController>().GetDamage();
                playerHealth -= enemyDamage;
                Debug.Log("Chaser hit, damage: " + enemyDamage + " -> player health: " + playerHealth);
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
        // Activates player mesh GameObject
        transform.GetChild(0).gameObject.SetActive(true);
    }
    private void ClearPlayer()
    {
        // Only set the player mesh GameObject as inactive, not the parent GameObject
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SetPlayerPosition(Vector3 newPos)
    {
        // Used for placing the player in build mode
        Vector3 playerPosition = new Vector3(newPos.x, transform.position.y, newPos.z);
        
        // Character controller needs to be disabled in order to manually move the player
        // to another position
        _characterController.enabled = false;
        transform.position = playerPosition;
        _startingPosition = playerPosition;
        _characterController.enabled = true;
    }

    private void ResetState()
    {
        // Reset entity state for retrying or entering build mode
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