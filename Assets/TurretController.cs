using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private GameObject cannonGameObject;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float startDelay;
    [SerializeField] private float firingRate;
    
    private ProjectileController _projectileController;
    private Transform _myTransform;
    private Vector3 _cannonPos;

    private void Awake()
    {
        _myTransform = transform;
        _cannonPos = cannonGameObject.transform.position; 
    }
    private void Start()
    {
        EnableGameplay();
    }

    private void EnableGameplay()
    {
        StartCoroutine(Shoot());
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
            _projectileController.SetProjectileSpeed(projectileSpeed);
            projectile.SetActive(true);
        }
    }
}
