using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{

    Mesh hexMesh;
    List<Vector3> vertices;
    List<Color> colors;
    List<int> triangles;
    List<Vector2> UV;

    void Awake()
    {

        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        vertices = new List<Vector3>();
        colors = new List<Color>();
        triangles = new List<int>();
        UV = new List<Vector2>();
    }

    int nmb;
    public void Triangulate(HexCellData[,,] cells)
    {
        hexMesh.Clear();
        vertices.Clear();
        //colors.Clear();
        UV.Clear();
        triangles.Clear();
        Destroy(gameObject.GetComponent<MeshCollider>());
        int[] chunkOrder = { 0, 1, 2, 3, 4, 5 };
        for (int z = 0; z < cells.GetLength(2); z++)
        {
            for (int x = 1; x < cells.GetLength(0) - 1; x++)
            {
                for (int y = 1; y < cells.GetLength(1) - 1; y++)
                {
                    if (cells[x - 1, y - 1, z] != null)
                    {
                        if (z < cells.GetLength(2) - 1)
                            if (cells[x - 1, y - 1, z + 1] == null || cells[x - 1, y - 1, z + 1].isTransparrent)
                                drawUp(cells[x - 1, y - 1, z]);
                        if (z > 0)
                            if (cells[x - 1, y - 1, z - 1] == null || cells[x - 1, y - 1, z - 1].isTransparrent)
                                drawDown(cells[x - 1, y - 1, z]);
                            drawEdges(cells[x - 1, y - 1, z]);

                    }
                }
            }
        }
        hexMesh.vertices = vertices.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.uv = UV.ToArray();
        hexMesh.RecalculateNormals();
        gameObject.AddComponent<MeshCollider>();
    }

    void drawUp(HexCellData cell)
    {
        Vector2 UVCenter = new Vector2(0.081f + cell.ID * 0.125f, 0.906f);//0.125 шаг
        Vector2 UVCorner = new Vector2(0.03f, 0.03f);
        float subf = cell.ID;
        if (cell.ID > 7)
        {
            UVCenter.x = 0.081f + ((7 * (subf / 7))) * 0.125f;
            UVCenter.y = 0.906f - (0.125f * (cell.ID / 7));
            //Cy = UVCenter.y;
        }

        Vector3 center = cell.localPosition;
        for (int i = 0; i < 6; i++)
        {
            AddTriangle(
                center,
                center + HexMetrics.corners[i],
                center + HexMetrics.corners[i + 1]
            );
            UV.Add(new Vector2(UVCenter.x, UVCenter.y));
            UV.Add(new Vector2(UVCenter.x + HexMetrics.corners[i].x * UVCorner.x, UVCenter.y + HexMetrics.corners[i].z * UVCorner.y));
            UV.Add(new Vector2(UVCenter.x + HexMetrics.corners[i + 1].x * UVCorner.x, UVCenter.y + HexMetrics.corners[i + 1].z * UVCorner.y));
            //AddTriangleColor(cell.color);
        }
    }

    void drawDown(HexCellData cell)
    {
        Vector2 UVdCenter = new Vector2(0.027f + cell.ID * 0.125f, -0.094f);// 0.125 шаг
        Vector2 UVdCorner = new Vector2(0.03f, 0.03f);
        float subf = cell.ID;
        if (cell.ID > 7)
        {
            UVdCenter.x = 0.027f + ((7 * (subf / 7))) * 0.125f;
            UVdCenter.y = -0.094f - (0.125f * (cell.ID / 7));
            //Cy = UVCenter.y;
        }
        Vector3 downCenter = cell.localPosition + new Vector3(0, HexMetrics.HexHeight, 0);
        for (int i = 0; i < 6; i++)
        {
            AddTriangle(
                downCenter + HexMetrics.corners[i + 1],
                downCenter + HexMetrics.corners[i],
                downCenter
            );

            UV.Add(new Vector2(UVdCenter.x + HexMetrics.corners[i + 1].x * UVdCorner.x, UVdCenter.y + HexMetrics.corners[i + 1].z * UVdCorner.y));
            UV.Add(new Vector2(UVdCenter.x + HexMetrics.corners[i].x * UVdCorner.x, UVdCenter.y + HexMetrics.corners[i].z * UVdCorner.y));
            UV.Add(new Vector2(UVdCenter.x, UVdCenter.y));

            //AddTriangleColor(cell.color);
        }
    }
    void drawEdges(HexCellData cell)
    {
        Vector3 downCenter = cell.localPosition + new Vector3(0, HexMetrics.HexHeight, 0);
        for (int i = 0; i < 6; i++)
        {
            AddTriangle(
                downCenter + HexMetrics.corners[i + 1] - new Vector3(0, HexMetrics.HexHeight, 0),
                downCenter + HexMetrics.corners[i],
                downCenter + HexMetrics.corners[i + 1]
            );
            //AddTriangleColor(cell.color);
            AddTriangle(
                downCenter + HexMetrics.corners[i],
                downCenter + HexMetrics.corners[i + 1] - new Vector3(0, HexMetrics.HexHeight, 0),
                downCenter + HexMetrics.corners[i] - new Vector3(0, HexMetrics.HexHeight, 0)
            );
            //AddTriangleColor(cell.color);
        }
        int nx = -36;//шаг 3
        float hexnmdtmp = cell.ID * 4;
        if (hexnmdtmp > 28)
        {
            nx /= 3 * (cell.ID / 7);
        }
        Vector2 uvWidth = new Vector2(0.25f, 0.25f) / 8;
        Vector2 uvCorner = new Vector2(0.03125f, (0.75f / nx) * 3);//шаг 4

        for (int i = 0; i < 3; i++)
        {
            float uvCornerx = uvCorner.x * (i) + 0.03125f * hexnmdtmp;

            UV.Add(new Vector2(uvCornerx + uvWidth.x, uvCorner.y + uvWidth.y));
            UV.Add(new Vector2(uvCornerx, uvCorner.y));
            UV.Add(new Vector2(uvCornerx + uvWidth.x, uvCorner.y));

            UV.Add(new Vector2(uvCornerx, uvCorner.y));
            UV.Add(new Vector2(uvCornerx + uvWidth.x, uvCorner.y + uvWidth.y));
            UV.Add(new Vector2(uvCornerx, uvCorner.y + uvWidth.y));
        }
        uvCorner.x = 0.03125f;
        uvCorner.y += 0.03125f;
        for (int i = 0; i < 3; i++)
        {
            float uvCornerx = uvCorner.x * (i) + 0.03125f * hexnmdtmp;

            UV.Add(new Vector2(uvCornerx + uvWidth.x, uvCorner.y + uvWidth.y));
            UV.Add(new Vector2(uvCornerx, uvCorner.y));
            UV.Add(new Vector2(uvCornerx + uvWidth.x, uvCorner.y));

            UV.Add(new Vector2(uvCornerx, uvCorner.y));
            UV.Add(new Vector2(uvCornerx + uvWidth.x, uvCorner.y + uvWidth.y));
            UV.Add(new Vector2(uvCornerx, uvCorner.y + uvWidth.y));
        }
    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

    void AddTriangleColor(Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }
}