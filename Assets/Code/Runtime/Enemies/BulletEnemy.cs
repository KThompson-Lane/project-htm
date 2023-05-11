using System;
using UnityEngine;

public class BulletEnemy : Bullet
{
    [SerializeField]
    private HealthManager healthManager; // for player health
    
    private float _moveSpeed = 0; //todo - accessor!!!
    
    private void Update()
    {
        MoveProjectile();
    }
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

        _moveSpeed = 0;
        gameObject.SetActive(false);
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * _moveSpeed);
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }
}