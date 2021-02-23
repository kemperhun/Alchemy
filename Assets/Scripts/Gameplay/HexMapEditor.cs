using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour
{

    //public Color[] colors;

    public HexGrid hexGrid;
    public GameManager GameManager;
    private Color activeColor;
    private int activeID = 1;

    void Awake()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //SelectColor(0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                if (hit.transform.parent.CompareTag("Ground"))
                {
                    hit.transform.parent.GetComponent<HexGrid>().AddCell(hit.point, activeID);
                    GameManager.SaveChunk(hit.transform.parent.GetComponent<HexGrid>());
                }
            }
        }
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                if (hit.transform.parent.CompareTag("Ground"))
                {
                    hit.transform.parent.GetComponent<HexGrid>().RemoveCell(hit.point);
                    GameManager.SaveChunk(hit.transform.parent.GetComponent<HexGrid>());
                }
            }
        }
    }

    public void SelectBlockId(int index)
    {
        activeID = index;
    }
}