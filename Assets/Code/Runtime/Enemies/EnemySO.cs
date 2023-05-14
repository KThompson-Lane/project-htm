using Code.Runtime.Enemies;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObject/Enemy")]
public class EnemySO : ScriptableObject
{
    public float level = 1; //default to base
    public float moveRange; 
        
    private float _maxHealth;
    private float _speed;
    [SerializeField] private float rotationSpeed = 3.0f; //Note - seems to be the right speed for all enemies but may need changing later
    //public EnemyAttackSO[] enemyAttackTypes;
    public EnemyRangedSO[] enemyRangedTypes;
    public EnemyMeleeSO[] enemyMeleeTypes;

    public Sprite enemySprite;
    public Sprite enemyIcon;
    public AnimatorOverrideController enemyAnimator;

    public void OnEnable()
    {
        _maxHealth = level; //Note - this may want changing in the future - thinking about bosses (might have separate SO though)
        _speed = level / 4; //Note - this may also want changing in the future
    }

    public float GetSpeed()
    {
        return _speed = level / 4;
    }

    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }

    public float GetMaxHealth()
    {
        return _maxHealth = level;
    }
}
