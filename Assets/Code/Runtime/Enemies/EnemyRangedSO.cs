using UnityEngine;

namespace Code.Runtime.Enemies
{
    [CreateAssetMenu(fileName = "EnemyRangedSO", menuName = "ScriptableObject/EnemyRanged")]
    public class EnemyRangedSO : EnemyAttackSO
    {
        [Range(0, 359)] public float angleSpread;
        public float startingDistance = 0.1f;
        public float bulletsInBurst = 1;
        public float waveStepAmount = 0;
    
        //public int damage = 1;
        //public float range = 0f; //todo - fire range
        //public float rateOfFire = 120f;
        public float bulletForce = 5f;
        public GameObject projectile;
    }
}