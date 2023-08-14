// --------------------------------------------------------- 
// StartView.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 

using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class StartView : MonoBehaviour
{
    #region variable

    [SerializeField] private Button _startButton;
    [SerializeField] private TextMeshProUGUI startButtonText;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color costOverColor;

    #endregion

    #region method

    public IObservable<Unit> OnClickStartAsObservable() => _startButton.OnClickAsObservable();

    public void SetEnableButton(bool enable)
    {
        _startButton.interactable = enable;
    }

    public void SetCostOverButton(bool enable)
    {
        startButtonText.text = enable ? "コスト\nオーバー" : "スタート";
        startButtonText.color = enable ? costOverColor : defaultColor;
        _startButton.interactable = !enable;
    }

    #endregion
}