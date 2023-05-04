using UnityEngine;

[CreateAssetMenu(fileName = "HealthPickup", menuName = "ScriptableObject/HealthPickup")]
public class HealthPickup : PickupSO
{
    public int amount;
    
    public override bool Apply()
    {
        // Change Health
        return healthManager.IncreaseHealth(amount);
        
        //todo - Destroy pick up if health changed

    }
}
