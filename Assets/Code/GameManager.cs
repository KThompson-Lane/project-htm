using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UI_Manager uiManager;

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

        // If time runs out, show game over screen
        if (time <= TimeSpan.FromSeconds(0))
        {
            uiManager.ShowGameOverScreen(true);
        }
    }

    public float GetRemainingTime()
    {
        return _remainingTime;
    }
    

    // Manage game state
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //todo - change to first level
    }
}
