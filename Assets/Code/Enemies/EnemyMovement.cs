using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] 
    private float MoveSpeed = 3.0f;
    
    [SerializeField] 
    private float RotationSpeed = 3.0f;
    
    private Rigidbody2D _mRb2d;
    private Transform _mPlayerTransform;

    // Damage on collision variables
    [SerializeField]
    private HealthManager healthManager;

    private const int Damage = 2;

    // Start is called before the first frame update
    void Start()
    {
        _mRb2d = GetComponent<Rigidbody2D>();
        _mPlayerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        var position = _mPlayerTransform.position;
        _mRb2d.MovePosition(Vector2.MoveTowards(_mRb2d.position, position, MoveSpeed*Time.fixedDeltaTime));
        
        Vector3 vectorToTarget = position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90; //z rotation. Note: Atan2 takes y first, then x, subtract 90degrees for sprite rotation
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation =  Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * RotationSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag is "Player")
        {
            //Debug.Log("Ouchies");
            //collision.gameObject.GetComponent<Health>().TakeDamage(2);
            healthManager.DecreaseHealth(Damage);
        }
    }
}
