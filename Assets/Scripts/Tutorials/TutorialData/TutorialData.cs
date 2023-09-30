// --------------------------------------------------------- 
// TutorialData.cs 
// 
// CreateDay: 2023/09/30
// Creator  : Ushimaru
// --------------------------------------------------------- 
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class TutorialDataElement
{
	[SerializeField]
	private string m_name;      //	�C���X�y�N�^�[�̕\���p

	[SerializeField]
	private float	m_duration;         //	����
	[SerializeField]
	private float	m_delay;            //	�S�̂̒x��
	[SerializeField]
	private float	m_textDuration;		//	�e�L�X�g�̕\������
	[SerializeField]
	private float	m_textDelay;		//	�e�L�X�g�̒x��
	[SerializeField, TextArea]
	private string	m_aboutText;		//	����̕���
	[SerializeField]
	private float	m_forcusRadius;     //	�t�H�[�J�X�̔��a
	[SerializeField]
	private Vector2 m_forcusOffset;     //	�t�H�[�J�X�̒��S
	[SerializeField]
	private Vector2 m_forcusScale;      //	�t�H�[�J�X�̃X�P�[��
	[SerializeField]
	private float	m_alpha;			//	�A���t�@
	[SerializeField]
	private Ease	m_easeType;			//	�C�[�W���O�^�C�v

	//	�v���p�e�B
	public float	Duration		=> m_duration;
	public float	Delay			=> m_delay;
	public float	TextDuration	=> m_textDuration;
	public float	TextDelay		=> m_textDelay;
	public string	AboutText		=> m_aboutText;
	public float	ForcusRadius	=> m_forcusRadius;
	public Vector2	ForcusOffset	=> m_forcusOffset;
	public Vector2	ForcusScale		=> m_forcusScale;
	public float	Alpha			=> m_alpha;
	public Ease		EaseType		=> m_easeType;
}

[CreateAssetMenu(menuName = "ScriptableObjects/TutorialData", fileName = "TutorialData")]
public class TutorialData : ScriptableObject
{
	[SerializeField]
	private TutorialDataElement[]	m_elements;

	public TutorialDataElement[] Elements => m_elements;
}