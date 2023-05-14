using UnityEngine;

namespace Code.Runtime.Enemies
{
    [CreateAssetMenu(fileName = "EnemyRangedSO", menuName = "ScriptableObject/EnemyRanged")]
    public class EnemyRangedSO : EnemyAttackSO
    {
        [Range(0, 359)] public float angleSpread;
        public float startingDistance = 0.1f; // todo - if we wanted to give offset
        public float bulletsInBurst = 1;
        public float waveStepAmount = 0;
        public float bulletForce = 5f;
        public GameObject projectile;
    }
}