// --------------------------------------------------------- 
// AudioSettingsView.cs 
// 
// CreateDay: 2023/08/05
// Creator  : fuwa
// --------------------------------------------------------- 
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsView : MonoBehaviour
{
    [SerializeField] private Slider _bgmVolumeSlider;

    [SerializeField] private Slider _seVolumeSlider;

    public IObservable<float> OnChangeBgmVolumeAsObservable() => _bgmVolumeSlider.OnValueChangedAsObservable().Skip(1);

    public IObservable<float> OnChangeSeVolumeAsObservable() => _seVolumeSlider.OnValueChangedAsObservable().Skip(1);

    public IObservable<Unit> OnPointerUpSeVolumeAsObservable() => _seVolumeSlider.OnPointerUpAsObservable().AsUnitObservable();

    public void SetBgmVolume(float volume)
    {
        _bgmVolumeSlider.value = volume;
    }

    public void SetSeVolume(float volume)
    {
        _seVolumeSlider.value = volume;
    }
}
