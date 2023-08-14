// --------------------------------------------------------- 
// ResultController.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using System.Collections;
using TMPro;

public class ResultController : MonoBehaviour
{
    #region variable

    [SerializeField]
    TextMeshProUGUI _timeText = default;
    [SerializeField]
    TextMeshProUGUI _scoreText = default;

    #endregion
    #region property

    #endregion
    #region method

    private void Start()
    {
        float cTime = TimeController.Instance.TimerStop();
        _timeText.text = $"{Mathf.Floor(cTime / 60):00}:{cTime % 60:00}";

        string score = "";

        var stage = ProgressManager.Instance.CurrentStage;


        if (cTime < stage.StageRank[0].Time)
        {
            score = stage.StageRank[0].Rank;
        }
        else if (cTime < stage.StageRank[1].Time)
        {
            score = stage.StageRank[1].Rank;
        }
        else if (cTime < stage.StageRank[2].Time)
        {
            score = stage.StageRank[2].Rank;
        }
        else
        {
            score = stage.StageRank[3].Rank;
        }


        _scoreText.text = score;
    }

    #endregion
}
