using System.Runtime.ConstrainedExecution;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public Text coinsText, scoreText, maxScoreText;

    PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(GameManager.sharedInstance.currentGameState)
        {
            case GameState.inGame:
            case GameState.gameOver:
                int coins = GameManager.sharedInstance.collectedObject;
                float score = controller.GetTravelledDistance();
                float maxScore = PlayerPrefs.GetFloat("maxscore", 0);

                coinsText.text = coins.ToString();
                scoreText.text = "Score: " + score.ToString("f1");
                maxScoreText.text = "Max Score: " + maxScore.ToString("f1");
                break;
        }
    }
}
