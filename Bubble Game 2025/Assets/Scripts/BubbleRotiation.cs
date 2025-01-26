using UnityEngine;

public class RotateAroundYAxis : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 50f;

    // Center point for rotation
    public Vector3 rotationCenter = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        // Calculate the rotation for this frame
        float rotationThisFrame = rotationSpeed * Time.deltaTime;

        // Rotate around the specified center point along the global Y axis
        transform.RotateAround(rotationCenter, Vector3.down, rotationThisFrame);
    }
}
