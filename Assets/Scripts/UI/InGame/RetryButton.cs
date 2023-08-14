// --------------------------------------------------------- 
// RetryButton.cs 
// 
// CreateDay: 2023/08/06
// Creator  : fuwa
// --------------------------------------------------------- 
using System;
using Builder.System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RetryButton : MonoBehaviour
{
    [SerializeField] private Button _retryButton;
    [SerializeField] private PartBuilder _partBuilder;
    private readonly ResultModel _model = new();

    private void Reset()
    {
        _retryButton = GetComponent<Button>();
    }

    private void Awake()
    {
        _retryButton.onClick.AddListener(Retry);

        StateMachine.Instance.CurrentSceneType
            .Subscribe(scene =>
            {
                if (scene == SceneType.InGame)
                    _retryButton.gameObject.SetActive(true);
                else
                {
                    if (_retryButton == null) return;
                    _retryButton.gameObject.SetActive(false);
                }
            })
            .AddTo(this);
    }

    private void Retry()
    {
        AudioManager.Instance.PlaySe(AudioClipName.ButtonClick);

        _partBuilder.Retry();
        _model.MoveToRetry();
    }
}
