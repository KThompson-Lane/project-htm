using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;

    // Update is called once per frame
    void Update()
    {
        //Fire on Click
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //Create bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
