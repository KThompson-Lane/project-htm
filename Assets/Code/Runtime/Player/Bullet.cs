using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected float Damage;
    private void OnTriggerEnter2D(Collider2D col)
    {
        var enemyController = col.gameObject.GetComponent<EnemyController>();
        
        if (enemyController != null)
        {
            enemyController.TakeDamage(1); //todo - variable damage
        }
        
        gameObject.SetActive(false);
    }

    private void Start()
    {
        //Ensure bullet is destroyed after a set time so they don't linger
        StartCoroutine(DisableAfter(10));
    }

    public IEnumerator DisableAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
    public void SetDamage(float damage)
    {
        Damage = damage;
    }
}
