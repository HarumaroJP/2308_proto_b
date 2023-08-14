// --------------------------------------------------------- 
// StateMachine.cs 
// 
// CreateDay: 2023/08/05
// Creator  : fuwa
// ---------------------------------------------------------

using System;
using UniRx;
using UnityEngine;

public class StateMachine : Singleton<StateMachine>
{
    #region variable

    private readonly IReactiveProperty<SceneType> _currentSceneType = new ReactiveProperty<SceneType>();

    #endregion
    #region property

    public IReadOnlyReactiveProperty<SceneType> CurrentSceneType => _currentSceneType;

    #endregion
    private void Start()
    {
        SetCurrentSceneType(SceneType.Title);
    }
    public void SetCurrentSceneType(SceneType sceneType)
    {
#if UNITY_EDITOR
        Debug.Log("current scene type: " + sceneType);
#endif
        _currentSceneType.Value = sceneType;
    }
}
