using System;
using System.Collections;
using System.Collections.Generic;
using Builder.System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BindPresenter : MonoBehaviour
{
    [SerializeField] private BindView bindView;
    [SerializeField] private PartBinder partBinder;

    private void Start()
    {
        bindView.OnClickBindLeftObservable()
            .Subscribe(_ =>
            {
                bool canSwitch = partBinder.TrySwitchBindState(0);
                bindView.SetBindLeftTextState(canSwitch);
            }, ex => Debug.LogError("Error: " + ex.Message))
            .AddTo(this);

        bindView.OnClickBindRightObservable()
            .Subscribe(_ =>
            {
                bool canSwitch = partBinder.TrySwitchBindState(1);
                bindView.SetBindRightTextState(canSwitch);
            }, ex => Debug.LogError("Error: " + ex.Message))
            .AddTo(this);
    }
}