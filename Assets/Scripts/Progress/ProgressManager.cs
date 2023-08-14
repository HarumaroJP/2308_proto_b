// --------------------------------------------------------- 
// ProgressManager.cs 
// 
// CreateDay: 2023/08/05
// Creator  : fuwa
// ---------------------------------------------------------
using System;
using UnityEngine;
using System.Collections.Generic;
using UniRx;
public class ProgressManager : Singleton<ProgressManager>
{
    #region variable

    [SerializeField] private StageData[] _stageData;
    private Dictionary<StageData, Progress> _progressData;

    #endregion
    #region property

    public IReactiveProperty<StageData> OnChangedCurrentStage { get; } = new ReactiveProperty<StageData>();
    public int CurrentStageIndex { get; private set; }
    public StageData CurrentStage => _stageData[CurrentStageIndex];
    public IReadOnlyList<StageData> StageData => _stageData;
    public IReadOnlyDictionary<StageData, Progress> ProgressData => _progressData;
    private GameObject _currentStageObject;

    #endregion
    #region method

    public void SetCurrentStageIndex(int index)
    {
#if UNITY_EDITOR
        Debug.Log($"CurrentStageIndex: {index}");
#endif
        CurrentStageIndex = index;
        OnChangedCurrentStage.Value = CurrentStage;
    }
    public void SetCurrentStageObject(GameObject stageObject)
    {
        _currentStageObject = stageObject;
    }
    public void Unload()
    {
        Destroy(_currentStageObject);
        _currentStageObject = null;
    }

    #endregion
}
