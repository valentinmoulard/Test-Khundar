using System.Collections.Generic;
using UnityEngine;

public class MazeDataGenerator
{
    //will be used by the data generation algorithm to determine whether a space is empty
    public float placementThreshold;

    public MazeDataGenerator()
    {
        placementThreshold = .1f;
    }

    /// <summary>
    /// function which sets the data of the maze
    /// 1 for a wall and 0 for a walkable surface
    /// </summary>
    /// <param name="sizeRows"></param>
    /// <param name="sizeCols"></param>
    /// <returns></returns>
    public int[,] FromDimensions(int sizeRows, int sizeCols)
    {
        int[,] maze = new int[sizeRows, sizeCols];
        //GetUpperBounds return the index of the last element while GetLenght returns the number of elements in an array
        //both of the function take the dimention in parameter
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        //we loop on the 2d grid and set their values
        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                //borders of the maze set to 1 (walls)
                if (i == 0 || j == 0 || i == rMax || j == cMax)
                {
                    maze[i, j] = 1;
                }

                //generates 0 or 1 for the data of the maze
                else if (i % 2 == 0 && j % 2 == 0)
                {
                    if (Random.value > placementThreshold)
                    {
                        maze[i, j] = 1;

                        int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                        maze[i + a, j + b] = 1;
                    }
                }
            }
        }
        return maze;
    }
}