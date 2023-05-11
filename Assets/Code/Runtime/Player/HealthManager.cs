using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class HealthManager : ScriptableObject //todo - probably rename this to be PlayerHealthManagerSO
{
    public float health = 3;
    
    [SerializeField]
    public float maxHealth = 3;
    
    private bool _godMode = false;

    [NonSerialized] public UnityEvent<float> HealthChangedEvent; //Note: currently only when health is lost/reset
    [NonSerialized] public UnityEvent<float> MaxHealthChangedEvent;
    [NonSerialized] public UnityEvent HealthDepletedEvent;

    private void OnEnable()
    {
        // When game starts ensure health is set to max
        health = maxHealth;
        
        // Set up events
        HealthChangedEvent ??= new UnityEvent<float>();
        MaxHealthChangedEvent ??= new UnityEvent<float>();
        HealthDepletedEvent ??= new UnityEvent();
    }

    public void DecreaseHealth(int amount)
    {
        if (_godMode) return; //ensure we are not in god mode
        health -= amount;
        // Trigger healthChangedEvent
        HealthChangedEvent.Invoke(health);

        if (!(health <= 0)) return;
        HealthDepletedEvent.Invoke();
    }

    public bool IncreaseHealth(int amount) //todo - could combine with decrease and have ModifyHealth
    {
        // Ensure health is not already full
        if (health >= maxHealth)
            return false;

        // Ensure new health value isn't above max health, or below zero
        health = Math.Clamp(health + amount, 0, maxHealth);
        
        // Trigger healthChangedEvent
        HealthChangedEvent.Invoke(health);
        return true;
    }

    public void ResetHealth()
    {
        //reset health
        health = maxHealth;
        HealthChangedEvent.Invoke(health);
    }

    public void ToggleGodMode()
    {
        _godMode = !_godMode;
        Debug.Log("God Mode: " + _godMode);
    }
}
