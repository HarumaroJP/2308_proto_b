// --------------------------------------------------------- 
// EnemyInfo.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    public List<EnemySetting> enemySettings;
}

[System.Serializable]
public class EnemySetting
{
    [Tooltip("“G‚â”z’u•¨")]
    public GameObject EnemyObject;
    [Tooltip("Enum")]
    public Enemy.EnemyState EnemyState;
}