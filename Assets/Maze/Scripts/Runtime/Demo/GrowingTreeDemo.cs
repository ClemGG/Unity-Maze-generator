using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class GrowingTreeDemo : MonoBehaviour
    {
        [field: SerializeField, Range(0, 3)] private int LambdaSelection { get; set; } = 0;
        [field: SerializeField] private Vector2Int GridSize { get; set; } = new(4, 4);


#if UNITY_EDITOR

        private void OnValidate()
        {
            GridSize = new(Mathf.Clamp(GridSize.x, 1, 100), Mathf.Clamp(GridSize.y, 1, 100));
        }
#endif


        [ContextMenu("Cleanup UI")]
        void CleanupUI()
        {
            OrthogonalMaze.CleanupUI();
        }

        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            var grid = new ColoredGrid(GridSize.x, GridSize.y);
            GrowingTree.SetLambdaMethod(SetLambdaForGrowingTree());

            Cell start = grid.RandomCell();
            grid.Execute(GenerationType.GrowingTree, start);
            grid.SetDistances(start.GetDistances());

            grid.DisplayGrid(DisplayMode.UIImage);
        }

        private Func<List<Cell>, Cell> SetLambdaForGrowingTree() => LambdaSelection switch
        {
            //Selects a cell at random (executes Simple Prim)
            0 => (active) => active.Sample(),
            //Selects the last cell (executes Recursive Backtracker)
            1 => (active) => active.Last(),
            //Selects the first cell (creates elongated corridors)
            2 => (active) => active.First(),
            //Mixes between the Recursive Backtracker and the Simple Prim
            3 => (active) => (2.Sample() == 0) ? active.Sample() : active.Last(),
            _ => null,
        };
    }
}