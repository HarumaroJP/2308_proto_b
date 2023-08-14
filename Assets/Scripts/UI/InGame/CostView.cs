// --------------------------------------------------------- 
// CostView.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CostView : MonoBehaviour
{
    #region variable

    [SerializeField] private GameObject costParentObject;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Slider _costSlider;

    #endregion


    #region method

    public void UpdateCost(int cost, int totalCost)
    {
        _costText.text = $"コスト：{cost}/{totalCost}";
        _costSlider.maxValue = totalCost;
        _costSlider.DOValue(cost, 0.3f);
    }

    public void Show()
    {
        costParentObject.SetActive(true);
    }

    public void Hide()
    {
        costParentObject.SetActive(false);
    }

    #endregion
}