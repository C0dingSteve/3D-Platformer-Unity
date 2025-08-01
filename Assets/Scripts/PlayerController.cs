using UnityEngine;
using Assets.Scripts.Utility;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravityModifier;

    private Vector3 _moveDirection;

    private Camera _cam;
    private CharacterController _charController;
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _knockBackDuration;
    [SerializeField] private Vector2 _knockBackVelocity;

    private TimedAction _knockBackTimedAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _charController = GetComponent<CharacterController>();
        _charController.LogNullStatus();

        _knockBackTimedAction = gameObject.AddComponent<TimedAction>();
        _knockBackTimedAction.LogNullStatus();

        _moveDirection = Vector3.zero;

        _cam = Camera.main;
    }

    void Update()
    {
        if (!_knockBackTimedAction.IsActive)
        {
            CalculateMovement();
        }
        ApplyMovement();
    }

    private void CalculateMovement()
    {
        float inputX = Input.GetAxisRaw("Horizontal"); // Left Right
        float inputY = Input.GetAxisRaw("Vertical");   // Up Down

        float currentYVelocity = _moveDirection.y;

        // Find move direction on XZ plane
        _moveDirection = Vector3.ProjectOnPlane(_cam.transform.forward * inputY + _cam.transform.right * inputX, Vector3.up);
        _moveDirection.Normalize();

        // Add Speed and gravity
        _moveDirection *= _moveSpeed; // Scale the speed
        _moveDirection.y = currentYVelocity; // Reset the Up Down velocity

        if (_charController.isGrounded)
        {
            // Apply jump force (overwrites y velocity for jump)
            if (Input.GetButtonDown("Jump"))
            {
                _moveDirection.y = _jumpForce;
            }
        }
        else
        {
            // Increase vertical velocity due to gravitational acceleration
            _moveDirection.y += Physics.gravity.y * Time.deltaTime * _gravityModifier;
        }

        if (inputX != 0 || inputY != 0) // when there is some input
        {
            //transform.rotation = Quaternion.Euler(0f, _cam.transform.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(_moveDirection.x, 0f, _moveDirection.z));
            _playerModel.transform.rotation = Quaternion.Slerp(_playerModel.transform.rotation, newRotation, _rotateSpeed * Time.deltaTime);
        }
    }

    public void ActivateKnockBack()
    {
        // Need to update the _moveDirection with _knockBackVelocity 
        // because the movement needs to transition properly
        _moveDirection.y = _knockBackVelocity.y; 
        _knockBackTimedAction.RunAction(_knockBackDuration, KnockbackPerFrame);
    }

    // Whenever player KnockbackPerFrame is activated
    // player position auto updates due to ApplyMovement in update
    private void KnockbackPerFrame()
    {
        if (_charController.isGrounded) _knockBackTimedAction.StopAction();

        float currentYVelocity = _moveDirection.y;
        _moveDirection = -_playerModel.transform.forward * _knockBackVelocity.x; // knoback along z axis of player model
        _moveDirection.y = currentYVelocity; // Apply current y velocity starting from knockbackPower.y value

        _moveDirection.y += Physics.gravity.y * Time.deltaTime * _gravityModifier;
    }

    private void ApplyMovement()
    {
        // Apply final displacement based on velocity, scaled to frame time
        _charController.Move(_moveDirection * Time.deltaTime);

        _animator.SetFloat("moveSpeed", Vector3.ProjectOnPlane(_moveDirection, Vector3.up).magnitude);
        _animator.SetBool("isGrounded", _charController.isGrounded);
    }
}
