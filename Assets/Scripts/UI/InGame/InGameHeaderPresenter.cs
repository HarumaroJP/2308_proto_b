// --------------------------------------------------------- 
// InGameHeaderPresenter.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 

using Builder.System;
using Part;
using UniRx;
using UnityEngine;

public class InGameHeaderPresenter : MonoBehaviour
{
    #region variable

    private CostModel _costModel;
    private ResultModel _resultModel;
    [SerializeField] private MissionView _missionView;
    [SerializeField] private CostView _costView;
    [SerializeField] private StartView _startView;
    [SerializeField]private RetryButton _retryButton;
    [SerializeField] private PartBuilder partBuilder;

    #endregion

    #region method

    private void Start()
    {
        StageData stageData = ProgressManager.Instance.CurrentStage;
        _costModel = new CostModel(stageData.StageCost);

        partBuilder.OnStart += () =>
        {
            _costView.Hide();
        };

        partBuilder.OnRetry += () =>
        {
            _costView.Show();
        };

        partBuilder.OnPartAdded += element =>
        {
            element.OnPartPutOut += RemoveCostBridge;
            _costModel.AddCost(element.PartInfo.Cost);
        };

        partBuilder.OnPartRemoved += element =>
        {
            element.OnPartPutOut -= RemoveCostBridge;
        };

        _costModel.CostOverState.Subscribe(value =>
                {
                    _startView.SetCostOverButton(value);
                },
                ex => Debug.LogError("Error: " + ex.Message))
            .AddTo(this);

        _costModel.ConsumedCost.Subscribe(
                value =>
                {
                    _costView.UpdateCost(value, _costModel.TotalCost);
                },
                ex => Debug.LogError("Error: " + ex.Message)
                )
            .AddTo(this);

        _retryButton.OnClickRetryAsObservable()
            .Subscribe(
                _ =>
                {
                    AudioManager.Instance.PlaySe(AudioClipName.ButtonClick);
                    partBuilder.Retry();
                    _resultModel.MoveToRetry();
                },
                ex => Debug.LogError("Error: " + ex.Message)
                )
            .AddTo(this);

        StateMachine.Instance.CurrentSceneType
            .Subscribe(
                sceneType =>
                {
                    switch (sceneType)
                    {
                        case SceneType.Builder:
                            _costView.Show();
                            _missionView.Show(_costModel.GetStageName());
                            _costView.UpdateCost(_costModel.ConsumedCost.Value, _costModel.TotalCost);
                            break;
                        case SceneType.ShowTarget:
                            _costModel.Reset();
                            _missionView.Hide();
                            _costView.Hide();
                            break;
                        case SceneType.Title:
                        case SceneType.StageSelect:
                        case SceneType.InGame:
                        default:
                            _costView.Hide();
                            break;
                    }
                },
                ex => Debug.LogError("Error: " + ex.Message)
                )
            .AddTo(this);

        _costModel.Start();
    }

    void RemoveCostBridge(PartElement element)
    {
        _costModel.RemoveCost(element.PartInfo.Cost);
    }

    #endregion
}
