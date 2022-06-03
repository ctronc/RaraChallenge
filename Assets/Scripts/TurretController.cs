using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private GameObject cannonGameObject;
    [SerializeField] private int projectileDamage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float startDelay;
    [SerializeField] private float firingRate;
    
    private ProjectileController _projectileController;
    private Transform _myTransform;
    private Vector3 _cannonPos;
    private Coroutine _shootCoroutine;
    
    private void Awake()
    {
        // sets spawn y position for transform
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        
        _myTransform = transform;
        _cannonPos = cannonGameObject.transform.position;
    }
    
    private void OnEnable()
    {
        GameStateManager.OnGameReset += ResetState;
    }
    
    private void Start()
    {
        EnableGameplay();
    }

    private void EnableGameplay()
    {
        _shootCoroutine = StartCoroutine(Shoot());
    }
    
    IEnumerator Shoot() {
        yield return new WaitForSeconds(startDelay);
 
        while (true) {
            LaunchProjectile();
            yield return new WaitForSeconds(firingRate);
        }
    }

    private void LaunchProjectile()
    {
        GameObject projectile = ProjectilePool.SharedInstance.GetPooledObject();
        if (projectile != null)
        {
            _projectileController = projectile.GetComponent<ProjectileController>();

            projectile.transform.position = new Vector3(_myTransform.position.x, _cannonPos.y, _cannonPos.z + 0.2f);
            projectile.transform.rotation = _myTransform.rotation;
            
            _projectileController.ActivateProjectile();
            _projectileController.SetProjectileDamage(projectileDamage);
            _projectileController.SetProjectileSpeed(projectileSpeed);
        }
    }

    private void ResetState()
    {
        if (_shootCoroutine != null)
        {
            StopCoroutine(_shootCoroutine);
            _shootCoroutine = StartCoroutine(Shoot());
        }
    }
    
    private void OnDisable()
    {
        GameStateManager.OnGameReset -= ResetState;
    }
}
