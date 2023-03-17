using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : ScriptableObject
{
    public int health = 3;
    
    [SerializeField]
    public int maxHealth = 3;

    [System.NonSerialized] public UnityEvent<int> healthChangedEvent;

    private void OnEnable()
    {
        // When game starts ensure health is set to max
        health = maxHealth;
        // Set up healthChangedEvent
        healthChangedEvent ??= new UnityEvent<int>();
    }

    public void DecreaseHealth(int amount)
    {
        health -= amount;
        // Trigger healthChangedEvent
        healthChangedEvent.Invoke(health);
    }
}
