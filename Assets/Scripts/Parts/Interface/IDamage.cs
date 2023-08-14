using UnityEngine;

interface IDamage
{
    /// <summary>ダメージを送る関数</summary>
    /// <param name="damage">与えるダメージ数</param>
    public void SendDamage(int damage);
}
