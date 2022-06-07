using UnityEngine;

//Variáveis foram mantidas públicas para debug

//Mantive o gerenciamento de Input e gerenciamento do jogador em um mesmo script para
//evitar adicionar mais referências e dependencias externas e para impedir multiplos
//Updates rodando ao mesmo tempo

public class PlayerManager : MonoBehaviour
{
    public bool gameover;
    public bool pause;

    public enum Platform { mobile, console};
    public Platform selectedPlatform;

    [Header("Mobile Input Manager")]
    public float playerDirection;
    Camera cam;
    Vector2 currentMousePos;
    Vector2 playerPress;
    Vector2 playerSecPress;

    [Header("Console Input Manager")]
    public float paddleSpeed;
    float playerCurrentYPos;

    [SerializeField] GameManager gameManager;

    [Header("Tutorial Manager")]
    [SerializeField] GameObject tutorialConsole;

    void Start()
    {
        cam = Camera.main;
        gameManager = FindObjectOfType<GameManager>();
        selectedPlatform = (Platform)gameManager.selectedPlatform;
        tutorialConsole = GameObject.FindGameObjectWithTag("Tutorial");
    }

    void Update()
    {
        if (!gameover && !pause)
        {
            if (gameManager.selectedPlatform == GameManager.Platform.mobile)
            {
                MobileInput();
            }

            if (gameManager.selectedPlatform == GameManager.Platform.console)
            {
                ConsoleInput();
            }
        }

        if (gameManager.selectedPlatform == GameManager.Platform.console)
        {
            ConsoleInputExtras();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (tutorialConsole != null)
            {
                if (tutorialConsole.activeSelf == true)
                {
                    tutorialConsole.SetActive(false);
                    gameManager.RestartGame();
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!gameover && !pause)
        {
            playerPress = cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void MobileInput()
    {
        if (Input.GetMouseButton(0))
        {
            currentMousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            if (currentMousePos.x < 0 && (currentMousePos.y > -6.5f && currentMousePos.y < 6.5f))
            {
                transform.position = new Vector3(transform.position.x, currentMousePos.y, transform.position.z);
            }

            playerSecPress = cam.ScreenToWorldPoint(Input.mousePosition);
            playerDirection = playerSecPress.y - playerPress.y;
        }
    }

    void ConsoleInput()
    {
        if (Input.GetKey(KeyCode.UpArrow) ||
                    Input.GetKey(KeyCode.W))
        {
            if (transform.position.y < 6.5f)
            {
                playerCurrentYPos = transform.position.y;
                playerCurrentYPos += paddleSpeed * Time.deltaTime;
                transform.position = new Vector3(transform.position.x, playerCurrentYPos, transform.position.z);
            }

            playerDirection = 1;
        }

        if (Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.S))
        {
            if (transform.position.y > -6.5f)
            {
                playerCurrentYPos = transform.position.y;
                playerCurrentYPos -= paddleSpeed * Time.deltaTime;
                transform.position = new Vector3(transform.position.x, playerCurrentYPos, transform.position.z);
            }

            playerDirection = -1;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) ||
            Input.GetKeyUp(KeyCode.DownArrow) ||
            Input.GetKeyUp(KeyCode.W) ||
            Input.GetKeyUp(KeyCode.S))
        {
            playerDirection = 0;
        }
    }

    public void ConsoleInputExtras()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (pause)
            {
                gameManager.QuitGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (gameover)
            {
                gameManager.RestartGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.PauseGame();
        }
    }
}