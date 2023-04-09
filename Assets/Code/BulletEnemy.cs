using UnityEngine;

public class BulletEnemy : Bullet
{
    [SerializeField]
    private HealthManager healthManager; // for player health
    
    private void OnCollisionEnter2D(Collision2D col)
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
}