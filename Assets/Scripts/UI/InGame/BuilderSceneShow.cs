// --------------------------------------------------------- 
// BuilderSceneShow.cs 
// 
// CreateDay: 2023/08/05
// Creator  : fuwa
// --------------------------------------------------------- 
using UniRx;
using UnityEngine;
public class BuilderSceneShow : MonoBehaviour
{
    #region variable

    [SerializeField] private GameObject _builder;

    #endregion

    #region method

    private void Awake()
    {
        StateMachine.Instance.CurrentSceneType
            .Where(x => x == SceneType.Builder)
            .Subscribe(_ => ChangeScene())
            .AddTo(this);
    }
    private void ChangeScene()
    {
        _builder.SetActive(true);
    }

    #endregion
}
