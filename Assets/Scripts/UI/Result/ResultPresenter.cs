// --------------------------------------------------------- 
// ResultPresenter.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 

using UniRx;
using UnityEngine;
public class ResultPresenter : MonoBehaviour
{
    #region variable

    private ResultActivator _partBuilder;
    [SerializeField] private ResultView _view;
    private readonly ResultModel _model = new();

    #endregion
    #region method

    private void Awake()
    {
        _view.OnClickToStageSelectAsObservable()
            .Subscribe(
                _ =>
                {
                    _view.Destroy();
                    _model.MoveToStageSelect();
                },
                ex => Debug.LogError("Error: " + ex.Message)
                ).AddTo(this);

        _view.OnClickToRetryAsObservable()
            .Subscribe(
                _ =>
                {
                    _view.Destroy();
                    _partBuilder.Retry();
                    _model.MoveToRetry();
                },
                ex => Debug.LogError("Error: " + ex.Message)
                ).AddTo(this);
        _view.OnClickToNextStageAsObservable()
            .Subscribe(
                _ =>
                {
                    _view.Destroy();
                    _model.MoveToNextStage();
                },
                ex => Debug.LogError("Error: " + ex.Message)
                ).AddTo(this);
    }
    private void Start()
    {
        _partBuilder = FindObjectOfType<ResultActivator>();
    }

    #endregion
}
