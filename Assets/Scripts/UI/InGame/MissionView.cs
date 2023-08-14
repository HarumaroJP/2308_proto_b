// --------------------------------------------------------- 
// MissionView.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 
using TMPro;
using UnityEngine;
public class MissionView : MonoBehaviour
{
    #region variable

    [SerializeField] private TextMeshProUGUI _missionText;

    #endregion
    #region method

    public void Show(string mission)
    {
        _missionText.gameObject.SetActive(true);
        _missionText.text = mission;
    }
    public void Hide()
    {
        _missionText.gameObject.SetActive(false);
    }

    #endregion
}
