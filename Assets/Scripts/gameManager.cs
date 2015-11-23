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
    public GameObject settingsMenu;
    public InputField jumpSpeed;
    public InputField movementWait;
    public InputField roundDuration;

    private int timeCount = 60;
    private int player1WinCount = 0;
    private int player2WinCount = 0;
    private float playerSpeed = 0.125f;
    private float playerWait = 1.0f;

    void Start ()
    {

        StartButton.SetActive(true);
        settingsMenu.SetActive(true);
	
	}


    public void StartGame ()
    {
        if (roundDuration.text != "")
            timeCount = int.Parse(roundDuration.text);

        timeRemaining.text = timeCount.ToString();

        if (jumpSpeed.text != "")
        {
            playerSpeed = float.Parse(jumpSpeed.text);

            GameObject.Find("player1").GetComponent<playerControl>().SetPlayerSpeed(playerSpeed);
            GameObject.Find("player2").GetComponent<playerControl>().SetPlayerSpeed(playerSpeed);
        }

        if (movementWait.text != "")
        {
            playerWait = float.Parse(movementWait.text);

            GameObject.Find("player1").GetComponent<playerControl>().SetMovementWait(playerWait);
            GameObject.Find("player2").GetComponent<playerControl>().SetMovementWait(playerWait);
        }

        StartCoroutine(BeginRound());
    }


    private IEnumerator BeginRound ()
    {
        StartButton.SetActive(false);
        RoundCountdownParent.SetActive(true);
        settingsMenu.SetActive(false);

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
