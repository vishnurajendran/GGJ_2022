using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimEvent : MonoBehaviour
{
    public BattleAnimEventComplete OnBeginAnimComplete;
    public BattleAnimEventComplete OnWinAnimComplete;
    public BattleAnimEventComplete OnLoseAnimComplete;

    public void WinAnimCompleted()
    {
        OnWinAnimComplete?.Invoke();
    }

    public void LoseAnimCompleted()
    {
        OnLoseAnimComplete?.Invoke();
    }

    public void BeginAnimCompleted()
    {
        OnBeginAnimComplete?.Invoke();
    }
}
