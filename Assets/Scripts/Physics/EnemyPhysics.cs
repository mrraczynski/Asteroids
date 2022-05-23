using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;

public class EnemyPhysics : Physics
{

    public EnemyPhysics(float x, float y, float dx, float dy, float sz) : base(x, y, dx, dy)
    {
        pos = new Vector2(x, y);
        vel = new Vector2(dx, dy);
        size = sz;
        checkCollision = false;
        GameController.AddThreat(this);
    }

    public override Vector2 Moving(float dTimeS, float acceleration = 0.0f)
    {
        Vector2 playerPos = GameController.GetPlayerCoordinates();
        vel = new Vector2(playerPos.X - pos.X, playerPos.Y - pos.Y);
        vel = GameController.ClampMagnitude(vel, acceleration);
        pos.X += vel.X * dTimeS;
        pos.Y += vel.Y * dTimeS;
        pos = ScreenWrapping.Wrapping(pos);
        if (checkCollision)
        {
            GameController.CheckThreatCollistion(this);
        }
        return pos;
    }

}