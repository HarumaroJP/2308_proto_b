using Part;
using System;
using UnityEngine;

public abstract class StationaryPart : MonoBehaviour, IDamage
{
    [SerializeField]
    /// <summary>PartのHP</summary>
    protected int _hp;

    /// <summary>最大HP</summary>
    protected int _maxHp;

    /// <summary>このパーツのアクションを呼び出すButtonの番号</summary>
    [SerializeField]
    protected int _actionButton;

    protected DestructionRateController _rate;

    public Action OnDestroyCallback;

    //Property
    /// <summary>PartのHP</summary>
    public int HP
    {
        get => _hp;
        set => _hp = value;
    }

    /// <summary>最大HP</summary>
    public int MaxHP => _maxHp;

    /// <summary>このパーツのアクションを呼び出すButtonの番号</summary>
    public int ActionButton
    {
        get => _actionButton;
        set => _actionButton = value;
    }

    public KeyCode KeyActionButton
    {
        get
        {
            if (_actionButton == 0)
            {
                return KeyCode.LeftArrow;
            }
            else
            {
                return KeyCode.RightArrow;
            }
        }
    }


    public DestructionRateController DestructionRateController
    {
        set => _rate = value;
    }


    /// <summary>ゲーム開始時の処理</summary>
    public virtual void OnStart()
    {
        _maxHp = _hp;
    }

    /// <summary>破壊時の処理</summary>
    public abstract void Break();

    /// <summary>アクションが押された際の処理</summary>
    public abstract void Action();

    public abstract void SendDamage(int damage);

    private void OnDestroy()
    {
        OnDestroyCallback?.Invoke();
    }
}