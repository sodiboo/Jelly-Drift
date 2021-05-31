using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeButton : MonoBehaviour
{
    public Gamemode gamemode;
    
    public void SetGamemode()
    {
        GameState.Instance.gamemode = gamemode;
    }
}
