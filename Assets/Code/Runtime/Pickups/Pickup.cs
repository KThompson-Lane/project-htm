using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private PickupSO pickupSo;

    private void OnTriggerEnter2D(Collider2D col)
    {
        bool destroy = false;
        if (col.gameObject.tag is not "Player") return;
        destroy = pickupSo.Apply();
        
        if(destroy)
            Destroy(gameObject); //todo - only if heal works
    }
}
