// --------------------------------------------------------- 
// BuildTutorialState.cs 
// 組み立ての際のチュートリアル
//
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEditor.Build;
using Unity.VisualScripting;

public class BuildTutorialState : ITutorialState
{
	#region variable 
	//	ステートマシン
	private TutorialStateMachine	m_stateMachine;

	private float	m_radius;		//	
	private Vector2 m_offset;
	private Vector2 m_scale;
	private float	m_alpha;

	private string m_tutorialText;

	private TutorialData m_data;

	#endregion

	#region property

	public string TutorialText { get { return m_tutorialText; } }
	

	#endregion

	#region method

	//	コンストラクタ
	public BuildTutorialState(TutorialStateMachine stateMachine, TutorialData data)
	{
		m_stateMachine = stateMachine;
		m_tutorialText = "";
		m_data = data;
	}




	/*--------------------------------------------------------------------------------
	|| ステートの開始処理
	--------------------------------------------------------------------------------*/
	public void OnEnterState()
	{
		StateMachine.Instance.CurrentSceneType
			.Where(_ => _ == SceneType.Builder)
			.Subscribe((_) => StartTutorial())
			.AddTo(m_stateMachine);

	}

	/*--------------------------------------------------------------------------------
	|| ステートの終了処理
	--------------------------------------------------------------------------------*/
	public void OnExitState()
	{
	}


	/*--------------------------------------------------------------------------------
	|| ステートの更新処理
	--------------------------------------------------------------------------------*/
	public void UpdateState()
	{
		Color col = m_stateMachine.BackgroundImage.color;
		col.a = m_alpha;
		m_stateMachine.BackgroundImage.color = col;

		Material bgMat = m_stateMachine.BackgroundImage.material;
		bgMat.SetFloat("_radius", m_radius);
		bgMat.SetFloat("_circleOffsetX", m_offset.x);
		bgMat.SetFloat("_circleOffsetY", m_offset.y);
		bgMat.SetFloat("_circleScaleX", m_scale.x);
		bgMat.SetFloat("_circleScaleY", m_scale.y);
	}


	/*--------------------------------------------------------------------------------
	|| チュートリアルの開始
	--------------------------------------------------------------------------------*/
	private async void StartTutorial()
	{
		//	次の状態に遷移可能フラグ
		bool progressive = false;
		//	チュートリアルパネルの表示
		m_stateMachine.TutorialPanel.gameObject.SetActive(true);

		var sequence = DOTween.Sequence();

		for(int i = 0; i < m_data.Elements.Length; i++)
		{
			TutorialDataElement element = m_data.Elements[i];

			await DOTween.Sequence()
				.SetDelay(element.Delay)
				.Append(DOTween.To(() => m_radius, (value) => m_radius = value, element.ForcusRadius, element.Duration).SetEase(element.EaseType))
				.Join(DOTween.To(() => m_offset, (value) => m_offset = value, element.ForcusOffset, element.Duration).SetEase(element.EaseType))
				.Join(DOTween.To(() => m_scale, (value) => m_scale = value, element.ForcusScale, element.Duration).SetEase(element.EaseType))
				.Join(DOTween.To(() => m_alpha, (value) => m_alpha = value, element.Alpha, element.Duration).SetEase(element.EaseType))
				.Append(DOTween.To(() => "", (value) => m_tutorialText = value, element.AboutText, element.TextDuration)	.SetDelay(element.TextDelay))
				.OnComplete(() => progressive = true);
				
			await UniTask.WaitUntil( () => 
			(progressive && Input.GetMouseButton(0)) ||
			(i >= m_data.Elements.Length - 1 && progressive)
				);
			progressive = false;
			m_tutorialText = "";
		}

		//	全てのチュートリアルが完了したら、パネルごと非表示に設定する
		m_stateMachine.TutorialPanel.gameObject.SetActive(false);
	}

	#endregion
}