using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private UnityEngine.Vector2 cachedSize;
    public Physics physics { get; set; }
    public float size = 1;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        physics.onDestroyCallback += DestroyEnemy;
        cachedSize = ScreenBounds.S.cachedScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        System.Numerics.Vector2 mov = physics.Moving(Time.deltaTime, 1);
        transform.position = new Vector2(mov.X, mov.Y);
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
