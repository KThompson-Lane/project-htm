using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private PickupSO pickupSo;

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
        
        if(destroy)
            gameObject.SetActive(false); //todo - only if heal works
    }
}
