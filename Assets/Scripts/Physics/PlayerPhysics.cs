using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;

public class PlayerPhysics : Physics
{
    public delegate void OnBulletFired(BulletPhysics bullet, float x, float y);
    public event OnBulletFired onBulletFiredEvent;
    public int laserShots { get; private set; }
    public int laserRechargeTime { get; private set; }
    public int currentLaserShots { get; private set; }
    private bool isLaserRecharging;
    private TimeSpan laserRecharge;
    private DateTime rechargeStartTime;

    public PlayerPhysics(float x, float y, float dx, float dy, int laserSh = 3, int laserRchSec = 5) : base(x, y, dx, dy)
    {
        pos = new Vector2(x, y);
        vel = new Vector2(dx, dy);
        checkCollision = true;
        GameController.SetPlayer(this);
        laserShots = laserSh;
        currentLaserShots = laserShots;
        laserRecharge = new TimeSpan(0, 0, laserRchSec);
    }

    public void Fire(float velocity, float dTimeS, float bulletLifeTime)
    {
        Vector2 bulletVelocity = new Vector2();
        //bulletVelocity.X += (float)Math.Sin(currentAngle) * velocity;
        //bulletVelocity.Y += (float)Math.Cos(currentAngle) * velocity;
        bulletVelocity.X += (float)Math.Sin(currentAngle * Math.PI / 180) * velocity;
        bulletVelocity.Y += (float)Math.Cos(currentAngle * Math.PI / 180) * velocity;
        BulletPhysics bullet = new BulletPhysics(pos.X, pos.Y, bulletVelocity.X, bulletVelocity.Y, bulletLifeTime, currentAngle, velocity);
        onBulletFiredEvent(bullet, pos.X, pos.Y);
    }

    public Vector2 FireLaser()
    {
        Vector2 end;
        if (currentLaserShots > 0)
        {
            LaserPhysics laser = new LaserPhysics(pos.X, pos.Y, 0.0f, 0.0f, currentAngle);
            end = laser.Moving();
            currentLaserShots--;
            return end;
        }
        else
        {
            return new Vector2(pos.X, pos.Y);
        }
    }

    public override Vector2 Moving(float dTimeS, float acceleration = 0)
    {
        if(currentLaserShots < laserShots && !isLaserRecharging)
        {
            isLaserRecharging = true;
            rechargeStartTime = DateTime.Now;
        }
        if(isLaserRecharging && DateTime.Now - rechargeStartTime >= laserRecharge && currentLaserShots < laserShots)
        {
            currentLaserShots++;
            isLaserRecharging = false;
            laserRechargeTime = 0;
        }
        else if(isLaserRecharging && currentLaserShots < laserShots)
        {
            laserRechargeTime = laserRecharge.Seconds - (DateTime.Now - rechargeStartTime).Seconds;
            UnityEngine.Debug.Log(laserRechargeTime);
        }
        return base.Moving(dTimeS, acceleration);
    }

    public override void Destroy()
    {
        GameController.GameOver();
    }

    public void SetLaserShots(int laserSh)
    {
        laserShots = laserSh;
        currentLaserShots = laserShots;
    }
}
