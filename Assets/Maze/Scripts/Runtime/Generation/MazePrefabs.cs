using Project.Pool;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public static class MazePrefabs
    {
        public static readonly ClassPooler<GameObject> UIImagePooler = new
            (
                new Pool<GameObject>("cell ui img", 10000, () => Object.Instantiate(Resources.Load<GameObject>("Maze Prefabs/cell ui img"))),
                new Pool<GameObject>("line ui img", 40000, () => Object.Instantiate(Resources.Load<GameObject>("Maze Prefabs/line ui img")))
            );

    }
}