using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeComplete : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the player reaches the exit, load the next level
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.gameController.NextLevel();
        }
    }
}
