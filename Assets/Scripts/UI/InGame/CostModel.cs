// --------------------------------------------------------- 
// MissionModel.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 

using System;
using UniRx;

public class CostModel
{
    #region variable

    public readonly int StageCost;
    private readonly ReactiveProperty<int> consumedCost;
    private readonly ReactiveProperty<bool> playableState;

    #endregion

    #region property

    public IReadOnlyReactiveProperty<int> ConsumedCost => consumedCost;
    public IReadOnlyReactiveProperty<bool> PlayableState => playableState;
    public IObservable<bool> CostOverState => consumedCost.Select(IsCostOver);

    #endregion

    #region method

    public CostModel(int cost)
    {
        StageCost = cost;
        consumedCost = new ReactiveProperty<int>(0);
        playableState = new ReactiveProperty<bool>(false);
    }

    public void Start()
    {
        consumedCost.Value = 0;
    }

    public void AddCost(int cost)
    {
        consumedCost.Value += cost;

        //コストが1以上 and ステージコスト内であればプレイ可能
        playableState.Value = IsPlayable(consumedCost.Value);
    }

    public void RemoveCost(int cost)
    {
        if (consumedCost.Value <= 0)
        {
            consumedCost.Value = 0;
        }
        else
        {
            consumedCost.Value -= cost;
        }

        playableState.Value = IsPlayable(consumedCost.Value);
    }

    bool IsPlayable(int cost)
    {
        return cost > 0 && !IsCostOver(cost);
    }

    bool IsCostOver(int cost)
    {
        return cost > StageCost;
    }


    public void Reset()
    {
        consumedCost.Value = 0;
        playableState.Value = false;
    }

    #endregion
}
