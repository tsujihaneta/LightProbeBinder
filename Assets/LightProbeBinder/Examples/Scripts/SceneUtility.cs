using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneUtility
{
    public static void LoadSceneAsyncAdditive(string name)
    {
        if (!IsSceneLoaded(name))
        {
            SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        }
    }

    public static bool IsSceneLoaded(string name)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (string.Equals(scene.name, name))
            {
                return true;
            }
        }

        return false;
    }
}
