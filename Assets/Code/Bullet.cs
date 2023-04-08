using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected float _damage;
    private void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);

        var enemyController = col.gameObject.GetComponent<EnemyController>();
        
        if (enemyController != null)
        {
            enemyController.TakeDamage(1); //todo - variable damage
        }
    }

    private void Start()
    {
        //Ensure bullet is destroyed after a set time so they don't linger
        Destroy(gameObject, 10f);
    }
    
    public void SetDamage(float damage)
    {
        _damage = damage;
    }
}
