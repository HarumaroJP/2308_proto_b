// --------------------------------------------------------- 
// EraserController.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using System.Collections;
public class EraserController : StationaryPart
{
    #region variable 

    [SerializeField]
    private ParticleSystem _breakEffect = default;

    #endregion
    #region property
    #endregion
    #region method

    public override void Action()
    {
    }

    public override void Break()
    {
        if (_breakEffect)
        {
            var temp = Instantiate(_breakEffect);
            temp.transform.position = this.transform.position;
        }


        //この処理は抽象クラスStationaryPartのOnDestroyで実装されているので子クラスでも呼んでしまうと２パーツ分マイナスされます。
        //_rate.DecrimentPlayerParts();

        Destroy(this.gameObject);
    }

    public override void OnStart()
    {
    }

    public override void SendDamage(int damage)
    {
        _hp -= damage;

        if (_hp <= 0)
        {
            Break();
        }
    }
    #endregion
}