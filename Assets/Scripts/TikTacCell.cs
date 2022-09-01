using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TikTacCell : MonoBehaviour
{
    public event Action<int, int> HadClick = delegate { };
    [SerializeField] Sprite _none;
    [SerializeField] Sprite _tik;
    [SerializeField] Sprite _tak;

    private SpriteRenderer _spriteRenderer;
    private int _x,_y;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetFigure(Figure figure)
    {
        if (figure == Figure.none) _spriteRenderer.sprite = _none;
        if (figure == Figure.tik) _spriteRenderer.sprite = _tik;
        if (figure == Figure.tak) _spriteRenderer.sprite = _tak;
    }
    public void SetMapPosition(int x, int y)
    {
        _x = x;
        _y = y;
    }
    private void OnMouseDown()
    {
        HadClick.Invoke(_x, _y);
    }
}
