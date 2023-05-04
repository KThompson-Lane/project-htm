using UnityEngine;

[CreateAssetMenu(fileName = "PickupSO", menuName = "ScriptableObject/Pickup")]
public class PickupSO : ScriptableObject
{
    public int health;
    public int damage;
    public float rateOfFire;
    public bool invulnerable;
    
    [SerializeField]
    private HealthManager healthManager;
    
    
    
    
    
    
}
