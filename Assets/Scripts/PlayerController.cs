using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Field
    float _horizontalMovement;
    [SerializeField] float speed;

    //Reference
    Rigidbody2D _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void GetHorizontalMovement(InputAction.CallbackContext ctx)
    {
        if(ctx.started) 
        { 
            _horizontalMovement = ctx.ReadValue<float>();
        }
        else if(ctx.canceled)
        {
            _horizontalMovement = 0f;
        }
        
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_horizontalMovement * speed, _rb.velocity.y);
    }
}
