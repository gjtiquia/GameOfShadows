using UnityEngine;
using System;
using GJ.UnityToolbox;

public class PlayerMovement : MonoBehaviour, IPlayerComponent
{
    // PUBLIC EVENTS
    public event Action<bool, float> GroundedChanged;
    public event Action Jumped;

    // SERIALIZED MEMBERS
    [SerializeField] private PlayerMovementSettings _settings;

    [Header("References")]
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private CapsuleCollider2D _collider;

    // PRIVATE MEMBERS
    private Vector2 _frameVelocity;
    private float _time;
    private bool _grounded;
    private float _frameLeftGrounded;
    private bool _jumpToConsume;
    private bool _bufferedJumpUsable;
    private bool _endedJumpEarly;
    private bool _coyoteUsable;
    private float _timeJumpWasPressed;

    private bool _hasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _settings.JumpBuffer && _timeJumpWasPressed > 0;
    private bool _canUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _settings.CoyoteTime;


    // MonoBehaviour INTERFACE
    private void OnValidate()
    {
        CommonUtilities.AssertIsNotNull(this, _settings);
        CommonUtilities.AssertIsNotNull(this, _input);
        CommonUtilities.AssertIsNotNull(this, _rigidbody);
        CommonUtilities.AssertIsNotNull(this, _collider);
    }

    // IPlayerComponent INTERFACE
    public void OnUpdate()
    {
        _time += Time.deltaTime;

        if (_input.CurrentInput.JumpDown)
        {
            _jumpToConsume = true;
            _timeJumpWasPressed = _time;
        }
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        HandleJump();
        HandleDirection();
        HandleGravity();
        ApplyMovement();
    }

    // PRIVATE METHODS
    private void CheckCollisions()
    {
        // Ground and Ceiling
        bool groundHit = Physics2D.CapsuleCast(_collider.bounds.center, _collider.size, _collider.direction, 0, Vector2.down, _settings.GrounderDistance, _settings.GroundLayer);
        bool ceilingHit = Physics2D.CapsuleCast(_collider.bounds.center, _collider.size, _collider.direction, 0, Vector2.up, _settings.GrounderDistance, _settings.GroundLayer);

        // Hit a Ceiling
        if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

        // Landed on the Ground
        if (!_grounded && groundHit)
        {
            _grounded = true;
            _coyoteUsable = true;
            _bufferedJumpUsable = true;
            _endedJumpEarly = false;
            GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
        }
        // Left the Ground
        else if (_grounded && !groundHit)
        {
            _grounded = false;
            _frameLeftGrounded = _time;
            GroundedChanged?.Invoke(false, 0);
        }
    }

    private void HandleJump()
    {
        if (!_endedJumpEarly && !_grounded && !_input.CurrentInput.JumpHeld && _frameVelocity.y > 0)
            _endedJumpEarly = true;

        if (!_jumpToConsume && !_hasBufferedJump) return;

        if (_grounded || _canUseCoyote)
            ExecuteJump();

        _jumpToConsume = false;
    }

    private void ExecuteJump()
    {
        _endedJumpEarly = false;
        _timeJumpWasPressed = 0;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;
        _frameVelocity.y = _settings.JumpPower;

        Jumped?.Invoke();
    }

    private void HandleDirection()
    {
        if (_input.CurrentInput.Move.x == 0)
        {
            var deceleration = _grounded ? _settings.GroundDeceleration : _settings.AirDeceleration;
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _input.CurrentInput.Move.x * _settings.MaxSpeed, _settings.Acceleration * Time.fixedDeltaTime);
        }
    }

    private void HandleGravity()
    {
        if (_grounded && _frameVelocity.y <= 0f)
        {
            _frameVelocity.y = _settings.GroundingForce;
        }
        else
        {
            var inAirGravity = _settings.FallAcceleration;
            if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _settings.JumpEndEarlyGravityModifier;
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_settings.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
        }
    }

    private void ApplyMovement()
    {
        _rigidbody.velocity = _frameVelocity;
    }
}
