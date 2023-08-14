// --------------------------------------------------------- 
// StageSelectView.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class StageSelectView : MonoBehaviour
{
    #region variable

    [SerializeField] private Button _backToTitleButton;
    [SerializeField] private Button[] _stageButtons;

    #endregion
    #region property

    public int StageButtonsLength => _stageButtons.Length;

    #endregion
    #region method

    public IObservable<Unit> OnClickBackToTitleAsObservable() => _backToTitleButton.OnClickAsObservable();
    public IObservable<Unit> OnClickStageAsObservable(int index) => _stageButtons[index].OnClickAsObservable();

    #endregion
}
