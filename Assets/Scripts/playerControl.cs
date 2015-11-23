using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class playerControl : MonoBehaviour {

    public int playerScoreNum = 0;
    public bool hasFallen = false;
	public bool canMove = false;
    public bool isMoving = false;
	public Vector3 deathTargetPos = Vector3.zero;
	public GameObject deathUp;
	public GameObject deathDown;
	public GameObject deathRight;
	public GameObject deathLeft;
	public Text playerScore;
	public Image joystickImage;
	public GameObject otherPlayer1;

    private int currentFloat = 0;
	private float xPos;
	private float yPos;
    private float playerSpeed = 0.125f;
    private float delayPeriod = 1.0f;
	private bool resetJoystick = true;
	private bool moveOffScreen = false;
    private bool once = false;
	private string moveDirection = "";
	private GameObject[] iceFloats = new GameObject[35];
	private Vector3 targetPos = new Vector3(-100, -100, -100);
	private Color originColor;

	void Start () 
	{

        originColor = joystickImage.color;

		for (int i=0; i<35; i++)
		{
			iceFloats[i] = GameObject.Find("iceFloat_" + i.ToString());
		}

		if (gameObject.name == "player1")
		{
			gameObject.transform.position = new Vector3(iceFloats[34].transform.position.x, iceFloats[34].transform.position.y, 0.0f);
			currentFloat = 34;
		}
		else if (gameObject.name == "player2")
		{
			gameObject.transform.position = new Vector3(iceFloats[28].transform.position.x, iceFloats[28].transform.position.y, 0.0f);
			currentFloat = 28;
		}
		
		canMove = true;
	
	}


    public void SetPlayerSpeed (float speed)
    {
        playerSpeed = speed;
    }


    public void SetMovementWait (float duration)
    {
        delayPeriod = duration;
    }


	void MovePlayer(string direction)
	{

		if (direction == "right")
		{
			if (currentFloat != 6 && currentFloat != 13 && currentFloat != 20 && currentFloat != 27 && currentFloat != 34)
				currentFloat++;
			else
			{
				moveOffScreen = true;
				currentFloat -= 6;
			}
		}
		else if (direction == "left")
		{
			if (currentFloat != 0 && currentFloat != 7 && currentFloat != 14 && currentFloat != 21 && currentFloat != 28)
				currentFloat--;
			else
			{
				moveOffScreen = true;
				currentFloat += 6;
			}
		}
		else if (direction == "up")
		{
			if (currentFloat >= 7)
				currentFloat -= 7;
			else
			{
				moveOffScreen = true;
				currentFloat += 28;
			}
		}
		else if (direction == "down")
		{
			if (currentFloat < 28)
				currentFloat += 7;
			else
			{
				moveOffScreen = true;
				currentFloat -= 28;
			}
		}

		targetPos = new Vector3(iceFloats[currentFloat].transform.position.x, iceFloats[currentFloat].transform.position.y, 0.0f);

		canMove = false;
        isMoving = true;
		resetJoystick = false;

	}


	void OnTriggerEnter2D (Collider2D hit)
	{
        if (hit.gameObject.tag == "Player")
        {
            //need to account for both players moving at the same time, running into each other
            if (isMoving == true && hit.gameObject.GetComponent<playerControl>().isMoving == false && hit.gameObject.GetComponent<playerControl>().hasFallen == false
                && hit.gameObject.GetComponent<Renderer>().enabled == true)
            {
                if (moveDirection == "right")
                {
                    hit.gameObject.GetComponent<playerControl>().deathTargetPos = hit.gameObject.GetComponent<playerControl>().deathRight.transform.position;
                }
                else if (moveDirection == "left")
                {
                    hit.gameObject.GetComponent<playerControl>().deathTargetPos = hit.gameObject.GetComponent<playerControl>().deathLeft.transform.position;
                }
                else if (moveDirection == "up")
                {
                    hit.gameObject.GetComponent<playerControl>().deathTargetPos = hit.gameObject.GetComponent<playerControl>().deathUp.transform.position;
                }
                else if (moveDirection == "down")
                {
                    hit.gameObject.GetComponent<playerControl>().deathTargetPos = hit.gameObject.GetComponent<playerControl>().deathDown.transform.position;
                }

                hit.gameObject.GetComponent<playerControl>().canMove = false;
                hit.gameObject.GetComponent<playerControl>().hasFallen = true;
                hit.gameObject.GetComponent<playerControl>().joystickImage.color = new Color(0.25f, 0.25f, 0.25f, 1);

                playerScoreNum++;

                if (gameObject.name == "player1")
                {
                    playerScore.text = "P1:  " + playerScoreNum.ToString();
                }
                else if (gameObject.name == "player2")
                {
                    playerScore.text = "P2:  " + playerScoreNum.ToString();
                }
            }
        }

	}


	private IEnumerator ResetPenguin ()
	{

        bool tempContinue = false;

		gameObject.GetComponent<SpriteRenderer>().enabled = false;

		if (gameObject.name == "player1")
		{
			if (otherPlayer1.GetComponent<playerControl>().currentFloat != 34)
			{
				gameObject.transform.position = new Vector3(iceFloats[34].transform.position.x, iceFloats[34].transform.position.y, 0.0f);
				currentFloat = 34;
			}
			else
			{
				yield return new WaitForSeconds(1.0f);

                StartCoroutine(ResetPenguin());

                tempContinue = true;
			}
		}
		else if (gameObject.name == "player2")
		{
            if (otherPlayer1.GetComponent<playerControl>().currentFloat != 28)
            {
                gameObject.transform.position = new Vector3(iceFloats[28].transform.position.x, iceFloats[28].transform.position.y, 0.0f);
                currentFloat = 28;
            }
            else
            {
                yield return new WaitForSeconds(1.0f);

                StartCoroutine(ResetPenguin());

                tempContinue = true;
            }
		}

        if (tempContinue == false)
        {
            yield return new WaitForSeconds(1.0f);

            gameObject.GetComponent<SpriteRenderer>().enabled = true;

            joystickImage.color = originColor;

            canMove = true;
        }
		
	}


    public void Reset ()
    {
        StopAllCoroutines();

        playerScoreNum = 0;
        if (gameObject.name == "player1")
        {
            playerScore.text = "P1:  " + playerScoreNum.ToString();
        }
        else if (gameObject.name == "player2")
        {
            playerScore.text = "P2:  " + playerScoreNum.ToString();
        }

        hasFallen = false;
        canMove = false;
        isMoving = false;

        gameObject.GetComponent<SpriteRenderer>().enabled = true;

        deathTargetPos = Vector3.zero;

        resetJoystick = true;
        moveOffScreen = false;
        moveDirection = "";
        joystickImage.color = originColor;

        targetPos = new Vector3(-100, -100, -100);

        Start();
    }


    private IEnumerator DelayNextMove (float duration)
    {
        yield return new WaitForSeconds(duration);

        canMove = true;
        once = false;
    }


    void Update()
    {

        if (Camera.main.GetComponent<gameManager>().gameOver == false)
        {
            if (gameObject.name == "player1")
            {
                xPos = CrossPlatformInputManager.GetAxis("Horizontal_1");
                yPos = CrossPlatformInputManager.GetAxis("Vertical_1");
            }
            else if (gameObject.name == "player2")
            {
                xPos = CrossPlatformInputManager.GetAxis("Horizontal_2");
                yPos = CrossPlatformInputManager.GetAxis("Vertical_2");
            }

            if (Mathf.Abs(xPos) <= 0.15f && Mathf.Abs(yPos) <= 0.15f)
            {
                resetJoystick = true;
            }

            if (canMove == true && resetJoystick == true)
            {
                if (Mathf.Abs(xPos) > Mathf.Abs(yPos))
                {
                    if (xPos >= 0.85f)
                    {
                        moveDirection = "right";
                        MovePlayer(moveDirection);
                    }
                    else if (xPos <= -0.85f)
                    {
                        moveDirection = "left";
                        MovePlayer(moveDirection);
                    }
                }
                else if (Mathf.Abs(yPos) > Mathf.Abs(xPos))
                {
                    if (yPos >= 0.85f)
                    {
                        moveDirection = "up";
                        MovePlayer(moveDirection);
                    }
                    else if (yPos <= -0.85f)
                    {
                        moveDirection = "down";
                        MovePlayer(moveDirection);
                    }
                }
            }
            else
            {
                if (hasFallen == true)
                {
                    if (gameObject.transform.position != deathTargetPos)
                    {
                        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, deathTargetPos, playerSpeed);

                        if (Vector3.Distance(gameObject.transform.position, deathTargetPos) < 0.05f)
                        {
                            gameObject.transform.position = deathTargetPos;
                        }
                    }
                    else
                    {
                        StartCoroutine(ResetPenguin());
                        hasFallen = false;
                    }
                }
                else if (gameObject.GetComponent<Renderer>().enabled == true)
                {
                    if (moveOffScreen == false)
                    {
                        if (targetPos != new Vector3(-100, -100, -100))
                        {
                            if (gameObject.transform.position != targetPos)
                            {
                                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPos, playerSpeed);

                                if (Vector3.Distance(gameObject.transform.position, targetPos) < 0.05f)
                                {
                                    isMoving = false;
                                    gameObject.transform.position = targetPos;
                                }
                            }
                            else
                            {
                                if (once == false)
                                {
                                    StartCoroutine(DelayNextMove(delayPeriod));
                                    once = true;
                                }
                            }
                        }

                    }
                    else
                    {
                        if (moveDirection == "right")
                        {
                            if (gameObject.GetComponent<Renderer>().isVisible == true)
                                gameObject.transform.Translate(Vector3.right * playerSpeed);
                            else
                            {
                                gameObject.transform.position = new Vector3(-12.0f, transform.position.y, 0.0f);
                                moveOffScreen = false;
                            }
                        }
                        else if (moveDirection == "left")
                        {
                            if (gameObject.GetComponent<Renderer>().isVisible == true)
                                gameObject.transform.Translate(Vector3.left * playerSpeed);
                            else
                            {
                                gameObject.transform.position = new Vector3(12.0f, transform.position.y, 0.0f);
                                moveOffScreen = false;
                            }
                        }
                        else if (moveDirection == "up")
                        {
                            if (gameObject.GetComponent<Renderer>().isVisible == true)
                                gameObject.transform.Translate(Vector3.up * playerSpeed);
                            else
                            {
                                gameObject.transform.position = new Vector3(transform.position.x, -8.0f, 0.0f);
                                moveOffScreen = false;
                            }
                        }
                        else if (moveDirection == "down")
                        {
                            if (gameObject.GetComponent<Renderer>().isVisible == true)
                                gameObject.transform.Translate(Vector3.down * playerSpeed);
                            else
                            {
                                gameObject.transform.position = new Vector3(transform.position.x, 8.0f, 0.0f);
                                moveOffScreen = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
