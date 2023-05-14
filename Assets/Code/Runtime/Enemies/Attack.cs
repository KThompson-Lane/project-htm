using Code.Runtime;
using UnityEngine;

public abstract class Attack : MonoBehaviour // Used for Enemy Attacking
{
    private EnemyAttackSO _enemyAttackSo;
    //private bool _attackType; //todo - change to enum, currently true is ranged

    protected float _rateOfFire;
    protected int _damage;
    
    protected float _coolDown = 0.0f;
    protected float _interval; // time between shots
    
    [SerializeField]
    protected HealthManager healthManager; // for player health
    
    // Ranged - shooting
    //private Transform _firePoint;
    //private GameObject _bulletPrefab;
    //private float _bulletForce;
    //private bool _isShooting;
    //private float _angleSpread;
    //private float _projectilePerBurst;
    //private float _startingDistance;
    //private float _bulletsInBurst;
//
    //private float test = 0;
    //private bool alternateBurst = false;
    
    // Melee - collision damage
    //private bool _inCollision;
    
    protected Animator _animator;

    public virtual void Start()
    {
        //_rateOfFire = _enemyAttackSo.rateOfFire;



        //_damage = _enemyAttackSo.damage;  
        //_attackType = _enemyAttackSo.ranged;
        //_bulletPrefab = _enemyAttackSo.projectile;
        //_bulletForce = _enemyAttackSo.bulletForce;
        //_startingDistance = _enemyAttackSo.startingDistance;
//
        //_angleSpread = _enemyAttackSo.angleSpread;
        //_bulletsInBurst = _enemyAttackSo.bulletsInBurst;

        _interval = 60 / _rateOfFire; //1 second / rate of fire
        _animator = GetComponent<Animator>();
    }

    public void Update()
    {
        //Attack when not on cooldown
        //if (_coolDown <= 0f)
        //{
        //    if (_attackType)
        //    {
        //        Shoot();
        //        _animator.SetTrigger("Attack");
        //        _coolDown = _interval;
        //    }
        //    else if (_inCollision)
        //    {
        //        Hit();
        //        _animator.SetTrigger("Attack");
        //        _coolDown = _interval;
        //    }
        //}
//
        //if (_coolDown > 0)
        //{
        //    // update cooldown
        //    _coolDown -= Time.deltaTime;
        //}

        //_firePoint.Rotate(new Vector3(0,0,10));
    }

    

    public void SetHealthManager(HealthManager hm)
    {
        healthManager = hm;
    }

    public void SetEnemyAttackSO(EnemyAttackSO eso)
    {
        _enemyAttackSo = eso;
    }

    

    

    

    
}