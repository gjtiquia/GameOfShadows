using GJ.UnityToolbox;
using UnityEngine;
using UnityEngine.Assertions;

namespace TarodevController
{
    /// <summary>
    /// VERY primitive animator example.
    /// </summary>
    public class PlayerAnimator : MonoBehaviour
    {
        // SERIALIZED FIELDS
        [Header("Animation")]
        [SerializeField] private string _groundedKey = "Grounded";
        [SerializeField] private string _idleSpeedKey = "IdleSpeed";
        [SerializeField] private string _jumpKey = "Jump";

        [Header("References")]
        [SerializeField] private Animator _anim;
        [SerializeField] private GameObject _displayParent;

        [Header("Particles")]
        [SerializeField] private ParticleSystem _jumpParticles;
        [SerializeField] private ParticleSystem _launchParticles;
        [SerializeField] private ParticleSystem _moveParticles;
        [SerializeField] private ParticleSystem _landParticles;

        [Header("Jump FX")]
        [SerializeField] private GameObject _jumpFXPrefab;
        [SerializeField] private float _jumpFXDuration = 0.5f; // TODO

        [Header("Audio Clips")]
        [SerializeField] private AudioClip[] _footsteps;

        // PRIVATE MEMBERS
        private AudioSource _audioSource;
        private IPlayerController _player;
        private bool _grounded;

        private int _groundedHash;
        private int _jumpHash;

        // MonoBehaviour INTERFACE
        private void OnValidate()
        {
            Assert.IsNotNull(_anim, "Animator component is required");
            Assert.IsNotNull(_displayParent, "Display parent GameObject is required");
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _player = GetComponentInParent<IPlayerController>();
            if (_player == null)
                Debug.LogError("PlayerAnimator requires a parent GameObject with IPlayerController", this);

            // Cache animation hashes
            _groundedHash = Animator.StringToHash(_groundedKey);
            _jumpHash = Animator.StringToHash(_jumpKey);
        }

        private void OnEnable()
        {
            _player.Jumped += OnJumped;
            _player.GroundedChanged += OnGroundedChanged;

            if (_moveParticles != null)
                _moveParticles.Play();
        }

        private void OnDisable()
        {
            _player.Jumped -= OnJumped;
            _player.GroundedChanged -= OnGroundedChanged;

            if (_moveParticles != null)
                _moveParticles.Stop();
        }

        private void Update()
        {
            if (_player == null) return;

            HandleSpriteFlip();
        }

        // PRIVATE METHODS
        private void HandleSpriteFlip()
        {
            if (_player.FrameInput.x != 0)
            {
                var scale = _displayParent.transform.localScale;
                scale.x = Mathf.Abs(scale.x) * (_player.FrameInput.x < 0 ? -1 : 1);
                _displayParent.transform.localScale = scale;
            }
        }

        private void OnJumped()
        {
            _anim.SetTrigger(_jumpHash);
            _anim.ResetTrigger(_groundedHash);

            if (_grounded) // Avoid coyote
            {
                if (_jumpParticles != null)
                    _jumpParticles.Play();

                if (_launchParticles != null)
                    _launchParticles.Play();

                // Spawn jump FX
                GameObject jumpFX = ObjectPool.Instance.Get(_jumpFXPrefab);
                jumpFX.transform.position = transform.position;
                ObjectPool.Instance.ReturnDeferred(jumpFX, _jumpFXDuration);
            }
        }

        private void OnGroundedChanged(bool grounded, float impact)
        {
            _grounded = grounded;
            if (grounded)
            {
                if (_landParticles != null)
                {
                    _landParticles.transform.localScale = Vector3.one * Mathf.InverseLerp(0, 40, impact);
                    _landParticles.Play();
                }

                if (_moveParticles != null)
                    _moveParticles.Play();

                _anim.SetTrigger(_groundedHash);

                if (_audioSource != null)
                    _audioSource.PlayOneShot(_footsteps[Random.Range(0, _footsteps.Length)]);
            }
            else
            {
                if (_moveParticles != null)
                    _moveParticles.Stop();
            }
        }
    }
}