﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class RenderFrustum : MonoBehaviour {
    
    private static RenderFrustum instance;
    private static Material lineMat;
    private static GameObject g;
        
    List<Edge> edges;

    private struct Edge
    {
        public readonly Vector3 start;
        public readonly Vector3 end;
        public readonly Color color;

        public Edge(Vector3 start, Vector3 end, Color color)
        {
            this.start = start;
            this.end = end;
            this.color = color;
        }
    }

    public static RenderFrustum INSTANCE
    {
        get { return instance; }
    }
    
    private static Material LineMaterial
    {
        get 
        {
            if (!lineMat)
            {
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                lineMat = new Material(shader);
                lineMat.hideFlags = HideFlags.HideAndDontSave;
                lineMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                lineMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                lineMat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                lineMat.SetInt("_ZWrite", 0);

                g = new GameObject("g");
            }
            return lineMat;
        }
    }
       
    public static void DrawFrustum(Camera cam)
    {
        Vector3[] nearCorners = new Vector3[4]; //Approx'd nearplane corners
        Vector3[] farCorners = new Vector3[4]; //Approx'd farplane corners
        Plane[] camPlanes = GeometryUtility.CalculateFrustumPlanes(cam); //get planes from matrix

        Plane temp = camPlanes[1]; camPlanes[1] = camPlanes[2]; camPlanes[2] = temp; //swap [1] and [2] so the order is better for the loop
        for (int i = 0; i < 4; i++)
        {
            nearCorners[i] = Plane3Intersect(camPlanes[4], camPlanes[i], camPlanes[(i + 1) % 4]); //near corners on the created projection matrix
            farCorners[i] = Plane3Intersect(camPlanes[5], camPlanes[i], camPlanes[(i + 1) % 4]); //far corners on the created projection matrix
        }
        for (int i = 0; i < 4; i++)
        {
            DrawLine(nearCorners[i], nearCorners[(i + 1) % 4], Color.red); //near corners on the created projection matrix
            DrawLine(farCorners[i], farCorners[(i + 1) % 4], Color.blue); //far corners on the created projection matrix
            DrawLine(nearCorners[i], farCorners[i], Color.green); //sides of the created projection matrix
        }
    }



    public static Vector3 Plane3Intersect(Plane p1, Plane p2, Plane p3)
    { //get the intersection point of 3 planes
        return ((-p1.distance * Vector3.Cross(p2.normal, p3.normal)) +
                (-p2.distance * Vector3.Cross(p3.normal, p1.normal)) +
                (-p3.distance * Vector3.Cross(p1.normal, p2.normal))) /
         (Vector3.Dot(p1.normal, Vector3.Cross(p2.normal, p3.normal)));
    }

    

    public static void DrawCoordinateSystem(Vector3 position, Quaternion rotation, float size)
    {
        DrawLine(position, position + rotation * Vector3.up * size, Color.green);
        DrawLine(position, position + rotation * Vector3.right * size, Color.red);
        DrawLine(position, position + rotation * Vector3.forward * size, Color.blue);
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        INSTANCE.AddEdge(start, end, color);
    }

    private void AddEdge(Vector3 start, Vector3 end, Color color)
    {
        this.edges.Add(new Edge(start, end, color));
    }

    private void Awake()
    {
        instance = this;
        this.edges = new List<Edge>();
    }

    private void OnPostRender()
    {
        LineMaterial.SetPass(0);
        GL.PushMatrix();
        GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
        GL.RenderTargetBarrier();
        GL.MultMatrix(g.transform.transform.localToWorldMatrix);
        GL.Begin(GL.LINES);

        foreach(Edge edge in edges)
        {
            GL.Color(edge.color);
            GL.Vertex(edge.start);
            GL.Vertex(edge.end);
        }
       
        GL.End();
        GL.PopMatrix();

        edges.Clear();
    }



}
