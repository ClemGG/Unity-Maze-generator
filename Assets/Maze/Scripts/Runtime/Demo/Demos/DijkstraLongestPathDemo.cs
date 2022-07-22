using System;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class DijkstraLongestPathDemo : MonoBehaviour
    {

        [field: SerializeField] private GenerationSettingsSO Settings { get; set; }
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
            var grid = new DistanceGrid(Settings);

            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            Cell start = grid[0, 0];
            genAlg.Execute(grid, start);

            Distances distances = start.GetDistances();
            (Cell newStart, int distance) = distances.Max();

            var newDistances = newStart.GetDistances();
            (Cell goal, int goalDistance) = newDistances.Max();
            grid.Distances = newDistances.PathTo(goal);

            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            _drawMethod = InterfaceFactory.GetDrawMode(Settings);
            _drawMethod.Draw(grid);
        }
    }
}