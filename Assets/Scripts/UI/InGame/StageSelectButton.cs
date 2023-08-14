// --------------------------------------------------------- 
// StageSelectButton.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;
public class StageSelectButton : MonoBehaviour
{
    #region variable

    [SerializeField] private GameObject _gameObject;
    [SerializeField] private Button _stageSelectButton;
    private readonly ResultModel _model = new();

    #endregion

    #region method

    private void Reset()
    {
        _stageSelectButton = GetComponent<Button>();
    }

    private void Start()
    {
        _stageSelectButton.onClick.AddListener(StageSelect);

        StateMachine.Instance.CurrentSceneType
            .Subscribe(scene =>
            {
                _stageSelectButton.gameObject.SetActive(scene == SceneType.Builder);
            })
            .AddTo(this);
    }

    private void StageSelect()
    {
        AudioManager.Instance.PlaySe(AudioClipName.ButtonClick);
        _gameObject.SetActive(false);
        _model.MoveToStageSelect();
    }

    #endregion
}
