using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Camera cam;
    
    Vector2 _movement;
    Vector2 _mousePos;

    // Update is called once per frame
    private void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        //convert mouse position from screen co-ords to world units
        _mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }
    
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + _movement * moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = _mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f; //z rotation. Note: Atan2 takes y first, then x
        rb.rotation = angle;
    }
}
