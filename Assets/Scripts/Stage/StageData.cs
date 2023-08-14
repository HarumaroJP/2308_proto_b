// --------------------------------------------------------- 
// StageData.cs 
// 
// CreateDay: 2023/08/05
// Creator  : fuwa
// --------------------------------------------------------- 
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObjects/CreateStageData")]
public class StageData : ScriptableObject
{
    #region variable

    [SerializeField] private string _sceneName;
    [SerializeField] private int _stageCost;
    [SerializeField] private GameObject _stagePrefab;
    [SerializeField] private List<TimeRank> _stageRank;

    #endregion
    #region property

    public string SceneName => _sceneName;
    public GameObject StagePrefab => _stagePrefab;
    public int StageCost => _stageCost;
    public IReadOnlyList<TimeRank> StageRank => _stageRank;

    #endregion
    #region method

    #endregion
}
