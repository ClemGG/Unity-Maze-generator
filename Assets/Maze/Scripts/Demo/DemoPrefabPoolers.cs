using UnityEngine;
using Project.Pool;

namespace Project.Procedural.MazeGeneration
{
    public static class DemoPrefabPoolers
    {
        public static ClassPooler<GameObject> UIImagePooler = new
            (
                new Pool<GameObject>("cell ui img", 10000, () => Object.Instantiate(Resources.Load<GameObject>("Maze Prefabs/cell ui img"))),
                new Pool<GameObject>("line ui img", 60000, () => Object.Instantiate(Resources.Load<GameObject>("Maze Prefabs/line ui img")))
            );
    }
}