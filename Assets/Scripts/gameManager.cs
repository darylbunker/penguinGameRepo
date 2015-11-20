using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameManager : MonoBehaviour {

    public Text timeRemaining;
    public Text winner;
    public Image winnerBG;
    public bool gameOver = true;
    public GameObject player1;
    public GameObject player2;
    public Image p1_Gold1;
    public Image p1_Gold2;
    public Image p2_Gold1;
    public Image p2_Gold2;
    public GameObject NextRoundButton;
    public GameObject NewGameButton;
    public GameObject RoundCountdownParent;
    public Text RoundCountdown;
    public GameObject StartButton;

    private int timeCount = 60;
    private int player1WinCount = 0;
    private int player2WinCount = 0;
	
	void Start ()
    {

        timeRemaining.text = timeCount.ToString();
        StartButton.SetActive(true);
	
	}


    public void StartGame ()
    {
        StartCoroutine(BeginRound());
    }


    private IEnumerator BeginRound ()
    {
        StartButton.SetActive(false);
        RoundCountdownParent.SetActive(true);

        RoundCountdown.text = "3";
        yield return new WaitForSeconds(1.0f);
        RoundCountdown.text = "2";
        yield return new WaitForSeconds(1.0f);
        RoundCountdown.text = "1";
        yield return new WaitForSeconds(1.0f);
        RoundCountdown.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        RoundCountdownParent.SetActive(false);

        StartCoroutine(Countdown(timeCount));
    }


    private IEnumerator Countdown(int timeLeft)
    {
        gameOver = false;

        yield return new WaitForSeconds(1.0f);

        timeLeft--;

        timeRemaining.text = timeLeft.ToString();

        if (timeLeft > 0)
            StartCoroutine(Countdown(timeLeft));
        else
        {
            gameOver = true;
            DeclareWinner();
        }

    }


    private void DeclareWinner()
    {

        winnerBG.GetComponent<Image>().enabled = true;

        if (player1.GetComponent<playerControl>().playerScoreNum > player2.GetComponent<playerControl>().playerScoreNum)
        {
            winner.text = "Player 1 wins!";
            player1WinCount++;

            if (player1WinCount == 1)
            {
                p1_Gold1.GetComponent<Image>().enabled = true;
                NextRoundButton.SetActive(true);
            }
            else if (player1WinCount == 2)
            {
                p1_Gold2.GetComponent<Image>().enabled = true;
                NewGameButton.SetActive(true);
            }
        }
        else if (player2.GetComponent<playerControl>().playerScoreNum > player1.GetComponent<playerControl>().playerScoreNum)
        {
            winner.text = "Player 2 wins!";

            player2WinCount++;

            if (player2WinCount == 1)
            {
                p2_Gold1.GetComponent<Image>().enabled = true;
                NextRoundButton.SetActive(true);
            }
            else if (player2WinCount == 2)
            {
                p2_Gold2.GetComponent<Image>().enabled = true;
                NewGameButton.SetActive(true);
            }
        }
        else
        {
            winner.text = "It's a tie!";
            NextRoundButton.SetActive(true);
        }

    }


    public void NewRound()
    {
        winnerBG.GetComponent<Image>().enabled = false;
        winner.text = "";
        NextRoundButton.SetActive(false);

        player1.GetComponent<playerControl>().Reset();
        player2.GetComponent<playerControl>().Reset();

        Start();
    }


    public void NewGame ()
    {
        NewRound();

        player1WinCount = 0;
        player2WinCount = 0;

        p1_Gold1.GetComponent<Image>().enabled = false;
        p1_Gold2.GetComponent<Image>().enabled = false;
        p2_Gold1.GetComponent<Image>().enabled = false;
        p2_Gold2.GetComponent<Image>().enabled = false;

        NewGameButton.SetActive(false);
    }

	
	void Update ()
    {
	
	}
}
