// --------------------------------------------------------- 
// PlayerDestructionRateView.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using System.Collections;
using TMPro;
public class PlayerDestructionRateView : MonoBehaviour
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
        _enemyRateText.text = $"プレイヤー破壊率：{_destructionRateController.PlayerCurrentRate}%";
    }

    #endregion
}
