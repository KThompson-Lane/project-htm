using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected int Damage;
    private void OnTriggerEnter2D(Collider2D col)
    {
        var enemyController = col.gameObject.GetComponent<EnemyController>();
        
        if (enemyController != null)
        {
            enemyController.TakeDamage(Damage); //todo - variable damage
        }
        
        Destroy(gameObject);
    }

    private void Start()
    {
        //Ensure bullet is destroyed after a set time so they don't linger
        Destroy(gameObject, 10f);
    }
    
    public void SetDamage(int damage)
    {
        Damage = damage;
    }
}
