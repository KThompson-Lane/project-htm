using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] 
    private float MoveSpeed = 3.0f; //todo - add to SO
    
    [SerializeField] 
    private float RotationSpeed = 3.0f; //todo - add to SO

    [SerializeField]
    private EnemySO enemySO;
    
    private Rigidbody2D _mRb2d;
    private Transform _mPlayerTransform; //for look at location

    // Damage on collision variables
    [SerializeField]
    private HealthManager healthManager; // for player health

    // Start is called before the first frame update
    void Start()
    {
        _mRb2d = GetComponent<Rigidbody2D>();
        _mPlayerTransform = GameObject.FindWithTag("Player").transform; //todo - might want to add to SO so not all enemies follow player
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
        //todo - select type of attack based on enemyAttackType
        if (collision.gameObject.tag is "Player")
        {
            //Debug.Log("Ouchies");
            healthManager.DecreaseHealth(enemySO.enemyAttackType.damage);
        }
    }
}
