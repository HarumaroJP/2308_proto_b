// --------------------------------------------------------- 
// Progress.cs 
// 
// CreateDay: 2023/08/05
// Creator  : fuwa
// ---------------------------------------------------------
using UnityEngine;
public class Progress
{
    #region variable

    private int _rank;
    private float _time;

    #endregion
    #region property

    public bool IsClear { get; private set; }

    public int Rank
    {
        get => _rank;
        set
        {
            if (!IsClear)
            {
                Debug.Log("Failed to set rank：まだクリアしてないよ");
                return;
            }
            _rank = value;
        }
    }
    public float Time
    {
        get => _time;
        private set
        {
            if (!IsClear)
            {
                Debug.Log("Failed to set time：まだクリアしてないよ");
                return;
            }
            _time = value;
        }
    }

    #endregion
    #region method

    #endregion
}
