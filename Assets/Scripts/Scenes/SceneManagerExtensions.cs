// --------------------------------------------------------- 
// SceneManagerExtensions.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerExtensions : SceneManager
{
    #region variable

    private readonly Dictionary<SceneType, AsyncOperation> _asyncSceneDictionary = new();

    #endregion
    #region method

    public IEnumerator LoadSceneAsync(SceneType sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        _asyncSceneDictionary[sceneName] = LoadSceneAsync(sceneName.ToString(), mode);
        yield return _asyncSceneDictionary[sceneName];
    }

    public IEnumerator UnloadSceneAsync(SceneType sceneName)
    {
        string scene = sceneName.ToString();
        yield return UnloadSceneAsync(scene);
    }

    #endregion
}
