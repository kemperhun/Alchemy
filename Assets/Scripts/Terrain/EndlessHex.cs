using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
public class Tile
{
    public Transform Hex;
    public float CreationTime;
    public Tile(Transform ter, float ct)
    {
        Hex = ter;
        CreationTime = ct;
    }
}

public class EndlessHex : MonoBehaviour
{
    public HexGrid HexChunkPrefab;
    public int ChunkPerLoad = 3;
    public Transform Player;
    public Transform[] Villages;
    Vector3 MovedPos;

    GameManager GameManager;
    //string path;
    string[] ExistedChunks;
    void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Random.InitState(GameManager.GetRandomSeed());
        //path = Application.dataPath + "/Configs/Config";
        ChunkPerLoad = GameManager.GetLoadDistance();
        ExistedChunks = GameManager.LoadChunksName();
        MovedPos.x = (Mathf.Sqrt(3) * HexMetrics.outerRadius) * HexChunkPrefab.width;
        MovedPos.z = HexMetrics.outerRadius * HexChunkPrefab.height * 1.5f;
        //rnd =
        StartPos = new Vector3(Player.position.x, 0, Player.position.z);
        ChunkUpdate();
        StartCoroutine(EnablePlayer(5));
        //Player.gameObject.SetActive(true);
    }
    Vector3 StartPos;

    public static Dictionary<Vector2, Tile> chunks = new Dictionary<Vector2, Tile>();
    void ChunkUpdate()
    {
        float updateTime = Time.realtimeSinceStartup;

        Vector3 p = Player.transform.position;
        float xBound = (Mathf.Floor(p.x / MovedPos.x) * MovedPos.x);
        float zBound = (Mathf.Floor(p.z / MovedPos.z) * MovedPos.z);
        for (int x = -ChunkPerLoad; x < ChunkPerLoad; x++)
        {
            for (int z = -ChunkPerLoad; z < ChunkPerLoad; z++)
            {
                float tx = ((x * MovedPos.x) + xBound);
                float tz = ((z * MovedPos.z) + zBound);
                Vector3 HexPos = new Vector3(tx, 0, tz);

               // if (Vector3.Distance(Player.transform.position, HexPos) < ChunkPerLoad * (HexChunkPrefab.width + HexChunkPrefab.height) * .75f)
                //{
                    HexPos = transform.InverseTransformPoint(HexPos);
                    HexCoordinates coordinates = HexCoordinates.FromPosition(HexPos);

                    string tilename = (coordinates.X).ToString() + "_" + (coordinates.Z).ToString();
                    if (!chunks.ContainsKey(new Vector2(coordinates.X, coordinates.Z)))
                    {
                        //if (!ExistedChunks.Contains(tilename))
                        //{
                        GameObject ter = CreateChunk(HexPos.x, HexPos.z);
                        ter.name = tilename;
                        Tile tile = new Tile(ter.transform, updateTime);
                        chunks.Add(new Vector2(coordinates.X, coordinates.Z), tile);
                        //ExistedChunks.Concat(new string[] { tilename }).ToArray();
                        //StartCoroutine(SaveDelay(Random.Range(1, 2), ter.GetComponent<HexGrid>()));
                        /*}
                        else
                        {
                            GameObject ter = new GameObject();
                            ter.AddComponent<HexGrid>();
                            HexGrid tmp = GameManager.LoadChunk(tilename);
                            ter.GetComponent<HexGrid>().cellPrefab = tmp.cellPrefab;
                            ter.GetComponent<HexGrid>().cells = tmp.cells;
                            ter.GetComponent<HexGrid>().reff = tmp.reff;
                            ter.GetComponent<HexGrid>().hei = tmp.hei;
                            ter.GetComponent<HexGrid>().rnd = tmp.rnd;
                            Tile tile = new Tile(ter.transform, updateTime);
                            chunks.Add(new Vector2(coordinates.X, coordinates.Z), tile);
                        }
                    */
                    }
                    else
                    {
                        chunks[new Vector2(coordinates.X, coordinates.Z)].CreationTime = updateTime;
                    }
                //}
            }
        }
        Dictionary<Vector2, Tile> newTerrain = new Dictionary<Vector2, Tile>();
        foreach (Vector2 tls in chunks.Keys)
        {
            if (chunks[tls].CreationTime < updateTime)
            {
                //Destroy(tls.Hex.gameObject);
                StartCoroutine(DestroyByTimer(Random.value, chunks[tls]));
            }
            else
            {
                newTerrain.Add(tls, chunks[tls]);
            }
        }
        chunks = newTerrain;
        StartPos = new Vector3(Player.position.x, 0, Player.position.z);

    }

    float xMove = 0;
    float zMove = 0;
    float rnd;
    private void Update()
    {
        xMove = (Player.position.x - StartPos.x);
        zMove = (Player.position.z - StartPos.z);
        if (Mathf.Abs(xMove) >= MovedPos.x * (ChunkPerLoad * 0.1f) || Mathf.Abs(zMove) >= MovedPos.z * (ChunkPerLoad * 0.1f))
        {
            ChunkUpdate();
        }
    }


    public float reff = 1;
    public float hei = 1;
    GameObject CreateChunk(float x, float z)
    {
        HexGrid cell = Instantiate(HexChunkPrefab);
        cell.GetComponent<HexGrid>().SetChunk(1);
        cell.transform.position = new Vector3(x, 0, z);
        cell.gameObject.SetActive(true);
        cell.transform.SetParent(transform, true);
        return cell.gameObject;
    }
    GameObject LoadChunk(float x, float z)
    {
        HexGrid cell = Instantiate(HexChunkPrefab);
        cell.GetComponent<HexGrid>().SetChunk(1);

        cell.transform.position = new Vector3(x, 0, z);
        cell.gameObject.SetActive(true);
        cell.transform.SetParent(transform, true);
        return cell.gameObject;
    }

    public void ChunVol(Slider ch)
    {
        ChunkPerLoad = (int)ch.value;
    }
    public void Chunlenght(Text ch)
    {
        //ChunkPerLoad = (int)ch.value;
        ch.text = "cur chunks = " + ChunkPerLoad;
    }
    private IEnumerator DestroyByTimer(float time, Tile tile)
    {
        yield return new WaitForSeconds(time);
        Destroy(tile.Hex.gameObject);
    }
    private IEnumerator EnablePlayer(float time)
    {
        yield return new WaitForSeconds(time);
        Player.gameObject.SetActive(true);
    }
    private IEnumerator SaveDelay(float time, HexGrid grid)
    {
        yield return new WaitForSeconds(time);
        GameManager.SaveChunk(grid);
    }
}
