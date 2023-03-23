using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float _totalHealth;
    private float _currentHealth;
    
    [SerializeField]
    private EnemySO enemySO; //Note - this may change in future if other things that take damage don't use enemySO

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _totalHealth = enemySO.GetMaxHealth();
        _currentHealth = _totalHealth;
        Debug.Log("Health is: " + _totalHealth);
    }

    protected void OnEnable()
    {
        
    }

    public virtual void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        Debug.Log("Health is: " + _currentHealth);

        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
