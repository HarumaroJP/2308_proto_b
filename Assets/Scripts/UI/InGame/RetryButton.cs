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

    private void Reset()
    {
        _retryButton = GetComponent<Button>();
    }
    public IObservable<Unit> OnClickRetryAsObservable() => _retryButton.OnClickAsObservable();
    private void Awake()
    {
        StateMachine.Instance.CurrentSceneType
            .Subscribe(scene =>
            {
                if (scene == SceneType.InGame)
                {
                    _retryButton.gameObject.SetActive(true);
                }
                else
                {
                    if (_retryButton == null) return;
                    _retryButton.gameObject.SetActive(false);
                }
            })
            .AddTo(this);
    }
}
