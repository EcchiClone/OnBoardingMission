using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager
{
    public event Action OnStartGame;
    public event Action OnEnemyGen;
    public event Action OnEndGame;
    public void InvokeOnStartGame() { OnStartGame?.Invoke(); }
    public void InvokeOnEnemyGen() { OnEnemyGen?.Invoke(); }
    public void InvokeOnEndGame() { OnEndGame?.Invoke(); }
}
