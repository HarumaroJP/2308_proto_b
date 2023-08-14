// --------------------------------------------------------- 
// AudioManager.cs 
// 
// CreateDay: 2023/08/05
// Creator  : fuwa
// ---------------------------------------------------------
using System;
using UnityEngine;
using UniRx;
public class AudioManager : Singleton<AudioManager>
{
    #region variable

    [SerializeField] private AudioResource _audioResource;
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _seSource;
    private readonly ReactiveProperty<float> _bgmVolume = new();
    private readonly ReactiveProperty<float> _seVolume = new();

    public IReadOnlyReactiveProperty<float> BgmVolume => _bgmVolume;
    public IReadOnlyReactiveProperty<float> SeVolume => _seVolume;

    #endregion

    #region method

    private void Start()
    {
        _bgmVolume.Subscribe(volume => _bgmSource.volume = volume);
        _seVolume.Subscribe(volume => _seSource.volume = volume);
        _bgmVolume.Value = 0.2f;
        _seVolume.Value = 0.3f;
        PlayBgm(AudioClipName.TitleBgm);
    }
    public void SetBgmVolume(float volume)
    {
        _bgmVolume.Value = volume;
    }

    public void SetSeVolume(float volume)
    {
        _seVolume.Value = volume;
    }

    public void PlayBgm(AudioClipName audioClipName)
    {
        if (!_audioResource.TryGetValue(audioClipName, out var clip))
        {
            Debug.LogError($"{audioClipName} is not found");
            return;
        }
        _bgmSource.clip = clip;
        _bgmSource.loop = true;
        _bgmSource.Play();
    }

    public void PlaySe(AudioClipName audioClipName)
    {
        if (!_audioResource.TryGetValue(audioClipName, out var clip))
        {
            Debug.LogError($"{audioClipName} is not found");
            return;
        }
        _seSource.loop = false;
        _seSource.PlayOneShot(clip);
    }

    #endregion
}
