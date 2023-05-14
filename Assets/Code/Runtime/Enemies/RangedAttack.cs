﻿using UnityEngine;

namespace Code.Runtime.Enemies
{
    public class RangedAttack : Attack
    {
        private EnemyRangedSO _enemyRangedSo;
        
        private Transform _firePoint;
        private GameObject _bulletPrefab;
        private float _bulletForce;
        private bool _isShooting;
        private float _angleSpread;
        private float _projectilePerBurst;
        private float _startingDistance;
        private float _bulletsInBurst;

        private float _waveStepAmount;
        private float currentWaveStep = 0;
        private bool alternateBurst = false;

        public override void Start()
        {
            _rateOfFire = _enemyRangedSo.rateOfFire;
            _damage = _enemyRangedSo.damage; 
            
            _bulletPrefab = _enemyRangedSo.projectile;
            _bulletForce = _enemyRangedSo.bulletForce;
            _startingDistance = _enemyRangedSo.startingDistance;

            _angleSpread = _enemyRangedSo.angleSpread;
            _bulletsInBurst = _enemyRangedSo.bulletsInBurst;

            _waveStepAmount = _enemyRangedSo.waveStepAmount;
            
            
            
            base.Start();
            
        }

        public void Update()
        {
            //Attack when not on cooldown
            if (_coolDown <= 0f)
            {
                Shoot2();
                _animator.SetTrigger("Attack");
                _coolDown = _interval;
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

        private void Shoot2()
        {
            if (_bulletsInBurst > 1)
            {
                MultiBulletCone(_waveStepAmount);   
            }
            else
                SingleBullet();
        }

        private void Shoot()
        {
        if (_angleSpread != 0)
        {
            // Shoot multiple bullets in a cone shape
            
            // Calc cone
            var firePointPosition = _firePoint.position; //todo - update fire point position!!
            var targetAngle = Mathf.Atan2(firePointPosition.y, firePointPosition.x) * Mathf.Rad2Deg; // find in game angle
            var angleStep = _angleSpread / (_bulletsInBurst);
            var halfAngleSpread = _angleSpread / 2f;
            var startAngle = targetAngle - halfAngleSpread;
            
            startAngle += currentWaveStep;
            currentWaveStep += 7.5f;
            
            
            //if (alternateBurst) // offset sideways alternating bursts
            //{
            //    startAngle += angleStep / 2;
            //   
            //    alternateBurst = false;
            //}
            //else
            //{
//
            //    alternateBurst = true;
            //}
            var currentAngle = startAngle;


            // Shoot bullets in bursts
            for (var i = 0; i < _bulletsInBurst; i++)
            {
                float dist = _startingDistance;
                
                
                //float dist; // offsets forwards alternating bursts of bullets
                
                //if (alternateBurst)
                //{
                //    dist = 0.5f;
                //    alternateBurst = false;
                //}
                //else
                //{
                //    dist = _startingDistance;
                //    alternateBurst = true;
                //}

                
                
                //if (i % 2 == 0) // offsets forwards alternating bullets
                //{
                //    dist = 0.5f;
                //}
                
                


                
                
                var pos = FindBulletSpawnLocation(currentAngle, dist);

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
            //// Create single bullet
            //var bullet = ObjectPooler.SharedInstance.GetPooledObject("EnemyBullet");
            //if (bullet == null)
            //    return;
            //bullet.transform.position = _firePoint.position;
            //bullet.transform.rotation = _firePoint.rotation;
            //bullet.SetActive(true);
            //bullet.GetComponent<Bullet>().SetDamage(_damage);
            //Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            //rb.AddForce(_firePoint.up * _bulletForce, ForceMode2D.Impulse); //todo - maybe change to move speed
        }
        }

        private void SingleBullet()
        {
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

        // Shoot multiple bullets in a cone shape
        private void MultiBulletCone(float stepAmount) //angle, ect
        {
            // Calc cone
            var firePointPosition = _firePoint.position;
            var targetAngle = Mathf.Atan2(firePointPosition.y, firePointPosition.x) * Mathf.Rad2Deg; // find in game angle
            var angleStep = _angleSpread / (_bulletsInBurst);
            var halfAngleSpread = _angleSpread / 2f;
            var startAngle = targetAngle - halfAngleSpread;

            if (stepAmount > 0)
            {
                if (currentWaveStep > startAngle + halfAngleSpread * 2)
                    currentWaveStep = stepAmount;
                
                startAngle += currentWaveStep;
                currentWaveStep += stepAmount;
            }

            var currentAngle = startAngle;


            // Shoot bullets in bursts
            for (var i = 0; i < _bulletsInBurst; i++)
            {
                float dist = _startingDistance;

                var pos = FindBulletSpawnLocation(currentAngle, dist);

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

        private void BulletWaveEffect()
        {
        }

        private Vector2 FindBulletSpawnLocation(float currentAngle, float distance)
        {
            var position = _firePoint.position;
            var x = position.x + distance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
            var y = position.y + distance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

            var newPosition = new Vector2(x, y);
        
            return newPosition;
        }
        
        public void SetEnemyRangedSO(EnemyRangedSO eso)
        {
            _enemyRangedSo = eso;
        }
    }
}