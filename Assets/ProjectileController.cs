using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float _projectileSpeed = 1;

    
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
    }

    public void SetProjectileSpeed(float speed)
    {
        _projectileSpeed = speed;
    }
}
