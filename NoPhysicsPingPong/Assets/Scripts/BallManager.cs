using UnityEngine;

public class BallManager : MonoBehaviour
{
    [Header("Movement Controllers")]
    public float speed;
    public float originalSpeed;
    public float speedIncrement;
    public Vector2 direction;

    [Header("Game Limits")]
    public float vertLimit; // 7.5f
    public float horLimit; // 9.6f
    public float limitHitPlayerHor; //-7.6f //Limite usado para calcular quando a bolinha "bateu" na frente do paddle do jogador
    public float limitHitPlayerVert; //-8.15f //Limite usado para calcular quando a bolinha "bateu" em cima ou embaixo do paddle do jogador
    public float paddleVerticalOffset; //1.25f

    [Header("State Manager")]
    public bool pause;
    public bool canMove;
    public bool gameOver;

    [Header("Player Paddle Manager")]
    [SerializeField] GameObject playerPaddle;
    [SerializeField] PlayerManager playerManager;

    [Header("AI Paddle Manager")]
    [SerializeField] GameObject aIPaddle;
    [SerializeField] AIPaddleManager aIPaddleManager;

    [Header("Game Manager")]
    [SerializeField] GameManager gameManager;

    [Header("Audio Manager")]
    [SerializeField] AudioManager audioManager;

    [Header("Plus Manager")]
    [SerializeField] PlusManager plusManager;

    [Header("Init and Reset Manager")]
    public int initialDirection;
    public float resetPosition;

    void Awake()
    {
        //Aleatóriamente calcula se a bolinha vai para cima ou para baixo
        //quando a partida começa [apenas na primeira vez]
        initialDirection = Random.Range(0, 1);
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        playerPaddle = GameObject.FindGameObjectWithTag("Player");
        playerManager = playerPaddle.GetComponent<PlayerManager>();

        aIPaddle = GameObject.FindGameObjectWithTag("Enemy");
        aIPaddleManager = aIPaddle.GetComponent<AIPaddleManager>();

        audioManager = FindObjectOfType<AudioManager>();
        plusManager = FindObjectOfType<PlusManager>();

        aIPaddleManager.aISpeed = speed / 2;

        direction = -Vector2.one.normalized;
        direction.y = -direction.y;

        originalSpeed = speed;
    }

    void Update()
    {
        if (!pause && canMove && !gameOver)
        {
            transform.Translate(direction * speed * Time.deltaTime);

            BallBounce();
            CheckPlayerPaddlePos();
        }
    }

    public void BallBounce()
    {
        if (transform.localPosition.y > vertLimit && direction.y > 0 ||
            transform.localPosition.y < (vertLimit *-1) && direction.y < 0)
        {
            audioManager.BallHittedLimits();
            direction.y = -direction.y;
        }

        if (transform.localPosition.x < (horLimit * -1) && direction.x < 0)
        {
            audioManager.BallDestroyed();

            canMove = false;
            direction.x = -direction.x;
            transform.localPosition = new Vector3((resetPosition * -1), 0, 0);

            speed = speed / 2; //Corta pela metade a velocidade da bolinha quando o jogador perde
            aIPaddleManager.ResetPaddle();

            gameManager.AddEnemyScore();
            gameManager.CheckIfGameIsOver();

            //PlusManager
            if (plusManager != null)
            {
                plusManager.transform.rotation = Quaternion.Euler(0, 0, 180);
                PlusManagerParameters();
            }
            //PlusManager
        }


        //Esse if controla se a bolinha passou pelo paddle da AI
        if (transform.localPosition.x > horLimit && direction.x > 0)
        {
            audioManager.BallDestroyed();

            canMove = false;
            direction.x = -direction.x;
            transform.localPosition = new Vector3(resetPosition, 0, 0);

            aIPaddleManager.aISpeed += aIPaddleManager.aISpeed / 2; //Corta a velocidade da AI para tornar a partida mais justa
            aIPaddleManager.ResetPaddle();

            gameManager.AddPlayerScore();
            gameManager.CheckIfGameIsOver();

            //PlusManager
            if (plusManager != null)
            {
                plusManager.transform.rotation = Quaternion.Euler(0, 0, 0);
                PlusManagerParameters();
            }
            //PlusManager
        }
    }

    public void CheckPlayerPaddlePos()
    {
        if (direction.x < 0)
        {
            //Verifica se a posição horizontal da bolinha está a frente do paddle do jogador
            //Ou se está ligeiramente passando pelo jogador mas é possível rebater pelo topo/base
            if (transform.position.x < limitHitPlayerHor && transform.position.x > limitHitPlayerVert)
            {
                //Verifica se a bolinha está verticalmente dentro da área do paddle do jogador
                if (transform.position.y > (playerPaddle.transform.position.y - paddleVerticalOffset) &&
                    transform.position.y < (playerPaddle.transform.position.y + paddleVerticalOffset))
                {
                    speed += speedIncrement;

                    direction.x = -direction.x;
                    audioManager.BallHittedPaddle();
                    if (plusManager != null)
                    {
                        plusManager.PhoneShake();
                    }

                    //Esses dois IFs controlam a direção que a bolinha deve tomar verticalmente
                    //baseado na trajetória do paddle do jogador ao "colidir" com a bolinha
                    if (direction.y > 0)
                    {
                        if (playerManager.playerDirection < 0)
                        {
                            direction.y = -direction.y;
                        }
                    }

                    if (direction.y < 0)
                    {
                        if (playerManager.playerDirection > 0)
                        {
                            direction.y = -direction.y;
                        }
                    }
                }
            }
        }

        if (direction.x > 0)
        {
            //Verifica se a posição horizontal da bolinha está a frente do paddle da AI
            //Ou se está ligeiramente passando pela AI mas é possível rebater pelo topo/base
            //Neste caso está multiplicando por -1 para reutilizar os valores do jogador, mas positivo
            if (transform.position.x > (limitHitPlayerHor * -1) &&
                transform.position.x < (limitHitPlayerVert * -1))
            {
                if (transform.position.y > (aIPaddle.transform.position.y - paddleVerticalOffset) &&
                    transform.position.y < (aIPaddle.transform.position.y + paddleVerticalOffset))
                {
                    speed += speedIncrement;
                    audioManager.BallHittedPaddle();
                    if (plusManager != null)
                    {
                        plusManager.PhoneShake();
                    }
                    direction.x = -direction.x;
                }
            }
        }
    }

    public void PlusManagerParameters()
    {
        plusManager.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        plusManager.PlayerExplosion();
        plusManager.PlayScreenshake();
        plusManager.PhoneShake();
    }
}