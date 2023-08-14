// --------------------------------------------------------- 
// AudioResource.cs 
// 
// CreateDay: 2023/08/05
// Creator  : fuwa
// --------------------------------------------------------- 
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioResource", menuName = "ScriptableObjects/AudioResourceData")]
public class AudioResource : ScriptableObject
{
    #region variable

    [SerializeField] private List<KeyAndValue<AudioClipName, AudioClip>> _audioList;

    #endregion
    #region property

    #endregion
    #region method

    public bool TryGetValue(AudioClipName audioClipName, out AudioClip audioClip)
    {
        audioClip = _audioList.Find(x => x.Key == audioClipName).Value;
        return audioClip != null;
    }

    #endregion
}

[Serializable]
public class KeyAndValue<TKey, TValue>
{
    public TKey Key;
    public TValue Value;

    public KeyAndValue(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
    public KeyAndValue(KeyValuePair<TKey, TValue> pair)
    {
        Key = pair.Key;
        Value = pair.Value;
    }
}
