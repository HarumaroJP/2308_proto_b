// --------------------------------------------------------- 
// ForceStateChanger.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 

using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;

public class ForceStateChanger : MonoBehaviour
{
    #region variable

    [SerializeField] private SceneType sceneType;

    #endregion

    #region property

    #endregion

    #region method

    private async void Start()
    {
        await UniTask.Yield();

        StateMachine.Instance.SetCurrentSceneType(sceneType);
    }

    #endregion
}