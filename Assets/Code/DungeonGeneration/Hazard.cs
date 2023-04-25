using UnityEngine;

namespace Code.DungeonGeneration
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Hazard : MonoBehaviour
    {
        //TODO: Replace flat damage amount w/ a hazard/effect SO giving the hazard tile different effects
        [SerializeField] private HealthManager playerHealthManager;
        private int _damage;
        public void SetDamage(int value) => _damage = value;

        private void Awake()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                playerHealthManager.DecreaseHealth(_damage);
        }
    }
}