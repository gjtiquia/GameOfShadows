using GJ.UnityToolbox;
using UnityEngine;

public interface IPlayerComponent
{
    public void OnUpdate();
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerDisplay _display;

    private void OnValidate()
    {
        CommonUtilities.AssertIsNotNull(this, _input);
        CommonUtilities.AssertIsNotNull(this, _movement);
        CommonUtilities.AssertIsNotNull(this, _display);
    }

    private void Update()
    {
        // Execution order matters!
        _input.OnUpdate();
        _movement.OnUpdate();
        _display.OnUpdate();
    }
}

