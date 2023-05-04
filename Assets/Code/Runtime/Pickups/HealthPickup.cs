using UnityEngine;

[CreateAssetMenu]
public class HealthPickup : MonoBehaviour
{
    public int healthValue = 2;
    
    [SerializeField]
    private HealthManager healthManager; // for player health

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag is "Player")
            PickUp();
    }

    private void PickUp()
    {
        // Increase Health
        healthManager.IncreaseHealth(healthValue);
        
        //Destroy pick up - todo only if health increased.
        Destroy(gameObject);
    }
}
