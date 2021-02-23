using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class HexCellData
{
    public int ID;
    public bool isTransparrent;
    public bool isRotatable;
    public int angle;
    public Vector3 localPosition = Vector3.zero;
}

public class HexCell : MonoBehaviour
{
    public HexCellData hexCellData;
    //public HexCoordinates coordinates;

    public HexCell(HexCellData HexCellData)
    {
        hexCellData = HexCellData;
    }

    public HexCell(int id, bool transp, bool rotate, int angl)
    {
        hexCellData = new HexCellData
        {
            ID = id,
            isTransparrent = transp,
            isRotatable = rotate,
            angle = angl
        };
    }
    public HexCell(int id, bool transp)
    {
        hexCellData = new HexCellData
        {
            ID = id,
            isTransparrent = transp
        };
    }

    public static Dictionary<BlockType, HexCell> blocks = new Dictionary<BlockType, HexCell>()
    {
        { BlockType.Air,   new HexCell(0,true)},
        { BlockType.Dirt,  new HexCell(2,false)},
        { BlockType.Grass, new HexCell(1,false)},
        { BlockType.Stone, new HexCell(3,false)},
        { BlockType.stove, new HexCell(9,false,true, 0)},
        { BlockType.Wood,  new HexCell(5,false)},
        { BlockType.Log,   new HexCell(4,false)},
    };
}
public enum BlockType { Air, Dirt, Grass, Stone, stove, Wood, Log }
