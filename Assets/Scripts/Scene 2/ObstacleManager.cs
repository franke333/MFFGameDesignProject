using UnityEngine;
using UnityEngine.Events;

public class ObstacleManager : MonoBehaviour
{
    private const float endTime = 60.0f;
    private const float lastSpawnTime = 55.00f;

    private int obstacleCount = 0;
    private Obstacle[] obstacles = new Obstacle[3];
    private int carIndex = 1;
    private float accumulator = 0.0f;
    private bool end = false;

    public GameObject ObstacleLeftPrefab;
    public GameObject ObstacleMiddlePrefab;
    public GameObject ObstacleRightPrefab;

    public EndSceneDramaticScript EndSceneScript;
    public UnityEvent OnGameOver;

    void Update()
    {
        if (end)
            return;

        accumulator += Time.deltaTime;

        if (accumulator >= endTime)
        {
            SpawnObstacle(true);
            end = true;
            return;
        }

        if (accumulator < lastSpawnTime && Random.value < 0.001f)
            SpawnObstacle();
    }

    private void SpawnObstacle(bool end = false)
    {
        if (end)
        {
            for (int i = 0; i < obstacles.Length; i++)
            {
                SpawnObstacle(i);
            }

            UpdateVisibility(carIndex);
            return;
        }

        if (obstacleCount == 2)
            return;

        while (true)
        {
            int index = Random.Range(0, obstacles.Length);

            if (obstacles[index] != null)
                continue;
            
            SpawnObstacle(index);
            break;
        }

        UpdateVisibility(carIndex);
    }

    private void SpawnObstacle(int index)
    {
        Obstacle obstacle = new Obstacle();
        obstacle.Objects[0] = Instantiate(ObstacleLeftPrefab);
        obstacle.Objects[1] = Instantiate(ObstacleMiddlePrefab);
        obstacle.Objects[2] = Instantiate(ObstacleRightPrefab);

        obstacles[index] = obstacle;
        obstacleCount++;

        obstacle.Objects[0].GetComponent<ObstacleController>().OnMoveEnd += (sender, e) =>
        {
            if (obstacles[index] == null)
                return;

            Destroy(obstacle.Objects[0]);
            Destroy(obstacle.Objects[1]);
            Destroy(obstacle.Objects[2]);
            obstacles[index] = null;
            obstacleCount--;

            if (index == 0)
                index = 2;
            else if (index == 2)
                index = 0;

            if (carIndex == index)
            {
                OnGameOver?.Invoke();
                EndSceneScript.Run();
            }
        };
    }

    public void UpdateVisibility(int index)
    {
        carIndex = index;

        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i]?.UpdateVisibility(i + (index - 1));
        }
    }

    class Obstacle
    {
        // Left, Middle, Right
        public GameObject[] Objects = new GameObject[3];

        public void UpdateVisibility(int line)
        {
            for (int i = 0; i < Objects.Length; i++)
            {
                Objects[i].GetComponentInChildren<SpriteRenderer>().enabled = i == line;
            }
        }
    }
}
