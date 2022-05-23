using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class AsteroidController : MonoBehaviour
{
    public float dx = 2f;
    public float dy = 2f;
    public float scale = 1f;
    public float size = 1;
    public Physics physics { get; set; }
    public bool isShard = false;
    private UnityEngine.Vector2 cachedSize;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        physics.onDestroyCallback += DestroyAsteroid;
        if (!isShard)
        {
            transform.localScale = new UnityEngine.Vector3(scale, scale, scale);
        }
        else
        {
            transform.localScale = new UnityEngine.Vector3(scale/2, scale/2, scale/2);
        }
        cachedSize = ScreenBounds.S.cachedScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        System.Numerics.Vector2 pos = physics.Moving(Time.deltaTime);
        transform.position = new UnityEngine.Vector2(pos.X, pos.Y);
    }

    void DestroyAsteroid()
    {
        Destroy(gameObject);
    }
}