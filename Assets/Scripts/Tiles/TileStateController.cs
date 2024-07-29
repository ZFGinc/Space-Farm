using System;
using UnityEngine;

public class TileStateController : MonoBehaviour
{
    public enum State
    {
        NULL,    
        Grass,
        Bald,
        Plow,
        WateredPlow,
        IsGrowing,
        IsGrowed
    }
    public enum Event
    {
        ExitNull,
        Cut,
        Plowed,
        Watering,
        Seed,
        FullyGrow,
        Collect
    }

    public FiniteStateMachine<State,Event> fsm = new FiniteStateMachine<State,Event>(State.NULL);
    public event Action Default, ArableLand;
    public event Action<bool> Grass;

    [SerializeField, Range(0f,1f)] private float _targetPerlin = 0.7f;

    private Vector2 _coordinates;

    private void Start()
    {
        _coordinates = new Vector2(transform.position.x, transform.position.z);

        InitializeStateMashite();
    }

    private void InitializeStateMashite()
    {
        fsm.AddTransition(State.Grass, Event.Cut, State.Bald);
        fsm.AddTransition(State.Bald, Event.Plowed, State.Plow);
        fsm.AddTransition(State.Plow, Event.Watering, State.WateredPlow);
        fsm.AddTransition(State.WateredPlow, Event.Seed, State.IsGrowing);
        fsm.AddTransition(State.IsGrowing, Event.FullyGrow, State.IsGrowed);
        fsm.AddTransition(State.IsGrowed, Event.Collect, State.Plow);
    }

    public void GenerateGrass()
    {
        Default?.Invoke();

        if (UnityEngine.Random.Range(0f,1f) >= _targetPerlin)
        {
            fsm.AddTransition(State.NULL,Event.ExitNull, State.Grass);
            Grass?.Invoke(true);
            return;
        }

        fsm.AddTransition(State.NULL,Event.ExitNull, State.Bald);
    }
}
