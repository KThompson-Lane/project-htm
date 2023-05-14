using System;
using Codice.Client.BaseCommands;
using UnityEngine;

public class BulletEnemy : Bullet
{
    [SerializeField]
    private HealthManager healthManager; // for player health

    private GameObject _owner;

    private float _moveSpeed = 0;
    
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

        var playerController = col.gameObject.GetComponent<PlayerController>();
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

    public void SetOwner(GameObject owner)
    {
        _owner = owner;
    }
}