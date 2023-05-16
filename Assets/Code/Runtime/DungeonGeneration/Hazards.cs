using UnityEngine;

// UNUSED CODE

namespace Code.Runtime.DungeonGeneration
{
    [RequireComponent(typeof(Collider2D))]
    public class Hazards : MonoBehaviour
    {
        //TODO: Replace flat damage amount w/ a hazard/effect SO giving the hazard tile different effects
        [SerializeField] private HealthManager playerHealthManager;
        
        [SerializeField] int damage;

        private void Awake()
        {
            //GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player stepped on hazard");
                //playerHealthManager.DecreaseHealth(damage); //todo - needs to pass a game object 
            }
        }
    }
}