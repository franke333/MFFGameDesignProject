using CharlesEngine;

using Unity.Mathematics;

using UnityEngine;

public class CarController : MonoBehaviour
{
    private const KeyCode leftKey = KeyCode.A;
    private const KeyCode rightKey = KeyCode.D;

    public CEGameObject RoadStripsLeft;
    public CEGameObject RoadStripsRight;
    public CEGameObject RoadLineLeft;
    public CEGameObject RoadLineRight;

    public ObstacleManager ObstacleManager;

    private int oldPos = 1;
    public int CarPos { get; private set; } = 1;

    void Update()
    {
        if (Input.GetKeyDown(leftKey))
        {
            CarPos--;
        }
        if (Input .GetKeyDown(rightKey)) 
        {
            CarPos++;
        }
        CarPos = math.clamp(CarPos, 0, 2);

        switch (CarPos)
        {
            case 0:
                RoadLineLeft.Show();
                RoadStripsLeft.Hide();
                RoadStripsRight.Show();
                RoadLineRight.Hide();
                break;
            case 1:
                RoadLineLeft.Hide();
                RoadStripsLeft.Show();
                RoadStripsRight.Show();
                RoadLineRight.Hide();
                break;
            case 2:
                RoadLineLeft.Hide();
                RoadStripsLeft.Show();
                RoadStripsRight.Hide();
                RoadLineRight.Show();
                break;
        }

        if (oldPos != CarPos)
            ObstacleManager.UpdateVisibility(CarPos);
        oldPos = CarPos;
    }
}
