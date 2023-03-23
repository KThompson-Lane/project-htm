using UnityEngine;

[CreateAssetMenu(fileName = "EnemeySO", menuName = "ScriptableObject/Enemy")]
public class EnemySO : ScriptableObject
{
    public float level = 1; //default to base
    //Note - current health cannot be stored here as it would effect all enemies using this SO
    private float _maxHealth;
    private float _speed;
    public EnemyAttackSO enemyAttackType;
    
    //todo - add sprite?

    public void OnEnable()
    {
        _maxHealth = level;
        _speed = level / 2;
    }

    public float GetSpeed()
    {
        return _speed;
    }
    
    public float GetMaxHealth()
    {
        return _maxHealth;
    }
}
