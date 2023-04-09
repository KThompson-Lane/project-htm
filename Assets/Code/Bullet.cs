using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected float Damage;
    private void OnTriggerEnter2D(Collider2D col)
    {
        var enemyController = col.gameObject.GetComponent<EnemyController>();
        
        if (enemyController != null)
        {
            enemyController.TakeDamage(1); //todo - variable damage
        }
        
        Destroy(gameObject);
    }

    private void Start()
    {
        //Ensure bullet is destroyed after a set time so they don't linger
        Destroy(gameObject, 10f);
    }
    
    public void SetDamage(float damage)
    {
        Damage = damage;
    }
}
