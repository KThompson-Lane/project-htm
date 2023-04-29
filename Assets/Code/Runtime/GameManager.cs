using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UI_Manager uiManager;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private DungeonFloor dungeonFloor;

    public float timePassed;
    
    public float timeLimit;
    private float _remainingTime;

    public Animator playerAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0;
        
        // Timer
        _remainingTime = timeLimit;
        Time.timeScale = 0;
    }

    private void OnPlayerHit(float _)
    {
        playerAnimator.SetTrigger("Hit");
    }
    
    private void LateUpdate()
    {
        timePassed += Time.deltaTime;
        
        // Decrement timer
        _remainingTime -= Time.deltaTime;
        var time = TimeSpan.FromSeconds(_remainingTime);

        // If time runs out, pause and show game over screen
        if (time > TimeSpan.FromSeconds(0)) return;
        PauseGame(true);
        uiManager.ShowGameOverScreen(true);
    }
    
    private void OnEnable()
    {
        // Listen for events
        healthManager.HealthDepletedEvent.AddListener(PauseGame);
        healthManager.HealthChangedEvent.AddListener(OnPlayerHit);
        dungeonFloor.RoomClearedEvent.AddListener(IncTimer);
    }

    private void OnDisable()
    {
        // Stop listening for events
        healthManager.HealthDepletedEvent.RemoveListener(PauseGame);
        healthManager.HealthChangedEvent.RemoveListener(OnPlayerHit);
        dungeonFloor.RoomClearedEvent.RemoveListener(IncTimer);
    }

    // Timer Functions
    public float GetRemainingTime()
    {
        return _remainingTime;
    }
    
    private void IncTimer() //todo - might want to take a value determined by the room cleared/ current level??
    {
        _remainingTime += 10;
    }


    // Manage game state

    public void StartGame()
    {
        _remainingTime = timeLimit;
        PauseGame(false);
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //todo - change to first level
        _remainingTime = timeLimit;
        PauseGame(false);
    }

    // When player dies, (todo) pause menu
    private void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }
}
