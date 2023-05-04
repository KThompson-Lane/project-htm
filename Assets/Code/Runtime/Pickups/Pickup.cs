using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private PickupSO pickupSo;
    
    [SerializeField]
    private PlayerAttackSO playerAttackSo;
    
    private int _health;
    private int _damage;
    private float _rateOfFire;
    private bool _invulnerable; //todo - add

    [SerializeField]
    private HealthManager healthManager; // for player health

    private void Start()
    {
        _health = pickupSo.health;
        _damage = pickupSo.damage;
        _rateOfFire = pickupSo.rateOfFire;
        _invulnerable = pickupSo.invulnerable;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag is "Player")
            PickUp();
    }

    private void PickUp()
    {
        bool destroy = false;
        if (_health != 0)
        {
            // Increase Health
            healthManager.IncreaseHealth(_health);
            
            //todo - if changed
            destroy = true;
        }

        if (_damage != 0)
        {
            playerAttackSo.ModifyDamage(_damage);
            destroy = true;
        }

        if (_rateOfFire != 0)
        {
            playerAttackSo.ModifyRoF(_rateOfFire);
            destroy = true;
        }

        //Destroy pick up
        if(destroy)
            Destroy(gameObject);
    }
}
