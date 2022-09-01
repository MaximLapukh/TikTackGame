using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayField : MonoBehaviour 
{
    public event Action<int, int> HadClickEvent = delegate { };

    [SerializeField] TikTacCell _cellPref;
    [SerializeField] Vector2 _offsetCells;

    private Dictionary<(int x, int y), TikTacCell> _cells = new Dictionary<(int x, int y), TikTacCell>();
    public void GenerateField(Figure[,] map)
    {
        int Size = (int)Mathf.Sqrt( map.Length);
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                float centerX = ((Size - 1) * _offsetCells.x) / 2;
                float centerY = ((Size - 1) * _offsetCells.y) / 2;

                TikTacCell cell = Instantiate(_cellPref,
                   new Vector3(x * _offsetCells.x - centerX, y * _offsetCells.y - centerY),
                   _cellPref.transform.rotation);
                cell.transform.SetParent(transform);

                cell.SetMapPosition(x, y);
                cell.SetFigure(map[x, y]);
                cell.HadClick += HadClick;
                _cells.Add((x, y), cell);
            }
        }
    }
    public void SetFigureCell(int x ,int y, Figure figure)
    {
        if(_cells.TryGetValue((x,y), out TikTacCell cell)){
            cell.SetFigure(figure);
        }
    }
    public void HadClick(int x, int y)
    {
        HadClickEvent.Invoke(x, y);
    }
}
