using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //the different references of the UI elements
    //we have 4 main UI
    //Title screen, in game, finalScore and credits
    public GameObject InGameUI;
    public Text levelIndicator;

    public GameObject TitleUI;
    public GameObject title;
    public GameObject characterImage;

    public GameObject EndGameUI;
    public Text finalScore;

    public GameObject CreditsUI;

    private bool movingRight;
    private bool growingText;

    private float t = 0;

    private void Start()
    {
        movingRight = true;
        growingText = true;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        float deltaTime2 = deltaTime / 3;
        t += deltaTime;

        //text animation
        if (growingText)
        {
            title.transform.localScale += new Vector3(deltaTime2, deltaTime2, deltaTime2);
        }
        else
        {
            title.transform.localScale -= new Vector3(deltaTime2, deltaTime2, deltaTime2);
        }
        if (title.transform.localScale.x > 1.2)
        {
            growingText = false;
        }
        else if (title.transform.localScale.x < 0.8)
        {
            growingText = true;
        }

        //change the color of the text
        title.GetComponent<Text>().color = Color.Lerp(Color.red, Color.blue, Mathf.PingPong(Time.time, 1));

        //character animation
        if (movingRight)
        {
            characterImage.transform.position += new Vector3(1, 0, 0) * deltaTime;
        }
        else
        {
            characterImage.transform.position -= new Vector3(1, 0, 0) * deltaTime;
        }
        if (characterImage.GetComponent<RectTransform>().position.x < -3)
        {
            movingRight = true;
            characterImage.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (characterImage.GetComponent<RectTransform>().position.x > 3)
        {
            movingRight = false;
            characterImage.transform.localScale = new Vector3(-1, 1, 1);
        }
    }


    public void UpdateLevelIndicator(int level)
    {
        levelIndicator.text = "Level " + level.ToString();
    }

    public void UpdateFinalScore(int level)
    {
        finalScore.text = level.ToString();
    }


    public void ActivateInGameUI()
    {
        InGameUI.SetActive(true);
        TitleUI.SetActive(false);
        EndGameUI.SetActive(false);
        CreditsUI.SetActive(false);
    }

    public void ActivateTitleUI()
    {
        InGameUI.SetActive(false);
        TitleUI.SetActive(true);
        EndGameUI.SetActive(false);
        CreditsUI.SetActive(false);
    }

    public void ActivateEndGameUI()
    {
        InGameUI.SetActive(false);
        TitleUI.SetActive(false);
        EndGameUI.SetActive(true);
        CreditsUI.SetActive(false);
    }

    public void ActivateCreditUI()
    {
        InGameUI.SetActive(false);
        TitleUI.SetActive(false);
        EndGameUI.SetActive(false);
        CreditsUI.SetActive(true);
    }
}
