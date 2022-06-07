using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum Platform { mobile, console };
    public Platform selectedPlatform;

    [Header("Pause Manager")]
    public bool pause;
    public GameObject pauseButton;

    [Header("Quit Manager")]
    public GameObject quitButton;

    [Header("Countdown Manager")]
    public TextMeshProUGUI countdownText;
    public GameObject countdownBG;

    [Header("Game Over Manager")]
    public TextMeshProUGUI whoWonText;
    public GameObject whoWonBG;
    public GameObject retryButton;

    [Header("Score Manager")]
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI enemyScoreText;
    public int playerScore;
    public int enemyScore;

    [Header("Ball Manager")]
    public BallManager ballManager;

    [Header("Player Manager")]
    public PlayerManager playerManager;

    [Header("AI Manager")]
    public AIPaddleManager aIManager;

    [Header("Button Graphics Manager")]
    public Button pauseButtonImage;
    SpriteState pauseSpriteState;
    public Sprite[] pauseSprites;

    public Button quitButtonImage;
    SpriteState quitSpriteState;
    public Sprite[] quitSprites;

    public Button retyButtonImage;
    SpriteState retrySpriteState;
    public Sprite[] retrySprites;

    [Header("Audio Manager")]
    public AudioManager audioManager;

    void Awake()
    {
        #if UNITY_EDITOR
            selectedPlatform = Platform.console;
        #endif

        #if UNITY_STANDALONE
            selectedPlatform = Platform.console;
        #endif

        #if UNITY_IOS
            selectedPlatform = Platform.mobile;
        #endif

        #if UNITY_ANDROID
            selectedPlatform = Platform.mobile;
        #endif
    }

    void Start()
    {
        ballManager = FindObjectOfType<BallManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        aIManager = FindObjectOfType<AIPaddleManager>();
        audioManager = FindObjectOfType<AudioManager>();


        //Gerenciador de UI baseado na plataforma
        if (selectedPlatform == Platform.console)
        {
            pauseSpriteState = pauseButtonImage.spriteState;
            pauseButtonImage.image.sprite = pauseSprites[2];
            pauseSpriteState.pressedSprite = pauseSprites[3];

            quitSpriteState = quitButtonImage.spriteState;
            quitButtonImage.image.sprite = quitSprites[2];
            quitSpriteState.pressedSprite = quitSprites[3];

            retrySpriteState = retyButtonImage.spriteState;
            retyButtonImage.image.sprite = retrySprites[2];
            retrySpriteState.pressedSprite = retrySprites[3];
        }

        if (selectedPlatform == Platform.mobile)
        {
            pauseSpriteState = pauseButtonImage.spriteState;
            pauseButtonImage.image.sprite = pauseSprites[0];
            pauseSpriteState.pressedSprite = pauseSprites[1];

            quitSpriteState = quitButtonImage.spriteState;
            quitButtonImage.image.sprite = quitSprites[0];
            quitSpriteState.pressedSprite = quitSprites[1];

            retrySpriteState = retyButtonImage.spriteState;
            retyButtonImage.image.sprite = retrySprites[0];
            retrySpriteState.pressedSprite = retrySprites[1];
        }
    }

    public IEnumerator StartCountdown()
    {
        countdownBG.SetActive(true);
        WaitForSeconds time = new WaitForSeconds(1f);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            audioManager.PlayCountdownStart();
            yield return time;
        }

        countdownText.text = "Go!";
        audioManager.PlayCountdownGo();
        yield return time;

        countdownText.text = "";
        countdownBG.SetActive(false);

        playerManager.pause = false;
        ballManager.pause = false;
        aIManager.pause = false;

        ballManager.canMove = true;
    }

    public void AddPlayerScore()
    {
        playerScore++;
        playerScoreText.text = playerScore.ToString();
        CheckIfGameIsOver();
    }

    public void AddEnemyScore()
    {
        enemyScore++;
        enemyScoreText.text = enemyScore.ToString();
        CheckIfGameIsOver();
    }

    public void CheckIfGameIsOver()
    {
        if (enemyScore > 2 || playerScore > 2)
        {
            playerManager.gameover = true;
            ballManager.gameOver = true;
            aIManager.gameOver = true;

            countdownText.text = "game over";
            countdownText.fontSize = 40;
            retryButton.SetActive(true);
            pauseButton.SetActive(false);

            if (playerScore > 2)
            {
                whoWonText.text = "Player victory";
                whoWonBG.SetActive(true);
            }
            else
            {
                whoWonText.text = "AI victory";
                whoWonBG.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(StartCountdown());
        }
    }

    public void RestartGame()
    {
        playerManager.gameover = false;
        ballManager.gameOver = false;
        aIManager.gameOver = false;

        enemyScore = 0;
        playerScore = 0;

        playerScoreText.text = "0";
        enemyScoreText.text = "0";
        countdownText.text = "";
        countdownText.fontSize = 80;

        whoWonText.text = "";
        whoWonBG.SetActive(false);

        retryButton.SetActive(false);
        pauseButton.SetActive(true);

        ballManager.speed = ballManager.originalSpeed;

        StartCoroutine(StartCountdown());
    }

    public void PauseGame()
    {
        pause = !pause;

        if (!pause)
        {
            quitButton.SetActive(false);
            pauseButton.SetActive(true);
            StartCoroutine(StartCountdown());
        }

        if (pause)
        {
            StopAllCoroutines();

            ballManager.canMove = false;

            playerManager.pause = true;
            ballManager.pause = true;
            aIManager.pause = true;

            countdownText.text = "paused";
            countdownBG.SetActive(true);

            quitButton.SetActive(true);

            if (selectedPlatform == Platform.console)
            {
                pauseButton.SetActive(false);
            }
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
}