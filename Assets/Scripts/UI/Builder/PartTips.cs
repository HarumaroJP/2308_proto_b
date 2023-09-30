// --------------------------------------------------------- 
// PartTips.cs 
// 
// CreateDay: 
// Creator  : haru
// --------------------------------------------------------- 

using UnityEngine;
using System.Collections;
using Part;

[System.Serializable]
class ElementTipPair
{
    public PartElement element;
    public GameObject tip;
}


public class PartTips : MonoBehaviour
{
    #region variable

    [SerializeField] private ElementTipPair[] tipPairs = default;

    #endregion

    #region method

    private void Awake()
    {
        foreach (ElementTipPair pair in tipPairs)
        {
            RegisterTip(pair.element, pair.tip);
        }
    }

    private void RegisterTip(PartElement partElement, GameObject tip)
    {
        partElement.OnTipShow += () =>
        {
            tip.SetActive(true);
        };

        partElement.OnTipHide += () =>
        {
            tip.SetActive(false);
        };

        partElement.OnDuplicated += element =>
        {
            tip.SetActive(false);
            RegisterTip(element, tip);
        };
    }

    #endregion
}