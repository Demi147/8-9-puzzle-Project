using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    #endregion
    private void Awake()
    {
        instance = this;
    }

    #region varibles
    [Header("Tilemap")]
    public Tilemap tilemap;

    [Header("Board varibles")]
    public List<Vector3> positions; //must order
    public List<CellPiece> cells;
    public List<int> board;

    [Header("User values")]
    public int moves;

    [Header("Search stuff")]
    public BreadthFirstMono searchAlgorithm;
    #endregion

    #region Events 
    public event EventHandler OnGUIReset;
    public event EventHandler OnBoardReset;
    public event OnMoveEventHandeler OnMove;
    public event OnPopUpMessageShowEventHandeler OnPopUpMessageShow;

    //Delegates
    public delegate void OnMoveEventHandeler(object source, int _moves);
    public delegate void OnPopUpMessageShowEventHandeler(object source , string _message);
    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        CalculateBoard();
        moves = 0;

        UIManager.instance.OnFileLoaded += Instance_OnFileLoaded;
        searchAlgorithm.OnSearchComplete += SearchAlgorithm_OnSearchComplete;
    }

    private void SearchAlgorithm_OnSearchComplete(List<int> _moves)
    {
        StartCoroutine(Solve(_moves));
    }

    private void Instance_OnFileLoaded(object source, string path)
    {
        LoadFromFile(path);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<int> CalculateBoard()
    {
        int[] tmp = new int[9];

        for (int i = 0; i < 9; i++)
        {
            foreach (var item in cells)
            {
                if (item.transform.position == positions[i])
                {
                    tmp[i] = item.CellValue;
                    break;
                }
                else
                {
                    tmp[i] = -1;
                }
            }
        }

        board = tmp.ToList();
        return tmp.ToList();
    }

    public void LoadFromFile(string _path)
    {
        try
        {

            if (Path.GetExtension(_path)!=".csv")
            {
                ShowPopup("Not a csv file");
                return;
            }

            using (StreamReader file = new StreamReader(_path))
            {
                //load first line
                var line = file.ReadLine();
                var list = Helper.LoadStringIntoBoard(line);
                if (Helper.CheckIfValidBoardState(list))
                {
                    SetBoard(list);
                }
                else
                {
                    ShowPopup("Invalid csv format");
                }
            }
        }
        catch (Exception ex)
        {
            ShowPopup(ex.Message);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #region Methods For Cells / Tiles
    public bool CheckForOverLappingCells(Vector3 _cellPos)
    {
        foreach (var item in cells)
        {
            if (item.transform.position == _cellPos)
            {
                //yes to overlapping cell
                return true;
            }
        }
        return false;
    }

    public void CellMoved()
    {
        moves += 1;

        OnMove?.Invoke(this,moves);

        if (CheckIfWin())
        {
            //User has won
            OnPopUpMessageShow?.Invoke(this,"You have won!");
            //call for reset
            ResetGame();
        }

    }
    #endregion

    #region Board / Game Logic
    bool CheckIfWin()
    {
        var board = CalculateBoard();

        for (int i = 0; i < 8; i++)
        {
            if (board[i] != i)
            {
                //it doesnt checks out
                return false;
            }
        }

        return true;
    }

    public void ResetGame()
    {
        moves = 0;
        
        OnBoardReset?.Invoke(this,EventArgs.Empty);
        OnGUIReset?.Invoke(this, EventArgs.Empty);
    }

    void SetBoard(List<int> _board)
    {
        for (int i = 0; i < _board.Count; i++)
        {
            foreach (var item in cells)
            {
                if (item.CellValue == _board[i])
                {
                    item.transform.position = positions[i];
                }
            }
        }
    }

    void ShowPopup(string _message)
    {
        OnPopUpMessageShow?.Invoke(this, _message);
    }

    public void SolvePuzzleWithFirst()
    {
        searchAlgorithm.PerformSearchMethod(CalculateBoard());
    }

    IEnumerator Solve(List<int> _ThingToSolve)
    {
        foreach (var item in _ThingToSolve)
        {
            yield return null;
            foreach (var tile in cells)
            {
                if (tile.transform.position == positions[item])
                {
                    tile.MoveCell();
                }
            }
        }
    }

    #endregion
}
