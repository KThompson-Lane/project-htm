using System;
using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI Player1Health;

    [SerializeField] private HealthManager _healthManager;

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
    }

    private void Start()
    {
        ChangeHeartContainers(_healthManager.health);
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
        Player1Health.text = "";
        //todo - can we do this without the loop?
        for (int i = 0; i < newHP; i+=2)
        {
            Player1Health.text += "<sprite index=1>";
        }
    }

    //  TODO: Implement
    //private TextMeshPro Player2Health; - will need a new health manager
   // private TextMeshPro Timer;
}
