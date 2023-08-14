// --------------------------------------------------------- 
// RocketPencilController.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 

using UnityEngine;
using System.Collections;

public class RocketPencilController : StationaryPart
{
    #region variable

    [SerializeField] private int _bulletCount = default;
    [SerializeField] private BulletController _bullet = default;
    [SerializeField] private SpriteRenderer[] _renderer;
    [SerializeField] private Transform _muzzle = default;
    [SerializeField] private ParticleSystem _breakEffect = default;

    #endregion

    #region property

    #endregion

    #region method

    private void Start()
    {
        OnStart();
    }

    public override void Action()
    {
        if (_bulletCount > 0)
        {
            var temp = Instantiate(_bullet);
            temp.transform.position = _muzzle.position;
            temp.Initialize(transform.right, ActionButton);
            _bulletCount--;
            _renderer[_bulletCount].enabled = false;
        }
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

    public override void OnStart() { }

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