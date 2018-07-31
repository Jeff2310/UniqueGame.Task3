using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager> {

    //Mark it serializable So we can edit the array in the Hierarchy
    public class Pool
    {
        public string name;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools = new List<Pool>();
    public Dictionary<string,Queue<GameObject>> poolDic = new Dictionary<string, Queue<GameObject>>();



	// Use this for initialization
	void Start () {

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objPool.Enqueue(obj);
            }

            poolDic.Add(pool.name, objPool);
        }
            


	}
	
	public GameObject SpawnFromPool(string name,Vector3 position = new Vector3(),Quaternion rotation = new Quaternion())
    {
        if (!poolDic.ContainsKey(name))
        {
            Debug.LogWarning("No " + name + "in the Pooling Dictionary!");
        }
        GameObject objToSpawn = poolDic[name].Dequeue();

        //Set for the basis
        
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;

        //Implement the Start method
        IPoolObject poolObject =  objToSpawn.GetComponent<IPoolObject>();
        if (poolObject != null)
        {
            poolObject.PoolObjectStart();
        }
        
        
        
        poolDic[name].Enqueue(objToSpawn);
        objToSpawn.SetActive(true);
        return objToSpawn;
    }

    public void DisableByName(string name)
    {
        if (!poolDic.ContainsKey(name))
        {
            Debug.LogWarning("No " + name + "in the Pooling Dictionary!");
            return;
        }
        foreach (GameObject item in poolDic[name])
        {
            item.SetActive(false);
        }
    }

    public void DisableAll()
    {
        foreach (var queue in poolDic.Values)
        {
            foreach (var obj in queue)
            {
                obj.SetActive(false);
            }
        }
    }
}

interface IPoolObject {

    void PoolObjectStart();
}
