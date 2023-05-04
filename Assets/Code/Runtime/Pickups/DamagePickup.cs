using UnityEngine;

[CreateAssetMenu(fileName = "DamagePickup", menuName = "ScriptableObject/DamagePickup")]
public class DamagePickup : PickupSO
{
    public int amount;

    public override bool Apply()
    {
        // Change Damage
        playerAttackSo.ModifyDamage(amount);
        return true; //todo - this
    }
}
