//	Copyright (c) Kimpuppy.
//	http://bakak112.tistory.com/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable()]
public class PoolObject
{
    public string Tag;

    public GameObject Prefab;

    public int Size;

    public PoolObject(string tag, GameObject prefab, int size)
    {
        Tag = tag;
        Prefab = prefab;
        Size = size;
    }
}

public interface IPoolable
{
    void OnObjectPool();
}

public class ObjectPooler : MonoSingleton<ObjectPooler>
{
    public List<PoolObject> PoolList;

    private Dictionary<string, Queue<GameObject>> _poolDictionary;

    private void Awake()
    {
        _poolDictionary = new Dictionary<string, Queue<GameObject>>();

        for(int i = 0; i < PoolList.Count; i++)
            CreatePool(PoolList[i]);
    }

    public void CreatePool(PoolObject poolObject)
    {
        PoolList.Add(poolObject);

        Queue<GameObject> poolQueue = new Queue<GameObject>();
        for(int j = 0; j < poolObject.Size; j++)
        {
            GameObject obj = Instantiate(poolObject.Prefab);
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
        _poolDictionary.Add(poolObject.Tag, poolQueue);
    }

    public void CreatePool(string tag, GameObject prefab, int size)
    {
        PoolObject poolObject = new PoolObject(tag, prefab, size);
        CreatePool(poolObject);
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!_poolDictionary.ContainsKey(tag)) return null;

        GameObject spawnObject = _poolDictionary[tag].Dequeue();

        spawnObject.SetActive(true);
        spawnObject.transform.position = position;
        spawnObject.transform.rotation = rotation;

        IPoolable poolObject = spawnObject.GetComponent<IPoolable>();
        if(poolObject != null)
            poolObject.OnObjectPool();
        
        _poolDictionary[tag].Enqueue(spawnObject);

        return spawnObject;
    }
}
