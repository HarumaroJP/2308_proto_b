// --------------------------------------------------------- 
// HowToControl.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using System.Collections;
using UniRx;

public class HowToControl : MonoBehaviour
{
    #region variable

    #endregion
    #region property

    #endregion
    #region method

    private void Awake()
    {
        StateMachine.Instance.CurrentSceneType
            .Subscribe(
                x =>
                {
                    if (gameObject == null) return;
                    gameObject.SetActive(x == SceneType.InGame);
                },
                ex => Debug.LogError("Error: " + ex.Message)
                )
            .AddTo(this);
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    #endregion
}
