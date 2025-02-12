using UnityEngine;

public class LensCameraController : MonoBehaviour
{
    public Transform lensCamera1;
    public Transform lensCamera2;
    public Transform mainCamera;

    private void Update()
    {
        AdjustLensOrientation(lensCamera1);
        AdjustLensOrientation(lensCamera2);
    }

    // Adjust the orientation of each lens
    private void AdjustLensOrientation(Transform lens)
    {     
        Vector3 lensDirection = lens.position - mainCamera.position;
        transform.rotation = Quaternion.LookRotation(lensDirection, transform.parent.up);
    }
}
