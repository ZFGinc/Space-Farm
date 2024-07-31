using System;
using UnityEngine;

public interface IControllable
{
    event Action<bool> LockMove;

    void Move(Vector3 direction);
    void CursorMove(Vector3 direction);
    void Jump();
    void Sprint(bool sprint);
    void SwitchControlScheme(ControlScheme scheme);
    void LockMoving(float time);
}

