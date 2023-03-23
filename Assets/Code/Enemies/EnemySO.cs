using UnityEngine;

[CreateAssetMenu(fileName = "EnemeySO", menuName = "ScriptableObject/Enemy")]
public class EnemySO : ScriptableObject
{
    public int health = 3; //todo - sort
    public float speed = 5f; //todo - sort
    public EnemyAttackSO enemyAttackType;
    
    //todo - can we add sprite in here?
}
