using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI Player1Health;

    //  TODO: Implement
    //private TextMeshPro Player2Health;
   // private TextMeshPro Timer;
   public int PlayerOneHP = 4;
    
    // Start is called before the first frame update
    void Start()
    {
        Player1Health.text = "";
        for (int i = 0; i < PlayerOneHP; i+=2)
        {
            Player1Health.text += "<sprite index=1>";
        }
    }
    //  TODO: Call this somewhere.
    void OnHealthChanged()
    {
        Player1Health.text = "";
        for (int i = 0; i < PlayerOneHP; i+=2)
        {
            Player1Health.text += "<sprite index=1>";
        }
    }
}
