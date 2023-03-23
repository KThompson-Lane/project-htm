using UnityEngine;

[CreateAssetMenu(fileName = "EnemeySO", menuName = "ScriptableObject/Enemy")]
public class EnemySO : ScriptableObject
{
    public float level = 1; //default to base
    //Note - current health cannot be stored here as it would effect all enemies using this SO
    private float maxHealth; //todo - sort
    private float speed;
    public EnemyAttackSO enemyAttackType;
    
    //todo - add sprite?

    public void OnEnable()
    {
        maxHealth = level; //todo - implement health
        speed = level / 2;
    }

    public float getSpeed()
    {
        return speed;
    }
    
    public float getMaxHealth()
    {
        return maxHealth;
    }
}
