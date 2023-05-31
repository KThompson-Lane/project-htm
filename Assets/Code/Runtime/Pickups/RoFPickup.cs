using UnityEngine;

[CreateAssetMenu(fileName = "RoFPickup", menuName = "ScriptableObject/RoFPickup")]
public class RoFPickup : PickupSO
{
    [SerializeField] protected PlayerAttackSO playerAttackSo;
    public int amount;

    public override bool Apply()
    {
        // Change RoF
        playerAttackSo.ModifyRoF(amount);
        return true;
    }
}
