using UnityEngine;

[CreateAssetMenu(fileName = "HealthPickup", menuName = "ScriptableObject/HealthPickup")]
public class HealthPickup : PickupSO
{    
    [SerializeField] protected HealthManager healthManager; // for player health
    public int amount;
    
    public override bool Apply()
    {
        // Change Health
        return healthManager.IncreaseHealth(amount);
    }
}
