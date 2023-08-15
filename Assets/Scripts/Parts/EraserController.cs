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


        //���̏����͒��ۃN���XStationaryPart��OnDestroy�Ŏ�������Ă���̂Ŏq�N���X�ł��Ă�ł��܂��ƂQ�p�[�c���}�C�i�X����܂��B
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