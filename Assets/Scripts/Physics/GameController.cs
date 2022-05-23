using System.Collections;
using System.Collections.Generic;
using System;
using System.Numerics;

public static class GameController
{
    private static string gameOverText;
    private static DateTime spawnWaitStart;
    private static TimeSpan spawnWait;
    private static bool isWaitingForSpawn;
    private static List<Physics> ThreatsList;
    private static List<Physics> BulletsList;
    public static PlayerPhysics player { get; private set; }
    public static bool isGameOver { get; private set; }
    public static string XCoorditaneText { get; private set; }
    public static string YCoorditaneText { get; private set; }
    public static string dXCoorditaneText { get; private set; }
    public static string dYCoorditaneText { get; private set; }
    public static string angleText { get; private set; }
    public static string laserShotsText { get; private set; }
    public static string laserRechargeTimeText { get; private set; }

    public delegate void OnGameOverEvent(string gameOverText);
    public static event OnGameOverEvent onGameOverEvent;

    public static void Awake()
    {
        isGameOver = false;
        gameOverText = "GAME OVER!\nPRESS SPACE TO RESTART";
        player = new PlayerPhysics(0.1f, 0.1f, 0, 0);
        player.onOnInstantiateCallback += AsteroidGame.S.PlayerSpawn;
        onGameOverEvent += AsteroidGame.S.GameOver;
        for (int i = 0; i < 1; i++)
        {
            Vector2 enemyPosition = GetThreatPosition(player);
            Vector2 enemyVelocity = ScreenWrapping.GetRundomScreenLoc();
            enemyVelocity = ClampMagnitude(enemyVelocity, 1);
            Physics enemy = new EnemyPhysics(enemyPosition.X, enemyPosition.Y, enemyVelocity.X, enemyVelocity.Y, 0.15f);
            enemy.onOnInstantiateCallback += AsteroidGame.S.EnemySpawn;
        }
        for (int i = 0; i < 5; i++)
        {
            Vector2 position = GetThreatPosition(player);
            Vector2 velocity = ScreenWrapping.GetRundomScreenLoc();
            velocity = ClampMagnitude(velocity, 1);
            Physics asteroid = new AsteroidPhysics(position.X, position.Y, velocity.X, velocity.Y, 0.3f);
            asteroid.onOnInstantiateCallback += AsteroidGame.S.AsteroidSpawn;
        }
    }

    public static void Start()
    {        
        foreach(Physics threat in ThreatsList)
        {
            if (threat.onOnInstantiateCallback != null)
            {
                threat.onOnInstantiateCallback(threat, threat.GetPosition());
            }
        }
        player.onOnInstantiateCallback(player, player.GetPosition());
    }

    public static void Update()
    {
        if (!isGameOver)
        {
            XCoorditaneText = "X: " + player.GetPosition().X.ToString("0.##");
            YCoorditaneText = "Y: " + player.GetPosition().Y.ToString("0.##");
            dXCoorditaneText = "dX: " + player.GetVelocity().X.ToString("0.##");
            dYCoorditaneText = "dY: " + player.GetVelocity().Y.ToString("0.##");
            angleText = "Angle: " + player.GetCurrentAngle().ToString("0.##");
            laserShotsText = "Laser Shots: " + player.currentLaserShots;
            laserRechargeTimeText = "Laser Recharge: " + (player.laserRechargeTime == 0 ? 0 : player.laserRechargeTime);
            if (ThreatsList.Count < 2 && !isWaitingForSpawn)
            {
                Random randTime = new Random();
                spawnWait = new TimeSpan(0, 0, randTime.Next(1, 3));
                spawnWaitStart = DateTime.Now;
                isWaitingForSpawn = true;
            }
            if (isWaitingForSpawn && DateTime.Now >= spawnWaitStart + spawnWait)
            {
                Random randEnemyChance = new Random(Guid.NewGuid().GetHashCode());
                if (randEnemyChance.Next(1, 4) > 1)
                {
                    Random randThreatCount = new Random(Guid.NewGuid().GetHashCode());
                    int count = randThreatCount.Next(1, 4);
                    for (int i = 0; i < count; i++)
                    {
                        Vector2 position = GetThreatPosition(player);
                        Vector2 velocity = ScreenWrapping.GetRundomScreenLoc();
                        velocity = ClampMagnitude(velocity, 1);
                        Physics asteroid = new AsteroidPhysics(position.X, position.Y, velocity.X, velocity.Y, 0.3f);
                        asteroid.onOnInstantiateCallback += AsteroidGame.S.AsteroidSpawn;
                        asteroid.onOnInstantiateCallback(asteroid, asteroid.GetPosition());
                    }
                }
                else
                {
                    for (int i = 0; i < 1; i++)
                    {
                        Vector2 enemyPosition = GetThreatPosition(player);
                        Vector2 enemyVelocity = ScreenWrapping.GetRundomScreenLoc();
                        enemyVelocity = ClampMagnitude(enemyVelocity, 1);
                        Physics enemy = new EnemyPhysics(enemyPosition.X, enemyPosition.Y, enemyVelocity.X, enemyVelocity.Y, 0.15f);
                        enemy.onOnInstantiateCallback += AsteroidGame.S.EnemySpawn;
                        enemy.onOnInstantiateCallback(enemy, enemy.GetPosition());
                    }
                    Random randThreatCount = new Random(Guid.NewGuid().GetHashCode());
                    int count = randThreatCount.Next(1, 3);
                    for (int i = 0; i < count; i++)
                    {
                        Vector2 position = GetThreatPosition(player);
                        Vector2 velocity = ScreenWrapping.GetRundomScreenLoc();
                        velocity = ClampMagnitude(velocity, 1);
                        Physics asteroid = new AsteroidPhysics(position.X, position.Y, velocity.X, velocity.Y, 0.3f);
                        asteroid.onOnInstantiateCallback += AsteroidGame.S.AsteroidSpawn;
                        asteroid.onOnInstantiateCallback(asteroid, asteroid.GetPosition());
                    }
                }
                isWaitingForSpawn = false;
            }
        }
    }

    static GameController()
    {
        ThreatsList = new List<Physics>();
        BulletsList = new List<Physics>();
    }

    public static Vector2 GetPlayerCoordinates()
    {
        return player.GetPosition();
    }

    public static void SetPlayer(PlayerPhysics pl)
    {
        if (pl.GetType() == typeof(PlayerPhysics))
        {
            player = pl;
        }
    }

    public static void AddThreat(Physics threat)
    {
        if (threat != null)
        {
            ThreatsList.Add(threat);
        }
    }

    public static void AddBullet(BulletPhysics bullet)
    {
        if (bullet != null)
        {
            BulletsList.Add(bullet);
        }
    }

    public static bool CheckThreatCollistion(Physics bullet)
    {
        foreach (Physics threat in ThreatsList)
        {
            float threatSize = threat.GetSize();
            if (Vector2.DistanceSquared(threat.GetPosition(), bullet.GetPosition()) <= threatSize)
            {
                if (ThreatsList.Remove(threat))
                {
                    if (bullet.GetType() == typeof(PlayerPhysics))
                    {
                        bullet.Destroy();
                    }
                    else
                    {
                        RemoveBullet(bullet);
                    }
                    threat.Destroy();
                    return true;
                }
            }
        }
        return false;
    }

    public static bool CheckThreat(Physics threat)
    {
        return ThreatsList.Contains(threat);
    }

    public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)
    {
        float sqrmag = vector.LengthSquared();
        if (sqrmag > maxLength * maxLength)
        {
            float mag = (float)Math.Sqrt(sqrmag);
            float normalizedX = vector.X / mag;
            float normalizedY = vector.Y / mag;
            return new Vector2(normalizedX * maxLength,
                normalizedY * maxLength);
        }
        return vector;
    }

    public static void AddShard(AsteroidPhysics shard)
    {
        AddThreat(shard);
        shard.onOnInstantiateCallback += AsteroidGame.S.AsteroidSpawn;
        shard.onOnInstantiateCallback(shard, shard.GetPosition());
    }

    public static void RemoveBullet(Physics bullet)
    {
        if (bullet.GetType() == typeof(BulletPhysics))
        {
            BulletsList.Remove(bullet);
        }
    }

    public static void GameOver()
    {
        foreach (Physics threat in ThreatsList)
        {
            if (threat.onDestroyCallback != null)
            {
                threat.onDestroyCallback();
            }
        }
        ThreatsList.Clear();
        foreach (Physics bullet in BulletsList)
        {
            if (bullet.onDestroyCallback != null)
            {
                bullet.onDestroyCallback();
            }
        }
        BulletsList.Clear();
        if (player.onDestroyCallback != null)
        {
            player.onDestroyCallback(   );
        }
        onGameOverEvent(gameOverText);
        isGameOver = true;
    }

    private static Vector2 GetThreatPosition(Physics obj)
    {
        Vector2 position;
        do
        {
            position = ScreenWrapping.GetRundomScreenLoc();
        } while (Vector2.DistanceSquared(obj.GetPosition(), position) < 2);
        return position;
    }
}

