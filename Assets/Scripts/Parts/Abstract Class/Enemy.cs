using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamage
{
    /// <summary>EnemyのHP</summary>
    [SerializeField]
    protected int _hp;

    public abstract void SendDamage(int damage);
    
    public enum EnemyState { blackBorad,book,chalk,enemy};

 

    public struct EnemyData
    {
        public EnemyState _enemyState;
        public Vector3 _enemyPosition;
        public Quaternion _enemyRotation;
    }

    public EnemyData _enemyData;

    public void Reset()
    {
        Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        if(GameObject.FindAnyObjectByType<EnemyInfoReset>() != null)
        GameObject.FindAnyObjectByType<EnemyInfoReset>().EnemyRemoved(this);
    }

}
