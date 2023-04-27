using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class HealthManager : ScriptableObject //todo - probably rename this to be PlayerHealthManagerSO
{
    public float health = 3;
    
    [SerializeField]
    public float maxHealth = 3;

    [System.NonSerialized] public UnityEvent<float> HealthChangedEvent;
    [System.NonSerialized] public UnityEvent<float> MaxHealthChangedEvent;
    [System.NonSerialized] public UnityEvent<bool> HealthDepletedEvent;

    private void OnEnable()
    {
        // When game starts ensure health is set to max
        health = maxHealth;
        
        // Set up healthChangedEvent
        HealthChangedEvent ??= new UnityEvent<float>();
        MaxHealthChangedEvent ??= new UnityEvent<float>();
        HealthDepletedEvent ??= new UnityEvent<bool>();
    }

    public void DecreaseHealth(float amount)
    {
        health -= amount;
        
        // Trigger healthChangedEvent
        HealthChangedEvent.Invoke(health);

        if (!(health <= 0)) return;
        HealthDepletedEvent.Invoke(true);
        
        // Show death screen
        UI_Manager uiManager = UI_Manager.Instance;
        uiManager.ShowGameOverScreen(true);

        //reset health
        health = maxHealth;
        HealthChangedEvent.Invoke(health);
    }
}
