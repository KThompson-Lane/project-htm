using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemySO enemySO;

    private EnemyAttackSO _enemyAttackSo;
    private Shooting _shooting;

    private float _currentHealth;
    
    [SerializeField] 
    private float RotationSpeed = 3.0f; //todo - might want to move to SO, might want to have this fixed if it seems right, or maybe a func of move speed
    
    private float _moveSpeed;
    private float _range;
    
    private Rigidbody2D _mRb2d;
    private Transform _mPlayerTransform; // for look at location

    // Damage on collision variables
    [SerializeField]
    private HealthManager healthManager; // for player health

    // Start is called before the first frame update
    void Start()
    {
        _enemyAttackSo = enemySO.enemyAttackType;
        _shooting = GetComponent<Shooting>();
        
        // health 
        _currentHealth = enemySO.GetMaxHealth();
        
        // movement
        _moveSpeed = enemySO.GetSpeed();
        _range = _enemyAttackSo.range;
        _mRb2d = GetComponent<Rigidbody2D>();
        _mPlayerTransform = GameObject.FindWithTag("Player").transform; //todo - might want to change if not all enemies follow player
        
        // shooting
        if (_enemyAttackSo.ranged != true) return;
        this.AddComponent<Shooting>();
        _shooting = GetComponent<Shooting>();
        _shooting.firePoint = _mRb2d.transform;
        _shooting.bulletPrefab = _enemyAttackSo.projectile;
        _shooting.rateOfFire = _enemyAttackSo.rateOfFire;
        _shooting.damage = _enemyAttackSo.damage; //todo - variable for enemyAttackType

    }

    private void FixedUpdate()
    {
        // Movement
        var playerPosition = _mPlayerTransform.position;
        var enemyPosition = transform.position;
        var vectorToTarget = playerPosition - enemyPosition;
        
        // move towards player, in range
        var playerToEnemyDistance = Vector3.Distance(playerPosition, enemyPosition);
        if(playerToEnemyDistance >= _range)
            _mRb2d.MovePosition(_mRb2d.position + (Vector2)vectorToTarget * (_moveSpeed * Time.fixedDeltaTime));
        
        
        // rotate to look at player
        var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90; //z rotation. Note: Atan2 takes y first, then x, subtract 90degrees for sprite rotation
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation =  Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * RotationSpeed);
        
        // Shooting
        if (_enemyAttackSo.ranged != true) return;
        _shooting.UpdateEnemy();
    }

    // todo - should every enemy do 1 damage on collision??
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_range != 0) return; //todo - change this to not use rate of fire when more types are added
        if (collision.gameObject.tag is "Player")
        {
            //Debug.Log("Ouchies");
            healthManager.DecreaseHealth(enemySO.enemyAttackType.damage);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        Debug.Log("Health is: " + _currentHealth);

        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
