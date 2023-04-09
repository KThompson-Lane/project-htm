using System;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private EnemyAttackSO _enemyAttackSo;
    private bool _attackType; //todo - change to enum, currently true is ranged

    private float _rateOfFire;
    private float _damage;
    
    private float _coolDown = 0.0f;
    private float _interval; // time between shots
    
    [SerializeField]
    private HealthManager healthManager; // for player health
    
    // Ranged - shooting
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce;
    private bool _isShooting;
    
    // Melee - collision damage
    private bool _inCollision;

    public void Start()
    {
        _enemyAttackSo = GetComponent<EnemyController>().GetEnemyAttackSO();
        
        _rateOfFire = _enemyAttackSo.rateOfFire;
        _damage = _enemyAttackSo.damage;
        _attackType = _enemyAttackSo.ranged;
        
        _interval = 60 / _rateOfFire; //1 second / rate of fire
    }

    public void Update()
    {
        //Attack when not on cooldown
        if (_coolDown <= 0f)
        {
            if (_attackType)
            {
                Shoot();
                _coolDown = _interval;
            }
            else if (_inCollision)
            {
                Hit();
                _coolDown = _interval;
            }
        }

        if (_coolDown > 0)
        {
            // update cooldown
            _coolDown -= Time.deltaTime;
        }
    }

    private void Hit()
    {
        healthManager.DecreaseHealth(_damage);
    }

    private void Shoot()
    {
        //Create bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().SetDamage(_damage);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
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