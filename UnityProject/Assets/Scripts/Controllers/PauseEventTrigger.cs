using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseEventTrigger : MonoBehaviour
{
    public void CallPauseEvent(bool pause)
    {
        EventManager.OnPauseGameTrigger(pause);
    }
}
