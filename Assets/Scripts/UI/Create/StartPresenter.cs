// --------------------------------------------------------- 
// StartPresenter.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 

using Builder.System;
using UniRx;
using UnityEngine;

public class StartPresenter : MonoBehaviour
{
    #region variable

    [SerializeField] private StartView _view;
    [SerializeField] private PartBuilder _partBuilder;
    private readonly StartModel _model = new();

    #endregion

    #region method

    private void Awake()
    {
        _view.OnClickStartAsObservable()
            .Subscribe(
                _ =>
                {
                    _partBuilder.Play();
                    _model.MoveToFight();
                    AudioManager.Instance.PlaySe(AudioClipName.ButtonClick);
                },
                ex => Debug.LogError("Error: " + ex.Message)
            )
            .AddTo(this);

        //
        // _partBuilder.OnPartAdded += element =>
        // {
        //     _view.SetEnableButton(_partBuilder.CurrentParts.Count > 0);
        // };
        //
        // _partBuilder.OnPartRemoved += element =>
        // {
        //     _view.SetEnableButton(_partBuilder.CurrentParts.Count > 0);
        // };
    }

    #endregion
}