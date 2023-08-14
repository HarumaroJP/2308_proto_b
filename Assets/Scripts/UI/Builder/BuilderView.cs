using System;
using System.Collections;
using System.Collections.Generic;
using Builder.System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class BuilderView : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [FormerlySerializedAs("stopButton")] [SerializeField] private Button retryButton;
    [SerializeField] private PartBuilder partBuilder;

    private void Start()
    {
        startButton.onClick.AsObservable().Subscribe(_ => partBuilder.Play()).AddTo(this);
        retryButton.onClick.AsObservable().Subscribe(_ => partBuilder.Retry()).AddTo(this);
    }
}