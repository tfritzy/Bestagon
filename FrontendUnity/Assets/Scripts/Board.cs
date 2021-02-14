using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject Hexagon;
    public Dictionary<int, Dictionary<int, GameObject>> Hexagons;

    private void Start()
    {
        Hexagons = new Dictionary<int, Dictionary<int, GameObject>>();
    }

    public void HandleBoardStateChange(Schema.BoardState boardState)
    {
        foreach (Schema.HexagonSet set in boardState.HexagonSets)
        {
            if (Hexagons.ContainsKey(set.PlayerId) == false)
            {
                Hexagons[set.PlayerId] = new Dictionary<int, GameObject>();
            }

            foreach (Schema.Hexagon hexagon in set.Hexagons)
            {
                Vector2 position = new Vector2(hexagon.Position.X, hexagon.Position.Y);
                Hexagons[set.PlayerId][hexagon.Id] = Instantiate(Hexagon, position, new Quaternion(), this.transform);
            }
        }
    }
}
