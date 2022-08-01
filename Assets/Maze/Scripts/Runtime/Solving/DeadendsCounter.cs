using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Enums;

namespace Project.Procedural.MazeGeneration
{
    public class DeadendsCounter : MonoBehaviour
    {
        [field: SerializeField] private int Tries { get; set; } = 100;
        [field: SerializeField] private GenerationSettingsSO GenerationSettings { get; set; }


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
                    var grid = new DistanceGrid(GenerationSettings);

                    //Dynamically creates the generation algorithm
                    Type algType = Type.GetType($"Project.Procedural.MazeGeneration.{algorithm}");
                    IGeneration genAlg = (IGeneration)Activator.CreateInstance(algType, GenerationSettings);
                    genAlg.ExecuteSync(grid);

                    deadendCounts[i] = grid.GetDeadends().Count;
                }

                int totalDeadends = 0;
                for (int i = 0; i < deadendCounts.Length; i++)
                {
                    totalDeadends += deadendCounts[i];
                }
                averages.Add(algorithm, totalDeadends / deadendCounts.Length);
            });

            int totalCells = GenerationSettings.GridSize.x * GenerationSettings.GridSize.y;
            print($"Average dead-ends per {GenerationSettings.GridSize} maze ({totalCells} cells):");

            var sortedAverages = averages.OrderBy(key => -key.Value);

            foreach (KeyValuePair<GenerationType, int> pair in sortedAverages)
            {
                float percentage = averages[pair.Key] * 100f / (float)totalCells;
                print($"{pair.Key}: {averages[pair.Key]} / {totalCells} ({percentage}%)");
            }
        }
    }
}