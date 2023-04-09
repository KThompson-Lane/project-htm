using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObject/Enemy")]
public class EnemySO : ScriptableObject
{
    public float level = 1; //default to base
    //Note - current health cannot be stored here as it would effect all enemies using this SO
    private float _maxHealth;
    private float _speed;
    [SerializeField] private float rotationSpeed = 3.0f; //Note - seems to be the right speed for all enemies but may need changing later
    public EnemyAttackSO enemyAttackType;

    public Sprite enemySprite;

    public void OnEnable()
    {
        _maxHealth = level; //Note - this may want changing in the future - thinking about bosses (might have separate SO though)
        _speed = level / 4; //Note - this may also want changing in the future
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }
}
