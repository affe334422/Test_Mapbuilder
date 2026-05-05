
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

public static class KeyboardHelper
{
    private static KeyboardState previous;
    private static KeyboardState current;
    private static bool AllkeysUp = true;
    private static List<Keys> Klist = new List<Keys>{Keys.Down,Keys.W,Keys.A,Keys.S,Keys.D,Keys.Right,Keys.Up,Keys.Delete,Keys.A,Keys.B,Keys.O,Keys.Left};
    public static void Update()
    {
        previous = current;
        current = Keyboard.GetState();
        foreach(Keys k in Klist)
        {
            AllkeysUp = true;
            if (previous.IsKeyDown(k))
            {
                AllkeysUp = false;
                break;
            }
        }
    }
    public static bool AllKeysUp()
    {
        return AllkeysUp;
    }
    public static bool HoldingKey(Keys k)
    {
        return current.IsKeyDown(k)&&previous.IsKeyDown(k);
    }
    public static bool IsKeyDown(Keys k)
    {
        return current.IsKeyDown(k);
    }
    public static bool IsKeyUp(Keys k)
    {
        return current.IsKeyUp(k);
    }

}
