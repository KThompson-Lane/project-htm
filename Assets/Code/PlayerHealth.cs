using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    protected override void Start()
    {
        base.Start();
        Debug.Log("CALLING PLAYER CONSTRUCTOR");
        UI_Manager.Instance.ChangePlayerHealth(totalHealth);
    }

    public override void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Health is: " + currentHealth);
        //  Set player health in UI
        UI_Manager.Instance.ChangePlayerHealth(currentHealth);
        if (currentHealth <= 0)
        {
            //reset game
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
