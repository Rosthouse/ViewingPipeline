using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RenderFrustum : MonoBehaviour {
	private Camera camera;
	[SerializeField] private bool renderFrustum;

    private static Material lineMat;

    public bool IsRendering {
		get { return this.renderFrustum; }
	}
    
    // Use this for initialization
    void Start () {
		this.camera = GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.renderFrustum && this.camera != null) {
            DrawFrustum(camera);
            DrawCoordinateSystem(this.transform.position, this.transform.rotation, 5);
            DrawCoordinateSystem(Vector3.zero, Quaternion.identity, 5);
            //Gizmos.DrawFrustum (Vector3.zero, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, camera.aspect);
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
            Debug.DrawLine(nearCorners[i], nearCorners[(i + 1) % 4], Color.red, 0, true); //near corners on the created projection matrix
            Debug.DrawLine(farCorners[i], farCorners[(i + 1) % 4], Color.blue, 0, true); //far corners on the created projection matrix
            Debug.DrawLine(nearCorners[i], farCorners[i], Color.green, 0, true); //sides of the created projection matrix
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

        RenderFrustum.DrawLine(position, position + rotation * Vector3.up * size, Color.green);
        RenderFrustum.DrawLine(position, position + rotation * Vector3.right * size, Color.red);
        RenderFrustum.DrawLine(position, position + rotation * Vector3.forward * size, Color.blue);
        //GL.Begin(GL.LINES);

        //GL.Color(Color.green);
        //GL.Vertex(position);
        //GL.Vertex(position + rotation * Vector3.up * size);

        //Debug.DrawLine(position, position + rotation * Vector3.up * size, Color.green, 0, true);
        //Debug.DrawLine(position, position + rotation * Vector3.right * size, Color.red, 0, true);
        //Debug.DrawLine(position, position + rotation * Vector3.forward * size, Color.blue, 0, true);
        //GL.End();
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        return;
        GL.Begin(GL.LINES);

        lineMat.SetPass(0);

        GL.Color(Color.green);
        GL.Vertex(start);
        GL.Vertex(end);

        GL.End();
    }
}
