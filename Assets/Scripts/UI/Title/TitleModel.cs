// --------------------------------------------------------- 
// TitleModel.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 
using Cysharp.Threading.Tasks;
public class TitleModel
{
    #region variable

    private readonly SceneManagerExtensions _sceneManager = new();

    #endregion
    #region method

    public async void MoveToStageSelect()
    {
        await _sceneManager.LoadSceneAsync(SceneType.StageSelect);
        StateMachine.Instance.SetCurrentSceneType(SceneType.StageSelect);
    }

    #endregion
}
