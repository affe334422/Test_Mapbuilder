using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public static class MouseHelper
{
    private static MouseState current;
    private static MouseState previous;
    private static Stopwatch MouseWatch = new Stopwatch();
    
    public static void Update()
    {
        previous = current;
        current = Mouse.GetState();
        if (isPressed())
        {
            if (!MouseWatch.IsRunning)
            {
                MouseWatch.Restart();
            }
        }
        if (isReleased())
        {
            MouseWatch.Reset();
        }
    }

    public static Stopwatch TimePressed()
    {
        return MouseWatch;
    }
    public static bool IsHovering(Rectangle rect)
    {
        return rect.Contains(current.Position);
    }

    public static bool LeftClick(Rectangle rect)
    {
        return  rect.Contains(current.Position) && 
                current.LeftButton == ButtonState.Pressed && 
                previous.LeftButton == ButtonState.Released;
    }
    public static bool Click()
    {
        return  current.LeftButton == ButtonState.Pressed && 
                previous.LeftButton == ButtonState.Released;
    }
    public static bool RightClick()
    {
        return  current.RightButton == ButtonState.Pressed &&
                previous.RightButton == ButtonState.Released;
    }

    public static bool isReleased()
    {
        return  current.LeftButton == ButtonState.Released &&
                previous.LeftButton == ButtonState.Released;
    }
    public static bool isPressed()
    {
        return  current.LeftButton == ButtonState.Pressed &&
                previous.LeftButton == ButtonState.Pressed;
    }
    public static bool IsHolding(Rectangle rec)
    {
        return  rec.Contains(current.Position) &&
                current.LeftButton == ButtonState.Pressed &&
                previous.LeftButton == ButtonState.Pressed;
    }
    public static Vector2 PreviousPosition()
    {
        return previous.Position.ToVector2();
    }
    public static Vector2 CurretPosition()
    {
        return current.Position.ToVector2();
        
    }
}
