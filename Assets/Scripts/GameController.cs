using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MazeConstructor))]

public class GameController : MonoBehaviour
{
    //gamecontroller containing most of the data to manage the game. Also has the singleton pattern
    public static GameController gameController;
    //reference to the UI manager
    public GameObject UIManager;
    UIManager uiManager;
    //reference to the player
    public GameObject player;
    //at what level the player is
    public int level;

    //width and height of the maze
    public int width;
    public int height;
    public int initialWidth;
    public int initialHeight;

    public float initialCameraZoom;

    private bool playerInstanciated;

    private MazeConstructor generator;
    public GameObject cam;
    void Awake()
    {
        if (gameController == null)
        {
            gameController = this;
        }
        else if (gameController != this)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// at the begining of the game
    /// </summary>
    void Start()
    {
        playerInstanciated = false;
        //stores the initial parameters (zoom level, width and height) used to restart the levels
        initialCameraZoom = cam.GetComponent<Camera>().orthographicSize;
        initialWidth = width;
        initialHeight = height;
        //take the reference of the UI manager and show the correct UI
        uiManager = UIManager.GetComponent<UIManager>();
        uiManager.ActivateTitleUI();
    }

    /// <summary>
    /// function called when the player hits the start button on the main menu
    /// </summary>
    public void StartGame()
    {
        //shows the correct UI
        uiManager.ActivateInGameUI();
        
        //generates the maze with the right sizes
        generator = GetComponent<MazeConstructor>();
        width = initialWidth;
        height = initialHeight;
        generator.GenerateNewMaze(initialWidth, initialHeight);

        //set the camera to the center of the maze as the maze grow through the levels
        cam.GetComponent<Camera>().orthographicSize = initialCameraZoom;
        CameraPosition();

        //we start at level 1
        level = 1;
        uiManager.UpdateLevelIndicator(level);
        if (playerInstanciated == false)
        {
            //we instanciate the player and set it to the right position
            player = Instantiate(player, Vector3.zero, Quaternion.identity);
            playerInstanciated = true;
        }
        else
        {
            //if we already instatiated the player, just set it active
            //can happen if we restart playing. We don't need to instatiate an other player
            player.SetActive(true);
        }
        //sets the player at the right location
        player.GetComponent<PlayerController>().CharacterInitialPosition();
    }


    //FUNCTIONS MANAGING THE CAMERA ======================================================================================
    //(As I only have 2 functions for the camera, I decided to put them in the GameController
    //to avoid accessing the camera in an other script from here)

    /// <summary>
    /// function to re position the camera to the center of the maze
    /// </summary>
    void CameraPosition()
    {
        cam.transform.position = new Vector3(width * 0.5f, height * 0.5f - 0.5f, -1);
    }
    /// <summary>
    /// function that zooms out as the maze grows
    /// </summary>
    /// <returns></returns>
    IEnumerator ZoomOutCoroutine()
    {
        float timeStamp = Time.time;
        player.GetComponent<PlayerController>().disabledControls = true;
        while (Time.time - timeStamp < 1)
        {
            cam.GetComponent<Camera>().orthographicSize += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        player.GetComponent<PlayerController>().disabledControls = false;
    }



    //Function concerning the elements of the maze
    /// <summary>
    /// Function to destroy the current maze (used when we go to the next level, end the game or go back to the main menu
    /// </summary>
    void DestroyCurrentMaze()
    {
        Transform currentMazeTransform = GetComponent<MazeConstructor>().currentMaze.transform;
        foreach (Transform child in currentMazeTransform)
        {
            Destroy(child.gameObject);
        }
    }
    /// <summary>
    /// function that calls the method in the Astar script to calculate the optimal path considering the position of the player
    /// </summary>
    public void CalculateOptimalPath()
    {
        generator.ShowOptimalPath(player.transform.position);
    }
    /// <summary>
    /// function to destroy the game objects used to show the optimal path.
    /// Used when we click on the button to show the optimal path again (the player might have moved since last time the optimal path was calculated)
    /// and used when we generate a new maze, go back to the menu or end the game
    /// </summary>
    public void DestroyOptimalPath()
    {
        Transform currentPathTransform = GetComponent<MazeConstructor>().optimalPath.transform;
        foreach (Transform child in currentPathTransform.transform)
        {
            Destroy(child.gameObject);
        }
    }


    //MANAGE THE STATES OF THE GAME========================================================================

    /// <summary>
    /// next level function
    /// </summary>
    public void NextLevel()
    {
        DestroyCurrentMaze();
        DestroyOptimalPath();

        //add 1 to the level counter
        level++;
        //upadate the UI
        uiManager.UpdateLevelIndicator(level);
        //we expand the maze
        if (level % 2 == 0)
        {
            generator.GenerateNewMaze(width++, height++);
        }
        else
        {
            generator.GenerateNewMaze(width, height++);
        }

        //and we reposition the character to the entrance of the new maze
        player.GetComponent<PlayerController>().CharacterInitialPosition();
        //replace the camera too
        CameraPosition();
        //zoom out as the level increases
        if (level % 2 == 1)
        {
            StartCoroutine(ZoomOutCoroutine());
        }
    }

    public void QuitGame()
    {
        player.SetActive(false);
        //destroy the previous maze
        DestroyCurrentMaze();
        DestroyOptimalPath();
        uiManager.ActivateEndGameUI();
        uiManager.UpdateFinalScore(level);
    }

    public void ToMainMenu()
    {
        uiManager.ActivateTitleUI();
        DestroyCurrentMaze();
        DestroyOptimalPath();
        cam.transform.position = new Vector3(initialWidth * 0.5f, initialHeight * 0.5f, -1);
    }

    public void ToCredits()
    {
        uiManager.ActivateCreditUI();
    }

    
}