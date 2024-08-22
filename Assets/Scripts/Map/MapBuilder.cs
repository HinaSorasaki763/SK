using System.Text;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    public void BuildHollowSquare()
    {
        int size = 11; // 正方形的边长
        int halfSize = size / 2;
        StringBuilder stringBuilder = new StringBuilder();
        int count = 0;
        for (int y = -halfSize; y <= halfSize; y++)
        {
            for (int x = -halfSize; x <= halfSize; x++)
            {
                // 只在边界位置放置墙壁
                if (x == -halfSize || x == halfSize || y == -halfSize || y == halfSize)
                {
                    Vector3 position = new Vector3(x, y+5, 0);
                    GameObject wall = ResourcePool.Instance.GetWall(position);

                    wall.transform.position = position;
                    count++;
                    stringBuilder.AppendLine($"{count} block ,  pos = {wall.transform.position}");
                }
            }
        }
        Debug.Log(stringBuilder.ToString());
    }

    public void ClearMap()
    {
        foreach (Transform child in ResourcePool.Instance.wallParent)
        {
            ResourcePool.Instance.ReturnWall(child.gameObject);
        }
    }
}
