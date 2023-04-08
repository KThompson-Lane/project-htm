using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackSO", menuName = "ScriptableObject/EnemyAttack")]
public class EnemyAttackSO : ScriptableObject
{
    public float damage = 1f;
    public float range = 0f;
    public float rateOfFire = 120f;
    public GameObject projectile;
}
