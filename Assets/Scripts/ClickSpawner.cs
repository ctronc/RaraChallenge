using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject flagPrefab;
    [SerializeField] private GameObject wandererPrefab;
    [SerializeField] private GameObject chaserPrefab;
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private GameObject userLevel;
    
    private Camera _mainCamera;
    private GameObject _playerGameObject;
    private Transform _playerTransform;
    private PlayerState _playerState;
    private GameObject _entityToSpawn;
    private bool _clickSpawnEnabled;
    private bool _playerSelected;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _playerGameObject = GameObject.Find("Player");
        _playerTransform = _playerGameObject.GetComponent<Transform>();
        _playerState = _playerGameObject.GetComponent<PlayerState>();
        _entityToSpawn = null;
    }
    
    private void OnEnable()
    {
        GameStateManager.OnBuildMode += SetBuildMode;
        GameStateManager.OnPlayMode += SetPlayMode;
    }

    void Update()
    {
        if (_clickSpawnEnabled)
        {
            SpawnOnClick();
        }
    }

    private void SpawnOnClick()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Floor"))
            {
                if (_entityToSpawn != null && _playerSelected == false)
                {
                    GameObject tmpGo;
                    tmpGo = Instantiate(_entityToSpawn, hit.point, Quaternion.identity);
                    tmpGo.transform.parent = userLevel.transform;
                }

                if (_playerSelected)
                { 
                   _playerState.SetPlayerPosition(hit.point);
                   _playerState.SetPlayerYPosition();
                   _playerState.DisplayPlayer();
                }
            }
        }
    }

    public void EntitySelector(string selectedEntity)
    {
        _entityToSpawn = selectedEntity switch
        {
            "coin" => coinPrefab,
            "flag" => flagPrefab,
            "wanderer" => wandererPrefab,
            "chaser" => chaserPrefab,
            "turret" => turretPrefab,
            _ => null
        };

        if (selectedEntity == "player")
        {
            _playerSelected = true;
        }
        else
        {
            _playerSelected = false;
        }
    }

    private void SetBuildMode()
    {
        _clickSpawnEnabled = true;
    }

    private void SetPlayMode()
    {
        _clickSpawnEnabled = false;
    }
    
    private void OnDisable()
    {
        GameStateManager.OnBuildMode -= SetBuildMode;
        GameStateManager.OnPlayMode += SetPlayMode;
    }
}
