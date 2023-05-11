using Code.Runtime;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private EnemyAttackSO _enemyAttackSo;
    private bool _attackType; //todo - change to enum, currently true is ranged

    private float _rateOfFire;
    private int _damage;
    
    private float _coolDown = 0.0f;
    private float _interval; // time between shots
    
    [SerializeField]
    private HealthManager healthManager; // for player health
    
    // Ranged - shooting
    private Transform _firePoint;
    private GameObject _bulletPrefab;
    private float _bulletForce;
    private bool _isShooting;
    private float _angleSpread;
    private float _projectilePerBurst;
    private float _startingDistance;
    private float _bulletsInBurst;
    
    // Melee - collision damage
    private bool _inCollision;
    
    private Animator _animator;

    public void Start()
    {
        _rateOfFire = _enemyAttackSo.rateOfFire;
        _damage = _enemyAttackSo.damage;
        _attackType = _enemyAttackSo.ranged;
        _bulletPrefab = _enemyAttackSo.projectile;
        _bulletForce = _enemyAttackSo.bulletForce;

        _angleSpread = _enemyAttackSo.angleSpread;
        _bulletsInBurst = _enemyAttackSo.bulletsInBurst;

        _interval = 60 / _rateOfFire; //1 second / rate of fire
        _animator = GetComponent<Animator>();
    }

    public void Update()
    {
        //Attack when not on cooldown
        if (_coolDown <= 0f)
        {
            if (_attackType)
            {
                Shoot();
                _animator.SetTrigger("Attack");
                _coolDown = _interval;
            }
            else if (_inCollision)
            {
                Hit();
                _animator.SetTrigger("Attack");
                _coolDown = _interval;
            }
        }

        if (_coolDown > 0)
        {
            // update cooldown
            _coolDown -= Time.deltaTime;
        }
    }

    public void SetBulletForce(float force)
    {
        _bulletForce = force;
    }

    public void SetFirePoint(Transform point)
    {
        _firePoint = point;
    }

    public void SetHealthManager(HealthManager hm)
    {
        healthManager = hm;
    }

    public void SetEnemyAttackSO(EnemyAttackSO eso)
    {
        _enemyAttackSo = eso;
    }

    private void Hit()
    {
        healthManager.DecreaseHealth(_damage);
    }

    private void Shoot()
    {
        if (_angleSpread != 0)
        {
            // Shoot multiple bullets in a cone shape
            
            // Calc cone
            var firePointPosition = _firePoint.position;
            var targetAngle = Mathf.Atan2(firePointPosition.y, firePointPosition.x) * Mathf.Rad2Deg; // find in game angle
            var angleStep = _angleSpread / (_bulletsInBurst);
            var halfAngleSpread = _angleSpread / 2f;
            var startAngle = targetAngle - halfAngleSpread;
            var currentAngle = startAngle;


            // Shoot bullets in bursts
            for (var i = 0; i < _bulletsInBurst; i++)
            {
                var pos = FindBulletSpawnLocation(currentAngle);

                // Create bullet
                var bullet = ObjectPooler.SharedInstance.GetPooledObject("EnemyBullet");
                if (bullet == null)
                    return;
                bullet.transform.position = pos;
                bullet.transform.rotation = Quaternion.identity;
                bullet.SetActive(true);
                bullet.transform.right = bullet.transform.position - firePointPosition;
                bullet.GetComponent<Bullet>().SetDamage(_damage);
                if (bullet.TryGetComponent(out BulletEnemy bulletEnemy))
                {
                    bulletEnemy.SetMoveSpeed(_bulletForce);
                }
                
                currentAngle += angleStep;
            }
        }
        else
        {
            // Create single bullet
            var bullet = ObjectPooler.SharedInstance.GetPooledObject("EnemyBullet");
            if (bullet == null)
                return;
            bullet.transform.position = _firePoint.position;
            bullet.transform.rotation = _firePoint.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().SetDamage(_damage);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(_firePoint.up * _bulletForce, ForceMode2D.Impulse); //todo - maybe change to move speed
        }
    }

    private Vector2 FindBulletSpawnLocation(float currentAngle)
    {
        var position = _firePoint.position;
        var x = position.x + 0.1f * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        var y = position.y + 0.1f * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        var newPosition = new Vector2(x, y);
        
        return newPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag is "Player")
        {
            _inCollision = true;
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag is "Player")
        {
            _inCollision = false;
        }
    }
}