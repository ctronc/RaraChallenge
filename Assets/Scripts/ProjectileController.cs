using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private int _projectileDamage;
    private float _projectileSpeed;
    private GameObject _projectileGo;

    private void Awake()
    {
        _projectileGo = transform.GetChild(0).gameObject;
    }
    
    private void OnEnable()
    {
        GameStateManager.OnLevelClear += DestroySelf;
    }

    private void OnDisable()
    {
        GameStateManager.OnLevelClear -= DestroySelf;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _projectileSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            DeactivateProjectile();
        }
        else if (other.CompareTag("Player"))
        {
            DeactivateProjectile();
        }
    }

    public void ActivateProjectile()
    {
        _projectileGo.SetActive(true);
    }

    public void DeactivateProjectile()
    {
        _projectileGo.SetActive(false);
    }

    public void SetProjectileSpeed(float speed)
    {
        _projectileSpeed = speed;
    }

    public void SetProjectileDamage(int damage)
    {
        _projectileDamage = damage;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
