using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInputSettings", menuName = "Player/Input Settings")]
public class PlayerInputSettings : ScriptableObject
{
    public bool SnapInput => _snapInput;
    public float VerticalDeadZoneThreshold => _verticalDeadZoneThreshold;
    public float HorizontalDeadZoneThreshold => _horizontalDeadZoneThreshold;

    [Header("Input Settings")]
    [SerializeField] private bool _snapInput = true;
    [SerializeField, Range(0.01f, 0.99f)] private float _verticalDeadZoneThreshold = 0.3f;
    [SerializeField, Range(0.01f, 0.99f)] private float _horizontalDeadZoneThreshold = 0.1f;
}