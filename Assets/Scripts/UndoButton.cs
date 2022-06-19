using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UndoButton : MonoBehaviour
{
    public void undoComman()
    {
        EventSystem.instance.UndobuttonClicked();
    }
}
