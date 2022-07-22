using System;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class DijkstraDemo : MonoBehaviour
    {
        [field: SerializeField] private GenerationSettingsSO GenerationSettings { get; set; }


        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            var grid = new DistanceGrid(GenerationSettings);

            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(GenerationSettings);
            Cell start = grid[0, 0];
            genAlg.Execute(grid, start);

            Distances distances = start.GetDistances();
            grid.Distances = distances;
            grid.DisplayGrid(DrawMode.Console);

            grid.Distances = distances.PathTo(grid[grid.Rows - 1, 0]);
            print("path from northwest corner to southwest corner:");
            grid.DisplayGrid(DrawMode.Console);
        }
    }
}