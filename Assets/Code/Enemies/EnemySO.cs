using UnityEngine;

[CreateAssetMenu(fileName = "EnemeySO", menuName = "ScriptableObject/Enemy")]
public class EnemySO : ScriptableObject
{
    //Note - current health cannot be stored here as it would effect all enemies using this SO
    public float maxHealth = 3f; //todo - sort
    public float speed = 5f; //todo - sort
    public EnemyAttackSO enemyAttackType;
    
    //todo - can we add sprite in here?
}
