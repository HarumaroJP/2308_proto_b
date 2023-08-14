// --------------------------------------------------------- 
// MissionShow.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 

using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
public class MissionShow : MonoBehaviour
{
    [SerializeField] private GameObject _mission;
    [SerializeField] private TextMeshProUGUI _missionText;

    public async UniTask ShowMission()
    {
        _mission.SetActive(true);
        _missionText.text = ProgressManager.Instance.CurrentStage.SceneName;
        AudioManager.Instance.PlaySe(AudioClipName.ShowTarget);
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
        _mission.SetActive(false);
    }
}
