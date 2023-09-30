// TutorialStateMachine.cs
//	�`���[�g���A���̃X�e�[�g���Ǘ�����i�X�e�[�g�p�^�[���j
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

	//	���݂̃X�e�[�g
	private ITutorialState	m_currentState;

	//	�e�X�e�[�g
	private BuildTutorialState m_targetState;   //	�ڕW

	//	�`���[�g���A���p�l��
	[SerializeField]
	private Transform	m_tutorialPanel;
	[SerializeField]
	private Image		m_background;

	//	�`���[�g���A���f�[�^
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
		//	�e�X�e�[�g�̍쐬
		m_targetState = new BuildTutorialState(this, m_buildTutorialData);
	}

	private void Start()
	{
		//	�X�e�[�g�̏����l��ݒ肷��
		ChangeState(m_targetState);
	}

	private void Update()
	{
		//	�X�e�[�g�����ݒ�̂Ƃ��͏������Ȃ�
		if (m_currentState == null)
			return;

		//	�X�e�[�g�̍X�V����
		m_currentState.UpdateState();
		//	�e�L�X�g���擾���Ă���
		m_tutorialText.Value = m_currentState.TutorialText;
	}


	/*--------------------------------------------------------------------------------
	|| �X�e�[�g�̕ύX
	--------------------------------------------------------------------------------*/
	public void ChangeState(ITutorialState nextState)
	{
#if UNITY_EDITOR
		Debug.Log("TutorialStateChanged : " + m_currentState + " => " + nextState);
#endif
		//	�J�ڐ�̃X�e�[�g���w�肳��Ă��Ȃ��Ƃ��̓G���[��\������
		if (nextState == null)
			Debug.LogError("�J�ڐ悪�w�肳��Ă��܂���B");

		if (m_currentState != null)
		{
			//	�X�e�[�g�̏I������
			m_currentState.OnExitState();
		}

		//	�X�e�[�g�̕ύX
		m_currentState = nextState;
		//	�X�e�[�g�̊J�n����
		m_currentState.OnEnterState();
	}

	#endregion
}