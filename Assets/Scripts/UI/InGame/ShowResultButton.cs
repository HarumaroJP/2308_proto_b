// --------------------------------------------------------- 
// ShowResultButton.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ShowResultButton : MonoBehaviour
{
    #region variable

    [SerializeField] private Button _button;
    [SerializeField] private GameObject _resultObject;

    #endregion

    #region method

    private void Awake()
    {
        _button.onClick.AddListener(ShowResult);
    }
    private void ShowResult()
    {
        Instantiate(_resultObject);
    }

    #endregion
}
