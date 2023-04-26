using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI Player1Health;

    [SerializeField] private TextMeshProUGUI Timer;

    [SerializeField] private HealthManager _healthManager;

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
        ChangeHeartContainers(_healthManager.health);
        remainingTime = TimeLimit;
    }

    private void LateUpdate()
    {
        remainingTime -= Time.deltaTime;
        var time = TimeSpan.FromSeconds(remainingTime);
        Timer.text = time.ToString("mm':'ss");
    }

    private void OnEnable()
    {
        // Listen for health changed trigger
        _healthManager.HealthChangedEvent.AddListener(ChangeHeartContainers);
    }
    
    private void OnDisable()
    {
        // Stop listening for health changed trigger
        _healthManager.HealthChangedEvent.RemoveListener(ChangeHeartContainers);
    }

    private void ChangeHeartContainers(float amount)
    {
        float newHP = _healthManager.health;
        //todo - can we do this without the loop?
        for (int i = 0; i < _healthManager.maxHealth /2; i++)
        {
            GameObject heart = Instantiate(heartPrefab, healthBar, true);
            var container = heart.GetComponent<HeartContainer>();
            container.UpdateContainer(HeartStatus.Full);
            hearts.Add(container);
        }
        
    }

    private void ClearHeartContainers()
    {
        hearts.Clear();
        foreach(Transform child in healthBar)
            Destroy(child.gameObject);
    }
    
    //  TODO: Implement
    //private TextMeshPro Player2Health; - will need a new health manager
   // private TextMeshPro Timer;
}
