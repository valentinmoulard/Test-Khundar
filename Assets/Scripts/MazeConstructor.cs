using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    //current maze is just a transform parent to contain all the level (so that is it more clear in the inspector)
    public GameObject currentMaze;
    //this GO will have as chils all the GO used to draw the path
    public GameObject optimalPath;

    //prefabs that will be instanciated considering the values contained in the grid/ maze
    public GameObject floor;
    public GameObject wall;
    public GameObject entrance;
    public GameObject exit;
    public GameObject path;


    //x and y position of the entrance and exit
    public int xEntrance;
    public int yEntrance;
    private int xExit;
    private int yExit;

    private MazeDataGenerator dataGenerator;
    //the maze data are contained in a 2d grid of int
    //this grid will be filled with 1 and 0.
    //1 for walls and 0 for the floor
    public int[,] data
    {
        get; private set;
    }

    void Start()
    {
        dataGenerator = new MazeDataGenerator();
    }

    /// <summary>
    /// Method used to Generate the random maze considering the size given in parameters
    /// </summary>
    /// <param name="sizeRows"></param>
    /// <param name="sizeCols"></param>
    public void GenerateNewMaze(int sizeRows, int sizeCols)
    {
        data = dataGenerator.FromDimensions(sizeRows, sizeCols);
        BuildMaze();
    }

    /// <summary>
    /// function that build the maze with the data calculated
    /// we instantiate the right prefabs considering the data of the maze
    /// we determine a random entrance and exit
    /// </summary>
    void BuildMaze()
    {
        GameController gameController = GetComponent<GameController>();
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        //set the exit
        SetExit();

        //set the entrance
        SetEntrance();

        //instanciate the right prefab considering the data calculated of the maze in the 2d array of int
        for (int i = rMax; i >= 0; i--)
        {
            for (int j = 0; j <= cMax; j++)
            {
                if (i == xExit && j == yExit)
                {
                    Instantiate(exit, new Vector3(i + 0.5f, j + 0.5f, 0), Quaternion.identity, currentMaze.transform);
                }
                else if (i == xEntrance && j == yEntrance)
                {
                    Instantiate(entrance, new Vector3(i + 0.5f, j + 0.5f, 0), Quaternion.identity, currentMaze.transform);
                }
                else if (maze[i, j] == 0)
                {
                    Instantiate(floor, new Vector3(i + 0.5f, j + 0.5f, 0), Quaternion.identity, currentMaze.transform);
                }
                else
                {
                    Instantiate(wall, new Vector3(i + 0.5f, j + 0.5f, 0), Quaternion.identity, currentMaze.transform);
                }
            }
        }
    }

    public void ShowOptimalPath(Vector3 playerPosition)
    {
        AStar aStar = new AStar(data);
        aStar.CreatePath(data, (int)playerPosition.x, (int)playerPosition.y, xExit, yExit);
        if (optimalPath.transform.childCount > 0)
        {
            foreach (Transform child in optimalPath.transform)
            {
                Destroy(child.gameObject);
            }
        }
        for (int i = 0; i < aStar.optimalPath.Count; i++)
        {
            Instantiate(path, new Vector3(aStar.optimalPath[i].x + 0.5f, aStar.optimalPath[i].y + 0.5f, 1), Quaternion.identity, optimalPath.transform);
        }
    }

    void SetExit()
    {
        bool validExit = false;
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        while (validExit == false)
        {
            int x = Random.Range(1, rMax - 1);
            int y = Random.Range(1, cMax - 1);
            if (maze[x, y] == 0)
            {
                xExit = x;
                yExit = y;
                validExit = true;
            }
        }
    }

    void SetEntrance()
    {
        bool validEntrance = false;
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        while (validEntrance == false)
        {
            int x = Random.Range(1, rMax - 1);
            int y = Random.Range(1, cMax - 1);
            if (maze[x, y] == 0)
            {
                xEntrance = x;
                yEntrance = y;
                validEntrance = true;
            }
        }
    }
}