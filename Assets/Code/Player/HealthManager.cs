using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class HealthManager : ScriptableObject //todo - probably rename this to be PlayerHealthManager
{
    public int health = 3;
    
    [SerializeField]
    public int maxHealth = 3;

    [System.NonSerialized] public UnityEvent<int> HealthChangedEvent;

    private void OnEnable()
    {
        // When game starts ensure health is set to max
        health = maxHealth;
        // Set up healthChangedEvent
        HealthChangedEvent ??= new UnityEvent<int>();
    }

    public void DecreaseHealth(int amount)
    {
        health -= amount;
        // Trigger healthChangedEvent
        HealthChangedEvent.Invoke(health);
    }
}
