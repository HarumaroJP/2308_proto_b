// --------------------------------------------------------- 
// BuilderSceneShow.cs 
// 
// CreateDay: 2023/08/05
// Creator  : fuwa
// --------------------------------------------------------- 

using Builder.System;
using UniRx;
using UnityEngine;

public class BuilderSceneShow : MonoBehaviour
{
    #region variable

    [SerializeField] private PartBuilder _builder;

    #endregion

    #region method

    private void Awake()
    {
        _builder.Initialize();

        StateMachine.Instance.CurrentSceneType
            .Where(x => x == SceneType.Builder)
            .Subscribe(_ => ChangeScene())
            .AddTo(this);
    }

    private void ChangeScene()
    {
        _builder.gameObject.SetActive(true);
    }

    #endregion
}