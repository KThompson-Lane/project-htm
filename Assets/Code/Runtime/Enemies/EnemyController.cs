using Code.Runtime.Enemies;
using Codice.Client.Common;
using UnityEngine;
using Time = UnityEngine.Time;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemySO enemySO;

    public delegate void EnemyDied(Vector3 deathPosition);
    public static event EnemyDied OnDie;
    
    public HealthManager healthManager;

    private EnemyAttackSO[] _enemyAttackSOs;

    private float _currentHealth;

    private float _moveSpeed;
    private float _rotationSpeed;
    private float _range;
    
    private GameObject _enemy;

    private Rigidbody2D _mRb2d;
    private Animator _animator;

    private Transform _mPlayerTransform; // for look at location

    private Sprite _sprite;

    // Start is called before the first frame update
    public void Initialise(EnemySO enemy)
    {
        enemySO = enemy;
        _enemyAttackSOs = enemySO.enemyAttackTypes;
        _enemy = gameObject;
        
        InitializeAttacks();

        // sprite
        _sprite = enemySO.enemySprite;
        GetComponent<SpriteRenderer>().sprite = _sprite;
        
        //Create polygon collider after setting sprite
        gameObject.AddComponent<PolygonCollider2D>();

        // health 
        _currentHealth = enemySO.GetMaxHealth();
        
        // movement
        _moveSpeed = enemySO.GetSpeed();
        _rotationSpeed = enemySO.GetRotationSpeed();
        _range = enemySO.moveRange;
        _mRb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = enemy.enemyAnimator;
    }

    private void Start()
    {
        if(_mRb2d == null)
            Initialise(enemySO);
        _mPlayerTransform = GameObject.FindWithTag("Player").transform; //todo - might want to change if not all enemies follow player
    }

    private void FixedUpdate()
    {
        // Movement
        var playerPosition = _mPlayerTransform.position;
        var enemyPosition = transform.position;
        var vectorToTarget = playerPosition - enemyPosition;

        // move towards player, in range
        var playerToEnemyDistance = Vector3.Distance(playerPosition, enemyPosition);
        if (playerToEnemyDistance >= _range)
        {
            //  Move this
            _mRb2d.MovePosition(_mRb2d.position + (Vector2)vectorToTarget * (_moveSpeed * Time.fixedDeltaTime));
            _animator.SetBool("Moving", true);
        }
        else
        {
            _animator.SetBool("Moving", false);
        }

        
        var vec = new Vector2(0, 0);
        _mRb2d.velocity = vec;

        // rotate to look at player
        var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90; //z rotation. Note: Atan2 takes y first, then x, subtract 90degrees for sprite rotation
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation =  Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * _rotationSpeed);
    }

    private void InitializeAttacks()
    {
        foreach (var attack in _enemyAttackSOs)
        {
            if (attack == null)
                return;

            switch (attack)
            {
                case EnemyMeleeSO so:
                {
                    var attackScript = gameObject.AddComponent<MeleeAttack>();
                    attackScript.SetHealthManager(healthManager);
                    attackScript.SetEnemyMeleeSO(so);
                    break;
                }
                case EnemyRangedSO so:
                {
                    var attackScript = gameObject.AddComponent<RangedAttack>();
                    attackScript.SetHealthManager(healthManager);
                    attackScript.SetEnemyRangedSO(so);
                    attackScript.SetEnemy(_enemy);
                    break;
                }
            }
        }
    }

    public virtual void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        Debug.Log("Health is: " + _currentHealth);
        _animator.SetTrigger("Hit");
        if (!(_currentHealth <= 0)) return;
        OnDie?.Invoke(transform.position);
        Destroy(gameObject);
    }
}
