// --------------------------------------------------------- 
// AudioSettings.cs 
// 
// CreateDay: 2023/08/05
// Creator  : fuwa
// --------------------------------------------------------- 
using UnityEngine;
using UniRx;
public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioSettingsView _audioSettingsView;
    private void Start()
    {
        _audioSettingsView.OnChangeBgmVolumeAsObservable().Subscribe(volume => AudioManager.Instance.SetBgmVolume(volume));
        _audioSettingsView.OnChangeSeVolumeAsObservable().Subscribe(volume => AudioManager.Instance.SetSeVolume(volume));
        _audioSettingsView.OnPointerUpSeVolumeAsObservable().Subscribe(_ => AudioManager.Instance.PlaySe(AudioClipName.ButtonClick));

        AudioManager.Instance.BgmVolume.Subscribe(volume => _audioSettingsView.SetBgmVolume(volume));
        AudioManager.Instance.SeVolume.Subscribe(volume => _audioSettingsView.SetSeVolume(volume));
    }
}
