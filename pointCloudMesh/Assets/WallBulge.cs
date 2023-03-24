using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WallBulge : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 50;
    public int zSize = 50;
    public float depth = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // Create a new Mesh object and assign it to the MeshFilter component of the game object
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Call the CreateShape method to generate the vertices and triangles of the mesh
        CreateShape();

        // Update the mesh to render the changes
        UpdateMesh();
    }

    // Method to create the shape of the mesh
    void CreateShape()
    {
        // Calculate the total number of vertices needed for the mesh
        vertices = new Vector3[(xSize + 1) * (zSize + 1) + 4];

        // Define the center point of the bump
        float centerX = xSize / 6f + xSize / 2f;
        float centerZ = zSize / 6f + zSize / 2f;

        // Define the radius of the bump, height of the bump, and the distance over which the bump falls off
        float radius = 8f;
        float height = 2.5f;
        float falloffDistance = 6f;

        // Loop through each point of the mesh
        int i = 0;
        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                // Calculate the distance of the current point from the center of the bump
                float distanceFromCenter = Mathf.Sqrt(Mathf.Pow(x - centerX, 2f) + Mathf.Pow(z - centerZ, 2f));
                // Set the default height of the point to 0
                float y = 0;

                // If the distance of the point from the center is less than or equal to the radius of the bump,
                // calculate the height of the point based on the distance from the center and the desired height and curve of the bump
                if (distanceFromCenter <= radius)
                {
                    // Calculate a value between 0 and 1 based on the current distance from the center and the radius of the bump
                    float t = distanceFromCenter / radius;
                    // Calculate the curve of the bump using a cosine function
                    float curve = Mathf.Cos(t * Mathf.PI * 0.25f);
                    // Calculate the falloff of the bump based on the distance from the center and the desired falloff distance
                    float falloff = Mathf.Clamp01((radius - distanceFromCenter) / falloffDistance);
                    // Set the height of the point to the curve multiplied by the desired height and falloff
                    y += curve * height * falloff;
                }

                // Set the position of the current vertex
                vertices[i] = new Vector3(x, y, z);
                // Increment the vertex index
                i++;
            }
        }

        // Define the four additional vertices at a depth below the plane
        vertices[i++] = new Vector3(0, -depth, 0);
        vertices[i++] = new Vector3(xSize, -depth, 0);
        vertices[i++] = new Vector3(xSize, -depth, zSize);
        vertices[i++] = new Vector3(0, -depth, zSize);      
        
        // Calculate the total number of triangles needed for the mesh
        triangles = new int[(xSize * zSize * 6) + 36];

        // Define variables to keep track of the current vertex and triangle indices
        int vert = 0;
        int tris = 0;

        // Loop through each quad of the top face of the cuboid
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                // Define the indices of the vertices for the current quad
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        // Define the indices for the sides of the cuboid
        // Front face
        triangles[tris + 0] = vertices.Length - 4;
        triangles[tris + 1] = vertices.Length - 3;
        triangles[tris + 2] = 0;
        triangles[tris + 3] = vertices.Length - 3;
        triangles[tris + 4] = 1;
        triangles[tris + 5] = 0;
        // Right face
        triangles[tris + 6] = vertices.Length - 3;
        triangles[tris + 7] = vertices.Length - 2;
        triangles[tris + 8] = xSize + 1;
        triangles[tris + 9] = vertices.Length - 2;
        triangles[tris + 10] = xSize + 2;
        triangles[tris + 11] = xSize + 1;
        // Back face
        triangles[tris + 12] = vertices.Length - 2;
        triangles[tris + 13] = vertices.Length - 1;
        triangles[tris + 14] = xSize * zSize + xSize;
        triangles[tris + 15] = vertices.Length - 1;
        triangles[tris + 16] = xSize * zSize + xSize + 1;
        triangles[tris + 17] = xSize * zSize + xSize;
        // Left face
        triangles[tris + 18] = vertices.Length - 1;
        triangles[tris + 19] = vertices.Length - 4;
        triangles[tris + 20] = xSize * zSize;
        triangles[tris + 21] = vertices.Length - 4;
        triangles[tris + 22] = 0;
        triangles[tris + 23] = xSize * zSize;
        // Bottom face
        triangles[tris + 24] = vert;
        triangles[tris + 25] = vert + (xSize + 1) * (zSize + 1) + 1;
        triangles[tris + 26] = vert + (xSize + 1) * (zSize + 1);
        triangles[tris + 27] = vert + (xSize + 1) * (zSize + 1);
        triangles[tris + 28] = vert + 1;
        triangles[tris + 29] = vert + (xSize + 1) * (zSize + 1) + 1;

        // Calculate the normal vectors for each face of the cuboid
        Vector3 topNormal = Vector3.up;
        Vector3 bottomNormal = Vector3.down;
        Vector3 frontNormal = Vector3.forward;
        Vector3 backNormal = Vector3.back;
        Vector3 leftNormal = Vector3.left;
        Vector3 rightNormal = Vector3.right;

        // Calculate the total number of triangles needed for the mesh
        triangles = new int[(xSize * zSize + 4 * xSize + 4 * zSize) * 2 * 3];

        // Define variables to keep track of the current vertex and triangle indices
        // vert = 0;
        // tris = 0;

        // Loop through each quad of the mesh and add triangles to create the cuboid
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                // Define the indices of the vertices for the current quad
                int topLeft = vert + 0;
                int topRight = vert + 1;
                int bottomLeft = vert + xSize + 1;
                int bottomRight = vert + xSize + 2;

                // Create triangles for the top face of the cuboid
                triangles[tris + 0] = topLeft;
                triangles[tris + 1] = bottomLeft;
                triangles[tris + 2] = topRight;
                triangles[tris + 3] = topRight;
                triangles[tris + 4] = bottomLeft;
                triangles[tris + 5] = bottomRight;

                // Create triangles for the bottom face of the cuboid
                triangles[tris + 6] = topLeft + (xSize + 1) * (zSize + 1);
                triangles[tris + 7] = bottomRight + (xSize + 1) * (zSize + 1);
                triangles[tris + 8] = bottomLeft + (xSize + 1) * (zSize + 1);
                triangles[tris + 9] = topLeft + (xSize + 1) * (zSize + 1);
                triangles[tris + 10] = topRight + (xSize + 1) * (zSize + 1);
                triangles[tris + 11] = bottomRight + (xSize + 1) * (zSize + 1);

                // Create triangles for the front face of the cuboid
                triangles[tris + 12] = topLeft + (xSize + 1) * (zSize + 1);
                triangles[tris + 13] = bottomLeft + (xSize + 1) * (zSize + 1);
                triangles[tris + 14] = topLeft;
                triangles[tris + 15] = topLeft;
                triangles[tris + 16] = bottomLeft + (xSize + 1) * (zSize + 1);
                triangles[tris + 17] = bottomLeft;

                // Create triangles for the back face of the cuboid
                triangles[tris + 18] = topRight;
                triangles[tris + 19] = bottomRight + (xSize + 1) * (zSize + 1);
                triangles[tris + 20] = topRight + (xSize + 1) * (zSize + 1);
                triangles[tris + 21] = topRight + (xSize + 1) * (zSize + 1);
                triangles[tris + 22] = topLeft + (xSize + 1) * (zSize + 1);
                triangles[tris + 23] = topRight + (xSize + 1) * (zSize + 1);
                triangles[tris + 24] = vertices.Length - 4;
                triangles[tris + 25] = vertices.Length - 4;
                triangles[tris + 26] = topRight + (xSize + 1) * (zSize + 1);
                triangles[tris + 27] = vertices.Length - 1;

                // Create triangles for the right face of the cuboid
                triangles[tris + 28] = vertices.Length - 2;
                triangles[tris + 29] = bottomLeft + (xSize + 1) * (zSize + 1);
                triangles[tris + 30] = bottomRight + (xSize + 1) * (zSize + 1);
                triangles[tris + 31] = vertices.Length - 2;
                triangles[tris + 32] = bottomRight + (xSize + 1) * (zSize + 1);
                triangles[tris + 33] = vertices.Length - 3;

                // Increment the vertex and triangle indices
                vert++;
                tris += 34;
            }

        }
    }
    
    // Method to update the mesh
    void UpdateMesh()
    {
    // Update the mesh to render the changes
    mesh.Clear();
    mesh.vertices = vertices;
    mesh.triangles = triangles;
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();
    }

}