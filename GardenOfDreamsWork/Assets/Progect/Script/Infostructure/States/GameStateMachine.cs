using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class GameStateMachine
{
    private Dictionary<Type, IExcitableState> _states;
    private IExcitableState _activeState;

    public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices allServices, InputActionAsset playerInput)
    {
        _states = new Dictionary<Type, IExcitableState>
        {
            [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, allServices,playerInput),
            [typeof(LoadProgressState)] = new LoadProgressState(this,allServices.Single<ISaveLoadBuildingService>()),
            [typeof(LoadLevelState)] = new LoadLevelState(this,sceneLoader,loadingCurtain,
                allServices.Single<IPlayerInputService>(), allServices.Single<ISaveLoadBuildingService>())
        };
    }

    public void Enter<TState>() where TState : class, IState
    {
        TState state = ChangeState<TState>();
        state.Enter();
    }

    public void Enter<TState,TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
    {
        TState state = ChangeState<TState>();
        state.Enter(payload);   
    }

    private TState ChangeState<TState>() where TState : class, IExcitableState
    {
        _activeState?.Exit();

        TState state = GetState<TState>();
        _activeState = state;

        return state;
    }

    private TState GetState<TState>() where TState : class, IExcitableState =>
        _states[typeof(TState)] as TState;
}
