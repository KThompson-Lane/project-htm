using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackSO", menuName = "ScriptableObject/EnemyAttack")]
public class EnemyAttackSO : ScriptableObject
{
    public float damage = 1;
    public float range = 10f; //todo - find good base value, implement this

}
