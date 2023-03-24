
/*
  Generate a mesh from code.
  (Code from Brackeys) 
  For an introduction, and how to setup your 
  scene to use this, start here:
  https://www.youtube.com/watch?v=64NblGkAabk
  Be sure to watch the above video to learn how to attach 
  this script to a Mesh Generator gameobject that contains 
  a Mesh Filter and a Mesh Renderer.
  Next, to see how the code below works, watch this:
  https://youtu.be/64NblGkAabk
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class planeBumpMesh : MonoBehaviour
{

Mesh mesh;

Vector3[] vertices;
int[] triangles;

public int xSize = 50;
public int zSize = 50;

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
}


    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        // optionally, add a mesh collider (As suggested by Franku Kek via Youtube comments).
        // To use this, your MeshGenerator GameObject needs to have a mesh collider
        // component added to it.  Then, just re-enable the code below.
        /*
        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        //*/
    }
    
    // private void OnDrawGizmos()
    // {
    //     if (vertices == null)
    //         return;

    //     for (int i=0; i<vertices.Length; i++)
    //     {
    //         Gizmos.DrawSphere(vertices[i], 0.1f);
    //     }
    // }
    

}