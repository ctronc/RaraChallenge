using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool SharedInstance;
    [SerializeField] private List<GameObject> pooledObjects;
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;

    private void OnEnable()
    {
        GameStateManager.OnGameReset += ResetState;
    }

    private void OnDisable()
    {
        GameStateManager.OnGameReset -= ResetState;
    }
    
    private void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
    
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    private void ResetState()
    {
        foreach (GameObject go in pooledObjects)
        {
            go.SetActive(false);
        }
    }


}
