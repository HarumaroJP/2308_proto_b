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
	//	�e�L�X�g
	[SerializeField]
	private TextMeshProUGUI			m_tutorialText;

	//	�`���[�g���A���X�e�[�g�}�V��
	[SerializeField]
	private TutorialStateMachine	m_model;

 #endregion

 #region method
 
 private void Awake()
 {
		//	�`���[�g���A���e�L�X�g
		m_model.TutorialText
			.Subscribe((value) => m_tutorialText.text = value)
			.AddTo(this);
 }
 #endregion
}