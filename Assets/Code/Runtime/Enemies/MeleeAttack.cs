using UnityEngine;

namespace Code.Runtime.Enemies
{
    public class MeleeAttack : Attack
    {
        // Melee - collision damage
        private bool _inCollision;
        private EnemyMeleeSO _enemyMeleeSo;

        public override void Start()
        {
            _rateOfFire = _enemyMeleeSo.rateOfFire;
            _damage = _enemyMeleeSo.damage; 
            
            base.Start();
        }

        public void Update()
        {
            //Attack when not on cooldown
            if (_coolDown <= 0f)
            {
                if (_inCollision)
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
        
        private void Hit()
        {
            healthManager.DecreaseHealth(_damage);
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
        
        public void SetEnemyMeleeSO(EnemyMeleeSO eso)
        {
            _enemyMeleeSo = eso;
        }
    }
}