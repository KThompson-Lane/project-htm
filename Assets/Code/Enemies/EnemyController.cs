using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemySO enemySO;

    private EnemyAttackSO enemyAttackSO;

    private Shooting shooting;

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
        enemyAttackSO = enemySO.enemyAttackType;
        shooting = GetComponent<Shooting>();
        
        // health 
        _currentHealth = enemySO.GetMaxHealth();
        
        // movement
        _moveSpeed = enemySO.GetSpeed();
        _range = enemyAttackSO.range;
        _mRb2d = GetComponent<Rigidbody2D>();
        _mPlayerTransform = GameObject.FindWithTag("Player").transform; //todo - might want to change if not all enemies follow player
        
        // shooting
        if (enemyAttackSO.ranged != true) return;
        this.AddComponent<Shooting>();
        shooting = GetComponent<Shooting>();
        shooting.firePoint = _mRb2d.transform;
        shooting.bulletPrefab = enemyAttackSO.projectile;
        shooting.rateOfFire = enemyAttackSO.rateOfFire;
        shooting.damage = enemyAttackSO.damage; //todo - variable for enemyAttackType

    }

    private void FixedUpdate()
    {
        // movement
        var playerPosition = _mPlayerTransform.position;
        var enemyPosition = transform.position;

        //todo - LOOK AT THIS WTF IS GOING ON
        var moveToPosition = playerPosition + (Vector3.Normalize(enemyPosition - playerPosition) * _range); // Note - this alone means they will move back if player is too close
        var dist = Vector3.Distance(playerPosition, enemyPosition);
        var test = Vector3.Distance(playerPosition, moveToPosition);
        if(dist >= test)
            _mRb2d.MovePosition(Vector2.MoveTowards(_mRb2d.position, moveToPosition, _moveSpeed*Time.fixedDeltaTime));
        
        Vector3 vectorToTarget = playerPosition - enemyPosition;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90; //z rotation. Note: Atan2 takes y first, then x, subtract 90degrees for sprite rotation
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation =  Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * RotationSpeed);
        
        // shooting
        if (enemyAttackSO.ranged != true) return;
        shooting.UpdateEnemy();
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

    public EnemySO GetEnemySo()
    {
        return enemySO;
    }
}
