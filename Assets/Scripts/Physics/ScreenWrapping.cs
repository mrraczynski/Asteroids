using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;

public static class ScreenWrapping
{
    public static Vector2 screenSize { get; set; }
    private static double cachedRand = double.MaxValue;

    public static Vector2 Wrapping(Vector2 pos)
    {
        Vector2 newPos = new Vector2();
        if (pos.X > screenSize.X / 2)
        {
            newPos.X = -(screenSize.X / 2);
            newPos.Y = pos.Y;
            return newPos;
        }
        if (pos.X < -(screenSize.X / 2))
        {
            newPos.X = screenSize.X / 2;
            newPos.Y = pos.Y;
            return newPos;
        }
        if (pos.Y > screenSize.Y / 2)
        {
            newPos.Y = -(screenSize.Y / 2);
            newPos.X = pos.X;
            return newPos;
        }
        if (pos.Y < -(screenSize.Y / 2))
        {
            newPos.Y = screenSize.Y / 2;
            newPos.X = pos.X;
            return newPos;
        }
        return pos;
    }

    public static bool EndOfScreenCheck(Vector2 pos)
    {
        if (pos.X > screenSize.X / 2)
        {
            return true;
        }
        if (pos.X < -(screenSize.X / 2))
        {
            return true;
        }
        if (pos.Y > screenSize.Y / 2)
        {
            return true;
        }
        if (pos.Y < -(screenSize.Y / 2))
        {
            return true;
        }
        return false;
    }

    public static Vector2 GetRundomScreenLoc()
    {
        Random random = new Random(Guid.NewGuid().GetHashCode());
        double randX = (random.NextDouble() * (screenSize.X / 2 + (screenSize.X / 2)) -(screenSize.X / 2));
        double randY = (random.NextDouble() * (screenSize.Y / 2 + (screenSize.Y / 2)) -(screenSize.Y / 2));
        return new Vector2((float)randX, (float)randY);
    }
}
