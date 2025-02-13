using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDisplaySettings", menuName = "Player/Display Settings")]
public class PlayerDisplaySettings : ScriptableObject
{
    public string GroundedKey => _groundedKey;
    public string JumpKey => _jumpKey;
    public string PunchKey => _punchKey;
    public string SpeedRunKey => _speedRunKey;
    public string SwordSlashKey => _swordSlashKey;

    public GameObject JumpFXPrefab => _jumpFXPrefab;
    public float JumpFXDuration => _jumpFXDuration;

    [Header("Animation Keys")]
    [SerializeField] private string _groundedKey = "Grounded";
    [SerializeField] private string _jumpKey = "Jump";
    [SerializeField] private string _punchKey = "Punch";
    [SerializeField] private string _speedRunKey = "SpeedRun";
    [SerializeField] private string _swordSlashKey = "SwordSlash";

    [Header("Jump FX")]
    [SerializeField] private GameObject _jumpFXPrefab;
    [SerializeField] private float _jumpFXDuration = 0.5f;
}