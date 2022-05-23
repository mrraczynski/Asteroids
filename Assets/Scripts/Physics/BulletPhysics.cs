using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;

public class BulletPhysics : Physics
{
    private float lifeTime;
    private DateTime createTime;

    public BulletPhysics(float x, float y, float dx, float dy, float lTime, float curAngle, float maxVel) : base(x, y, dx, dy)
    {
        pos = new Vector2(x, y);
        vel = new Vector2(dx, dy);
        createTime = DateTime.Now;
        lifeTime = lTime;
        currentAngle = curAngle;
        maxVelocity = maxVel;
        checkCollision = true;
        GameController.AddBullet(this);
    }

    public override Vector2 Moving(float dTimeS, float acceleration = 0.0f)
    {
        if((DateTime.Now - createTime).TotalSeconds >= lifeTime)
        {
            GameController.RemoveBullet(this);
            onDestroyCallback();
        }
        vel.X += (float)Math.Sin(currentAngle * Math.PI / 180) * acceleration * dTimeS;
        vel.Y += (float)Math.Cos(currentAngle * Math.PI / 180) * acceleration * dTimeS;
        //vel.X += (float)Math.Sin(currentAngle) * acceleration * dTimeS;
        //vel.Y += (float)Math.Cos(currentAngle) * acceleration * dTimeS;
        vel = GameController.ClampMagnitude(vel, maxVelocity);
        pos.X += vel.X * dTimeS;
        pos.Y += vel.Y * dTimeS;
        pos = ScreenWrapping.Wrapping(pos);
        if(GameController.CheckThreatCollistion(this))
        {
            onDestroyCallback();
        }
        return pos;
    }

}
