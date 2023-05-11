using System;
using UnityEngine;

public class BulletEnemy : Bullet
{
    [SerializeField]
    private HealthManager healthManager; // for player health

    public float moveSpeed = 0; //todo - accessor!!!
    private void OnTriggerEnter2D(Collider2D col)
    {
        //todo - make this better!!
        var colController = col.gameObject.GetComponent<EnemyController>();
        if (colController != null) // don't destroy if collides with another enemy - used to also get around the fire point issue
            return;

        var playerController = col.gameObject.GetComponent<PlayerMovement>();
        if (playerController != null)
        {
            healthManager.DecreaseHealth(Damage);
        }
        
        Destroy(gameObject);
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }

    private void Update()
    {
        MoveProjectile();
    }
}