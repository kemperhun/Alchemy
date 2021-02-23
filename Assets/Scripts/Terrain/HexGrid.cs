using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MassCells
{
    public string name;
    public HexCellData[,,] cells;
}

public class HexGrid : MonoBehaviour
{

    public int width = 8;
    public int height = 8;
    public int depth = 15;
    int ChunkLvl;
    public HexCell cellPrefab;
    public MassCells cells;
    HexMesh hexMesh;

    void Awake()
    {
        hexMesh = GetComponentInChildren<HexMesh>();
        ChunkLvl = 4;
        cells.cells = new HexCellData[height + 2, width + 2, depth];
        rnd = (Random.value + 1) * 100 * (Random.value + 1) * 100 * (Random.value + 1) * 100;
    }

    void Start()
    {
        cells.name = name;
        StartCoroutine(Optimise(Random.value * 2));
    }

    public void AddCell(Vector3 position, int ID)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);

        position.x = (coordinates.X * HexMetrics.innerRadius * 2 + coordinates.Z * HexMetrics.innerRadius);
        position.z = 1.5f * coordinates.Z * HexMetrics.outerRadius;
        position.y = coordinates.H + 1;
        int x = coordinates.X;
        int z = coordinates.Z;
        int h = coordinates.H + 1;
        if (cells.cells[x, z, h] == null || cells.cells[x, z, h].isTransparrent)
        {
            CreateSingleCell((BlockType)ID, position, x, z, h);
        }
        hexMesh.Triangulate(cells.cells);
    }

    public void RemoveCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        //int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        // HexCell cell = cells[index];
        cells.cells[coordinates.X, coordinates.Z, coordinates.H] = HexCell.blocks[0].hexCellData;
        hexMesh.Triangulate(cells.cells);
    }
    void CreateCell(int x, int z, Vector3 target)
    {
        Vector3 position;
        //position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        //position.z = z * (HexMetrics.outerRadius * 1.5f);

        position.x = (x * HexMetrics.innerRadius * 2 + z * HexMetrics.innerRadius);
        position.z = 1.5f * z * HexMetrics.outerRadius;

        int LandLevel = ChunkLvl + GetChunkHeight(target);

        for (int i = ChunkLvl; i < LandLevel; i++)
        {
            position.y = -HexMetrics.HexHeight * i;
            CreateSingleCell(BlockType.Dirt, position, x, z, i);
        }

        position.y = -HexMetrics.HexHeight * LandLevel;
        CreateSingleCell(BlockType.Grass, position, x, z, LandLevel);
    }

    HexCell CreateSingleCell(BlockType type, Vector3 pos, int x, int z, int h)
    {
        Vector3 position = pos;
        HexCell cell = Instantiate(cellPrefab);
        cells.cells[x, z, h] = cell.hexCellData;
        cell.hexCellData.ID = HexCell.blocks[type].hexCellData.ID;
        cell.hexCellData.isRotatable = HexCell.blocks[type].hexCellData.isRotatable;
        cell.hexCellData.isTransparrent = HexCell.blocks[type].hexCellData.isTransparrent;
        cell.hexCellData.localPosition = pos;
        cell.transform.SetParent(transform, false);
        //cell.name = "cell " + x + " " + z + " " + h + " ";
        cell.transform.localPosition = position;
        //cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z, h);
        return cell;
    }

    private IEnumerator Optimise(float time)
    {
        yield return new WaitForSeconds(time);
        //for (int h = 0; h < depth; h++)
        // {
        for (int z = 0; z < height + 2; z++)
        {
            for (int x = 0; x < width + 2; x++)
            {
                CreateCell(x, z, new Vector3(transform.position.x + x, 0, transform.position.z + z));
            }
        }
        //}
        hexMesh.Triangulate(cells.cells);
    }
    int chunk;

    public float reff = 1;
    public float hei = 1;
    public float rnd = 0;
    /*
     * 0- City
     * 1- DeathField
     * 2- LiveZone
     * 3- PermaCold
     */
    public void SetChunk(int ChunkID)
    {
        chunk = ChunkID;

        switch (ChunkID)
        {
            case 0:
                reff = 0.001f;
                hei = 0.001f;
                break;
            case 1:
                reff = 0.1f;
                hei = 6;
                break;
            case 2:
                reff = 0.04f;
                hei = 0.01f;
                break;
            case 3:
                reff = 0.04f;
                hei = 0.01f;
                break;

            default:

                break;
        }
    }
    public int GetChunkHeight(Vector3 target)
    {
        //return Mathf.FloorToInt(Mathf.PerlinNoise((transform.position.x + x) * reff + rnd, (transform.position.z + z) * reff + rnd) * HexMetrics.outerRadius * hei);
        switch (chunk)
        {
            case 0:
                return Mathf.FloorToInt(Mathf.PerlinNoise((target.x) * reff + rnd, (target.z) * reff + rnd) * HexMetrics.outerRadius * hei);
            case 1:
                return Mathf.FloorToInt(Mathf.PerlinNoise((target.x) * reff + rnd, (target.z) * reff + rnd) * HexMetrics.outerRadius * hei);
            case 2:
                return Mathf.FloorToInt(Mathf.PerlinNoise((target.x) * reff + rnd, (target.z) * reff + rnd) * HexMetrics.outerRadius * hei);
            case 3:
                return Mathf.FloorToInt(Mathf.PerlinNoise((target.x) * reff + rnd, (target.z) * reff + rnd) * HexMetrics.outerRadius * hei);
            default:
                return Mathf.FloorToInt(Mathf.PerlinNoise((target.x) * reff + rnd, (target.z) * reff + rnd) * HexMetrics.outerRadius * hei);
        }
    }
}
