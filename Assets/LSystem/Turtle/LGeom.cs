using System.Collections.Generic;
using UnityEngine;

public static class LGeom
{
    /// <summary>
    /// Creates multiple filled polygons where each 3 points are considered an individual triangle
    /// </summary>
    /// <returns></returns>
    public static GameObject FilledPolygon_Triangles(List<Vector3> points, Material material = null, string name = "PolygonTriangles")
    {
        if (points.Count < 3 || points.Count % 3 != 0)
        {
            Debug.LogError("FilledPolygon_Triangles: requires at least 3 points AND point count divisible by 3.");
            return null;
        }

        GameObject go = new GameObject(name);
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        if (material != null)
        {
            mr.material = material;
        }
        else
        {
            mr.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        }

        Mesh mesh = new Mesh();
        mesh.name = "PolygonMesh";

        int vCount = points.Count;
        Vector3[] vertices = new Vector3[vCount * 2]; // duplicate vertices
        Vector3[] normals = new Vector3[vCount * 2];

        // Copy original vertices for front and back
        for (int i = 0; i < vCount; i++)
        {
            vertices[i] = points[i];         // front
            vertices[i + vCount] = points[i]; // back
        }

        // Build triangles: front + back
        int triCount = vCount / 3;
        int[] triangles = new int[triCount * 6];

        int t = 0;
        for (int i = 0; i < vCount; i += 3)
        {
            // Front
            triangles[t++] = i;
            triangles[t++] = i + 1;
            triangles[t++] = i + 2;

            // Back (using duplicated verts)
            triangles[t++] = i + 2 + vCount;
            triangles[t++] = i + 1 + vCount;
            triangles[t++] = i + vCount;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Calculate normals
        mesh.RecalculateNormals();

        // Flip normals for back vertices
        for (int i = 0; i < vCount; i++)
        {
            normals[i] = mesh.normals[i];           // front
            normals[i + vCount] = -mesh.normals[i]; // back
        }
        mesh.normals = normals;

        mesh.RecalculateBounds();

        mf.mesh = mesh;

        return go;
    }

    /// <summary>
    /// Creates a filled polygon using strip triangulation
    /// </summary>
    /// <returns></returns>
    public static GameObject FilledPolygon_Strip(List<Vector3> points, Material material = null, string name = "PolygonStrip")
    {
        if (points.Count < 3)
        {
            Debug.LogError("FilledPolygon_Strip: Need at least 3 points.");
            return null;
        }

        GameObject go = new GameObject(name);
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        if (material != null)
        {
            mr.material = material;
        }
        else
        {
            mr.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        }

        Mesh mesh = new Mesh();
        mesh.name = "PolygonMesh";

        int vCount = points.Count;

        // Duplicate vertices
        Vector3[] vertices = new Vector3[vCount * 2];
        Vector3[] normals = new Vector3[vCount * 2];

        for (int i = 0; i < vCount; i++)
        {
            vertices[i] = points[i];
            vertices[i + vCount] = points[i];
        }

        // Build triangles
        List<int> triangles = new List<int>();

        for (int i = 0; i < vCount - 2; i++)
        {
            int a, b, c;

            if (i % 2 == 0)
            {
                a = i;
                b = i + 1;
                c = i + 2;
            }
            else
            {
                a = i + 1;
                b = i;
                c = i + 2;
            }

            // Front
            triangles.Add(a);
            triangles.Add(b);
            triangles.Add(c);

            // Back (using duplicated verts)
            triangles.Add(c + vCount);
            triangles.Add(b + vCount);
            triangles.Add(a + vCount);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();

        // Flip back-face normals
        for (int i = vCount; i < normals.Length; i++)
            normals[i] = -mesh.normals[i - vCount];

        for (int i = 0; i < vCount; i++)
            normals[i] = mesh.normals[i];

        mesh.normals = normals;

        mesh.RecalculateBounds();

        mf.mesh = mesh;

        return go;
    }

    /// <summary>
    /// Creates a filled polygon using fan triangulation
    /// </summary>
    /// <returns></returns>
    public static GameObject FilledPolygon_Fan(List<Vector3> points, Material material = null, string name = "PolygonFan")
    {
        if (points.Count < 3)
        {
            Debug.LogError("FilledPolygon_Fan: Need at least 3 points.");
            return null;
        }

        GameObject go = new GameObject(name);
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        if (material != null)
        {
            mr.material = material;
        }
        else
        {
            mr.material = new Material(Shader.Find("Universal Render Pipeline/Lit")); 
        }

        Mesh mesh = new Mesh();
        mesh.name = "PolygonMesh";

        int vCount = points.Count;
        Vector3[] vertices = new Vector3[vCount * 2]; // duplicate vertices
        Vector3[] normals = new Vector3[vCount * 2];

        // Copy vertices for front and back
        for (int i = 0; i < vCount; i++)
        {
            vertices[i] = points[i];          // front
            vertices[i + vCount] = points[i]; // back
        }

        // Build triangles: front + back
        List<int> triangles = new List<int>();

        for (int i = 1; i < vCount - 1; i++)
        {
            // Front
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);

            // Back (use duplicated vertices)
            triangles.Add(i + 1 + vCount);
            triangles.Add(i + vCount);
            triangles.Add(0 + vCount);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();

        // Calculate normals
        mesh.RecalculateNormals();

        // Flip normals for back vertices
        for (int i = 0; i < vCount; i++)
        {
            normals[i] = mesh.normals[i];            // front
            normals[i + vCount] = -mesh.normals[i];  // back
        }
        mesh.normals = normals;

        mesh.RecalculateBounds();

        mf.mesh = mesh;

        return go;
    }

    /// <summary>
    /// Creates a cylinder between 2 points with a specified radius
    /// </summary>
    /// <returns></returns>
    public static GameObject Cylinder(Vector3 p1, Vector3 p2, float radius, Material material = null, string name = "Cylinder")
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        go.name = name;


        if (material != null)
        {
            var mr = go.GetComponent<MeshRenderer>();
            mr.material = material;
        }
        Vector3 dir = p2 - p1;
        float length = dir.magnitude;

        if (length <= 0f)
        {
            Debug.LogWarning("CreateCylinder: p1 and p2 are the same point!");
            length = 0.01f;
        }

        go.transform.position = p1 + dir * 0.5f;
        go.transform.up = dir.normalized;
        go.transform.localScale = new Vector3(radius * 2f, length * 0.5f, radius * 2f);

        return go;
    }

    /// <summary>
    /// Creates a cylinder from a base point, direction, length and radius
    /// </summary>
    /// <param name="p1">Point of center of base of cylinder</param>
    /// <param name="radius">Radius of cylinder centered at p1</param>
    /// <param name="length">how long the cylinder is</param>
    /// <param name="direction">direction cylinder should be formed away from p1</param>
    /// <returns></returns>
    public static GameObject Cylinder(Vector3 p1, float radius, float length, Vector3 direction, Material material = null, string name = "Cylinder")
    {
        if (direction == Vector3.zero)
        {
            Debug.LogWarning("CreateCylinder: direction is zero. Using Vector3.up by default.");
            direction = Vector3.up;
        }

        Vector3 p2 = p1 + direction.normalized * length;
        return Cylinder(p1, p2, radius, material, name);
    }

    /// <summary>
    /// Creates a sphere centered at point with radius 
    /// </summary>
    /// <returns></returns>
    public static GameObject Sphere(Vector3 position, float radius, Material material = null, string name = "Sphere")
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = name;
        go.transform.position = position;
        float scale = radius * 2f;
        go.transform.localScale = new Vector3(scale, scale, scale);

        var mr = go.GetComponent<MeshRenderer>();
        if (material != null)
        {
            mr.material = material;
        }
        else
        {
            mr.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        }

        return go;
    }
}
