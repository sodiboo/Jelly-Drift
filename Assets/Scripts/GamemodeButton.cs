using UnityEngine;

public class GamemodeButton : MonoBehaviour
{
    public Gamemode gamemode;

    public void SetGamemode() => GameState.Instance.gamemode = gamemode;
}
