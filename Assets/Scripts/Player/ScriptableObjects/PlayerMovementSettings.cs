using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "Player/Movement Settings")]
public class PlayerMovementSettings : ScriptableObject
{
    public float MaxSpeed => _maxSpeed;
    public float Acceleration => _acceleration;
    public float GroundDeceleration => _groundDeceleration;
    public float AirDeceleration => _airDeceleration;
    public float GroundingForce => _groundingForce;
    public float JumpPower => _jumpPower;
    public float MaxFallSpeed => _maxFallSpeed;
    public float FallAcceleration => _fallAcceleration;
    public float JumpEndEarlyGravityModifier => _jumpEndEarlyGravityModifier;
    public float CoyoteTime => _coyoteTime;
    public float JumpBuffer => _jumpBuffer;
    public float GrounderDistance => _grounderDistance;
    public LayerMask GroundLayer => groundLayer;

    [Header("Movement Settings")]
    [SerializeField] private float _maxSpeed = 14f;
    [SerializeField] private float _acceleration = 120f;
    [SerializeField] private float _groundDeceleration = 60f;
    [SerializeField] private float _airDeceleration = 30f;
    [SerializeField, Range(0f, -10f)] private float _groundingForce = -1.5f;
    [SerializeField, Range(0f, 0.5f)] private float _grounderDistance = 0.05f;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpPower = 36f;
    [SerializeField] private float _maxFallSpeed = 40f;
    [SerializeField] private float _fallAcceleration = 110f;
    [SerializeField] private float _jumpEndEarlyGravityModifier = 3f;
    [SerializeField] private float _coyoteTime = 0.15f;
    [SerializeField] private float _jumpBuffer = 0.2f;

    [Header("Layer Settings")]
    [SerializeField] private LayerMask groundLayer;
}