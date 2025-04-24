using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    [SerializeField]
    private Pool[] poolArray;

    private Transform _objectPoolTransform;

    private Dictionary<int, Queue<Component>> _poolDictionary = new Dictionary<int, Queue<Component>>();

    public static PoolManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [System.Serializable]
    public struct Pool
    {
        public int poolSize;
        public GameObject prefab;
        public string componentType;

    }

    private void Start()
    {
        _objectPoolTransform = this.gameObject.transform;

        for (int i = 0; i < poolArray.Length; i++) {
            CreatePool(poolArray[i].prefab, poolArray[i].poolSize, poolArray[i].componentType);
        }  
    }

    private void CreatePool(GameObject prefab, int poolSize, string componentType)
    {
        int poolKey = prefab.GetInstanceID();

        string prefabName = prefab.name;

        GameObject parentGameObject = new GameObject(prefabName + "Anchor");
        parentGameObject.transform.SetParent(_objectPoolTransform);

        if (!_poolDictionary.ContainsKey(poolKey)) { 
        
            _poolDictionary.Add(poolKey, new Queue<Component>());

            for (int i = 0; i< poolSize; i++)
            {
                GameObject newObject = Instantiate(prefab, parentGameObject.transform) as GameObject;
                newObject.SetActive(false);
                _poolDictionary[poolKey].Enqueue(newObject.GetComponent(Type.GetType(componentType)));
            }
        }
    }


    public Component ReuseComponent(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();
        if (_poolDictionary.ContainsKey(poolKey))
        {
            Component componentToReuse = GetComponentFromPool(poolKey);
            ResetObject(position, rotation, componentToReuse, prefab);
            return componentToReuse;
        }
        else
        {
            Debug.Log("No Object Pool for " + prefab);
            return null;
        }
    }

    private Component GetComponentFromPool(int poolKey)
    {
        Component componentToReuse = _poolDictionary[poolKey].Dequeue();
        _poolDictionary[poolKey].Enqueue(componentToReuse);

        if (componentToReuse.gameObject.activeSelf == true)
        {
            componentToReuse.gameObject.SetActive(false);
        }

        return componentToReuse;
    }

    private void ResetObject(Vector3 position, Quaternion rotation, Component componentToRefuse, GameObject prefab)
    {
        componentToRefuse.transform.position = position;
        componentToRefuse.transform.rotation = rotation;
        componentToRefuse.gameObject.transform.localScale = prefab.transform.localScale;
    }

}
