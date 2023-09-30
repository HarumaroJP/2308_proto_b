// --------------------------------------------------------- 
// ResultActivator.cs 
// 
// CreateDay: 2023/08/05
// Creator  : Ushimaru
// --------------------------------------------------------- 

using System;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using Builder.System;
using UniRx;
using Unity.VisualScripting;

public class ResultActivator : MonoBehaviour
{
    #region variable

    //	勝敗フラグ
    public enum ResultState
    {
        DEFEAT = -1, //	敗北
        NONE = 0, //	未決定
        VICTORY = 1 //	勝利
    };

    [Header("Prefab")]
    [SerializeField, Tooltip("リザルトUIのPrefab")]
    private GameObject m_resultPrefab;

    [Header("リザルト")]
    [SerializeField, Tooltip("敗北になる破壊率")]
    private float m_defeatRate;

    [SerializeField, Tooltip("勝利になる破壊率")] private float m_victoryRate;

    //	Updateの有効フラグ
    private bool m_enableUpdate;


    //	破壊率管理
    private DestructionRateController m_destructionRate;

    [SerializeField] private PartBuilder _partBuilder;

    public event Action OnDefeat;

    #endregion

    #region property

    //	現在のステート
    public ResultState CurrentState { private set; get; }

    #endregion

    #region method

    //	初期化処理
    private void Start()
    {
        //	破壊率コントローラーの検索
        m_destructionRate = FindObjectOfType<DestructionRateController>();

        //	現在のシーン変数の購読
        StateMachine.Instance.CurrentSceneType
            .Subscribe(
                x => m_enableUpdate = x == SceneType.InGame,
                ex => Debug.LogError("Error: " + ex.Message)
            )
            .AddTo(this);
        StateMachine.Instance.CurrentSceneType
            .Where(x => x != SceneType.InGame)
            .Subscribe(
                _ => CurrentState = ResultState.NONE,
                ex => Debug.LogError("Error: " + ex.Message)
            )
            .AddTo(this);

        m_destructionRate.OnDead.Skip(1)
            .Subscribe(unit =>
            {
                CurrentState = ResultState.VICTORY;
                Instantiate(m_resultPrefab);
            })
            .AddTo(this);
    }

    //	更新処理
    private void Update()
    {
        //	プレイ中ではないときは処理しない
        if (!m_enableUpdate)
            return;

        //	勝敗が決定済みのときは処理しない
        if (CurrentState != ResultState.NONE)
            return;

        //	敵の破壊率が指定の値より大きくなったら”勝利”とする
        if (m_destructionRate.EnemyCurrentRate < m_victoryRate)
        {
            CurrentState = ResultState.VICTORY;
            Instantiate(m_resultPrefab);

            return;
        }

        //	プレイヤーの破壊率が指定の値より大きくなったら”敗北”とする
        if (m_destructionRate.PlayerCurrentRate < m_defeatRate)
        {
            CurrentState = ResultState.DEFEAT;
            OnDefeat?.Invoke();
            Retry();
        }
    }

    public void Retry()
    {
        Debug.Log("Retry");
        _partBuilder.Retry();
        StateMachine.Instance.SetCurrentSceneType(SceneType.Builder);
    }

    #endregion
}