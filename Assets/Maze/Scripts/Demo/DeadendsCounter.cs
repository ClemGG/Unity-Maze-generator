using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Project.Procedural.MazeGeneration.Enums;

namespace Project.Procedural.MazeGeneration
{
    public class DeadendsCounter : MonoBehaviour
    {
        [field: SerializeField] private int Tries { get; set; } = 100;
        [field: SerializeField] private Vector2Int GridSize { get; set; } = new(4, 4);


#if UNITY_EDITOR

        private void OnValidate()
        {
            GridSize = new(Mathf.Clamp(GridSize.x, 1, 100), Mathf.Clamp(GridSize.y, 1, 100));
        }
#endif

        [ContextMenu("Get Average Dead ends")]
        void Execute()
        {
            Dictionary<GenerationType, int> averages = new();
            ForEach<GenerationType>((algorithm) => 
            {
                print($"Running algorithm \"{algorithm}\"");
                var deadendCounts = new int[Tries];
                for (int i = 0; i < Tries; i++)
                {
                    var grid = new Grid(GridSize.x, GridSize.y);
                    grid.Execute(algorithm);
                    deadendCounts[i] = grid.GetDeadends().Count;
                }

                int totalDeadends = 0;
                for (int i = 0; i < deadendCounts.Length; i++)
                {
                    totalDeadends += deadendCounts[i];
                }
                averages.Add(algorithm, totalDeadends / deadendCounts.Length);
            });

            int totalCells = GridSize.x * GridSize.y;
            print($"Average dead-ends per {GridSize} maze ({totalCells} cells):");

            var sortedAverages = averages.OrderBy(key => -key.Value);

            foreach (KeyValuePair<GenerationType, int> pair in sortedAverages)
            {
                float percentage = averages[pair.Key] * 100f / (float)totalCells;
                print($"{pair.Key}: {averages[pair.Key]} / {totalCells} ({percentage}%)");
            }
        }
    }
}