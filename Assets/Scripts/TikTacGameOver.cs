using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TikTacGameOver : MonoBehaviour
{
    [SerializeField] UIScreen _tikWin;
    [SerializeField] UIScreen _takWin;
    [SerializeField] UIScreen _noneWin;
    [SerializeField] TikTacGameManager _gameManager;
    void Start()
    {
        _gameManager.GameOverEvent += OverGame;
    }

    private void OverGame(Figure winFigure)
    {
        if (winFigure == Figure.tik)      _tikWin.View();
        else if (winFigure == Figure.tak) _takWin.View();
        else                              _noneWin.View();
        
    }
}
