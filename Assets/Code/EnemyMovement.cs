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
    
    // Start is called before the first frame update
    void Start()
    {
        _mRb2d = GetComponent<Rigidbody2D>();
        _mPlayerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        _mRb2d.MovePosition(Vector2.MoveTowards(_mRb2d.position, _mPlayerTransform.position, MoveSpeed*Time.fixedDeltaTime));
        
        Vector3 vectorToTarget = _mPlayerTransform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90; //z rotation. Note: Atan2 takes y first, then x, subtract 90degrees for sprite rotation
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation =  Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * RotationSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag is "Player")
        {
            //Debug.Log("Ouchies");
        }
    }
}
