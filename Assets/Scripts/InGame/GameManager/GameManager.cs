
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
        playerHealth.deadEvent += PlayerDead;
    }
    void OnDisable()
    {
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
    }
    public void RestartGame(InputAction.CallbackContext context)
    {
        if (context.started && restartingGame == false)
        {
            Restart();
        }
    }
    public void PlayerDead()
    {
        if (restartingGame) return;

        restartingGame = true;
        Invoke(nameof(Restart), 1f);
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




}
