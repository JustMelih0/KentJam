
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerRef;
    public Statue statue;
    public string nextSceneName = "Test1";
    [HideInInspector] public Player playerMob;
    [HideInInspector] public PlayerStateMachine playerStateMachine;
    [HideInInspector] public PlayerInputController playerInputController;
    [HideInInspector] public PlayerInteractController playerInteractController;
    [HideInInspector] public PlayerHealth playerHealth;
    private bool restartingGame = false;
    public static GameManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (playerRef == null) return;

        playerRef.TryGetComponent(out playerMob);
        playerRef.TryGetComponent(out playerStateMachine);
        playerRef.TryGetComponent(out playerInputController);
        playerRef.TryGetComponent(out playerInteractController);
        playerRef.TryGetComponent(out playerHealth);
    }
    void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.deadEvent += PlayerDead;
    }
    void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.deadEvent -= PlayerDead;
    }
    void Start()
    {
        UIScreenFader.Instance.Open();
        if (playerRef == null) return;

        playerRef.TryGetComponent(out playerMob);
        playerRef.TryGetComponent(out playerStateMachine);
        playerRef.TryGetComponent(out playerInputController);
        playerRef.TryGetComponent(out playerInteractController);
        playerRef.TryGetComponent(out playerHealth);
    }
    public void RestartGame(InputAction.CallbackContext context)
    {
        if (context.started && restartingGame == false)
        {
            PlayerDead();
        }
    }
    public void MainMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            UIScreenFader.Instance.CloseAndLoadScene("Menu");
        }
    }
    public void PlayerDead()
    {
        if (restartingGame) return;

        AudioManager.Instance.PlaySFX("mouse-click");

        restartingGame = true;
        LockPlayerForRestart();
        Invoke(nameof(Restart), 0.2f);
    }
    public void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        UIScreenFader.Instance.CloseAndLoadScene(currentSceneName);
    }
    public void NextScene()
    {
        UIScreenFader.Instance.CloseAndLoadScene(nextSceneName);
    }




    private void LockPlayerForRestart()
    {
        playerInputController?.DisControls();

        if (playerRef == null) return;

        if (playerMob != null && playerMob.rgb2d != null)
        {
            playerMob.rgb2d.linearVelocity = Vector2.zero;
            playerMob.rgb2d.angularVelocity = 0f;
            playerMob.rgb2d.constraints |= RigidbodyConstraints2D.FreezePosition;
        }
    }
}
