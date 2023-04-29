using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class HealthManager : ScriptableObject //todo - probably rename this to be PlayerHealthManagerSO
{
    public float health = 3;
    
    [SerializeField]
    public float maxHealth = 3;
    
    private bool _godMode = false;

    [System.NonSerialized] public UnityEvent<float> HealthChangedEvent;
    [System.NonSerialized] public UnityEvent<float> MaxHealthChangedEvent;
    [System.NonSerialized] public UnityEvent HealthDepletedEvent;

    private void OnEnable()
    {
        // When game starts ensure health is set to max
        health = maxHealth;
        
        // Set up events
        HealthChangedEvent ??= new UnityEvent<float>();
        MaxHealthChangedEvent ??= new UnityEvent<float>();
        HealthDepletedEvent ??= new UnityEvent();
    }

    public void DecreaseHealth(float amount)
    {
        if (_godMode) return; //ensure we are not in god mode
        health -= amount;
        // Trigger healthChangedEvent
        HealthChangedEvent.Invoke(health);

        if (!(health <= 0)) return;
        HealthDepletedEvent.Invoke();

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
