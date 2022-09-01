using System;
using System.Collections.Generic;

public class TikTacGame
{
    public Figure[,] Map { get; private set; }
    public Figure CurrentMotionFigure { get; private set; } = Figure.tik;
    public int CountRounds { get; private set; }
    public int Size { get; private set; }
    
    public TikTacGame(int Size)
    {
        Map = new Figure[Size, Size];
        this.Size = Size;
    }
    public List<(int x, int y)> GetAllFreeCells()
    {
        List<(int x, int y)> freeCells = new List<(int x, int y)>();
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                if (Map[x, y] == Figure.none) freeCells.Add((x, y));
            }
        }
        return freeCells;
    }
    public bool IsFreeCell(int x, int y) => Map[x, y] == Figure.none;
    public bool HaveWin(Figure figure)
    {
        //get all chains equal Size of figure
        foreach (var chain in FindChainsFigure(figure))
        {
            if (chain.SizeChain >= Size) return true;
        }
        return false;
    }
    public void MakeMotion(int x, int y, Figure figure)
    {
        if (figure != CurrentMotionFigure) return;
        if (Map[x, y] != Figure.none) return;
        if (x >= Size || y >= Size) return;

        Map[x, y] = figure;
        CountRounds++;

        if (CurrentMotionFigure == Figure.tik) CurrentMotionFigure = Figure.tak;
        else if(CurrentMotionFigure == Figure.tak) CurrentMotionFigure = Figure.tik;
    }

    #region FindChainFigure
    public List<Chain> FindChainsFigure (Figure figure)
    {
        List<Chain> chains = new List<Chain>();

        FindHorizontalChains(figure, ref chains);
        FindVerticalChains(figure, ref chains);
        FindDiagonalChains(figure, ref chains);

        return chains;
    }
    private void FindHorizontalChains(Figure figure, ref List<Chain> chains)
    {
        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                if (Map[x, y] == figure)
                {
                    int SizeChain = 0;

                    while ((x + SizeChain) < Size)
                    {
                        SizeChain++;
                        if (!((x + SizeChain) < Size && Map[(x + SizeChain), y] == figure)) break;
                    }

                    if (SizeChain > 1)
                    {
                        Chain chain = new Chain();
                        if (!(x - 1 < 0) && Map[x - 1, y] == Figure.none) chain.ContinuesPoint.Add((x - 1, y));
                        if (x + SizeChain < Size && Map[x + SizeChain, y] == Figure.none) chain.ContinuesPoint.Add((x + SizeChain, y));
                        chain.SizeChain = SizeChain;
                        chains.Add(chain);
                    }
                    x += SizeChain;
                }
            }
        }
    }
    private void FindVerticalChains(Figure figure, ref List<Chain> chains)
    {
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                if (Map[x, y] == figure)
                {
                    int SizeChain = 0;

                    while ((y + SizeChain) < Size)
                    {
                        SizeChain++;
                        if (!((y + SizeChain) < Size && Map[x, (y + SizeChain)] == figure)) break;
                    }

                    if (SizeChain > 1)
                    {
                        Chain chain = new Chain();
                        if (!(y - 1 < 0) && Map[x, y - 1] == Figure.none) chain.ContinuesPoint.Add((x, y - 1));
                        if (y + SizeChain < Size && Map[x, y + SizeChain] == Figure.none) chain.ContinuesPoint.Add((x, y + SizeChain));
                        chain.SizeChain = SizeChain;
                        chains.Add(chain);
                    }
                    y += SizeChain;
                }
            }
        }
    }
    private void FindDiagonalChains(Figure figure, ref List<Chain> chains)
    {
        //first diagonal
        for (int i = 0; i < Size; i++)
        {
            
            if (Map[i, i] == figure)
            {
                int SizeChain = 0;

                while ((i + SizeChain) < Size)
                {
                    SizeChain++;
                    if (!((i + SizeChain) < Size && Map[(i + SizeChain), (i + SizeChain)] == figure)) break;
                }
                if (SizeChain > 1)
                {
                    Chain chain = new Chain();
                    if(!(i - 1 < 0) && Map[i - 1, i - 1] == Figure.none) chain.ContinuesPoint.Add((i - 1, i - 1));
                    if(i + SizeChain < Size && Map[i + SizeChain, i + SizeChain] == Figure.none) chain.ContinuesPoint.Add((i + SizeChain, i + SizeChain));
                    chain.SizeChain = SizeChain;
                    chains.Add(chain);
                }
                i += SizeChain;
            }

          
        }
        //second diagonal
        for (int i = 0; i < Size; i++)
        {
            int x = i;
            int y = Size - 1 - i;
            if (Map[x, y] == figure)
            {
                int SizeChain = 0;

                while ((x + SizeChain) < Size)
                {
                    SizeChain++;
                    if (!((x + SizeChain) < Size && Map[(x + SizeChain), (y - SizeChain)] == figure)) break;
                }
                if (SizeChain > 1)
                {
                    Chain chain = new Chain();
                    if (!(x - 1 < 0) && Map[x - 1, y + 1] == Figure.none) chain.ContinuesPoint.Add((x - 1, y + 1));
                    if (x + SizeChain < Size && Map[x + SizeChain, y - SizeChain] == Figure.none) chain.ContinuesPoint.Add((x+ SizeChain, y - SizeChain));
                    chain.SizeChain = SizeChain;
                    chains.Add(chain);
                }
                i += SizeChain;
            }
        }
    }
    #endregion

    #region FindAlmostReadyChains
    public List<Chain> FindAlmostReadyChains(Figure figure)
    {
        List<Chain> chains = new List<Chain>();
        FindVerticalAlmostChains(figure, ref chains);
        FindHorizontalAlmostChains(figure, ref chains);
        FindDiagonalAlmostChains(figure, ref chains);
        return chains;
    }
    private void FindVerticalAlmostChains(Figure figure, ref List<Chain> chains)
    {
        for (int x = 0; x < Size; x++)
        {
            int countFigures = 0;
            List<(int x, int y)> freeCells = new List<(int x, int y)>();
            for (int y = 0; y < Size; y++)
            {
                if (Map[x, y] == figure) countFigures++;
                if(Map[x,y] == Figure.none) freeCells.Add((x, y));
            }
            if (countFigures + 1 == Size)
            {
                Chain chain = new Chain();
                chain.SizeChain = countFigures;
                chain.ContinuesPoint.AddRange(freeCells);
                chains.Add(chain);
            }
        }
    }
    private void FindHorizontalAlmostChains(Figure figure, ref List<Chain> chains)
    {
        for (int y = 0; y < Size; y++)
        {
            int countFigures = 0;
            List<(int x, int y)> freeCells = new List<(int x, int y)>();
            for (int x = 0; x < Size; x++)
            {
                if (Map[x, y] == figure) countFigures++;
                if(Map[x, y] == Figure.none) freeCells.Add((x, y));
            }
            if (countFigures + 1 == Size)
            {
                Chain chain = new Chain();
                chain.SizeChain = countFigures;
                chain.ContinuesPoint.AddRange(freeCells);
                chains.Add(chain);
            }
        }
    }
    private void FindDiagonalAlmostChains(Figure figure, ref List<Chain> chains)
    {
        int countFigures = 0;
        List<(int x, int y)> freeCells = new List<(int x, int y)>();
        for (int i = 0; i < Size; i++)
        {
            if (Map[i, i] == figure) countFigures++;
            if (Map[i, i] == Figure.none) freeCells.Add((i, i));
        }
        if (countFigures + 1 == Size)
        {
            Chain chain = new Chain();
            chain.SizeChain = countFigures;
            chain.ContinuesPoint.AddRange(freeCells);
            chains.Add(chain);
        }
        countFigures = 0;
        freeCells.Clear();
        //second diagonal
        for (int i = 0; i < Size; i++)
        {
            int x = i;
            int y = Size - 1 - i;
          
            if (Map[x, y] == figure) countFigures++;
            if (Map[x, y] == Figure.none) freeCells.Add((x, y));
        }
        if (countFigures + 1 == Size)
        {
            Chain chain = new Chain();
            chain.SizeChain = countFigures;
            chain.ContinuesPoint.AddRange(freeCells);
            chains.Add(chain);
        }
    }
    #endregion
}
public class Chain
{
    public List<(int x, int y)> ContinuesPoint = new List<(int, int)>();
    public int SizeChain;
}
public enum Figure
{
    none,
    tik,
    tak
}
