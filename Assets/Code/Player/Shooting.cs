using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;

    public float rateOfFire = 60.0f;

    public float damage = 1f;
    
    private float _coolDown = 0.0f;

    private float _interval; // time between shots
    
    private bool _isShooting;

    public void Start()
    {
        _interval = 60 / rateOfFire; //1 second / rate of fire
    }

    // For Player
    public void OnShoot(InputAction.CallbackContext context)
    {
        _isShooting = context.action.triggered;
    }

    // Update is called once per frame
    private void Update() // Used for player
    {
        //Fire when left mouse held
        if(_isShooting)
        { 
            if (_coolDown <= 0f)
            {
                Shoot();
                _coolDown = _interval;
            }
        }

        if (_coolDown > 0)
        {
            // update cooldown
            _coolDown -= Time.deltaTime;
        }
    }
    
    public void UpdateEnemy() //todo - maybe rename this
    {
        //Fire when not on cooldown
        if (_coolDown <= 0f)
        {
            Shoot();
            _coolDown = _interval;
        }

        if (_coolDown > 0)
        {
            // update cooldown
            _coolDown -= Time.deltaTime;
        }
    }

   private void Shoot()
    {
        //Create bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().SetDamage(damage);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
