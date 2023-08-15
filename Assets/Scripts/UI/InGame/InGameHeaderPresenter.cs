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

    private CostModel _model;
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
        _model = new CostModel(stageData.StageCost);

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
                    _model.AddCost(element.PartInfo.Cost);
                },
                ex => Debug.LogError("Error: " + ex.Message)
            )
            .AddTo(this);

        partBuilder.OnPartRemoved.Subscribe(ev =>
                {
                    PartElement element = ev.Value;
                    _model.RemoveCost(element.PartInfo.Cost);
                },
                ex => Debug.LogError("Error: " + ex.Message)
            )
            .AddTo(this);

        _model.PlayableState.Subscribe(value =>
                {
                    _startView.SetEnableButton(value);
                },
                ex => Debug.LogError("Error: " + ex.Message))
            .AddTo(this);

        _model.CostOverState.Subscribe(value =>
            {
                _startView.SetCostOverButton(value);
            })
            .AddTo(this);

        _model.ConsumedCost.Subscribe(
                value =>
                {
                    _costView.UpdateCost(value, _model.StageCost);
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
                        _costView.Show();
                        _missionView.Show(ProgressManager.Instance.CurrentStage.SceneName);
                        _costView.UpdateCost(_model.ConsumedCost.Value, _model.StageCost);
                    }
                    else if (x == SceneType.ShowTarget)
                    {
                        _model.Reset();
                    }
                    else
                    {
                        _costView.Hide();
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