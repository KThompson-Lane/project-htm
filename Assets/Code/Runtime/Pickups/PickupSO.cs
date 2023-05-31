using UnityEngine;

[CreateAssetMenu(fileName = "PickupSO", menuName = "ScriptableObject/Pickup")]
public abstract class PickupSO : ScriptableObject
{
    [SerializeField] protected Sprite pickupSprite;
    public Sprite GetSprite() => pickupSprite;

    public abstract bool Apply();
}
