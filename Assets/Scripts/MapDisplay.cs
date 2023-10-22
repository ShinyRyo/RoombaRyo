using UnityEngine;
using System.Collections.Generic;

public class MapDisplay : MonoBehaviour
{
    public Material mapMaterial;  // マップのマテリアル
    public int mapSize = 100;  // マップのサイズ（ピクセル）

    public void UpdateMap(Dictionary<Vector2Int, bool> exploredMap)
    {
        Debug.Log("Updating map with " + exploredMap.Count + " explored cells.");  // ログステートメントを追加

        Texture2D texture = new Texture2D(mapSize, mapSize);

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                Color color = exploredMap.ContainsKey(position) && exploredMap[position] ? Color.white : Color.black;
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        mapMaterial.mainTexture = texture;
    }
}
