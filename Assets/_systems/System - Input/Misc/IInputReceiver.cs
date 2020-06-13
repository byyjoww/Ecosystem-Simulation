using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputReceiver
{
    void ReceiveSwipe(Vector2 StartPosition, Vector2 Delta, bool isValidated);
    void ReceiveDoubleSwipe(Vector2 StartPosition, Vector2 Delta, bool isValidated);
    Vector2 GetPosition();
}
