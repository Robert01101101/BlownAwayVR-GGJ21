using UnityEngine;

[CreateAssetMenu(menuName = "JamAssets/ObstacleCollection", fileName = "ObstacleCollection")]
public class ObstacleCollection : ScriptableObject
{
    [SerializeField]
    private GameObject[] obstacles = default;
    public GameObject[] Obstacles => obstacles;
    
    [SerializeField]
    private Material materialToAssign = default;
    public Material MaterialToAssign => materialToAssign;

    public GameObject RandomObstacle => obstacles[Random.Range(0, obstacles.Length)];
}
