using System;
using UnityEngine;

public interface IPlayerInputService : IService
{
    bool MouseLeftClick { get; }
    Vector2 MouseMove { get; }

    void RegisterActionMouseLeftClick(Action action);
    void RegisterActionMouseMove(Action<Vector2> action);
    void UnregisterActionMouseLeftClick(Action action);
    void UnregisterActionMouseMove(Action<Vector2> action);
}