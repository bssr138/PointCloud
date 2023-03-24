using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshFilter))]
public class CubeBumpMesh : MonoBehaviour
{

Mesh mesh;

Vector3[] vertices;
int[] triangles;

public int xSize = 50;
public int zSize = 50;
public float depth = 2f;    // Define the depth of the cuboid

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
    vertices = new Vector3[(xSize + 1) * (zSize + 1)];

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

    // Calculate the total number of triangles needed for the mesh
    triangles = new int[xSize * zSize * 6];

    // Define variables to keep track of the current vertex and triangle indices
    int vert = 0;
    int tris = 0;

    // Loop through each quad of the mesh
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

    CreateCuboid();
}
    
    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for (int i=0; i<vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
    


// Method to create the cuboid from the plane
void CreateCuboid()
{
 
    


}

// Modify the UpdateMesh method to set the vertices and triangles of the mesh and recalculate the normals
void UpdateMesh()
{
    // mesh.Clear();
    mesh.vertices = vertices;
    mesh.triangles = triangles;
    mesh.RecalculateNormals();
}

}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [RequireComponent(typeof(MeshFilter))]
// public class CubeBumpMesh : MonoBehaviour
// {
// // Define the size and number of subdivisions of the cube
// public float cubeSize = 1f;
// public int numSubdivisions = 3;

// // Define the bulge parameters
// public float bulgeAmount = 0.5f;
// public float bulgeWidth = 0.5f;

// Mesh mesh;
// Vector3[] vertices;
// int[] triangles;

// void Start()
// {
//     mesh = new Mesh();
//     GetComponent<MeshFilter>().mesh = mesh;

//     CreateCube();
//     UpdateMesh();
// }

// void CreateCube()
// {
//     // Define the vertices of the cube
//     Vector3[] baseVertices =
//     {
//         new Vector3(0, 0, 0),
//         new Vector3(cubeSize, 0, 0),
//         new Vector3(0, 0, cubeSize),
//         new Vector3(cubeSize, 0, cubeSize),
//         new Vector3(0, cubeSize, 0),
//         new Vector3(cubeSize, cubeSize, 0),
//         new Vector3(0, cubeSize, cubeSize),
//         new Vector3(cubeSize, cubeSize, cubeSize)
//     };

//     // Define the triangles of the cube
//     int[] baseTriangles =
//     {
//         0, 1, 2,
//         2, 1, 3,
//         4, 6, 5,
//         5, 6, 7,
//         0, 4, 1,
//         1, 4, 5,
//         2, 3, 6,
//         6, 3, 7,
//         0, 2, 4,
//         4, 2, 6,
//         1, 5, 3,
//         3, 5, 7
//     };

//     // Apply subdivisions to the cube
//     for (int i = 0; i < numSubdivisions; i++)
//     {
//         baseVertices = SubdivideVertices(baseVertices);
//         baseTriangles = SubdivideTriangles(baseTriangles, baseVertices.Length);
//     }

//     // Apply the bulge to the front face of the cube
//     for (int i = 0; i < baseVertices.Length; i++)
//     {
//         if (baseVertices[i].z == 0f)
//         {
//             float bulgeOffset = bulgeAmount * Mathf.Exp(-(baseVertices[i].x - cubeSize / 2f) * (baseVertices[i].x - cubeSize / 2f) / (2f * bulgeWidth * bulgeWidth));
//             baseVertices[i].z += bulgeOffset;
//         }
//     }

//     vertices = baseVertices;
//     triangles = baseTriangles;
// }

// Vector3[] SubdivideVertices(Vector3[] vertices)
// {
//     List<Vector3> newVertices = new List<Vector3>();

//     for (int i = 0; i < vertices.Length; i++)
//     {
//         newVertices.Add(vertices[i]);

//         if (i < vertices.Length - 1)
//         {
//             newVertices.Add((vertices[i] + vertices[i + 1]) / 2f);
//             newVertices.Add((vertices[i] + vertices[i + 1] + vertices[(i + 2) % vertices.Length]) / 3f);
//         }
//     }

//     return newVertices.ToArray();
// }

// int[] SubdivideTriangles(int[] triangles, int numVertices)
// {
//     List<int> newTriangles = new List<int>();

//     for (int i = 0; i < triangles.Length; i += 3)
//     {
//         int v1 = triangles[i];
//         int v2 = triangles[i + 1];
//         int v3 = triangles[i + 2];
//         int v12 = numVertices + (i / 3) * 2;
//         int v23 = v12 + 1;
//         int v31 = v12 + 2;

//         newTriangles.Add(v1);
//         newTriangles.Add(v12);
//         newTriangles.Add(v31);

//         newTriangles.Add(v12);
//         newTriangles.Add(v2);
//         newTriangles.Add(v23);

//         newTriangles.Add(v31);
//         newTriangles.Add(v23);
//         newTriangles.Add(v3);

//         newTriangles.Add(v12);
//         newTriangles.Add(v23);
//         newTriangles.Add(v31);
//     }

//     return newTriangles.ToArray();
// }

// void UpdateMesh()
// {
//     mesh.Clear();
//     mesh.vertices = vertices;
//     mesh.triangles = triangles;
//     mesh.RecalculateNormals();
//     mesh.RecalculateBounds();
// }

// }