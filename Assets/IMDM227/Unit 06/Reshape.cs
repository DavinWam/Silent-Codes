using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reshape : MonoBehaviour
{
    void Start()
    {
        // Get the existing mesh of the cylinder
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        // Calculate the new vertex positions by shrinking the top by 50%
        Vector3[] vertices = mesh.vertices;
        float shrinkFactor = 0.5f; // Adjust this factor as needed

        // Find the maximum height of the cylinder
        float maxHeight = Mathf.NegativeInfinity;
        foreach (Vector3 vertex in vertices)
        {
            if (vertex.y > maxHeight)
            {
                maxHeight = vertex.y;
            }
        }

        for (int i = 0; i < vertices.Length; i++)
        {
            // Check if the vertex is in the top half based on Y-coordinate
            if (vertices[i].y >= maxHeight / 2)
            {
                // Calculate the distance from the center of the top face
                float distance = Mathf.Sqrt(vertices[i].x * vertices[i].x + vertices[i].z * vertices[i].z);

                // Shrink the radius of the top vertices while keeping height unchanged
                vertices[i].x *= (1 - shrinkFactor * (distance / (maxHeight / 2)));
                vertices[i].z *= (1 - shrinkFactor * (distance / (maxHeight / 2)));

                // Calculate normals based on the modified vertex positions
                Vector3 normal = CalculateCylinderNormal(vertices[i]);
                mesh.normals[i] = normal;
            }
        }

        // Update the mesh with the new vertex positions and normals
        mesh.vertices = vertices;

    }

    // Function to calculate normals for a cylinder
    Vector3 CalculateCylinderNormal(Vector3 vertex)
    {
        // Assuming the cylinder is centered at the origin, calculate the normal
        Vector3 centerTop = new Vector3(0f, vertex.y, 0f);
        Vector3 normal = (vertex - centerTop).normalized;
        return normal;
    }
}
