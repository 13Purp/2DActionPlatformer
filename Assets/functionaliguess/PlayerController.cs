using Bullets;
using System.Collections;
using UnityEngine;
using Controller;
public class PlayerController : MonoBehaviour,IController
{

    private Rigidbody2D _rigidBody2D;
    private CapsuleCollider2D _capCol;
    private Vector2 _boxCastSize;
    private Vector2 _jumpVec;
    private Vector2 _jumpOffVec;


    private RaycastHit2D _jumpPoint;
    private float _moveX;
    private float _moveY;
    private float _standardMoveSpeed;
    public bool _facingRight;
    private bool _isJumping;
    private bool _jumpBuffered;
    private bool _isRunning;
    private bool _abortRun;
    private bool _usingWeapon;
    private bool _isWalking;


    private bool _wFire;
    private bool _sFire;



    private bool _isGrounded;
    private bool _wasGrounded;

    [SerializeField] private float _jumpForce;
    private Vector2 _gravityModifier;
    private bool _jump;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private LayerMask _jumpable;
    [SerializeField] private float _coliderSizeY;
    [SerializeField] private float _coliderSizeX;
    [SerializeField] private float _coyoteTime;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float velocityDebug;
    [SerializeField] private Weapon weapon;
    [SerializeField] private Animator animator;
    private bool _canJump;
    public float MoveSpeed
    {
        get
        {
            return _moveSpeed;
        }
        set
        {
            _moveSpeed = value;
        }
    }
    public float JumpForce
    {
        get
        {
            return _jumpForce;
        }
        set
        {
            _jumpForce = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        _rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        _capCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        _boxCastSize = new Vector2(_capCol.bounds.size.x * _coliderSizeX, _capCol.bounds.size.y * _coliderSizeY);
        _facingRight = true;
        _isJumping = false;
        _jumpVec = new Vector2(0f, _jumpForce - _rigidBody2D.velocity.y);
        _jumpOffVec= new Vector2(0f, -1f * _jumpForce);
        _gravityModifier = new Vector2(0, -20);
        _standardMoveSpeed = _moveSpeed;
        _usingWeapon = false;
        _wFire = false;
        _sFire = false;
        _isWalking = false;
        //weapon.displayWeapon();

        _isGrounded = true;
        _wasGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {

        _moveX = Input.GetAxisRaw("Horizontal");
        // moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
            _jump = true;

        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            _usingWeapon = !_usingWeapon;
            weapon.displayWeapon();
        }

       

       
        if (Input.GetKey(KeyCode.Alpha1))
        {
            weapon.SetWindBullet();
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            weapon.SetZapBullet();
        }

        if (_usingWeapon)
        {
            _wFire = Input.GetButton("Fire1");

            _sFire = Input.GetButton("Fire2");
        }




    }

    private void FixedUpdate()
    {

        if(_wFire)
            weapon.weakFire();
        if(_sFire) 
            weapon.strongFire();

        velocityDebug = _rigidBody2D.velocity.x;
        _jumpPoint = Physics2D.BoxCast(_capCol.bounds.center, _boxCastSize, 0f, Vector2.down, 0.1f, _jumpable);

        if (_moveX < -0.1 || _moveX > 0.1)
        {
            _isWalking = true;
            _rigidBody2D.AddForce(new Vector2(_moveX * _moveSpeed, 0f), ForceMode2D.Impulse);
        }
        else
        {
            _isWalking = false;
            _isRunning = false;
        }

        _wasGrounded = _isGrounded;
        _isGrounded = IsGrounded();
        //bool Jumped = false;

         if(_rigidBody2D.velocity.y < 0)
        {
            _rigidBody2D.AddForce(_gravityModifier, ForceMode2D.Force);
        }
        _isJumping = (_isJumping && _isGrounded) ? false : _isJumping;

        



        if (_wasGrounded && !_isGrounded && !_canJump)
        {
            StartCoroutine(CoyoteTimer());

        }

        if (_jump)//moveY > 0.1)
        {
            if (!_jumpBuffered)
                StartCoroutine(JumpBuffer());
            _jump = false;
            if (_isGrounded || _canJump && !_isJumping)
            {
                _isJumping = true;
                _canJump = false;
                _rigidBody2D.AddForce(_jumpVec, ForceMode2D.Impulse);
                if (_jumpPoint.rigidbody != null)
                    _jumpPoint.rigidbody.AddForce(_jumpOffVec, ForceMode2D.Impulse);
            }
        }

        else if (_jumpBuffered)
        {
            if (_isGrounded || _canJump && !_isJumping)
            {
                _isJumping = true;
                _canJump = false;
                _rigidBody2D.AddForce(_jumpVec, ForceMode2D.Impulse);
                if (_jumpPoint.rigidbody != null)
                    _jumpPoint.rigidbody.AddForce(_jumpOffVec, ForceMode2D.Impulse);
            }
        }

        animator.SetBool("isWalking", _isWalking);
        animator.SetBool("isShooting", _usingWeapon);
    
    }

    

    public void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180, 0f);
        _moveSpeed = _standardMoveSpeed;
    }

    public bool IsPlayer()
    {
        return true;
    }
    public bool FacingRight()
    {
        return _facingRight;
    }

    public bool IsGrounded()
    {
        return _jumpPoint.collider != null;
    }

    IEnumerator CoyoteTimer()
    {
        _canJump = true;
        yield return new WaitForSeconds(_coyoteTime);
        _canJump = false;

    }
    IEnumerator JumpBuffer()
    {
        _jumpBuffered = true;
        yield return new WaitForSeconds(0.1f);
        _jumpBuffered = false;

    }
    IEnumerator RunTimer()
    {
        _isRunning = true;
        yield return new WaitForSeconds(1.5f);
        if(!_abortRun)
            _moveSpeed = 0.6f;
        _isRunning = false;

    }



}
