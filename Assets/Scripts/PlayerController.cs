using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //speed of the character
    public float moveSpeed;
    //boolean used to disable or activate the controls
    public bool disabledControls;

    private void Start()
    {
        disabledControls = false;
    }

    void Update()
    {
        if (disabledControls == false)
        {
            //using Z, Q, S, D to control the character
            if (Input.GetKey(KeyCode.Z))
            {
                transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.position += Vector3.down * moveSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    //replace the character at the right position
    public void CharacterInitialPosition()
    {
        MazeConstructor mazeConstructor = GameController.gameController.GetComponent<MazeConstructor>();
        transform.position = new Vector3(mazeConstructor.xEntrance + 0.5f, mazeConstructor.yEntrance + 0.5f);
    }
}
