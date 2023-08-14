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

    private InGameHeaderModel _model;
    [SerializeField] private MissionView _missionView;
    [SerializeField] private CostView _costView;
    [SerializeField] private StartView _startView;
    [SerializeField] private PartBuilder partBuilder;

    #endregion

    #region method

    private void Start()
    {
        StageData stageData = ProgressManager.Instance.CurrentStage;
        _model = new InGameHeaderModel(stageData.StageCost);

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
            _model.AddCost(element.PartInfo.Cost);
        };

        partBuilder.OnPartRemoved += element =>
        {
            element.OnPartPutOut -= RemoveCostBridge;
        };

        _model.CostOverState.Subscribe(value =>
                {
                    _startView.SetCostOverButton(value);
                },
                ex => Debug.LogError("Error: " + ex.Message))
            .AddTo(this);

        _model.ConsumedCost.Subscribe(
                value =>
                {
                    _costView.UpdateCost(value, _model.TotalCost);
                },
                ex => Debug.LogError("Error: " + ex.Message)
                )
            .AddTo(this);

        StateMachine.Instance.CurrentSceneType
            .Where(x => x == SceneType.ShowTarget)
            .Subscribe(
                _ =>
                {
                    _missionView.Hide();
                    _costView.Hide();
                },
                ex => Debug.LogError("Error: " + ex.Message)
                )
            .AddTo(this);

        StateMachine.Instance.CurrentSceneType
            .Subscribe(
                x =>
                {
                    if (x == SceneType.Builder)
                    {
                        _costView.Show();
                        _missionView.Show(_model.GetStageName());
                        _costView.UpdateCost(_model.ConsumedCost.Value, _model.TotalCost);
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

        _model.Start();
    }

    void RemoveCostBridge(PartElement element)
    {
        _model.RemoveCost(element.PartInfo.Cost);
    }

    #endregion
}
