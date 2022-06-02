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

    private GameObject _pooledObjectsContainer;

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
        _pooledObjectsContainer = GameObject.Find("PooledObjects");
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool, _pooledObjectsContainer.transform, true);
            tmp.transform.GetChild(0).gameObject.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
    
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!pooledObjects[i].transform.GetChild(0).gameObject.activeInHierarchy)
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
            go.transform.GetChild(0).gameObject.SetActive(false);
        }
    }


}
