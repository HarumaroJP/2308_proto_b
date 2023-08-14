// --------------------------------------------------------- 
// Chalk.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using System.Collections;
public class Chalk : BlackBord
{
    protected override int CollisionDamage(int damage)
    {
        return damage;
    }
    private void Awake()
    {
        _enemyData._enemyPosition = this.transform.position;
        _enemyData._enemyRotation = this.transform.rotation;
        _enemyData._enemyState = EnemyState.chalk;
    }

}