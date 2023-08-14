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
    [Tooltip("�e��ł�")]
    public int bulletQuantity = default;
    [Tooltip("���˃��[�g")]
    public float bulletInterval = default;
    [Tooltip("�N�[���^�C��")]
    public float interval = default;
    [Tooltip("�e")]
    public GameObject bullet = default;
}