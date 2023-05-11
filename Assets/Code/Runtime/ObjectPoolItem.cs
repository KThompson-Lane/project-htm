using System;
using UnityEngine;

namespace Code.Runtime
{
    [Serializable]
    public class ObjectPoolItem
    {
        public int amountToPool;
        public GameObject objectToPool;
        public bool shouldExpand;
    }
}