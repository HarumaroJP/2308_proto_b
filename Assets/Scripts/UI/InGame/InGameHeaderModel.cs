// --------------------------------------------------------- 
// MissionModel.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 

using UniRx;

public class InGameHeaderModel
{
    #region variable

    private readonly ReactiveProperty<int> _consumedCost;
    private readonly ReactiveProperty<bool> _costOverState;
    public int TotalCost;

    #endregion

    #region property

    public IReadOnlyReactiveProperty<int> ConsumedCost => _consumedCost;
    public IReadOnlyReactiveProperty<bool> CostOverState => _costOverState;

    #endregion

    #region method

    public InGameHeaderModel(int cost)
    {
        TotalCost = cost;
        _consumedCost = new ReactiveProperty<int>(0);
        _costOverState = new ReactiveProperty<bool>(false);
    }

    public string GetStageName() => ProgressManager.Instance.CurrentStage.SceneName;

    public void Start()
    {
        _consumedCost.Value = 0;
    }

    public void AddCost(int cost)
    {
        _consumedCost.Value += cost;

        _costOverState.Value = _consumedCost.Value > TotalCost;
    }

    public void RemoveCost(int cost)
    {
        if (_consumedCost.Value <= 0)
        {
            _consumedCost.Value = 0;
            return;
        }

        _consumedCost.Value -= cost;
        _costOverState.Value = _consumedCost.Value > TotalCost;
    }

    public void Reset()
    {
        TotalCost = ProgressManager.Instance.CurrentStage.StageCost;
        _consumedCost.Value = 0;
        _costOverState.Value = false;
    }

    #endregion
}
