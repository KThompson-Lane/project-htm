using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour //todo - rename to PlayerController
{
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private GameManager gameManager;
    
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Camera cam;
    
    private Vector2 _movement;
    private Vector2 _mousePos;

    private Vector2 _movementInput = Vector2.zero;
    private Vector2 _rotationInput = Vector2.zero;

    private string _inputType;

    private Animator _playerAnimator;
    private int _yMoveID, _xMoveID;

    private void OnEnable()
    {
        _playerAnimator = GetComponent<Animator>();
        _xMoveID = Animator.StringToHash("Movement_X");
        _yMoveID = Animator.StringToHash("Movement_Y");
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (gameManager.paused)
            return;
        _movementInput = context.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (gameManager.paused)
            return;
        _rotationInput = context.ReadValue<Vector2>();
        _inputType = context.control.path;
    }
    
    public void OnGodMode(InputAction.CallbackContext context)
    {
        _inputType = context.control.path; // todo - remove if not needed
        // enable on GameManager
        healthManager.ToggleGodMode();
    }

    // Update is called once per frame
    private void Update()
    {
        _movement.x = _movementInput.x;
        _movement.y = _movementInput.y;

        //convert mouse position from screen co-ords to world units
        if (_inputType == "/Mouse/position")
        {
            _mousePos = cam.ScreenToWorldPoint(_rotationInput);
        }
        else //using controller
        {
            if (_rotationInput != new Vector2(0, 0))
            {
                _mousePos = _rotationInput;
            }
        }
    }
    
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + _movement * (moveSpeed * Time.fixedDeltaTime));
        Vector2 lookDir;

        if (_inputType == "/Mouse/position")
        {
            lookDir = _mousePos - rb.position;
        }
        else
        {
            lookDir = _mousePos;
        }
        
        var angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90; //z rotation. Note: Atan2 takes y first, then x
        rb.rotation = angle;
    }

    
    //  We do our animations last for performance sake
    private void LateUpdate()
    {
        //  Use the players current movement direction to determine which animation to play
        _playerAnimator.SetFloat(_xMoveID, _movementInput.x);
        _playerAnimator.SetFloat(_yMoveID, _movementInput.y);
    }
}
