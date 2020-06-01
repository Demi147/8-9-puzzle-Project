using System.Collections.Generic;
using System.Linq;

public static class Utlilities
{
    public static List<int> GetPosibleMoves(List<int> _board)
    {
        int emptyBlock = FindEmptyBlock(_board);

        bool TL = emptyBlock == 0;//Top left
        bool TR = emptyBlock == 2;//Top right
        bool BL = emptyBlock == 6;//Bottom Left
        bool BR = emptyBlock == 8;//Bottom Right
        bool ML = emptyBlock == 3;//Middle left
        bool MR = emptyBlock == 5;//Middle right
        bool TM = emptyBlock == 1;//Top middle
        bool BM = emptyBlock == 7;//Bottom middle
        bool Mid = emptyBlock == 4;//Middle

        int left = emptyBlock - 1;
        int right = emptyBlock + 1;
        int up = emptyBlock - 3;
        int down = emptyBlock + 3;

        List<int> results = new List<int>();

        if (TL)
        {
            results.Add(right);
            results.Add(down);
        }
        if (TR)
        {
            results.Add(left);
            results.Add(down);
        }
        if (BL)
        {
            results.Add(right);
            results.Add(up);
        }
        if (BR)
        {
            results.Add(left);
            results.Add(up);
        }
        if (ML)
        {
            results.Add(right);
            results.Add(up);
            results.Add(down);
        }
        if (MR)
        {
            results.Add(left);
            results.Add(up);
            results.Add(down);
        }
        if (TM)
        {
            results.Add(right);
            results.Add(left);
            results.Add(down);
        }
        if (BM)
        {
            results.Add(right);
            results.Add(left);
            results.Add(up);
        }
        if (Mid)
        {
            results.Add(right);
            results.Add(left);
            results.Add(up);
            results.Add(down);
        }

        return results;
    }

    public static int FindEmptyBlock(List<int> _board)
    {
        for (int i = 0; i < _board.Count; i++)
        {
            if (_board[i]==-1)
            {
                return i;
            }
        }
        return -10;
    }

    public static (List<int>, List<int>) PerformMove((List<int>, List<int>) _gameState,int index1 , int index2)
    {
        var _board = new List<int>(_gameState.Item1);
        var _moves = new List<int>(_gameState.Item2);
        //perform switch
        int temp = _board[index1];
        _board[index1] = _board[index2];
        _board[index2] = temp;

        _moves.Add(index2);

        return (_board,_moves);
    }

    public static bool CheckIfWin(List<int> _board)
    {

        for (int i = 0; i < 8; i++)
        {
            if (_board[i] != i)
            {
                //it doesnt checks out
                return false;
            }
        }

        return true;
    }

    public static bool CheckIfContains<T>(List<List<T>> _container, List<T> _thing)
    {
        foreach (var item in _container)
        {
            if (item.SequenceEqual(_thing))
            {
                return true;
            }
        }

        return false;
    }
}
