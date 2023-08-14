// --------------------------------------------------------- 
// StageSelectModel.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 
using Cysharp.Threading.Tasks;
public class StageSelectModel
{
    #region variable

    private readonly SceneManagerExtensions _sceneManager = new();

    #endregion

    #region method

    public async void MoveToTitle()
    {
        await _sceneManager.LoadSceneAsync(SceneType.Title);
        StateMachine.Instance.SetCurrentSceneType(SceneType.Title);
        AudioManager.Instance.PlayBgm(AudioClipName.TitleBgm);
    }

    public async void MoveToGame(int index)
    {
        await _sceneManager.LoadSceneAsync(SceneType.InGame);
        var levelLoader = new LevelLoader();
        levelLoader.Load(index);
        StateMachine.Instance.SetCurrentSceneType(SceneType.ShowTarget);
    }

    #endregion
}
