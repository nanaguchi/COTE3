using UnityEngine;

public class CelestialBodyController : MonoBehaviour
{
    [Header("orbit")]
    public Transform orbitCenter; 
    public float orbitSpeed = 1f; 

    [Header("rotation")]
    public float rotationSpeed = 1f; 

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        if (orbitCenter != null)
        {
            transform.RotateAround(orbitCenter.position, Vector3.up, orbitSpeed * Time.deltaTime);
        }
    }
}