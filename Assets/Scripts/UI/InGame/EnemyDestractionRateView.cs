// --------------------------------------------------------- 
// EnemyDestructionRateView.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using System;
using TMPro;
using UniRx;
using UnityEngine;
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
            .Where(hp => 0 <= hp && hp <= 100)
            .Subscribe(rate => _enemyRateText.text = $"破壊率：{100 - Mathf.Floor(rate * 100)}%")
            .AddTo(this);

        StateMachine.Instance.CurrentSceneType
            .Subscribe(scene =>
            {
                if (scene == SceneType.InGame)
                {
                    if (_enemyRateText == null) return;
                    _enemyRateText.gameObject.SetActive(true);
                }
                else
                {
                    if (_enemyRateText == null) return;
                    _enemyRateText.gameObject.SetActive(false);
                }
            });
    }

    #endregion
}
