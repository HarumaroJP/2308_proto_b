// TutorialStateMachine.cs
//	チュートリアルのステートを管理する（ステートパターン）
//
//CreateDay: 2023/09/09
//Creator  : Ushimaru
//
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class TutorialStateMachine : MonoBehaviour
{
	#region variable 

	//	現在のステート
	private ITutorialState	m_currentState;

	//	各ステート
	private BuildTutorialState m_targetState;   //	目標

	//	チュートリアルパネル
	[SerializeField]
	private Transform	m_tutorialPanel;
	[SerializeField]
	private Image		m_background;

	//	チュートリアルデータ
	[SerializeField]
	private TutorialData m_buildTutorialData;

	private readonly ReactiveProperty<string> m_tutorialText = new ReactiveProperty<string>();

	#endregion

	#region property
	public Transform	TutorialPanel	=> m_tutorialPanel;
	public Image		BackgroundImage => m_background;

	public IReadOnlyReactiveProperty<string> TutorialText => m_tutorialText; 

	#endregion

	#region method

	private void Awake()
	{
		//	各ステートの作成
		m_targetState = new BuildTutorialState(this, m_buildTutorialData);
	}

	private void Start()
	{
		//	ステートの初期値を設定する
		ChangeState(m_targetState);
	}

	private void Update()
	{
		//	ステートが未設定のときは処理しない
		if (m_currentState == null)
			return;

		//	ステートの更新処理
		m_currentState.UpdateState();
		//	テキストを取得してくる
		m_tutorialText.Value = m_currentState.TutorialText;
	}


	/*--------------------------------------------------------------------------------
	|| ステートの変更
	--------------------------------------------------------------------------------*/
	public void ChangeState(ITutorialState nextState)
	{
#if UNITY_EDITOR
		Debug.Log("TutorialStateChanged : " + m_currentState + " => " + nextState);
#endif
		//	遷移先のステートが指定されていないときはエラーを表示する
		if (nextState == null)
			Debug.LogError("遷移先が指定されていません。");

		if (m_currentState != null)
		{
			//	ステートの終了処理
			m_currentState.OnExitState();
		}

		//	ステートの変更
		m_currentState = nextState;
		//	ステートの開始処理
		m_currentState.OnEnterState();
	}

	#endregion
}