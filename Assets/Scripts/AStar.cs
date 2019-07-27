using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For more information about the implementation of A* algorithm
/// https://fr.wikipedia.org/wiki/Algorithme_A*
/// </summary>


public class AStar
{
    //2d array of spots used to calculate the shortest path
    private Spot[,] spots;
    //open list
    private List<Spot> openSet = new List<Spot>();
    //closed list
    private List<Spot> closedSet = new List<Spot>();
    //to store the shortest path
    public List<Spot> optimalPath = new List<Spot>();
    //start point and exit spot
    private Spot startSpot;
    private Spot exitSpot;

    public AStar(int[,] mazeData)
    {
        //I take the data used to generate the maze and I create the 2d array of spots
        spots = new Spot[mazeData.GetLength(0), mazeData.GetLength(1)];
    }

    public void CreatePath(int[,] mazeData, int xEntrance, int yEntrance, int xExit, int yExit)
    {
        //instantiation of the 2d array of spots
        for (int i = 0; i < mazeData.GetLength(0); i++)
        {
            for (int j = 0; j < mazeData.GetLength(1); j++)
            {
                //mazeData[i,j] == 1 => if it's an obstacle or not
                spots[i, j] = new Spot(i, j, mazeData[i,j] == 1 ? true : false);
            }
        }
        //here I define the start spot and exit spot
        startSpot = spots[xEntrance, yEntrance];
        exitSpot = spots[xExit, yExit];
        //we start by adding the startspot to the open list
        openSet.Add(startSpot);


        while (openSet.Count > 0)
        { 
            //winner keep the index of the spot with the lowest f cost
            int winner = 0;
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].F < openSet[winner].F)
                {
                    winner = i;
                }
                else if (openSet[i].F == openSet[winner].F)
                {
                    if (openSet[i].H < openSet[winner].H)
                    {
                        winner = i;
                    }
                }
            }

            Spot current = openSet[winner];

            //save the optimal path by checking the preivous spot of the current spot
            optimalPath = new List<Spot>();
            var temp = current;
            optimalPath.Add(temp);
            while (temp.previous != null)
            {
                optimalPath.Add(temp.previous);
                temp = temp.previous;
            }



            //if we reached the exit of the maze
            if (current == exitSpot)
            {
                break;
            }

            //remove the current spot from the openset and put it in the closedSet
            openSet.Remove(current);
            closedSet.Add(current);

            //creates the list of neighboors of the current Spot
            if (current.neighboors.Count == 0)
            {
                current.AddNeighboors(spots);
            }

            List<Spot> neighboors = current.neighboors;
            //we loop over the neighboors list
            for (int i = 0; i < neighboors.Count; i++)
            {
                Spot currentSpot = neighboors[i];
                //if the neighboor isn't in the closed set yet
                if (!closedSet.Contains(currentSpot) && currentSpot.isObstacle == false)
                {
                    //the g cost of the neighboor cost one more than the current spot
                    var tempG = currentSpot.G + 1;
                    //we check if it's a new path
                    bool newPath = false;

                    //if the neighboor is in the openSet
                    if (openSet.Contains(currentSpot))
                    {
                        //if the neighboor we are checking has a lower g cost
                        //we use the g cost of the neighboor as the new path because it is faster
                        if (tempG < currentSpot.G)
                        {
                            currentSpot.G = tempG;
                            newPath = true;
                        }
                    }
                    else
                    {
                        currentSpot.G = tempG;
                        newPath = true;
                        openSet.Add(currentSpot);
                    }

                    if (newPath)
                    {
                        //calculates the h and f cost
                        currentSpot.H = Heuristic(currentSpot, exitSpot);
                        currentSpot.F = currentSpot.G + currentSpot.H;
                        currentSpot.previous = current;
                    }
                }
            }
        }
    }

    /// <summary>
    /// heuristic to calculate the H cost
    /// (distance à vol d'oiseau)
    /// </summary>
    /// <param name="current"></param>
    /// <param name="exit"></param>
    /// <returns></returns>
    private int Heuristic(Spot current, Spot exit)
    {
        //manhattan heuristic (non diagonal)
        var dx = Mathf.Abs(current.x - exit.x);
        var dy = Mathf.Abs(current.y - exit.y);
        return 1 * (dx + dy);
    }

}

/// <summary>
/// class of Spot 
/// </summary>
public class Spot
{
    //x and y position
    public int x;
    public int y;
    //F, G, H costs (F = G + H)
    public int F;
    public int G;
    public int H;

    public bool isObstacle;

    public List<Spot> neighboors = new List<Spot>();

    //a reference to the previous spot (used to get the optimal path at the end)
    public Spot previous;

    //the constructor of the spot takes the x and y position and a boolean indicating if it's an obstacle or not
    public Spot(int _x, int _y, bool _isObstacle)
    {
        //x and y position
        x = _x;
        y = _y;
        //F, G, H costs
        F = 0;
        G = 0;
        H = 0;
        isObstacle = _isObstacle;
    }

    //methods that adds the neighboors of the current spot
    //takes the 2d array of spots in parameter
    public void AddNeighboors(Spot[,] maze)
    {
        if (x < maze.GetLength(0) - 1)
        {
            neighboors.Add(maze[x + 1, y]);
        }
        if (x > 0)
        {
            neighboors.Add(maze[x - 1, y]);
        }
        if (y < maze.GetLength(1) - 1)
        {
            neighboors.Add(maze[x, y + 1]);
        }
        if (y > 0)
        {
            neighboors.Add(maze[x, y - 1]);
        }
    }
}