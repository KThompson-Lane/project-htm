using System;
using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI Player1Health;

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

    //  TODO: Implement
    //private TextMeshPro Player2Health;
   // private TextMeshPro Timer;

   //  TODO: Call this somewhere.
    public void ChangePlayerHealth(int newHP)
    {
        Player1Health.text = "";
        for (int i = 0; i < newHP; i+=2)
        {
            Player1Health.text += "<sprite index=1>";
        }
    }
}
