using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    DamageTeam Team { get; }
    void TakeDamage(IDamageDealer damageComponent);
}