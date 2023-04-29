using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UI_Manager uiManager;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private DungeonFloor dungeonFloor;
    [SerializeField] private PlayerMovement playerMovement;

    public float timeLimit;
    private float _remainingTime;

    private int _enemiesKilled;

    public Animator playerAnimator;

    private InputActionAsset _playerInput;

    // Start is called before the first frame update
    void Start()
    {
        // Timer
        _remainingTime = timeLimit;
        Time.timeScale = 0;
        
        _playerInput = playerMovement.GetComponent<PlayerInput>().actions;
    }

    private void OnPlayerHit(float _)
    {
        playerAnimator.SetTrigger("Hit");
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
        healthManager.HealthDepletedEvent.AddListener(LoseGame);
        healthManager.HealthChangedEvent.AddListener(OnPlayerHit);
        dungeonFloor.RoomClearedEvent.AddListener(RoomCleared);
        dungeonFloor.EnemyKilledEvent.AddListener(IncEnemyKilled);
    }

    private void OnDisable()
    {
        // Stop listening for events
        healthManager.HealthDepletedEvent.RemoveListener(LoseGame);
        healthManager.HealthChangedEvent.RemoveListener(OnPlayerHit);
        dungeonFloor.RoomClearedEvent.RemoveListener(RoomCleared);
        dungeonFloor.EnemyKilledEvent.RemoveListener(IncEnemyKilled);
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

    private void IncEnemyKilled()
    {
        _enemiesKilled++;
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
        healthManager.ResetHealth();
    }
    
    private void PauseGame(bool pause)
    {
        // Disable players inputs
        if(pause)
            _playerInput.Disable();
        else
            _playerInput.Enable();
        
        Time.timeScale = pause ? 0 : 1;
    }

    private void RoomCleared(bool bossRoom)
    {
        if (bossRoom)
            WinGame(); //todo - will need changing when more floors added
        else
            IncTimer();
    }
    
    private void LoseGame()
    {
        // Pause and show death screen
        PauseGame(true);
        uiManager.ShowGameOverScreen(true);
    }
    
    private void WinGame()
    {
        // Pause and show win screen
        PauseGame(true);
        uiManager.ShowWinScreen();
    }
}
