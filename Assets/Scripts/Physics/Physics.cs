using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;

public class Physics
{
    private float currentScreenAngle;

    protected float size;
    protected Vector2 pos;
    public Vector2 vel;
    protected float maxVelocity;
    protected float currentAngle;
    protected bool checkCollision;

    public delegate void OnDestroy();
    public OnDestroy onDestroyCallback;

    public delegate void OnInstantiate(Physics obj, Vector2 position);
    public OnInstantiate onOnInstantiateCallback;

    public virtual Vector2 Moving(float dTimeS, float acceleration = 0.0f)
    {
        vel.X += (float)Math.Sin(currentAngle * Math.PI / 180) * acceleration * dTimeS;
        vel.Y += (float)Math.Cos(currentAngle * Math.PI / 180) * acceleration * dTimeS;
        vel = GameController.ClampMagnitude(vel, maxVelocity);
        pos.X += vel.X * dTimeS;
        pos.Y += vel.Y * dTimeS;
        pos = ScreenWrapping.Wrapping(pos);
        if (checkCollision)
        {
            GameController.CheckThreatCollistion(this);
        }
        return pos;
    }

    public Physics(float x, float y, float dx, float dy, float mv = 3f, float cAngle = 0.0f)
    {
        pos = new Vector2(x, y);
        vel = new Vector2(dx, dy);
        maxVelocity = mv;
        currentAngle = cAngle;
        currentScreenAngle = cAngle;
        if (onOnInstantiateCallback != null)
        {
            onOnInstantiateCallback(this, pos);
        }
    }

    public float AdjustAngle (float dTimeS, float angle = 0.0f)
    {
        currentAngle += angle;
        currentScreenAngle += angle;
        if(currentScreenAngle > 360)
        {
            currentScreenAngle = 0 + angle;
        }
        else if(currentScreenAngle < 0)
        {
            currentScreenAngle = 360 - Math.Abs(angle);
        }
        return currentAngle;
    }

    public Vector2 GetPosition()
    {
        return new Vector2(pos.X, pos.Y);
    }

    public Vector2 GetVelocity()
    {
        return new Vector2(vel.X, vel.Y); ;
    }

    public float GetCurrentAngle()
    {
        return currentScreenAngle;
    }

    public virtual void Destroy()
    {
        if (onDestroyCallback != null)
        {
            onDestroyCallback();
        }
    }

    public float GetSize()
    {
        return size;
    }
}