using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

public class CellPiece : MonoBehaviour
{
    [Header("Cell Options")]
    public int CellValue;

    [Header("Cell UI")]
    public TextMeshProUGUI textAsset;

    #region Private varibles
    private Tilemap tilemap;
    private Vector3 startPos;
    #endregion


    private void Awake()
    {
        
    }

    private void Instance_OnBoardReset(object sender, System.EventArgs e)
    {
        transform.position = startPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GameManager.instance.tilemap;
        textAsset.text = CellValue.ToString();

        startPos = transform.position;

        //subscribe to events
        GameManager.instance.OnBoardReset += Instance_OnBoardReset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (MoveCell())
        {
            GameManager.instance.CellMoved();
        }
    }

    #region Logic
    public bool MoveCell() // call this if we want to move the tile
    {
        //logic
        Vector3Int cell = tilemap.WorldToCell(transform.position);
        //can move?
        //up
        if (CanMove(cell + new Vector3Int(0,1,0)))
        {
            PerformMove(cell + new Vector3Int(0, 1, 0));
            return true;
        }
        //right
        if (CanMove(cell + new Vector3Int(1, 0, 0)))
        {
            PerformMove(cell + new Vector3Int(1, 0, 0));
            return true;
        }
        //down
        if (CanMove(cell + new Vector3Int(0, -1, 0)))
        {
            PerformMove(cell + new Vector3Int(0, -1, 0));
            return true;
        }
        //left
        if (CanMove(cell + new Vector3Int(-1, 0, 0)))
        {
            PerformMove(cell + new Vector3Int(-1, 0, 0));
            return true;
        }

        return false;
    }

    bool CanMove(Vector3Int _cell)
    {
        Tile tile = tilemap.GetTile<Tile>(_cell);
        if (tile == null)
        {
            if (GameManager.instance.CheckForOverLappingCells(tilemap.GetCellCenterWorld(_cell)))
            {
                return false;
            }
            return true;
        }
        return false;
    }

    void PerformMove(Vector3Int cell)
    {
        Vector3 cellCentre = tilemap.GetCellCenterWorld(cell);
        transform.position = cellCentre;
    }

    #endregion
}
