using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Physics physics;
    bool isSetAcceleration = false;
    System.Numerics.Vector2 pos;
    private UnityEngine.Vector2 cachedSize;

    public float velocity = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        cachedSize = ScreenBounds.S.cachedScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(physics != null && !isSetAcceleration)
        {
            isSetAcceleration = true;
            physics.onDestroyCallback += DestroyBullet;
        }
    }

    void FixedUpdate()
    {
        if (isSetAcceleration)
        {
            pos = physics.Moving(Time.deltaTime);
            transform.position = new UnityEngine.Vector2(pos.X, pos.Y);
        }
    }


    public void SetBulletPhysics(BulletPhysics bullet)
    {
        physics = bullet;
    }

    public float GetVelocity()
    {
        return velocity;
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
