using UnityEngine;
using System;

public class PlayerAttack : MonoBehaviour, IPlayerComponent
{
    public event Action OnPunch;
    public event Action OnSpeedRun;
    public event Action OnSwordSlash;

    [SerializeField] private PlayerInput _input;

    public void OnUpdate()
    {
        var input = _input.CurrentInput;

        if (input.PunchDown)
        {
            OnPunch?.Invoke();
        }
        else if (input.SpeedRunDown)
        {
            OnSpeedRun?.Invoke();
        }
        else if (input.SwordSlashDown)
        {
            OnSwordSlash?.Invoke();
        }
    }
}