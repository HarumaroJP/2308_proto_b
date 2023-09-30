// --------------------------------------------------------- 
// Transition.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using System.Collections;
using System;

public class Transition : Singleton<Transition>
{
	[Serializable]
	public enum FadeType
	{
		FadeIn,
		FadeOut
	}

	#region variable 

	[SerializeField]
	private Animator m_anim;
	[SerializeField]
	private FadeAnim m_fadeAnim;


	#endregion
	#region property

	public bool FadeInCompleted => m_fadeAnim.Progress <= 0.0f;
	public bool FadeOutCompleted => m_fadeAnim.Progress >= 1.0f;

	#endregion
	#region method

	public void PlayFade(FadeType type)
	{
		switch (type)
		{
			case FadeType.FadeIn:
				m_anim.SetTrigger("FadeIn");
				break;
			case FadeType.FadeOut:
				m_anim.SetTrigger("FadeOut");
				break;

			default:
				break;
		}
	}
	
	#endregion
}