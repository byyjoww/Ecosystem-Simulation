using System;
using UnityEngine;

public interface IUpdater
{
    event Action OnUpdateEvent;
    event Action OnFixedUpdateEvent;
}
