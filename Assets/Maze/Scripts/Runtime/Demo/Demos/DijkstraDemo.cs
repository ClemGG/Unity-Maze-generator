using System;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class DijkstraDemo : MonoBehaviour
    {
        [field: SerializeField] private GenerationSettingsSO GenerationSettings { get; set; }
        private IDraw _drawMethod;


        [ContextMenu("Cleanup")]
        void Cleanup()
        {
            if (_drawMethod is not null)
            {
                _drawMethod.Cleanup();
            }
        }

        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            var grid = new DistanceGrid(GenerationSettings);

            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(GenerationSettings);
            Cell start = grid[0, 0];
            genAlg.Execute(grid, start);

            _drawMethod = new ConsoleDraw();

            Distances distances = start.GetDistances();
            grid.Distances = distances;
            _drawMethod.Draw(grid);

            grid.Distances = distances.PathTo(grid[grid.Rows - 1, 0]);
            print("path from northwest corner to southwest corner:");
            _drawMethod.Draw(grid);

        }
    }
}