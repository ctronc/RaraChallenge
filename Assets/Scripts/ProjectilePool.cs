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

    private void Awake()
    {
        SharedInstance = this;
        _pooledObjectsContainer = GameObject.Find("PooledObjects");
    }
    
    private void OnEnable()
    {
        GameStateManager.OnGameReset += ResetState;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmpGo;
        for(int i = 0; i < amountToPool; i++)
        {
            tmpGo = Instantiate(objectToPool, _pooledObjectsContainer.transform, true);
            tmpGo.transform.GetChild(0).gameObject.SetActive(false);
            pooledObjects.Add(tmpGo);
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
    
    private void OnDisable()
    {
        GameStateManager.OnGameReset -= ResetState;
    }


}
