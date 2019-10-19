using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clasic mesh generator with map and size parameters
/// </summary>
public class MeshGenerator : MonoBehaviour
{
	public MeshGenerator.SquareGrid squareGrid;

	public MeshFilter walls;

	public float wallHeight = 0.5f;

	public PolygonCollider2D wallCollider;

	private Dictionary<int, List<MeshGenerator.Triangle>> triangleDictionary = new Dictionary<int, List<MeshGenerator.Triangle>>();

	private List<List<int>> outlines = new List<List<int>>();

	private HashSet<int> checkedVertices = new HashSet<int>();

	private List<Vector3> vertices;

	private List<int> triangles;

	private const float isoLevel = 0.7f;

	public void GenerateMesh(float[,] map, float squareSize)
	{
		this.triangleDictionary.Clear();
		this.outlines.Clear();
		this.checkedVertices.Clear();
		this.squareGrid = new MeshGenerator.SquareGrid(map, squareSize, 0.7f);
		this.vertices = new List<Vector3>();
		this.triangles = new List<int>();
		for (int i = 0; i < this.squareGrid.squares.GetLength(0); i++)
		{
			for (int j = 0; j < this.squareGrid.squares.GetLength(1); j++)
			{
				this.TriangulateSquare(this.squareGrid.squares[i, j]);
			}
		}
		Mesh mesh = new Mesh();
		base.GetComponent<MeshFilter>().mesh = mesh;
		mesh.vertices = this.vertices.ToArray();
		mesh.triangles = this.triangles.ToArray();
		mesh.RecalculateNormals();
		this.CreateWallMesh();
	}

	private void CreateWallMesh()
	{
		this.CalculateMeshOutlines();
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		Mesh mesh = new Mesh();
		List<Vector2> list3 = new List<Vector2>();
		this.wallCollider.pathCount = this.outlines.Count;
		for (int i = 0; i < this.outlines.Count; i++)
		{
			list3.Clear();
			for (int j = 0; j < this.outlines[i].Count - 1; j++)
			{
				int count = list.Count;
				list.Add(this.vertices[this.outlines[i][j]]);
				list.Add(this.vertices[this.outlines[i][j + 1]]);
				list.Add(this.vertices[this.outlines[i][j]] + Vector3.forward * this.wallHeight);
				list.Add(this.vertices[this.outlines[i][j + 1]] + Vector3.forward * this.wallHeight);
				list2.Add(count);
				list2.Add(count + 2);
				list2.Add(count + 3);
				list2.Add(count + 3);
				list2.Add(count + 1);
				list2.Add(count);
				list3.Add(this.vertices[this.outlines[i][j]]);
			}
			this.wallCollider.SetPath(i, list3.ToArray());
		}
		mesh.vertices = list.ToArray();
		mesh.triangles = list2.ToArray();
		mesh.RecalculateNormals();
		this.walls.mesh = mesh;
	}

	private void TriangulateSquare(MeshGenerator.Square square)
	{
		switch (square.configuration) {
		case 0:
			break;
			
			// 1 points:
		case 1:
			MeshFromPoints(square.centreLeft, square.centreBottom, square.bottomLeft);
			break;
		case 2:
			MeshFromPoints(square.bottomRight, square.centreBottom, square.centreRight);
			break;
		case 4:
			MeshFromPoints(square.topRight, square.centreRight, square.centreTop);
			break;
		case 8:
			MeshFromPoints(square.topLeft, square.centreTop, square.centreLeft);
			break;
			
			// 2 points:
		case 3:
			MeshFromPoints(square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft);
			break;
		case 6:
			MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.centreBottom);
			break;
		case 9:
			MeshFromPoints(square.topLeft, square.centreTop, square.centreBottom, square.bottomLeft);
			break;
		case 12:
			MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreLeft);
			break;
		case 5:
			MeshFromPoints(square.centreTop, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft, square.centreLeft);
			break;
		case 10:
			MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.centreBottom, square.centreLeft);
			break;
			
			// 3 point:
		case 7:
			MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft);
			break;
		case 11:
			MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft);
			break;
		case 13:
			MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft);
			break;
		case 14:
			MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBottom, square.centreLeft);
			break;
			
			// 4 point:
		case 15:
			MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
			checkedVertices.Add(square.topLeft.vertexIndex);
			checkedVertices.Add(square.topRight.vertexIndex);
			checkedVertices.Add(square.bottomRight.vertexIndex);
			checkedVertices.Add(square.bottomLeft.vertexIndex);
			break;
		
		}
	}

	private void MeshFromPoints(params MeshGenerator.Node[] points)
	{
		this.AssignVertcis(points);
		if (points.Length >= 3)
		{
			this.CreateTriangle(points[0], points[1], points[2]);
		}
		if (points.Length >= 4)
		{
			this.CreateTriangle(points[0], points[2], points[3]);
		}
		if (points.Length >= 5)
		{
			this.CreateTriangle(points[0], points[3], points[4]);
		}
		if (points.Length >= 6)
		{
			this.CreateTriangle(points[0], points[4], points[5]);
		}
	}

	private void AssignVertcis(MeshGenerator.Node[] points)
	{
		for (int i = 0; i < points.Length; i++)
		{
			if (points[i].vertexIndex == -1)
			{
				points[i].vertexIndex = this.vertices.Count;
				this.vertices.Add(points[i].position);
			}
		}
	}

	private void CreateTriangle(MeshGenerator.Node a, MeshGenerator.Node b, MeshGenerator.Node c)
	{
		this.triangles.Add(a.vertexIndex);
		this.triangles.Add(b.vertexIndex);
		this.triangles.Add(c.vertexIndex);
		MeshGenerator.Triangle triangle = new MeshGenerator.Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);
		this.AddTriangleToDictionary(triangle.vertexIndexA, triangle);
		this.AddTriangleToDictionary(triangle.vertexIndexB, triangle);
		this.AddTriangleToDictionary(triangle.vertexIndexC, triangle);
	}

	private void AddTriangleToDictionary(int vertexIndexKey, MeshGenerator.Triangle triangle)
	{
		if (this.triangleDictionary.ContainsKey(vertexIndexKey))
		{
			this.triangleDictionary[vertexIndexKey].Add(triangle);
			return;
		}
		List<MeshGenerator.Triangle> list = new List<MeshGenerator.Triangle>();
		list.Add(triangle);
		this.triangleDictionary.Add(vertexIndexKey, list);
	}

	private void CalculateMeshOutlines()
	{
		for (int i = 0; i < this.vertices.Count; i++)
		{
			if (!this.checkedVertices.Contains(i))
			{
				int connectedOutlineVertex = this.GetConnectedOutlineVertex(i);
				if (connectedOutlineVertex != -1)
				{
					this.checkedVertices.Add(i);
					List<int> list = new List<int>();
					list.Add(i);
					this.outlines.Add(list);
					this.FollowOutline(connectedOutlineVertex, this.outlines.Count - 1);
					this.outlines[this.outlines.Count - 1].Add(i);
				}
			}
		}
	}

	private void FollowOutline(int vertexIndex, int outlineIndex)
	{
		this.outlines[outlineIndex].Add(vertexIndex);
		this.checkedVertices.Add(vertexIndex);
		int connectedOutlineVertex = this.GetConnectedOutlineVertex(vertexIndex);
		if (connectedOutlineVertex != -1)
		{
			this.FollowOutline(connectedOutlineVertex, outlineIndex);
		}
	}

	private int GetConnectedOutlineVertex(int vertexIndex)
	{
		List<MeshGenerator.Triangle> list = this.triangleDictionary[vertexIndex];
		for (int i = 0; i < list.Count; i++)
		{
			MeshGenerator.Triangle triangle = list[i];
			for (int j = 0; j < 3; j++)
			{
				int num = triangle[j];
				if (num != vertexIndex && !this.checkedVertices.Contains(num) && this.IsOutlineEdge(vertexIndex, num))
				{
					return num;
				}
			}
		}
		return -1;
	}

	private bool IsOutlineEdge(int vertexA, int vertexB)
	{
		List<MeshGenerator.Triangle> list = this.triangleDictionary[vertexA];
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Contains(vertexB))
			{
				num++;
				if (num > 1)
				{
					break;
				}
			}
		}
		return num == 1;
	}


	private struct Triangle
	{
		public Triangle(int a, int b, int c)
		{
			this.vertexIndexA = a;
			this.vertexIndexB = b;
			this.vertexIndexC = c;
			this.vertices = new int[3];
			this.vertices[0] = a;
			this.vertices[1] = b;
			this.vertices[2] = c;
		}

		public int this[int i]
		{
			get
			{
				return this.vertices[i];
			}
		}

		public bool Contains(int vertexIndex)
		{
			return vertexIndex == this.vertexIndexA || vertexIndex == this.vertexIndexB || vertexIndex == this.vertexIndexC;
		}

		public int vertexIndexA;

		public int vertexIndexB;

		public int vertexIndexC;

		private int[] vertices;
	}

	public class SquareGrid
	{
		public SquareGrid(float[,] map, float squareSize, float isoLevel)
		{
			int length = map.GetLength(0);
			int length2 = map.GetLength(1);
			float num = (float)length * squareSize;
			float num2 = (float)length2 * squareSize;
			MeshGenerator.ControlNode[,] array = new MeshGenerator.ControlNode[length, length2];
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					Vector3 position = new Vector3(-num / 2f + (float)i * squareSize + squareSize / 2f, -num2 / 2f + (float)j * squareSize + squareSize / 2f, 0f);
					float u = 0f;
					if (j < length2 - 1)
					{
						u = map[i, j + 1];
					}
					float r = 0f;
					if (i < length - 1)
					{
						r = map[i + 1, j];
					}
					array[i, j] = new MeshGenerator.ControlNode(position, map[i, j], r, u, squareSize);
				}
			}
			this.squares = new MeshGenerator.Square[length - 1, length2 - 1];
			for (int k = 0; k < length - 1; k++)
			{
				for (int l = 0; l < length2 - 1; l++)
				{
					this.squares[k, l] = new MeshGenerator.Square(array[k, l + 1], array[k + 1, l + 1], array[k + 1, l], array[k, l]);
				}
			}
		}

		public MeshGenerator.Square[,] squares;
	}

	public class Square
	{
		public MeshGenerator.ControlNode topLeft;

		public MeshGenerator.ControlNode topRight;

		public MeshGenerator.ControlNode bottomRight;

		public MeshGenerator.ControlNode bottomLeft;

		public MeshGenerator.Node centreTop;

		public MeshGenerator.Node centreRight;

		public MeshGenerator.Node centreBottom;

		public MeshGenerator.Node centreLeft;

		public int configuration;

		public Square(MeshGenerator.ControlNode topLeft, MeshGenerator.ControlNode topRight, MeshGenerator.ControlNode bottomRight, MeshGenerator.ControlNode bottomLeft)
		{
			this.topLeft = topLeft;
			this.topRight = topRight;
			this.bottomRight = bottomRight;
			this.bottomLeft = bottomLeft;
			this.centreTop = topLeft.right;
			this.centreRight = bottomRight.above;
			this.centreBottom = bottomLeft.right;
			this.centreLeft = bottomLeft.above;
			if (topLeft.active)
			{
				this.configuration += 8;
			}
			if (topRight.active)
			{
				this.configuration += 4;
			}
			if (bottomRight.active)
			{
				this.configuration += 2;
			}
			if (bottomLeft.active)
			{
				this.configuration++;
			}
		}

		
	}

	public class Node
	{
		public Node(Vector3 position)
		{
			this.position = position;
		}

		public Vector3 position;

		public int vertexIndex = -1;
	}

	public class ControlNode : MeshGenerator.Node
	{
		public bool active
		{
			get
			{
				return this.surface >= 0.7f;
			}
		}

		public ControlNode(Vector3 position, float surface, float r, float u, float squareSize)
			: base(position)
		{
			this.surface = surface;
			float t = (0.7f - surface) / (u - surface);
			float t2 = (0.7f - surface) / (r - surface);
			this.above = new MeshGenerator.Node(Vector3.Lerp(this.position, this.position + Vector3.up * squareSize, t));
			this.right = new MeshGenerator.Node(Vector3.Lerp(this.position, this.position + Vector3.right * squareSize, t2));
		}

		public MeshGenerator.Node above;

		public MeshGenerator.Node right;

		public float surface;
	}
}
