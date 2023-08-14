// --------------------------------------------------------- 
// StartModel.cs 
// 
// CreateDay: 2023/08/04
// Creator  : fuwa
// --------------------------------------------------------- 

public class StartModel
{
    #region method

    public void MoveToFight()
    {
        StateMachine.Instance.SetCurrentSceneType(SceneType.InGame);
    }

    #endregion
}
