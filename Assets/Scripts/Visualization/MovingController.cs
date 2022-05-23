using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingController : MonoBehaviour
{
    private float hor;
    bool isUp = false;
    bool isTurnLeft = false;
    bool isTurnRight = false;
    Physics physics;
    private UnityEngine.Vector2 cachedSize;
    private float ang;
    private LineRenderer line;
    private double laserStartTime;

    public float accelerationSpeed = 0.01f;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10.0f;
    public float bulletLifeTime = 5.0f;

    private void Awake()
    {
        physics = GameController.player;
    }

    // Start is called before the first frame update
    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        cachedSize = ScreenBounds.S.cachedScale;
        ((PlayerPhysics)physics).onBulletFiredEvent += CreatingBullet;
        physics.onDestroyCallback += DestroyPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        isUp = Input.GetKey("up");
        isTurnRight = Input.GetKey("right");
        isTurnLeft = Input.GetKey("left");
        if(Input.GetKeyDown("space"))
        {
            ((PlayerPhysics)physics).Fire(bulletSpeed, Time.deltaTime, bulletLifeTime);
        }
        if (Input.GetKeyDown("left ctrl"))
        {
            System.Numerics.Vector2 endPoint = ((PlayerPhysics)physics).FireLaser();
            line.SetPositions(new Vector3[] { gameObject.transform.position, new Vector3(endPoint.X, endPoint.Y, gameObject.transform.position.z) });
            laserStartTime = Time.timeAsDouble;
        }
        if(Time.timeAsDouble - laserStartTime >= 0.1)
        {
            line.SetPositions(new Vector3[] { gameObject.transform.position, gameObject.transform.position });
        }
    }

    private void FixedUpdate()
    {
        System.Numerics.Vector2 pos;
        if (isUp)
        {
            pos = physics.Moving(Time.deltaTime, 3);
        }
        else
        {
            pos = physics.Moving(Time.deltaTime);
        }
        if(isTurnLeft)
        {
            ang = physics.AdjustAngle(Time.deltaTime, -5.0f);
            //Debug.Log(ang);
            //Debug.Log(transform.rotation.eulerAngles);
            transform.Rotate(new Vector3(0.0f, 0.0f, 5.0f));
            //transform.rotation = Quaternion.AngleAxis(ang, Vector3.forward);
        }
        else if (isTurnRight)
        {
            ang = physics.AdjustAngle(Time.deltaTime, 5.0f);
            //Debug.Log(ang);
            //Debug.Log(transform.rotation.eulerAngles);
            transform.Rotate(new Vector3(0.0f, 0.0f, -5.0f));
            //transform.rotation = Quaternion.AngleAxis(ang, Vector3.back);
        }
        transform.position = new UnityEngine.Vector2(pos.X, pos.Y);
    }

    public void CreatingBullet(BulletPhysics bullet, float x, float y)
    {
        Debug.Log(new Vector3(x, y, 0));
        GameObject bul = Instantiate(bulletPrefab, new Vector3(x, y, 0), transform.rotation);
        bul.GetComponent<BulletController>().SetBulletPhysics(bullet);
    }

    void DestroyPlayer()
    {
        Destroy(gameObject);
    }
}
