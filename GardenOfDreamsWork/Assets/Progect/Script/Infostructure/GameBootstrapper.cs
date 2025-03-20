using UnityEngine;
using UnityEngine.InputSystem;

public class GameBootstrapper : MonoBehaviour,ICoroutineRunner
{
    [SerializeField] private LoadingCurtain _loadingCurtain;
    [SerializeField] private InputActionAsset _playerInput;

    private Game _game;

    private void Awake()
    {
        _game = new Game(this,_loadingCurtain,_playerInput);
        _game.StateMachine.Enter<BootstrapState>();

        DontDestroyOnLoad(this);
    }

    public static void Print(object o) =>
        Debug.Log(o);
}
