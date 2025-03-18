using UnityEngine.InputSystem;

public class BootstrapState : IState
{
    private const string Main = "Main";
    private const string Initial = "Initial";

    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly AllServices _services;
    private readonly InputActionAsset _inputActions;

    public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
        AllServices services, InputActionAsset inputActions)
    {
        _stateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _services = services;
        _inputActions = inputActions;

        RegisterServices();
    }

    public void Enter()
    {
        _sceneLoader.Load(Initial, EnterLoadLevel);
    }

    public void Exit() { }

    private void EnterLoadLevel()
    {
        _stateMachine.Enter<LoadProgressState>();
    }

    private void RegisterServices()
    {
        _services.RegisterSingle<ISaveLoadBuildingService>(new SaveLoadBuilding());
        _services.RegisterSingle<IPlayerInputService>(new PlayerInput(_inputActions));
    }
}