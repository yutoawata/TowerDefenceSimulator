using UnityEngine;

public class DebugScript
{
    static public void DrawLog(string text)
    {
#if UNITY_EDITOR
        Debug.Log(text);
#endif
    }
}
