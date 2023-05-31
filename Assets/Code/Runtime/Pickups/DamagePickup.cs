using UnityEngine;

[CreateAssetMenu(fileName = "DamagePickup", menuName = "ScriptableObject/DamagePickup")]
public class DamagePickup : PickupSO
{
    [SerializeField] protected PlayerAttackSO playerAttackSo;
    public int amount;

    public override bool Apply()
    {
        // Change Damage
        playerAttackSo.ModifyDamage(amount);
        return true;
    }
}
