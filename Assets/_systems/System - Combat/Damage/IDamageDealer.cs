using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTeam { PLAYER = 0, ENEMY = 2}
public interface IDamageDealer
{
    int Damage { get; }
    List<DamageTeam> DealsDamageTo { get; }
}
