using System;

public static class ActionUtility
{
    public static void SafeCall(this Action action)
    {
        action?.Invoke();
    }
}
