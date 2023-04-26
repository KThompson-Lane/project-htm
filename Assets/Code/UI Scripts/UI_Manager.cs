using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Timer;

    [SerializeField] private HealthManager _healthManager;

    [FormerlySerializedAs("deathScreen")] [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private GameObject heartPrefab;

    [SerializeField] private Transform healthBar;
    private List<HeartContainer> hearts;

    //  TODO: Move into a game manager
    public float TimeLimit;
    private float remainingTime;
    
    private static UI_Manager _instance;

    public static UI_Manager Instance
    {
        get
        {
            if(_instance is null)
                Debug.LogError("UI Manager is null");
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        hearts = new List<HeartContainer>();
    }

    private void Start()
    {
        ChangeHeartContainers(_healthManager.maxHealth);
        ChangeHealth(_healthManager.health);
        remainingTime = TimeLimit;
    }

    private void LateUpdate()
    {
        remainingTime -= Time.deltaTime;
        var time = TimeSpan.FromSeconds(remainingTime);
        Timer.text = time.ToString("mm':'ss");
        
        // If time runs out, show death screen
        if (time <= TimeSpan.FromSeconds(0))
        {
            ShowGameOverScreen(true);
        }
    }

    private void OnEnable()
    {
        // Listen for health changed trigger
        _healthManager.MaxHealthChangedEvent.AddListener(ChangeHeartContainers);
        _healthManager.HealthChangedEvent.AddListener(ChangeHealth);

    }
    
    private void OnDisable()
    {
        // Stop listening for health changed trigger
        _healthManager.MaxHealthChangedEvent.RemoveListener(ChangeHeartContainers);
        _healthManager.HealthChangedEvent.RemoveListener(ChangeHealth);

    }

    private void ChangeHealth(float amount)
    {
        var health = (int) amount;
        foreach (var heart in hearts)
        {
            switch (health)
            {
                case > 1:
                    heart.UpdateContainer(HeartStatus.Full);
                    break;
                case 1:
                    heart.UpdateContainer(HeartStatus.Half);
                    break;
                default:
                    heart.UpdateContainer(HeartStatus.Empty);
                    break;
            }
            health -= 2;
        }
    }
    private void ChangeHeartContainers(float amount)
    {
        ClearHeartContainers();
        //todo - can we do this without the loop?
        for (int i = 0; i < _healthManager.maxHealth /2; i++)
        {
            GameObject heart = Instantiate(heartPrefab, healthBar, true);
            var container = heart.GetComponent<HeartContainer>();
            hearts.Add(container);
        }
        
    }

    private void ClearHeartContainers()
    {
        hearts.Clear();
        foreach(Transform child in healthBar)
            Destroy(child.gameObject);
    }
    
    public void ShowGameOverScreen(bool show)
    {
        //gameOverScreen.SetActive(!deathScreen.activeSelf);
        gameOverScreen.SetActive(show);
    }
    
    //todo - move to level manager?
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //todo - change to first level
    }

    //  TODO: Implement
    //private TextMeshPro Player2Health; - will need a new health manager
   // private TextMeshPro Timer;
}
