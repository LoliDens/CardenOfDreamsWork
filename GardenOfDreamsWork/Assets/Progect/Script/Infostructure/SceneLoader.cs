﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private ICoroutineRunner _coroutineRunner;

    public SceneLoader(ICoroutineRunner coroutineRunner) =>
        _coroutineRunner = coroutineRunner;

    public void Load(string name, Action onLoaded = null) =>
        _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

    public IEnumerator LoadScene(string name, Action onLoaded = null)
    {
        if (SceneManager.GetActiveScene().name == name)
        {
            onLoaded?.Invoke();
            yield break;
        }

        AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(name);
        //waitNextScene.completed += _ => onLoaded?.Invoke();

        while (!waitNextScene.isDone)
            yield return null;

        yield return new WaitForSeconds(0.5f);

        onLoaded?.Invoke();
    }
}
