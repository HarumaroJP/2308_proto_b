// --------------------------------------------------------- 
// ResultModel.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 
using Cysharp.Threading.Tasks;
public class ResultModel
{
    #region variable

    private readonly SceneManagerExtensions _sceneManager = new();
    private readonly LevelLoader _levelLoader = new();

    #endregion

    #region method

    public async void MoveToStageSelect()
    {
        await _sceneManager.LoadSceneAsync(SceneType.StageSelect);
        StateMachine.Instance.SetCurrentSceneType(SceneType.StageSelect);
    }

    public void MoveToRetry()
    {
        _levelLoader.Unload();
        _levelLoader.Load(ProgressManager.Instance.CurrentStageIndex);
        StateMachine.Instance.SetCurrentSceneType(SceneType.Builder);
    }

    public async void MoveToNextStage()
    {
        if (ProgressManager.Instance.CurrentStageIndex == 2) return;
        await _sceneManager.LoadSceneAsync(SceneType.InGame);
        StateMachine.Instance.SetCurrentSceneType(SceneType.ShowTarget);

        _levelLoader.LoadNext();
    }

    #endregion
}
