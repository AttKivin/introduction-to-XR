using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public float rotationSpeed = 15f; // Degrees per second

    void Update()
    {
        // Rotate the planet around the Y-axis at a constant speed
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
