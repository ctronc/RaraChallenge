using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private int _projectileDamage;
    private float _projectileSpeed;
    
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _projectileSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

    public void SetProjectileSpeed(float speed)
    {
        _projectileSpeed = speed;
    }

    public void SetProjectileDamage(int damage)
    {
        _projectileDamage = damage;
    }
}
