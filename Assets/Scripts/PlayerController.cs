using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpSpeed;
    
    private PlayerInput _playerInput;
    private CharacterController _characterController;
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private float _ySpeed;
    private bool _isJumpPressed;

    private void Awake()
    {
        // sets spawn y position for transform
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        
        // Callbacks for new player input system
        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;

        _playerInput.CharacterControls.Jump.started += OnJump;
        _playerInput.CharacterControls.Jump.canceled += OnJump;
    }
    
    private void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
    }
    
    void Update()
    {
        // Pull player to the ground if its not grounded
        _ySpeed += Physics.gravity.y * Time.deltaTime;

        if (_characterController.isGrounded)
        {
            // Just to be safe, keep pulling a bit in order to keep player grounded
            _ySpeed = -0.5f;

            // Player jump
            if (_isJumpPressed)
            {
                _ySpeed = jumpSpeed;
            }
        }

        // Apply WASD movement and jump to character controller
        Vector3 currentMovement = _currentMovement * playerSpeed;
        currentMovement.y = _ySpeed;
        _characterController.Move(currentMovement * Time.deltaTime);
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
    }

    private void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
    }
}
