using UnityEngine.InputSystem;

public class Game 
{
    public GameStateMachine StateMachine;

    public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain, InputActionAsset playerInput)
    {
        StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner),loadingCurtain, AllServices.Container,playerInput);
    }
}
