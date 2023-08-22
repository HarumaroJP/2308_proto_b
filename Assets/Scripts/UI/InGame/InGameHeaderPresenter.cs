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
    private ResultModel _resultModel = new();
    [SerializeField] private MissionView _missionView;
    [SerializeField] private CostView _costView;
    [SerializeField] private StartView _startView;
    [SerializeField] private RetryButton _retryButton;
    [SerializeField] private PartBuilder partBuilder;

    #endregion

    #region method

    private void Start()
    {
        StageData stageData = ProgressManager.Instance.CurrentStage;
        _costModel = new CostModel(stageData.StageCost);

        partBuilder.OnStart.Subscribe(_ =>
        {
            _costView.Hide();
        });

        partBuilder.OnRetry.Subscribe(_ =>
        {
            _costView.Show();
        });

        partBuilder.OnPartAdded.Subscribe(ev =>
                {
                    PartElement element = ev.Value;
                    _costModel.AddCost(element.PartInfo.Cost);
                },
                ex => Debug.LogError("Error: " + ex.Message)
            )
            .AddTo(this);

        partBuilder.OnPartRemoved.Subscribe(ev =>
                {
                    PartElement element = ev.Value;
                    _costModel.RemoveCost(element.PartInfo.Cost);
                },
                ex => Debug.LogError("Error: " + ex.Message)
            )
            .AddTo(this);

        _costModel.PlayableState.Subscribe(value =>
                {
                    _startView.SetEnableButton(value);
                },
                ex => Debug.LogError("Error: " + ex.Message))
            .AddTo(this);

        _costModel.CostOverState.Subscribe(value =>
            {
                _startView.SetCostOverButton(value);
            })
            .AddTo(this);

        _costModel.ConsumedCost.Subscribe(
                value =>
                {
                    _costView.UpdateCost(value, _costModel.StageCost);
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
                            _missionView.Show(ProgressManager.Instance.CurrentStage.SceneName);
                            _costView.UpdateCost(_costModel.ConsumedCost.Value, _costModel.StageCost);
                            break;
                        case SceneType.ShowTarget:
                            _costModel.Reset();
                            break;
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