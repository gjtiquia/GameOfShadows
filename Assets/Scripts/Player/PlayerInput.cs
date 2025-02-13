using UnityEngine;

public class PlayerInput : MonoBehaviour, IPlayerComponent
{
    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    // PUBLIC MEMBERS
    public FrameInput CurrentInput => _currentInput;

    // SERIALIZED MEMBERS
    [SerializeField] private PlayerInputSettings _settings;

    // PRIVATE MEMBERS
    private FrameInput _currentInput;

    // IPlayerComponent INTERFACE
    public void OnUpdate()
    {
        GatherInput();
    }

    // PRIVATE METHODS
    private void GatherInput()
    {
        var jumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C);
        var jumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C);

        var move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (_settings.SnapInput)
        {
            move.x = Mathf.Abs(move.x) < _settings.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(move.x);
            move.y = Mathf.Abs(move.y) < _settings.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(move.y);
        }

        _currentInput = new FrameInput
        {
            JumpDown = jumpDown,
            JumpHeld = jumpHeld,
            Move = move
        };
    }
}

