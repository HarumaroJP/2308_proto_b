// --------------------------------------------------------- 
// TimeController.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using System;
using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;
using UniRx;
public class TimeController : Singleton<TimeController>
{
    #region variable

    private float _timer;
    private bool _isStarted = false;

    #endregion
    #region property

    /// <summary>���݂̌o�ߎ���</summary>
    public float Timer => _timer;

    #endregion
    #region method

    /// <summary>�^�C�}�[�X�^�[�g</summary>
    public void TimerStart()
    {
        _isStarted = true;
    }

    /// <summary>�^�C�}�[�X�g�b�v</summary>
    /// <returns>���݂̌o�ߎ���</returns>
    public float TimerStop()
    {
        _isStarted = false;
        return _timer;
    }

    public void TimerReset()
    {
        _timer = 0;
    }
    private void Start()
    {
        StateMachine.Instance.CurrentSceneType
            .Where(x => x == SceneType.InGame)
            .Subscribe(_ =>
            {
                TimerReset();
                TimerStart();
            }).AddTo(this);
    }
    private void Update()
    {
        if (_isStarted)
        {
            _timer += Time.deltaTime;
        }
    }

    #endregion
}
