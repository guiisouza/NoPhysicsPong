using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public enum Platform { mobile, console };
    public Platform selectedPlatform;

    public Button startGameButton;
    SpriteState startSpriteState;
    public Sprite[] startSprites;

    void Awake()
    {
        #if UNITY_EDITOR
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
        if (selectedPlatform == Platform.console)
        {
            startSpriteState = startGameButton.spriteState;
            startGameButton.image.sprite = startSprites[2];
            startSpriteState.pressedSprite = startSprites[3];
        }

        if (selectedPlatform == Platform.mobile)
        {
            startSpriteState = startGameButton.spriteState;
            startGameButton.image.sprite = startSprites[0];
            startSpriteState.pressedSprite = startSprites[1];
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadGame();
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Portfolio()
    {
        Application.OpenURL("https://linkedoranean.itch.io/");
    }
}