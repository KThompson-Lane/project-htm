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

    private void OnEnable()
    {
        // When game starts ensure health is set to max
        health = maxHealth;
        // Set up healthChangedEvent
        HealthChangedEvent ??= new UnityEvent<float>();
    }

    public void DecreaseHealth(float amount)
    {
        health -= amount;
        // Trigger healthChangedEvent
        HealthChangedEvent.Invoke(health);

        if (!(health <= 0)) return;
        //reset game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //todo - change this to first level
        health = maxHealth;
        HealthChangedEvent.Invoke(health);
    }
}
