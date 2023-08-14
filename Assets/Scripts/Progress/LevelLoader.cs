// --------------------------------------------------------- 
// LevelLoader.cs 
// 
// CreateDay: 2023/08/05
// Creator  : fuwa
// ---------------------------------------------------------
using UnityEngine;
public class LevelLoader
{
    #region method

    public void LoadNext()
    {
        if (ProgressManager.Instance.CurrentStage == null)
        {
            Debug.LogError("Error: StageData is null.");
            return;
        }
        Unload();
        Load(ProgressManager.Instance.CurrentStageIndex + 1);
    }

    public void Load(int index)
    {
        ProgressManager.Instance.SetCurrentStageIndex(index);
        var obj = Object.Instantiate(ProgressManager.Instance.CurrentStage.StagePrefab);
        ProgressManager.Instance.SetCurrentStageObject(obj);
    }

    public void Unload()
    {
        ProgressManager.Instance.Unload();
    }

    #endregion
}
