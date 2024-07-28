using UnityEngine;

public interface IControllable
{
    void Move(Vector3 direction);
    void CursorMove(Vector3 direction);
    void Jump();
    void Sprint(bool sprint);
    void SwitchControlScheme(ControlScheme scheme);
}

