using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Timer;

    [SerializeField] private HealthManager _healthManager;
    [SerializeField] private GameObject heartPrefab;

    [SerializeField] private Transform healthBar;
    private List<HeartContainer> hearts;

    [SerializeField] private EndScreen endScreen;

    //  Change this
    public Animator transitionAnimator;
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
    }

    private void LateUpdate()
    {
        var remainingTime = GameManager.instance.GetRemainingTime();
        var time = TimeSpan.FromSeconds(remainingTime);
        Timer.text = time.ToString("mm':'ss");
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
    
    // Heart Containers

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
    
    // Screens
    public void ShowEndScreen(bool win, float duration, int kills, int roomsCleared, int levelsCleared, [CanBeNull] EnemySO attacker)
    {
        endScreen.ShowEndScreen(win, duration, kills, roomsCleared, levelsCleared, attacker);
    }

    // Transition animation
    public IEnumerator BeginTransition()
    {
        transitionAnimator.SetTrigger("Start");
        do
        {
            yield return null;
        } while (!transitionAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Start"));
        do
        {
            yield return null;
        } while (transitionAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Start"));
    }
    public IEnumerator FinishTransition()
    {
        transitionAnimator.SetTrigger("Finish");
        do
        {
            yield return null;
        } while (!transitionAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Finish"));
        do
        {
            yield return null;
        } while (transitionAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Finish"));
    }
    
    //  TODO: Implement
    //private TextMeshPro Player2Health; - will need a new health manager
   // private TextMeshPro Timer;
}
