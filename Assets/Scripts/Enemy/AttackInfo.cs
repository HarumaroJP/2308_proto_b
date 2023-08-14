// --------------------------------------------------------- 
// AttackInfo.cs 
// 
// CreateDay: 
// Creator  : Nomura
// --------------------------------------------------------- 
using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "AttackInfo")]
public class AttackInfo : ScriptableObject
{
    [Tooltip("弾を打つ回数")]
    public int bulletQuantity = default;
    [Tooltip("発射レート")]
    public float bulletInterval = default;
    [Tooltip("クールタイム")]
    public float interval = default;
    [Tooltip("弾")]
    public GameObject bullet = default;
}