using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstMono : MonoBehaviour
{
    private Queue<(List<int>, List<int>)> Open = new Queue<(List<int>, List<int>)>();
    private List<List<int>> Closed = new List<List<int>>();

    public delegate void OnSearchCompleteEventHandeler(List<int> _moves);
    public event OnSearchCompleteEventHandeler OnSearchComplete;

    public void PerformSearchMethod(List<int> _board)
    {
        StartCoroutine(PerformSearch(_board));
    }

    IEnumerator PerformSearch(List<int> _board)
    {
        Open.Clear();
        Closed.Clear();

        var posibleMoves = Utlilities.GetPosibleMoves(_board);
        int emptyBlock = Utlilities.FindEmptyBlock(_board);

        foreach (var item in posibleMoves)
        {
            var data = Utlilities.PerformMove((_board, new List<int>()), emptyBlock, item);
            Open.Enqueue(data);
        }
        Closed.Add(_board);

        bool flag = false;
        int Max = 999999;

        while (!flag && Max > 0)
        {
            var data = Open.Dequeue();

            if (Utlilities.CheckIfWin(data.Item1))
            {
                flag = true;
                //event
                OnSearchComplete?.Invoke(data.Item2);
                //break
                break;
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
            yield return null;
        }
    }
}
