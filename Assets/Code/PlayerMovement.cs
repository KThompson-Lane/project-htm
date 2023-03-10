using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Camera cam;
    
    private Vector2 _movement;
    private Vector2 _mousePos;

    private Vector2 _movementInput = Vector2.zero;
    private Vector2 _rotationInput = Vector2.zero;
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        _rotationInput = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    private void Update()
    {
        _movement.x = _movementInput.x;
        _movement.y = _movementInput.y;

        //convert mouse position from screen co-ords to world units
        _mousePos = cam.ScreenToWorldPoint(_rotationInput); //todo - won't work with controller
        //_mousePos = _rotationInput;
    }
    
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + _movement * (moveSpeed * Time.fixedDeltaTime));

        Vector2 lookDir = _mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f; //z rotation. Note: Atan2 takes y first, then x
        rb.rotation = angle;
    }
}
