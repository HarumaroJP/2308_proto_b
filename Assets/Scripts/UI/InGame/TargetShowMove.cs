// --------------------------------------------------------- 
// TargetShowMove.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using DG.Tweening;
using UniRx;
public class TargetShowMove : MonoBehaviour
{
    #region variable

    [SerializeField] private GameObject _stage;
    [SerializeField] private MissionShow _missionShow;

    private Vector3 _firstPosition;
    private Vector3 _targetPosition = new(-7, 0, 0);

    #endregion
    #region property

    #endregion
    #region method

    private void Awake()
    {
        _firstPosition = _stage.transform.position;
        StateMachine.Instance.CurrentSceneType
            .Where(x => x == SceneType.ShowTarget)
            .Subscribe(
                _ => MoveToTarget(),
                ex => Debug.LogError("Error: " + ex.Message)
                ).AddTo(this);
    }
    private void MoveToTarget()
    {
        _stage.transform.DOMove(_targetPosition, 1.5f)
            .SetEase(Ease.Linear)
            .OnComplete(MoveToBuilder);
    }

    private async void MoveToBuilder()
    {
        await _missionShow.ShowMission();
        StateMachine.Instance.SetCurrentSceneType(SceneType.Builder);
        _stage.transform.position = _firstPosition;
    }

    #endregion
}
