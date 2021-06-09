using UnityEngine;
using UnityEngine.Events;

public class BackGamemode : MonoBehaviour
{
    public UnityEvent race;
    public UnityEvent timeTrial;
    public UnityEvent chaos;

    private void Start()
    {
        race = race ?? new UnityEvent();
        timeTrial = timeTrial ?? new UnityEvent();
        chaos = chaos ?? new UnityEvent();
    }

    public void Back()
    {
        switch (GameState.Instance.gamemode)
        {
            case Gamemode.Race: race.Invoke(); break;
            case Gamemode.TimeTrial: timeTrial.Invoke(); break;
            case Gamemode.Chaos: chaos.Invoke(); break;
        }
    }
}
