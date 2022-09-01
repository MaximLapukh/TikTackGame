using System;
using System.Collections.Generic;
using UnityEngine;

public class TikTacAI 
{
    public (int x, int y) MakeMotion(TikTacGame game, Figure figure)
    {
        Figure enemyFigure = Figure.tik;
        if (figure == Figure.tik) enemyFigure = Figure.tak;
        (int x, int y) motion = (-1, -1);
        #region win if we can
        foreach (var item in game.FindAlmostReadyChains(figure))
        {
            if(item.ContinuesPoint.Count > 0)
            {
                motion = (item.ContinuesPoint[0].x, item.ContinuesPoint[0].y);
                game.MakeMotion(motion.x, motion.y, figure);
                return motion;
            }
        }
        #endregion
     
        #region broke enemy chain

        foreach (var item in game.FindAlmostReadyChains(enemyFigure))
        {
            if(item.ContinuesPoint.Count > 0)
            {
                motion = (item.ContinuesPoint[0].x, item.ContinuesPoint[0].y);
                game.MakeMotion(motion.x, motion.y, figure);
                return motion;
            }
        }
        #endregion

        #region if size more 3, find and broke long chains of enemey
        if (game.Size > 3)
        {
            foreach (var item in game.FindChainsFigure(enemyFigure))
            {
                if (item.ContinuesPoint.Count > 0)
                {
                    motion = (item.ContinuesPoint[0].x, item.ContinuesPoint[0].y);
                    game.MakeMotion(motion.x, motion.y, figure);
                    return motion;
                }
            } 
        }
        #endregion

        #region make second motion in center, if it able
        if (game.CountRounds == 1 && game.Size % 2 != 0)
        {
            int center = game.Size - (int)Math.Round((float)game.Size / 2, 0, MidpointRounding.AwayFromZero);
            if(game.IsFreeCell(center, center))
            {
                game.MakeMotion(center, center, figure);
                motion = (center, center);
                return motion;
            }
        }
#endregion
      
        #region motion make in angels
                if (game.IsFreeCell(0, 0))
                {
                    motion = (0,0);
                    game.MakeMotion(motion.x, motion.y, figure);
                    return motion;
                }
                else if (game.IsFreeCell(game.Size - 1, 0))
                {
                    motion = (game.Size - 1, 0);
                    game.MakeMotion(motion.x, motion.y, figure);
                    return motion;
                }
                else if (game.IsFreeCell(0, game.Size - 1))
                {
                    motion = (0, game.Size - 1);
                    game.MakeMotion(motion.x, motion.y, figure);
                    return motion;
                }
                else if (game.IsFreeCell(game.Size - 1, game.Size - 1))
                {
                    motion = (game.Size - 1, game.Size - 1);
                    game.MakeMotion(motion.x, motion.y, figure);
                    return motion;
                }

        #endregion
        
        #region random motion in other way
                List<(int x, int y)> freeCells = game.GetAllFreeCells();
                if (freeCells.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, freeCells.Count);
                    motion = (freeCells[randomIndex].x, freeCells[randomIndex].y);
                    game.MakeMotion(motion.x, motion.y, figure);
                    return motion;
                }
        #endregion

        return motion;
    }
}
