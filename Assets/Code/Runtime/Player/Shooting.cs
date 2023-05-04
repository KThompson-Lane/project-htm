using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;

    public float rateOfFire = 60.0f;

    public int damage = 1;
    
    private float _coolDown = 0.0f;

    private float _interval; // time between shots
    
    private bool _isShooting;
    [SerializeField] private Animator _gunAnimator;
    
    [SerializeField] private GameManager gameManager;
    
    [SerializeField]
    private PlayerAttackSO playerAttackSo;

    public void Start()
    {
        rateOfFire = playerAttackSo.rateOfFire;
        damage = playerAttackSo.baseDamage;
        //todo - all other so variables
        
        playerAttackSo.DamageModifiedEvent.AddListener(OnDamageModified);
        playerAttackSo.RoFModifiedEvent.AddListener(OnRoFModified);
        
        _interval = 60 / rateOfFire; //1 second / rate of fire
        _gunAnimator.SetFloat("Fire Rate", _interval);
    }

    private void OnDamageModified(int amount)
    {
        damage = amount;
    }

    private void OnRoFModified(float amount)
    {
        rateOfFire = amount;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        _isShooting = context.action.triggered;
    }

    // Update is called once per frame
    private void Update()
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

    private void Shoot()
    {
        //Create bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().SetDamage(damage);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        _gunAnimator.SetTrigger("Shoot");
    }
}
