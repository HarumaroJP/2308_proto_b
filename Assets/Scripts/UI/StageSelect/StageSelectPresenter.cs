// --------------------------------------------------------- 
// StageSelectPresenter.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 
using UniRx;
using UnityEngine;
public class StageSelectPresenter : MonoBehaviour
{
    #region variable

    [SerializeField] private StageSelectView _view;
    private readonly StageSelectModel _model = new();

    #endregion
    #region method

    private void Awake()
    {
        _view.OnClickBackToTitleAsObservable()
            .Subscribe(
                _ =>
                {
                    _model.MoveToTitle();
                    AudioManager.Instance.PlaySe(AudioClipName.ButtonClick);
                },
                ex => Debug.LogError("Error: " + ex.Message)
                ).AddTo(this);

        for (int i = 0; i < _view.StageButtonsLength; i++)
        {
            int index = i;
            _view.OnClickStageAsObservable(i)
                .Subscribe(
                    _ =>
                    {
                        _model.MoveToGame(index);
                        AudioManager.Instance.PlaySe(AudioClipName.ButtonClick);
                    },
                    ex => Debug.LogError("Error: " + ex.Message)
                    ).AddTo(this);
        }
    }

    #endregion
}
