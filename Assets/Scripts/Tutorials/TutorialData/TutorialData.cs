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
	private string m_name;      //	インスペクターの表示用

	[SerializeField]
	private float	m_duration;         //	時間
	[SerializeField]
	private float	m_delay;            //	全体の遅延
	[SerializeField]
	private float	m_textDuration;		//	テキストの表示時間
	[SerializeField]
	private float	m_textDelay;		//	テキストの遅延
	[SerializeField, TextArea]
	private string	m_aboutText;		//	解説の文字
	[SerializeField]
	private float	m_forcusRadius;     //	フォーカスの半径
	[SerializeField]
	private Vector2 m_forcusOffset;     //	フォーカスの中心
	[SerializeField]
	private Vector2 m_forcusScale;      //	フォーカスのスケール
	[SerializeField]
	private float	m_alpha;			//	アルファ
	[SerializeField]
	private Ease	m_easeType;			//	イージングタイプ

	//	プロパティ
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