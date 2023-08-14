// --------------------------------------------------------- 
// EnemyDestructionRateView.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using System;
using UnityEngine;
using UniRx;
using TMPro;
public class EnemyDestructionRateView : MonoBehaviour
{
    #region variable

    [SerializeField] private DestructionRateController _destructionRateController;
    [SerializeField] private TextMeshProUGUI _enemyRateText;

    #endregion
    #region property

    #endregion
    #region method

    private void Awake()
    {
        _destructionRateController.EnemyHpRate
            .Subscribe(rate => _enemyRateText.text = $"破壊率：{100 - Mathf.Floor(rate * 100)}%")
            .AddTo(this);

        StateMachine.Instance.CurrentSceneType
            .Subscribe(scene =>
            {
                if (scene == SceneType.InGame)
                    _enemyRateText.gameObject.SetActive(true);
                else
                {
                    if (_enemyRateText == null) return;
                    _enemyRateText.gameObject.SetActive(false);
                }
            });
    }

    #endregion
}
