using UnityEngine;

[CreateAssetMenu(fileName = "RoFPickup", menuName = "ScriptableObject/RoFPickup")]
public class RoFPickup : PickupSO
{
    public int amount;

    public override bool Apply()
    {
        // Change RoF
        playerAttackSo.ModifyRoF(amount);
        return true; //todo - this
    }
}
