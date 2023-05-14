using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Runtime
{
    public class ObjectPooler : MonoBehaviour
    {
        public static ObjectPooler SharedInstance;
        public List<GameObject> pooledObjects;
        public List<ObjectPoolItem> itemsToPool;


        void Awake() {
            SharedInstance = this;
        }

        private void Start()
        {
            pooledObjects = new List<GameObject>();
            foreach (var item in itemsToPool) {
                for (int i = 0; i < item.amountToPool; i++) {
                    var obj = Instantiate(item.objectToPool, transform);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                }
            }
        }
        
        public GameObject GetPooledObject(string objectTag)
        {
            var pooled = pooledObjects.FirstOrDefault(t => !t.activeInHierarchy && t.CompareTag(objectTag));
            if (pooled != null) return pooled;
            {
                var item = itemsToPool.First(t => t.objectToPool.CompareTag(objectTag));
                if (!item.shouldExpand) return pooled;
                var obj = Instantiate(item.objectToPool, transform);
                obj.SetActive(false);
                pooledObjects.Add(obj);
                return obj;
            }
        }

        public void DisableAllObjects()
        {
            pooledObjects.ForEach(go => go.SetActive(false));
        }
    }
}