using System;
using System.Collections;
using Code.DungeonGeneration;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UI_Manager uiManager;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private DungeonFloor dungeonFloor;
    [SerializeField] private PlayerMovement playerMovement;

    public float timePassed;
    
    public float timeLimit;
    private float _remainingTime;

    private int _enemiesKilled, _roomsCleared;

    private bool _hitThisRoom = false;

    public Animator playerAnimator;

    private InputActionAsset _playerInput;

    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0;
        
        // Timer
        _remainingTime = timeLimit;
        Time.timeScale = 0;
        
        _playerInput = playerMovement.GetComponent<PlayerInput>().actions;
    }

    private void LateUpdate()
    {
        timePassed += Time.deltaTime;
        
        // Decrement timer
        _remainingTime -= Time.deltaTime;
        var time = TimeSpan.FromSeconds(_remainingTime);

        // If time runs out, pause and show game over screen
        if (time > TimeSpan.FromSeconds(0)) return;
        LoseGame();
    }
    
    private void OnEnable()
    {
        // Listen for events
        healthManager.HealthDepletedEvent.AddListener(LoseGame);
        healthManager.HealthChangedEvent.AddListener(OnPlayerHit);
        dungeonFloor.RoomClearedEvent.AddListener(RoomCleared);
        dungeonFloor.EnemyKilledEvent.AddListener(IncEnemyKilled);
        dungeonFloor.OnRoomChange += RoomChanged;
    }

    private void OnDisable()
    {
        // Stop listening for events
        healthManager.HealthDepletedEvent.RemoveListener(LoseGame);
        healthManager.HealthChangedEvent.RemoveListener(OnPlayerHit);
        dungeonFloor.RoomClearedEvent.RemoveListener(RoomCleared);
        dungeonFloor.EnemyKilledEvent.RemoveListener(IncEnemyKilled);
        dungeonFloor.OnRoomChange -= RoomChanged;
    }

    // Timer Functions
    public float GetRemainingTime()
    {
        return _remainingTime;
    }
    
    private void IncTimer(float amount) //todo - might want to take a value determined by the room cleared/ current level??
    {
        _remainingTime += amount;
    }

    private void IncEnemyKilled()
    {
        _enemiesKilled++;
    }

    private void OnPlayerHit(float _)
    {
        playerAnimator.SetTrigger("Hit");
        _hitThisRoom = true;
    }


    // Manage game state

    public void StartGame()
    {
        _remainingTime = timeLimit;
        PauseGame(false);
    }
    
    public void RestartGame()
    {
        StartCoroutine(ReloadScene());
    }

    private IEnumerator ReloadScene()
    {
        yield return StartCoroutine(uiManager.BeginTransition());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //todo - change to first level
        _remainingTime = timeLimit;
        PauseGame(false);
        healthManager.ResetHealth();
    }
    
    public void PauseGame(bool pause)
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
        _roomsCleared++;
        if (bossRoom)
            WinGame(); //todo - will need changing when more floors added
        else if (_hitThisRoom)
            IncTimer(10); //todo - remove magic number
        else
            IncTimer(20); //todo - remove magic number

        _hitThisRoom = false;
    }

    private void RoomChanged(RoomIndex newRoom, Direction entryDirection)
    {
        StartCoroutine(ChangeRoom(newRoom, entryDirection));
    }

    private IEnumerator ChangeRoom(RoomIndex newRoom, Direction entryDirection)
    {
        Debug.Log("Changing room");
        //  Pause game
        PauseGame(true);
        
        //  Begin animation
        //  Wait for fade in to finish
        yield return StartCoroutine(uiManager.BeginTransition());
        //  Disable all powerups
        foreach (var pickup in FindObjectsOfType<Pickup>())
        {
            pickup.gameObject.SetActive(false);
        }

        //  Load new room
        dungeonFloor.ChangeRoom(newRoom);
        //  Move player.
        dungeonFloor.MovePlayer(entryDirection);
        
        //  Fade out
        //  Wait for fade out to finish
        yield return StartCoroutine(uiManager.FinishTransition());
        
        //  Unpause game
        PauseGame(false);
    }

    private void LoseGame()
    {
        // Pause and show death screen
        PauseGame(true);
        uiManager.ShowEndScreen(false, timePassed, _enemiesKilled, _roomsCleared);
    }
    
    private void WinGame()
    {
        // Pause and show win screen
        PauseGame(true);
        uiManager.ShowEndScreen(true, timePassed, _enemiesKilled, _roomsCleared);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
