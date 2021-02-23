using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{

    [SerializeField]
    private int x, z, h;

    public int X
    {
        get
        {
            return x;
        }
    }

    public int Z
    {
        get
        {
            return z;
        }
    }

    public int Y
    {
        get
        {
            return -X - Z;
        }
    }

    public int H
    {
        get
        {
            
            return  h;
        }
    }

    public HexCoordinates(int x, int z, int h)
    {
        this.x = x;
        this.z = z;
        this.h = h;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int z, int h)
    {
       
        //return new HexCoordinates(x * 2 + z,(int)( 1.5f * z), h);
        return new HexCoordinates(x , z, h);
        //position.x = (x * HexMetrics.innerRadius * 2 + z * HexMetrics.innerRadius);
        //position.z = 1.5f * z * HexMetrics.outerRadius;
    }

    public static HexCoordinates FromPosition(Vector3 position)
    {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = -x;

        float offset = position.z / (HexMetrics.outerRadius * 3f);
        x -= offset;
        y -= offset;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);

        if (iX + iY + iZ != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }
        }

        return new HexCoordinates(iX, iZ, Mathf.CeilToInt(position.y));
    }

    public override string ToString()
    {
        return "(" +
            X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }
}