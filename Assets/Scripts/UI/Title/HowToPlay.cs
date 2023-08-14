// --------------------------------------------------------- 
// HowToPlay.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{
	#region variable 

	[SerializeField]
	private Image	m_image;
	[SerializeField]
	private Sprite[] m_sprites;

	[SerializeField]
	private int m_pageNo = 0;		//	ƒy[ƒW”Ô†

 #endregion
 #region property
 #endregion
 #region method
 
	public void SetActive(bool active)
	{
		gameObject.SetActive(active);
		m_pageNo = 0;

		UpdateSprite();
	}

	public void PushNext()
	{
		m_pageNo++;
		if (m_pageNo >= m_sprites.Length)
		{
			SetActive(false);
			return;
		}

		UpdateSprite();
	}

	public void PushPrev()
	{
		m_pageNo--;
		if (m_pageNo < 0)
		{
			SetActive(false);
			return;
		}
		UpdateSprite();
	}


	private void UpdateSprite()
	{
		m_image.sprite = m_sprites[m_pageNo];
	}

 #endregion
}