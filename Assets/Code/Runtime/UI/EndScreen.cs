using System;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionDuration;
    [SerializeField] private TextMeshProUGUI enemiesKilled;
    [SerializeField] private TextMeshProUGUI roomsCleared;
    [SerializeField] private TextMeshProUGUI levelsCleared;
    [SerializeField] private TextMeshProUGUI missionStatus;
    [SerializeField] private GameObject failureCause;
    // Start is called before the first frame update

    public void ShowEndScreen(bool win, float duration, int kills, int rooms, int levels)
    {
        gameObject.SetActive(true);
        enemiesKilled.text = kills.ToString();
        roomsCleared.text = rooms.ToString();
        levelsCleared.text = levels.ToString();
        missionDuration.text = TimeSpan.FromSeconds(duration).ToString("mm':'ss");
        if (win)
        {
            missionStatus.text = "Mission Success!";
            failureCause.SetActive(false);
        }
        else
        {
            missionStatus.text = "Mission Failed!";
            failureCause.SetActive(true);
        }
    }
}
