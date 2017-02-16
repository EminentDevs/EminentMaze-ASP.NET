using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

//  The Maze Class representing the entire maze.
//  It contains the map of the whole maze and the solve function for solving the maze
public class Maze
{
    //  Creates the new maze instance with the input height and width
    public Maze(byte height, byte width)
    {
        Width = width;
        Height = height;

        Map = new Element[Height, Width];
        MazeBoxes = new Button[Height, Width];

        for (byte i = 0; i < Height; i++)
        {
            for (byte j = 0; j < Width; j++)
            {
                Map[i, j] = new Element(i, j);
                Map[i, j].Status = 0;
                MazeBoxes[i, j] = new Button();
                MazeBoxes[i, j].BackColor = Color.FromArgb(138, 138, 138);
            }
        }
    }

    //  Creates an array for the solution of the maze
    public void Create_Soltion(int n)
    {
        solution = new Element[n];
    }

    public Button[,] MazeBoxes;
    public Element [,] Map;
    public byte Width;
    public byte Height;
    public Element Start_Position;
    public Element Finish_Position;
    public Element[] solution;
    public Stack<Element> allWayToSolution;

    public void Solve()
    {
        solution = null;
        Stack<Element> Solution_Finder = new Stack<Element>();
        Map[0, 0].Status = 2;
        Solution_Finder.Push(Start_Position);
        allWayToSolution = new Stack<Element>();
        allWayToSolution.Push(Start_Position);
        while (Map[Height - 1, Width - 1].Status != 2 && Solution_Finder.Count != 0)
        {
            if (Check_rightNeighborUnmarked(Solution_Finder.Peek()))
            {
                Element temp = Map[Solution_Finder.Peek().position.Row, Solution_Finder.Peek().position.Col + 1];
                temp.Status = 2;
                Solution_Finder.Push(temp);
                allWayToSolution.Push(temp);
            }
            else if (Check_bottomNeighborUnmarked(Solution_Finder.Peek()))
            {
                Element temp = Map[Solution_Finder.Peek().position.Row + 1, Solution_Finder.Peek().position.Col];
                temp.Status = 2;
                Solution_Finder.Push(temp);
                allWayToSolution.Push(temp);
            }
            else if (Check_leftNeighborUnmarked(Solution_Finder.Peek()))
            {
                Element temp = Map[Solution_Finder.Peek().position.Row, Solution_Finder.Peek().position.Col - 1];
                temp.Status = 2;
                Solution_Finder.Push(temp);
                allWayToSolution.Push(temp);
            }
            else if (Check_topNeighborUnmarked(Solution_Finder.Peek()))
            {
                Element temp = Map[Solution_Finder.Peek().position.Row - 1, Solution_Finder.Peek().position.Col];
                temp.Status = 2;
                Solution_Finder.Push(temp);
                allWayToSolution.Push(temp);
            }
            else
            {
                Element temp = Map[Solution_Finder.Peek().position.Row, Solution_Finder.Peek().position.Col];
                allWayToSolution.Push(temp);
                Solution_Finder.Pop();
            }
        }

        // Export solution
        if (Solution_Finder.Count != 0)
        {
            solution = new Element[Solution_Finder.Count];
            solution = Solution_Finder.ToArray();
        }

        // Clear marks
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (Map[i, j].Status == 2)
                {
                    Map[i, j].Status = 0;
                }
            }
        }
    }

    //  This functions checks if the right neighbor of the current box is available
    bool Check_rightNeighborUnmarked(Element input)
    {
        if (input.position.Col + 1 < Width)
        {
            if (Map[input.position.Row, input.position.Col + 1].Status == 0)
                return true;
        }
        return false;
    }


    //  This functions checks if the bottom neighbor of the current box is available
    bool Check_bottomNeighborUnmarked(Element input)
    {
        if (input.position.Row + 1 < Height)
        {
            if (Map[input.position.Row + 1, input.position.Col].Status == 0)
                return true;
        }
        return false;
    }

    //  This functions checks if the left neighbor of the current box is available
    bool Check_leftNeighborUnmarked(Element input)
    {
        if (input.position.Col - 1 >= 0)
        {
            if (Map[input.position.Row, input.position.Col - 1].Status == 0)
                return true;
        }
        return false;
    }

    //  This functions checks if the top neighbor of the current box is available
    bool Check_topNeighborUnmarked(Element input)
    {
        if (input.position.Row - 1 >= 0)
        {
            if (Map[input.position.Row - 1, input.position.Col].Status == 0)
                return true;
        }
        return false;
    }
}
