using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private PickupSO pickupSo;
    
    [SerializeField]
    private PlayerAttackSO playerAttackSo;
    
    private float _health;
    private float _damage;
    private float _rateOfFire;
    private bool _invulnerable; //todo - add
    private float[] _modifiers;
    
    [SerializeField]
    private HealthManager healthManager; // for player health

    private void Start()
    {
        _health = pickupSo.health;
        _damage = pickupSo.damage;
        _rateOfFire = pickupSo.rateOfFire;
        _invulnerable = pickupSo.invulnerable;
        
        _modifiers = new []{_health, _damage, _rateOfFire};

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag is "Player")
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
