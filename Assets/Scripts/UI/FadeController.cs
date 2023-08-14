// --------------------------------------------------------- 
// FadeController.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
using System.Threading.Tasks;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;

public class FadeController : MonoBehaviour
{
    [Serializable]
    public enum FadeType
    {
        FadeIn,
        FadeOut
    }

    #region variable 

    [SerializeField]
    private UnityEvent _fadeInCallback;
    [SerializeField]
    private UnityEvent _fadeOutCallback;

    private Animator _anim;

    private bool _fadeOutEnded = false;
    private bool _fadeInEnded = false;

    #endregion
    #region property

    public UnityEvent FadeInCallback
    {
        get => _fadeInCallback;
        set => _fadeInCallback = value;
    }

    public UnityEvent FadeIOutCallback
    {
        get => _fadeOutCallback;
        set => _fadeOutCallback = value;
    }

    #endregion
    #region method

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        _fadeInCallback.Invoke();
        _fadeInEnded = true;
    }

    public void FadeOut()
    {
        _fadeOutCallback.Invoke();
        _fadeOutEnded = true;
    }

    public async Task PlayFadeAsunc(FadeType type)
    {
        if (type == FadeType.FadeIn)
        {
            _anim.SetTrigger("FadeIn");

            await UniTask.WaitUntil(() => _fadeInEnded, cancellationToken: this.destroyCancellationToken);
            _fadeInEnded = false;
        }
        else
        {
            _anim.SetTrigger("FadeOut");

            await UniTask.WaitUntil(() => _fadeOutEnded, cancellationToken: this.destroyCancellationToken);
            _fadeOutEnded = false;
        }
    }

    public void PlayFade(FadeType type)
    {
        if (type == FadeType.FadeIn)
        {
            _anim.SetTrigger("FadeIn");
        }
        else
        {
            _anim.SetTrigger("FadeOut");
        }
    }
#endregion
}