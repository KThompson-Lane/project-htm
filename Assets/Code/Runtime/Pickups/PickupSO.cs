using UnityEngine;

public class PickupSO : ScriptableObject
{
    public float health;
    public float damage;
    public float rateOfFire;
    public bool invulnerable;
    
    [SerializeField]
    private HealthManager healthManager;
    
    
    
    
    
    
}
