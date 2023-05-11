using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] protected GameObject poolObject;
    [SerializeField] protected int poolAmount = 20;

    public static ObjectPool instance;
    
    private List<GameObject> pooledObjects = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        for (int i = 0; i < poolAmount; i++)
        {
            var obj = Instantiate(poolObject, transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject RetrieveFromPool()
    {
        return pooledObjects.FirstOrDefault(t => !t.activeInHierarchy);
    }
    
}
