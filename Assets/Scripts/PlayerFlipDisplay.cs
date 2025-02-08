using UnityEngine;
using TarodevController;

public class PlayerFlipDisplay : MonoBehaviour
{
    [SerializeField] private GameObject objectToFlip;
    [SerializeField] private PlayerController playerController;

    private void OnValidate()
    {
        if (playerController == null)
            playerController = GetComponent<PlayerController>();

        if (objectToFlip == null)
            Debug.LogWarning("Please assign a GameObject to flip in the inspector", this);
    }

    private void Update()
    {
        Vector2 moveInput = playerController.FrameInput;

        if (moveInput.x != 0)
        {
            // Flip the object based on movement direction
            Vector3 scale = objectToFlip.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveInput.x);
            objectToFlip.transform.localScale = scale;
        }
    }
}