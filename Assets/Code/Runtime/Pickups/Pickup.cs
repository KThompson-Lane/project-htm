using System;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private PickupSO pickupSo;

    public UnityEvent<Vector3> OnPickupApply;

    private void Awake()
    {
        OnPickupApply = new UnityEvent<Vector3>();
    }

    public void SetPickup(PickupSO _pickupSo)
    {
        pickupSo = _pickupSo;
        GetComponent<SpriteRenderer>().sprite = _pickupSo.GetSprite();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        bool destroy = false;
        if (col.gameObject.tag is not "Player") return;
        destroy = pickupSo.Apply();

        if (destroy)
        {
            OnPickupApply?.Invoke(transform.position);
            OnPickupApply?.RemoveAllListeners();
            gameObject.SetActive(false); //todo - only if heal works
        }
    }
}
