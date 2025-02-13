using GJ.UnityToolbox;
using UnityEngine;

public class PlayerDisplay : MonoBehaviour, IPlayerComponent
{
    // SERIALIZED MEMBERS
    [SerializeField] private PlayerDisplaySettings _settings;

    [Header("References")]
    [SerializeField] private PlayerInput _input;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _displayParent;

    // PRIVATE MEMBERS
    private int _groundedHash;
    private int _jumpHash;
    private bool _grounded;

    // MonoBehaviour INTERFACE
    private void OnValidate()
    {
        CommonUtilities.AssertIsNotNull(this, _settings);
        CommonUtilities.AssertIsNotNull(this, _input);
        CommonUtilities.AssertIsNotNull(this, _movement);
        CommonUtilities.AssertIsNotNull(this, _animator);
        CommonUtilities.AssertIsNotNull(this, _displayParent);
    }

    private void Awake()
    {
        _groundedHash = Animator.StringToHash(_settings.GroundedKey);
        _jumpHash = Animator.StringToHash(_settings.JumpKey);
    }

    private void OnEnable()
    {
        _movement.Jumped += OnJumped;
        _movement.GroundedChanged += OnGroundedChanged;
    }

    private void OnDisable()
    {
        _movement.Jumped -= OnJumped;
        _movement.GroundedChanged -= OnGroundedChanged;
    }

    // IPlayerComponent INTERFACE
    public void OnUpdate()
    {
        HandleSpriteFlip();
    }

    private void HandleSpriteFlip()
    {
        if (_input.CurrentInput.Move.x != 0)
        {
            var scale = _displayParent.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (_input.CurrentInput.Move.x < 0 ? -1 : 1);
            _displayParent.transform.localScale = scale;
        }
    }

    private void OnJumped()
    {
        _animator.SetTrigger(_jumpHash);
        _animator.ResetTrigger(_groundedHash);

        if (_grounded) // Avoid coyote
        {
            GameObject jumpFX = ObjectPool.Instance.Get(_settings.JumpFXPrefab);
            jumpFX.transform.position = transform.position;
            ObjectPool.Instance.ReturnDeferred(jumpFX, _settings.JumpFXDuration);
        }
    }

    private void OnGroundedChanged(bool grounded, float impact)
    {
        _grounded = grounded;
        if (grounded)
        {
            _animator.SetTrigger(_groundedHash);
        }
    }
}

