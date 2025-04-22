using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement controls
    public enum ControlScheme { WASD, Arrows }
    [SerializeField] private ControlScheme _controlScheme = ControlScheme.WASD;

    // References
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Animator _anim;
    private Rigidbody2D _rb;
    private Collider2D _col;
    
    // Variables
    [SerializeField] private float _moveSpeed;
    private Vector2 _movement;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float inputX = 0f;
        float inputY = 0f;

        if (_controlScheme == ControlScheme.WASD)
        {
            inputX = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
            inputY = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        }
        else if (_controlScheme == ControlScheme.Arrows)
        {
            inputX = Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
            inputY = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
        }

        _movement = new Vector2(inputX, inputY).normalized;

        bool isWalking = _movement.sqrMagnitude > 0;
        _anim.SetBool("IsWalking", isWalking);

        if (_movement.x >= 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (_movement.x <= -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        _rb.MovePosition(_rb.position + _movement * _moveSpeed * Time.deltaTime);
    }

    public ControlScheme GetControlScheme()
    {
        return _controlScheme;
    }
}
