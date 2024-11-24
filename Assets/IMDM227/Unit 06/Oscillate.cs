using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour
{
    public float oscillationSpeed = 1f; // Adjust the speed of oscillation as needed
    public float oscillationAmplitude = 0.25f; // Adjust the amplitude of oscillation as needed

    private Mesh originalMesh;

    void Start()
    {
        // Get the existing mesh of the cylinder
        originalMesh = Instantiate(GetComponent<MeshFilter>().mesh);
    }

    void Update()
    {
        // Calculate oscillation phase based on time
        float phase = Time.time * oscillationSpeed;

        // Clone the original mesh to work on
        Mesh mesh = Instantiate(originalMesh);
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            // Check if the vertex is in the top or bottom half based on Y-coordinate
            if (vertices[i].y > 0)
            {
                // Calculate the distance from the center of the top face in the xz-plane
                float distance = Mathf.Sqrt(vertices[i].x * vertices[i].x + vertices[i].z * vertices[i].z);

                // Oscillate the radius of the top vertices in the xz-plane
                vertices[i].x *= 1 + oscillationAmplitude * Mathf.Sin(phase);
                vertices[i].z *= 1 + oscillationAmplitude * Mathf.Sin(phase);
            }
            else
            {
                // Calculate the distance from the center of the bottom face in the xz-plane
                float distance = Mathf.Sqrt(vertices[i].x * vertices[i].x + vertices[i].z * vertices[i].z);

                // Oscillate the radius of the bottom vertices in the xz-plane
                vertices[i].x *= 1 - oscillationAmplitude * Mathf.Sin(phase);
                vertices[i].z *= 1 - oscillationAmplitude * Mathf.Sin(phase);
            }
        }

        // Update the mesh with the new vertex positions
        mesh.vertices = vertices;

        // Calculate the new normals based on the modified vertex positions
        CalculateNormals(mesh);

        // Apply the modified mesh to the GameObject
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Function to calculate normals based on modified vertex positions
    void CalculateNormals(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = new Vector3[vertices.Length];

        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            int vertIndex1 = mesh.triangles[i];
            int vertIndex2 = mesh.triangles[i + 1];
            int vertIndex3 = mesh.triangles[i + 2];

            Vector3 v1 = vertices[vertIndex1];
            Vector3 v2 = vertices[vertIndex2];
            Vector3 v3 = vertices[vertIndex3];

            Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1).normalized;

            normals[vertIndex1] += normal;
            normals[vertIndex2] += normal;
            normals[vertIndex3] += normal;
        }

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i].Normalize();
        }

        mesh.normals = normals;
    }
}







