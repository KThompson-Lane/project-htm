using UnityEngine;

namespace Code.Runtime.Enemies
{
    public class MeleeAttack : Attack
    {
        // Melee - collision damage
        private bool _inCollision;
        private EnemyMeleeSO _enemyMeleeSo;
        private EnemySO _so;
        private GameObject _enemy;

        public override void Start()
        {
            RateOfFire = _enemyMeleeSo.rateOfFire;
            Damage = _enemyMeleeSo.damage; 
            
            base.Start();
        }

        public void Update()
        {
            //Attack when not on cooldown
            if (CoolDown <= 0f)
            {
                if (_inCollision)
                {
                    Hit();
                    Animator.SetTrigger("Attack");
                    CoolDown = Interval;
                }
            }

            if (CoolDown > 0)
            {
                // update cooldown
                CoolDown -= Time.deltaTime;
            }
        }
        
        private void Hit()
        {
            healthManager.DecreaseHealth(Damage, _so);
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
        
        public void SetSO(EnemySO so)
        {
            _so = so;
        }
        
        public void SetEnemy(GameObject enemy)
        {
            _enemy = enemy;
        }
    }
}