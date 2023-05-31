using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected int Damage;
    private void OnTriggerEnter2D(Collider2D col)
    {
        var enemyController = col.gameObject.GetComponent<EnemyController>();
        
        if (enemyController != null)
        {
            enemyController.TakeDamage(Damage);
        }
        
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GetComponent<TrailRenderer>().Clear();
        StartCoroutine(DisableAfter(10));
    }

    public IEnumerator DisableAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
    
    public void SetDamage(int damage)
    {
        Damage = damage;
    }
}
