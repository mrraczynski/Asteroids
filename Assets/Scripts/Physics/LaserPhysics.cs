using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;

public class LaserPhysics : Physics
{
    public LaserPhysics(float x, float y, float dx, float dy, float curAngle) : base(x, y, dx, dy)
    {
        pos = new Vector2(x, y);
        vel = new Vector2(dx, dy);
        currentAngle = curAngle;
        checkCollision = true;
    }

    public Vector2 Moving()
    {
        bool isEndOfScreen;
        vel.X += (float)Math.Sin(currentAngle * Math.PI / 180) * 10;
        vel.Y += (float)Math.Cos(currentAngle * Math.PI / 180) * 10;
        vel = GameController.ClampMagnitude(vel, 0.1f);
        do
        {
            isEndOfScreen = ScreenWrapping.EndOfScreenCheck(pos);
            vel = GameController.ClampMagnitude(vel, maxVelocity);
            pos.X += vel.X;
            pos.Y += vel.Y;
            GameController.CheckThreatCollistion(this);
        }
        while (!isEndOfScreen);
        return pos;
    }

}
