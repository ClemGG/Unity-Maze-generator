using System;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class DijkstraLongestPathDemo : MonoBehaviour
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
            (Cell newStart, int distance) = distances.Max();

            var newDistances = newStart.GetDistances();
            (Cell goal, int goalDistance) = newDistances.Max();
            grid.Distances = newDistances.PathTo(goal);

            grid.DisplayGrid(GenerationSettings.DrawMode);
        }
    }
}