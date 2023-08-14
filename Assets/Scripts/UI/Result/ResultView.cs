// --------------------------------------------------------- 
// ResultView.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
public class ResultView : MonoBehaviour
{
    #region variable

    [SerializeField] private GameObject _resultObject;
    [SerializeField] private Button _toStageSelectButton;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _toNextButton;

    #endregion

    #region method

    public IObservable<Unit> OnClickToStageSelectAsObservable() => _toStageSelectButton.OnClickAsObservable();
    public IObservable<Unit> OnClickToRetryAsObservable() => _retryButton.OnClickAsObservable();
    public IObservable<Unit> OnClickToNextStageAsObservable() => _toNextButton.OnClickAsObservable();

    public void Destroy()
    {
        Destroy(_resultObject);
    }

    #endregion
}
