using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon2DToMesh : MonoBehaviour
{
	[SerializeField]
	private PolygonCollider2D targetPolygon;
	public Mesh mesh;

	[ContextMenu("CreateMesh")]
	public void CreateMesh()
	{
		var vertices2d = targetPolygon.points;
		Triangulator tr = new Triangulator(vertices2d);
		int[] indices =  tr.Triangulate();
		Vector3[] vertices = new Vector3[vertices2d.Length];
		for (int i=0; i<targetPolygon.points.Length; i++)
		{
			vertices[i] = new Vector3(vertices2d[i].x, vertices2d[i].y, 0.0f);
		}
		mesh=new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = indices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		
		Destroy(targetPolygon);
		(gameObject.AddComponent<MeshFilter>() as MeshFilter).mesh = mesh;
		(gameObject.AddComponent<MeshCollider>() as MeshCollider).sharedMesh = mesh;
		(gameObject.AddComponent<MeshRenderer>() as MeshRenderer).enabled = false;
	} 
}
