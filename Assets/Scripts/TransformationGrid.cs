using UnityEngine;
using System.Collections.Generic;

public class TransformationGrid : MonoBehaviour {

	[SerializeField] private Transform prefab;
	[SerializeField] private int gridResolution = 10;
    [SerializeField, Range(0, 1)] private float pointScale = 1;
    [SerializeField] private Properties cameraProperties;
	private Transform[] grid;
	private List<Transformation> transformations;
	private Matrix4x4 transformation;

	void Awake () {
		grid = new Transform[gridResolution * gridResolution * gridResolution];
        GameObject cubeParent = new GameObject();
        cubeParent.name = "Cube";
		for (int i = 0, z = 0; z < gridResolution; z++) {
			for (int y = 0; y < gridResolution; y++) {
				for (int x = 0; x < gridResolution; x++, i++) {
					grid[i] = CreateGridPoint(x, y, z);
                    grid[i].parent = cubeParent.transform;
				}
			}
		}
		transformations = new List<Transformation>();
	}

	Transform CreateGridPoint (int x, int y, int z) {
		Transform point = Instantiate<Transform>(prefab);
		point.localPosition = GetCoordinates(x, y, z);
        point.localScale = Vector3.one * pointScale;
		point.GetComponent<MeshRenderer>().material.color = new Color(
			(float)x / gridResolution,
			(float)y / gridResolution,
			(float)z / gridResolution
		);
		return point;
	}

	Vector3 GetCoordinates (int x, int y, int z) {
		return new Vector3(
			x - (gridResolution - 1) * 0.5f,
			y - (gridResolution - 1) * 0.5f,
			z - (gridResolution - 1) * 0.5f
		);
	}

	void Update () {
		UpdateTransformation();
		for (int i = 0, z = 0; z < gridResolution; z++) {
			for (int y = 0; y < gridResolution; y++) {
				for (int x = 0; x < gridResolution; x++, i++) {
					grid[i].localPosition = TransformPoint(x, y, z);
				}
			}
		}
	}

	void UpdateTransformation () {
		GetComponents<Transformation>(transformations);
		if (transformations.Count > 0) {
            transformation = Matrix4x4.identity;// transformations[0].GetMatrix(cameraProperties);
            foreach(Transformation t in transformations) { 
			//for (int i = 1; i < transformations.Count; i++) {
                if (t.Active)
                {
                    transformation = t.GetMatrix(cameraProperties) * transformation;
                }
			}
		}
	}

	Vector3 TransformPoint (int x, int y, int z) {
		Vector3 coordinates = GetCoordinates(x, y, z);
		return transformation.MultiplyPoint(coordinates);
	}

    public void Forward()
    {
        for (int i = 3; i < transformations.Count; i++)
        {
            if (!transformations[i].Active)
            {
                transformations[i].Active = true;
                return;
            }
        }
    }

    public void Backwards()
    {
        for (int i = transformations.Count-1; i > 2; i--)
        {
            if (transformations[i].Active)
            {
                transformations[i].Active = false;
                return;
            }
        }
    }
}