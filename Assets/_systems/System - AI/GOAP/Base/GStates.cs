using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    // --- GOALS ---

    NotHungry = 0,
    NotExhausted = 1,
    NotEaten = 2,

    // --- CONDS ---

    IsInTargetRange = 100,
    HasTarget = 101,
}