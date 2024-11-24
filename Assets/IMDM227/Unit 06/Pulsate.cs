using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsate : MonoBehaviour
{
    public float pulsationSpeed = 1f; // Adjust the speed of pulsation as needed
    public float pulsationAmplitude = 0.25f; // Adjust the amplitude of pulsation as needed

    private Mesh originalMesh;

    void Start()
    {
        // Get the existing mesh of the cylinder
        originalMesh = Instantiate(GetComponent<MeshFilter>().mesh);
    }

    void Update()
    {
        // Calculate pulsation phase based on time
        float phase = Time.time * pulsationSpeed;

        // Clone the original mesh to work on
        Mesh mesh = Instantiate(originalMesh);
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            // Shrink and grow the vertices in the xz-plane using a sine function
            vertices[i].x *= 1 + pulsationAmplitude * Mathf.Sin(phase);
            vertices[i].z *= 1 + pulsationAmplitude * Mathf.Sin(phase);
        }

        // Update the mesh with the new vertex positions
        mesh.vertices = vertices;

        // Apply the modified mesh to the GameObject
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
