using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UI_Manager uiManager;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private DungeonFloor dungeonFloor;

    public float timeLimit;
    private float _remainingTime;

    // Start is called before the first frame update
    void Start()
    {
        // Timer
        _remainingTime = timeLimit;
    }

    private void LateUpdate()
    {
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
        dungeonFloor.RoomClearedEvent.AddListener(IncTimer);
    }

    private void OnDisable()
    {
        // Stop listening for events
        healthManager.HealthDepletedEvent.RemoveListener(PauseGame);
        dungeonFloor.RoomClearedEvent.RemoveListener(IncTimer);
    }

    // Timer Functions
    public float GetRemainingTime()
    {
        return _remainingTime;
    }
    
    private void IncTimer()
    {
        //UpdateTimer(10);
        _remainingTime += 10;
    }


    // Manage game state
    
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
