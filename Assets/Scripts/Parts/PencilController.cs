// --------------------------------------------------------- 
// PencilController1.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using System.Collections;
public class PencilController : StationaryPart
{
    #region variable 

    [SerializeField]
    private ParticleSystem _breakEffect = default;
    [SerializeField]
    private int _attackDamage = 3;

    #endregion
    #region property
    #endregion
    #region method
 
    private void Start ()
    {
        OnStart();
    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.SendDamage(_attackDamage);
        }
    }
    #endregion
}