using System.Collections.Generic;

public static class BreadthFirstSearch
{
    private static Queue<(List<int>, List<int>)> Open = new Queue<(List<int>, List<int>)>();
    private static List<List<int>> Closed = new List<List<int>>();

    public static List<int> PerformSearch(List<int> _board)// call from game manager
    {
        Open.Clear();
        Closed.Clear();

        if (Utlilities.CheckIfWin(_board))
        {
            return null;
        }

        var posibleMoves = Utlilities.GetPosibleMoves(_board);
        int emptyBlock = Utlilities.FindEmptyBlock(_board);

        foreach (var item in posibleMoves)
        {
            var data = Utlilities.PerformMove((_board,new List<int>()),emptyBlock,item);
            Open.Enqueue(data);
        }

        Closed.Add(_board);

        return PerformSearch();
    }

    public static List<int> PerformSearch()
    {
        var data = Open.Dequeue();

        if (Utlilities.CheckIfWin(data.Item1))
        {
            return data.Item2;
        }

        var posibleMoves = Utlilities.GetPosibleMoves(data.Item1);
        int emptyBlock = Utlilities.FindEmptyBlock(data.Item1);

        foreach (var item in posibleMoves)
        {
            var temp = Utlilities.PerformMove((data.Item1, data.Item2), emptyBlock, item);
            if (!Utlilities.CheckIfContains(Closed,temp.Item1))
            {
                Open.Enqueue(temp);
            }
        }

        Closed.Add(data.Item1);

        return PerformSearch();
    }

    public static List<int> PerformSearchWithLoop(List<int> _board)
    {
        Open.Clear();
        Closed.Clear();

        if (Utlilities.CheckIfWin(_board))
        {
            return null;
        }

        var posibleMoves = Utlilities.GetPosibleMoves(_board);
        int emptyBlock = Utlilities.FindEmptyBlock(_board);

        foreach (var item in posibleMoves)
        {
            var data = Utlilities.PerformMove((_board, new List<int>()), emptyBlock, item);
            Open.Enqueue(data);
        }
        Closed.Add(_board);

        bool flag = false;
        int Max = 9999;

        while (!flag && Max>0)
        {
            var data = Open.Dequeue();

            if (Utlilities.CheckIfWin(data.Item1))
            {
                flag = true;
                return data.Item2;
            }
            posibleMoves = Utlilities.GetPosibleMoves(data.Item1);
            emptyBlock = Utlilities.FindEmptyBlock(data.Item1);

            foreach (var item in posibleMoves)
            {
                var temp = Utlilities.PerformMove((data.Item1, data.Item2), emptyBlock, item);
                if (!Utlilities.CheckIfContains(Closed, temp.Item1))
                {
                    Open.Enqueue(temp);
                }
            }
            Closed.Add(data.Item1);
            Max--;
        }

        return null;
    }
}
