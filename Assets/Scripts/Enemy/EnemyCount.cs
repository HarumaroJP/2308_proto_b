// --------------------------------------------------------- 
// EnemyCount.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using System.Collections;
using UniRx;
public class EnemyCount : MonoBehaviour
{
    #region variable

    DestructionRateController destructionRateController = default;
    [SerializeField] private StartView _view;

    #endregion
    #region property

    #endregion
    #region method

    private void Awake()
    {
        StateMachine.Instance.CurrentSceneType
            .Subscribe(scene =>
            {
                if (scene == SceneType.InGame)
                {
                    CountStart();
                }
            });
    }

    private void CountStart()
    {
        int block = GameObject.FindObjectsByType<BlackBord>(FindObjectsSortMode.None).Length;
        int book = GameObject.FindObjectsByType<Book>(FindObjectsSortMode.None).Length;

        block -= book;

        int enemy = GameObject.FindObjectsByType<EnemyManager>(FindObjectsSortMode.None).Length;
        Debug.Log(block);
        destructionRateController = GameObject.FindAnyObjectByType<DestructionRateController>();

        if (destructionRateController != null)
        {
            destructionRateController.InitEnemyPartsCount(block);
            destructionRateController.InitEnemyCount(enemy);
        }
    }

    #endregion
}
