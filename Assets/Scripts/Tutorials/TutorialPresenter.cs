// --------------------------------------------------------- 
// TutorialPresenter.cs 
// 
// CreateDay: 2023/09/10
// Creator  : Ushimaru
// --------------------------------------------------------- 
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class TutorialPresenter : MonoBehaviour
{
	#region variable 
	//	テキスト
	[SerializeField]
	private TextMeshProUGUI			m_tutorialText;

	//	チュートリアルステートマシン
	[SerializeField]
	private TutorialStateMachine	m_model;

 #endregion

 #region method
 
 private void Awake()
 {
		//	チュートリアルテキスト
		m_model.TutorialText
			.Subscribe((value) => m_tutorialText.text = value)
			.AddTo(this);
 }
 #endregion
}