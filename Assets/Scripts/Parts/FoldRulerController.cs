// --------------------------------------------------------- 
// FoldRulerController.cs 
// 
// CreateDay: 23/08/04
// Creator  : Ryuto Ohmori
// --------------------------------------------------------- 

using System;
using UnityEngine;
using System.Collections;

public class FoldRulerController : StationaryPart
{
    #region variable

    [SerializeField] ParticleSystem _breakEffect = default;
    [SerializeField] HingeJoint2D _hinge = default;

    #endregion

    #region property

    #endregion

    #region method

    //Startにすると正常に動かない
    private void Awake()
    {
        SetEnable(false);
    }

    public override void Action()
    {
        var temp = _hinge.motor;
        temp.motorSpeed *= -1;
        _hinge.motor = temp;
    }

    public override void Break()
    {
        if (_breakEffect)
        {
            var temp = Instantiate(_breakEffect);
            temp.transform.position = this.transform.position;
        }

        Destroy(this.gameObject);
    }

    public override void OnStart()
    {
        SetEnable(true);
    }

    void SetEnable(bool enabled)
    {
        JointAngleLimits2D jointAngleLimits2D = _hinge.limits;
        jointAngleLimits2D.max = enabled ? 180f : 0f;
        jointAngleLimits2D.min = 0f;
        _hinge.limits = jointAngleLimits2D;
    }

    public override void SendDamage(int damage)
    {
        _hp -= damage;

        if (damage <= 0)
        {
            Break();
        }
    }

    #endregion
}