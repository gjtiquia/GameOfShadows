using System;
using UnityEngine;
using GJ.UnityToolbox;

public class PlayerAttack : MonoBehaviour, IPlayerComponent
{
    public event Action OnPunch;
    public event Action OnSpeedRun;
    public event Action OnSwordSlash;

    [SerializeField] private PlayerInput _input;
    [SerializeField] private PlayerMovement _movement;

    private bool _isGrounded => _movement.IsGrounded;

    private void OnValidate()
    {
        CommonUtilities.AssertIsNotNull(this, _input);
        CommonUtilities.AssertIsNotNull(this, _movement);
    }

    public void OnUpdate()
    {
        var input = _input.CurrentInput;

        if (input.PunchDown)
        {
            TryPunch();
        }
        else if (input.SpeedRunDown)
        {
            TrySpeedRun();
        }
        else if (input.SwordSlashDown)
        {
            TrySwordSlash();
        }
    }

    private void TryPunch()
    {
        if (!_isGrounded) return;
        OnPunch?.Invoke();
    }

    private void TrySpeedRun()
    {
        if (!_isGrounded) return;
        OnSpeedRun?.Invoke();
    }

    private void TrySwordSlash()
    {
        if (!_isGrounded) return;
        OnSwordSlash?.Invoke();
    }
}