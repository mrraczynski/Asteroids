using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;

public class AsteroidPhysics : Physics
{
    public bool isShard { get; private set; } = false;
    private bool isSetToDestroy = false;
    public AsteroidPhysics(float x, float y, float dx, float dy, float sz, bool isSh = false) : base(x, y, dx, dy)
    {        
        pos = new Vector2(x, y);
        vel = new Vector2(dx, dy);
        size = sz;
        checkCollision = false;
        isShard = isSh;
        if (isShard)
        {
            GameController.AddShard(this);
        }
        else
        {
            GameController.AddThreat(this);
        }
    }

    public override Vector2 Moving(float dTimeS, float acceleration = 0)
    {
        if (isSetToDestroy && onDestroyCallback != null)
        {
            if (!isShard && !GameController.isGameOver)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 velocity = ScreenWrapping.GetRundomScreenLoc();
                    velocity = GameController.ClampMagnitude(velocity, 2);
                    new AsteroidPhysics(pos.X, pos.Y, velocity.X, velocity.Y, size / 2, true);
                }
            }
            onDestroyCallback();
        }
        return base.Moving(dTimeS, acceleration);
    }

    public override void Destroy()
    {
        isSetToDestroy = true;
    }

}