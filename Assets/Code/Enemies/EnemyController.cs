using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemySO enemySO;

    private float _currentHealth;
    
    [SerializeField] 
    private float RotationSpeed = 3.0f; //todo - might want to move to SO, might want to have this fixed if it seems right, or maybe a func of move speed
    
    private float _moveSpeed;
    
    private Rigidbody2D _mRb2d;
    private Transform _mPlayerTransform; // for look at location

    // Damage on collision variables
    [SerializeField]
    private HealthManager healthManager; // for player health

    // Start is called before the first frame update
    void Start()
    {
        // health 
        _currentHealth = enemySO.GetMaxHealth();
        
        // movement
        _moveSpeed = enemySO.GetSpeed();
        _mRb2d = GetComponent<Rigidbody2D>();
        _mPlayerTransform = GameObject.FindWithTag("Player").transform; //todo - might want to add to SO so not all enemies follow player
    }

    private void FixedUpdate()
    {
        var position = _mPlayerTransform.position;
        _mRb2d.MovePosition(Vector2.MoveTowards(_mRb2d.position, position, _moveSpeed*Time.fixedDeltaTime));
        
        Vector3 vectorToTarget = position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90; //z rotation. Note: Atan2 takes y first, then x, subtract 90degrees for sprite rotation
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation =  Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * RotationSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //todo - select type of attack based on enemyAttackType
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
