using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        //Ensure bullet is destroyed after a set time so they don't linger
        Destroy(gameObject, 10f);
    }
}
