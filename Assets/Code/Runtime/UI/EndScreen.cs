using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionDuration;
    [SerializeField] private TextMeshProUGUI enemiesKilled;
    [SerializeField] private TextMeshProUGUI roomsCleared;
    [SerializeField] private TextMeshProUGUI levelsCleared;
    [SerializeField] private TextMeshProUGUI missionStatus;
    [SerializeField] private GameObject failureCause;
    [SerializeField] private Image failureObject;
    [SerializeField] private TextMeshProUGUI failureInfo;

    public void ShowEndScreen(bool win, float duration, int kills, int rooms, int levels, [CanBeNull] EnemySO attacker)
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
            if (attacker == null)
            {
                failureInfo.text = "Ran out of time";
                failureCause.SetActive(false);
                return;
            }
            failureCause.SetActive(true);
            failureObject.sprite = attacker.enemySprite;
        }
    }
}
