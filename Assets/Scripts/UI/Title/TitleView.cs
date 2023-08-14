// --------------------------------------------------------- 
// TitleView.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class TitleView : MonoBehaviour
{
    #region variable

    [SerializeField] private Button _startButton;
    [SerializeField] private Button _creditButton;

    #endregion
    #region method

    public IObservable<Unit> OnClickStartAsObservable() => _startButton.OnClickAsObservable();
    // public IObservable<Unit> OnClickCreditAsObservable() => _creditButton.OnClickAsObservable();

    #endregion
}
