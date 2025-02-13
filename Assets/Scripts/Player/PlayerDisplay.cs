using GJ.UnityToolbox;
using UnityEngine;

public class PlayerDisplay : MonoBehaviour, IPlayerComponent
{
    // SERIALIZED MEMBERS
    [SerializeField] private PlayerDisplaySettings _settings;

    [Header("References")]
    [SerializeField] private PlayerInput _input;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerAttack _attack;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _displayParent;

    // PRIVATE MEMBERS
    private bool _grounded;
    private int _groundedHash;
    private int _jumpHash;
    private int _punchHash;
    private int _speedRunHash;
    private int _swordSlashHash;

    // MonoBehaviour INTERFACE
    private void OnValidate()
    {
        CommonUtilities.AssertIsNotNull(this, _settings);
        CommonUtilities.AssertIsNotNull(this, _input);
        CommonUtilities.AssertIsNotNull(this, _movement);
        CommonUtilities.AssertIsNotNull(this, _animator);
        CommonUtilities.AssertIsNotNull(this, _displayParent);
        CommonUtilities.AssertIsNotNull(this, _attack);
    }

    private void Awake()
    {
        _groundedHash = Animator.StringToHash(_settings.GroundedKey);
        _jumpHash = Animator.StringToHash(_settings.JumpKey);
        _punchHash = Animator.StringToHash(_settings.PunchKey);
        _speedRunHash = Animator.StringToHash(_settings.SpeedRunKey);
        _swordSlashHash = Animator.StringToHash(_settings.SwordSlashKey);
    }

    private void OnEnable()
    {
        _movement.Jumped += OnJumped;
        _movement.GroundedChanged += OnGroundedChanged;

        _attack.OnPunch += OnPunch;
        _attack.OnSpeedRun += OnSpeedRun;
        _attack.OnSwordSlash += OnSwordSlash;
    }

    private void OnDisable()
    {
        _movement.Jumped -= OnJumped;
        _movement.GroundedChanged -= OnGroundedChanged;

        _attack.OnPunch -= OnPunch;
        _attack.OnSpeedRun -= OnSpeedRun;
        _attack.OnSwordSlash -= OnSwordSlash;
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

    private void OnPunch()
    {
        _animator.SetTrigger(_punchHash);
    }

    private void OnSpeedRun()
    {
        _animator.SetTrigger(_speedRunHash);
    }

    private void OnSwordSlash()
    {
        _animator.SetTrigger(_swordSlashHash);
    }
}

