using System.Collections.Generic;
using System.Linq;
using System;

public static class Helper
{
    public static bool CheckIfValidBoardState(List<int> _board)
    {
        if (_board.Count!=9)
        {
            return false;
        }
        //check if everything is unique
        List<int> temp = new List<int>();
        foreach (var item in _board)
        {
            if (!temp.Contains(item))
            {
                temp.Add(item);
            }
        }
        if (temp.Count != 9)
        {
            return false;
        }


        if (_board.Max() > 8)
        {
            return false;
        }
        if (_board.Min() < 0)
        {
            return false;
        }


        return true;
    }

    public static List<int> LoadStringIntoBoard(string _value)
    {
        try
        {
            List<int> board = new List<int>();
            var x = _value.Split(',');
            foreach (var item in x)
            {
                board.Add(int.Parse(item));
            }

            return board;
        }
        catch
        {
            return null;
        }
    }
}
