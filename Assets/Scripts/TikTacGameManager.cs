using System;
using System.Collections;
using UnityEngine;

public class TikTacGameManager : MonoBehaviour
{
    public event Action<Figure> GameOverEvent = delegate { };
    public event Action PlayerHadMotion = delegate { };
    public event Action HadMotion = delegate { }; // sign action like CheckWin
    public PlayMode PlayMode { get; private set; }
    [SerializeField] PlayField _playField;

    private TikTacGame _game;
    private bool _isStart;
    private Func<Figure> _choseFigurePlayer = () => Figure.none;
    public void StartGame(PlayMode playMode, int SizeField, bool playerGetFirst = false)
    {
        PlayMode = playMode;
        InitGame(SizeField);
        if(playMode == PlayMode.PVP)
        {
            _choseFigurePlayer = () => _game.CurrentMotionFigure;
            _playField.HadClickEvent += MakeMotionPlayer;
        }
        else if(playMode == PlayMode.PVE)
        {
            TikTacAI ai = new TikTacAI();
            Figure aiFigure = Figure.none;
            Action aiMotion = () =>
            {
                (int x, int y) motion = ai.MakeMotion(_game, aiFigure);
                if (motion.x >= 0) { 
                    _playField.SetFigureCell(motion.x, motion.y, aiFigure);
                    HadMotion.Invoke(); 
                }
                //add delay
            };
            if (playerGetFirst) {
                _choseFigurePlayer = () => Figure.tik;
                aiFigure = Figure.tak;
            }
            else {
                _choseFigurePlayer = () => Figure.tak;
                aiFigure = Figure.tik;
                aiMotion();
            }
           
            PlayerHadMotion += ()=> StartCoroutine(ActionWithDelay(aiMotion, 0.5f));
            _playField.HadClickEvent += MakeMotionPlayer;
        }
        else if(playMode == PlayMode.EVE)
        {
            TikTacAI ai = new TikTacAI();
            Action aiMotion = () =>
            {
                (int x, int y) motion = ai.MakeMotion(_game, _game.CurrentMotionFigure);
                if (motion.x >= 0)
                {
                    _playField.SetFigureCell(motion.x, motion.y, _game.CurrentMotionFigure);
                    HadMotion.Invoke();
                }
                //add delay
            };
            HadMotion += () => StartCoroutine(ActionWithDelay(aiMotion, 0.5f));
            aiMotion();
        }
        HadMotion += CheckOver;
        _isStart = true;
    }
    private IEnumerator ActionWithDelay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
    private void InitGame(int Size)
    {
        _game = new TikTacGame(Size);
        _playField.GenerateField(_game.Map);
    }
    public void MakeMotionPlayer(int x, int y)
    {
        Figure playerFigure = _choseFigurePlayer();
        if (_game.Map[x, y] == Figure.none && _game.CurrentMotionFigure == playerFigure)
        {
            _game.MakeMotion(x, y, playerFigure);
            _playField.SetFigureCell(x, y, playerFigure);
            PlayerHadMotion.Invoke();
            HadMotion.Invoke();
        }
    }
    public void CheckOver()
    {
        foreach (var item in _game.FindChainsFigure(Figure.tik))
        {
            if(item.SizeChain >= _game.Size)
            {
                GameOverEvent.Invoke(Figure.tik);
            }
        }
        foreach (var item in _game.FindChainsFigure(Figure.tak))
        {
            if (item.SizeChain >= _game.Size)
            {
                GameOverEvent.Invoke(Figure.tak);
            }
        }
        if (_game.GetAllFreeCells().Count == 0) GameOverEvent.Invoke(Figure.none);
        
    }
}