using UnityEngine;

//todo - use this!
public class PlayerAttackSO : ScriptableObject
{
    public float damage = 1f;
    public float range = 0f;
    public float rateOfFire = 120f;
    
    public void ModifyDamage(float amount)
    {
        damage += amount;
    }
    
    public void ModifyRoF(float amount)
    {
        rateOfFire += amount;
    }
}
