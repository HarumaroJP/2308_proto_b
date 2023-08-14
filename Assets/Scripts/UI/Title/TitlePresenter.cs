// --------------------------------------------------------- 
// TitlePresenter.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 
using UniRx;
using UnityEngine;
public class TitlePresenter : MonoBehaviour
{
    #region variable

    [SerializeField] private TitleView _view;
    private readonly TitleModel _model = new();

    #endregion
    #region method

    private void Awake()
    {
        _view.OnClickStartAsObservable()
            .Subscribe(
                _ =>
                {
                    _model.MoveToStageSelect();
                    AudioManager.Instance.PlaySe(AudioClipName.ButtonClick);
                    AudioManager.Instance.PlayBgm(AudioClipName.ResultBgm);
                },
                ex => Debug.LogError("Error: " + ex.Message)
                ).AddTo(this);
    }

    #endregion
}
