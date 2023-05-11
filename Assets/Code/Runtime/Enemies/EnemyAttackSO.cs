using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackSO", menuName = "ScriptableObject/EnemyAttack")]
public class EnemyAttackSO : ScriptableObject
{
    public bool ranged = false;
    [Range(0, 359)] public float angleSpread;
    public float startingDistance = 0.1f;
    
    public int damage = 1;
    public float range = 0f; //todo - fire range
    public float rateOfFire = 120f;
    public float bulletForce = 5f;
    public GameObject projectile;
}
