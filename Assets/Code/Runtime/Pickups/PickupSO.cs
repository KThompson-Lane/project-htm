using UnityEngine;

[CreateAssetMenu(fileName = "PickupSO", menuName = "ScriptableObject/Pickup")]
public abstract class PickupSO : ScriptableObject
{
    [SerializeField] protected PlayerAttackSO playerAttackSo;
    [SerializeField] protected HealthManager healthManager; // for player health

    public abstract bool Apply();
}
