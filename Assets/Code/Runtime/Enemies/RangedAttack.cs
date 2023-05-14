using UnityEngine;

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
        private float _currentWaveStep = 0;

        public override void Start()
        {
            RateOfFire = _enemyRangedSo.rateOfFire;
            Damage = _enemyRangedSo.damage; 
            
            // Bullet variables
            _bulletPrefab = _enemyRangedSo.projectile;
            _bulletForce = _enemyRangedSo.bulletForce;
            _startingDistance = _enemyRangedSo.startingDistance;

            // Multiple bullets variables
            _angleSpread = _enemyRangedSo.angleSpread;
            _bulletsInBurst = _enemyRangedSo.bulletsInBurst;
            _waveStepAmount = _enemyRangedSo.waveStepAmount;
            
            base.Start();
        }

        public void Update()
        {
            //Attack when not on cooldown
            if (CoolDown <= 0f)
            {
                Shoot();
                Animator.SetTrigger("Attack");
                CoolDown = Interval;
            }

            if (CoolDown > 0)
            {
                // update cooldown
                CoolDown -= Time.deltaTime;
            }
        }

        public void SetFirePoint(Transform point)
        {
            _firePoint = point;
        }

        private void Shoot()
        {
            if (_bulletsInBurst > 1)
            {
                MultiBulletCone(_waveStepAmount);   
            }
            else
                SingleBullet();
        }

        private void SingleBullet()
        {
            var bullet = ObjectPooler.SharedInstance.GetPooledObject("EnemyBullet");
            if (bullet == null)
                return;
            bullet.transform.position = _firePoint.position;
            bullet.transform.rotation = _firePoint.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().SetDamage(Damage);
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
                if (_currentWaveStep > startAngle + halfAngleSpread * 2)
                    _currentWaveStep = stepAmount;
                
                startAngle += _currentWaveStep;
                _currentWaveStep += stepAmount;
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
                bullet.GetComponent<Bullet>().SetDamage(Damage);
                if (bullet.TryGetComponent(out BulletEnemy bulletEnemy))
                {
                    bulletEnemy.SetMoveSpeed(_bulletForce);
                }
                
                currentAngle += angleStep;
            }
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