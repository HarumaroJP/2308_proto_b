// --------------------------------------------------------- 
// TargetDisplay.cs 
// 
// CreateDay: fuwa
// Creator  : 2023/08/04
// --------------------------------------------------------- 
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UniRx;
public class TargetDisplay : MonoBehaviour
{
    #region variable

    [SerializeField] private GameObject _missionBanner;
    [SerializeField] private TextMeshProUGUI _missionBannerText;

    private Vector3 _initialPosition;
    private readonly Vector3 _displayPosition = new(-7, 0, 0);

    #endregion

    #region method

    private void Awake()
    {
        _initialPosition = transform.position;
        SetupTargetDisplayObserver();
    }

    private void SetupTargetDisplayObserver()
    {
        StateMachine.Instance.CurrentSceneType
            .Where(sceneType => sceneType == SceneType.ShowTarget)
            .Subscribe(
                _ => DisplayTarget(),
                ex => Debug.LogError($"Error: {ex.Message}")
                )
            .AddTo(this);
    }
    private void DisplayTarget()
    {
        MoveStageToDisplayPosition()
            .OnComplete(OnCompleteMove);
    }

    private async void OnCompleteMove()
    {
        await DisplayMission();
        TransitionToBuilderScene();
        ResetStagePosition();
    }
    private Tweener MoveStageToDisplayPosition()
    {
        return transform
            .DOMove(_displayPosition, 1.5f)
            .SetEase(Ease.Linear);
    }

    private async UniTask DisplayMission()
    {
        _missionBanner.SetActive(true);
        _missionBannerText.text = ProgressManager.Instance.CurrentStage.SceneName;
        AudioManager.Instance.PlaySe(AudioClipName.ShowTarget);
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
        _missionBanner.SetActive(false);
    }
    
    private static void TransitionToBuilderScene()
    {
        StateMachine.Instance.SetCurrentSceneType(SceneType.Builder);
    }
    
    private void ResetStagePosition()
    {
        transform.position = _initialPosition;
    }

    #endregion
}