using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BindView : MonoBehaviour
{
    [SerializeField] private Button bindLeft;
    [SerializeField] private Button bindRight;

    [SerializeField] private Color bindLeftColor;
    [SerializeField] private Color bindRightColor;

    [SerializeField] private TextMeshProUGUI bindLeftText;
    [SerializeField] private TextMeshProUGUI bindRightText;

    public IObservable<Unit> OnClickBindLeftObservable() => bindLeft.OnClickAsObservable();
    public IObservable<Unit> OnClickBindRightObservable() => bindRight.OnClickAsObservable();

    public void SetBindLeftTextState(bool enable)
    {
        bindLeftText.text = enable ? "割当終了" : "割当開始";
        bindLeft.image.color = enable ? bindLeftColor : Color.white;
    }

    public void SetBindRightTextState(bool enable)
    {
        bindRightText.text = enable ? "割当終了" : "割当開始";
        bindRight.image.color = enable ? bindRightColor : Color.white;
    }
}