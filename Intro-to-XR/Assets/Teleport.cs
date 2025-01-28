using UnityEngine;
using UnityEngine.InputSystem;

public class XRTeleportPlayer : MonoBehaviour
{
    public Transform roomPosition;   // XR Origin's starting position
    public Transform externalPosition; // External Viewpoint

    public InputActionReference switchAction;

    private bool isInRoom = true;  // Tracks location

    void Start()
    {
        switchAction.action.Enable();
        switchAction.action.performed += TogglePosition;
    }

    void TogglePosition(InputAction.CallbackContext context)
    {
        // Switch between room and external viewpoint
        isInRoom = !isInRoom;
        transform.position = isInRoom ? roomPosition.position : externalPosition.position;
        transform.rotation = isInRoom ? roomPosition.rotation : externalPosition.rotation;
    }

    private void OnDestroy()
    {
        switchAction.action.performed -= TogglePosition;
    }
}
