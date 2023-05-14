using System;
using System.Collections;
using Code.DungeonGeneration;
using Code.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UI_Manager uiManager;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private DungeonFloor dungeonFloor;
    [FormerlySerializedAs("playerMovement")] [SerializeField] private PlayerController playerController;
    
    [SerializeField] private DungeonFloorScriptableObject[] levels;
    private int currentLevel;
    public float timePassed;
    
    public float timeLimit;
    private float _remainingTime;

    private int _enemiesKilled, _roomsCleared;

    private bool _hitThisRoom = false;

    public Animator playerAnimator;

    private InputActionAsset _playerInput;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0;
        
        // Timer
        _remainingTime = timeLimit;
        Time.timeScale = 0;
        
        _playerInput = playerController.GetComponent<PlayerInput>().actions; 
        dungeonFloor.LoadFloor(levels[currentLevel++]);
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
        DungeonFloor.OnRoomChange += RoomChanged;
    }

    private void OnDisable()
    {
        // Stop listening for events
        healthManager.HealthDepletedEvent.RemoveListener(LoseGame);
        healthManager.HealthChangedEvent.RemoveListener(OnPlayerHit);
        dungeonFloor.RoomClearedEvent.RemoveListener(RoomCleared);
        dungeonFloor.EnemyKilledEvent.RemoveListener(IncEnemyKilled);
        DungeonFloor.OnRoomChange -= RoomChanged;
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

    public void ClearFloor()
    {
        dungeonFloor.PortalActive = true;
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
            ClearFloor(); //todo - will need changing when more floors added
        if (_hitThisRoom)
            IncTimer(10); //todo - remove magic number
        else
            IncTimer(20); //todo - remove magic number
        _hitThisRoom = false;
    }

    public void NextLevel()
    {
        dungeonFloor.PortalActive = false;
        if(currentLevel == levels.Length)
            WinGame();
        else
        {
            BeginFloorChange();
        }
    }

    private void BeginFloorChange()
    {
        PauseGame(true);
        playerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        playerAnimator.SetTrigger("Dissolve");
    }
    public void FinishFloorChange()
    {
        StartCoroutine(ChangeFloor());
        playerAnimator.updateMode = AnimatorUpdateMode.Normal;
        PauseGame(false);
    }
    
    IEnumerator ChangeFloor()
    {
        yield return new WaitForEndOfFrame();
        dungeonFloor.LoadFloor(levels[currentLevel++]);
        dungeonFloor.ClearRoom();
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
        ObjectPooler.SharedInstance.DisableAllObjects();

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
        uiManager.ShowEndScreen(false, timePassed, _enemiesKilled, _roomsCleared, currentLevel);
    }
    
    private void WinGame()
    {
        // Pause and show win screen
        PauseGame(true);
        uiManager.ShowEndScreen(true, timePassed, _enemiesKilled, _roomsCleared, currentLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
