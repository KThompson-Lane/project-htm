using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected int totalHealth;
    private int _currentHealth;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _currentHealth = totalHealth;
        Debug.Log("Health is: " + totalHealth);
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
