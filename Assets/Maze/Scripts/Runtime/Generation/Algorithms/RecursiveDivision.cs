using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{

    //The Recursive Division adds walls instead of removing them.
    //It cuts the maze in half and divides each created section again until it reaches individual cells.
    public class RecursiveDivision : IGeneration
    {
        private Vector2Int RoomSize { get; set; } = new(1, 1);
        private bool BiasTowardsRooms { get; set; } = false;

        private GenerationProgressReport _report;
        private List<Cell> _unlinkedCells;
        private IProgress<GenerationProgressReport> _progress;

        public RecursiveDivision(GenerationSettingsSO generationSettings)
        {
            RoomSize = generationSettings.RoomSize;
            BiasTowardsRooms = generationSettings.BiasTowardsRooms;
        }


        public void ExecuteSync(IGrid grid, Cell start = null)
        {

            //Links all cells together to create an empty maze
            grid.LinkAll();

            if(BiasTowardsRooms)
                DivideWithBias(grid, 0, 0, grid.Rows, grid.Columns);
            else
                Divide(grid, 0, 0, grid.Rows, grid.Columns);
        }




        public IEnumerator ExecuteAsync(IGrid grid, IProgress<GenerationProgressReport> progress, Cell start = null)
        {
            _report = new();
            _unlinkedCells = new();
            _progress = progress;

            //Links all cells together to create an empty maze
            grid.LinkAll();

            if (BiasTowardsRooms)
                yield return DivideWithBiasAsync(grid, 0, 0, grid.Rows, grid.Columns);
            else
                yield return DivideAsync(grid, 0, 0, grid.Rows, grid.Columns);

            _report.ProgressPercentage = (float)((grid.Size() - _unlinkedCells.Count) * 100 / grid.Size()) / 100f;
            _report.UpdateTrackTime(Time.deltaTime);
            _progress.Report(_report);
        }

        #region Sync

        private void Divide(IGrid grid, int row, int column, int height, int width)
        {
            //Will stop the divide if the zone is smaller than a given threshold or randomly
            //Warning : The room needs to be that exact RoomSize for the divide to stop.
            //Changing the && in that condition with a || will create many more rooms than passages.
            if (height <= 1 || width <= 1 || height <= RoomSize.y && width <= RoomSize.x && 4.Sample() == 0) return;

            //Although we could divide randomly, dividing based on the aspect ratio of the region tends
            //to give good results, and avoids producing areas with lots of long vertical or
            //horizontal passages.
            if (height > width)
            {
                DivideHorizontally(grid, row, column, height, width);
            }
            else
            {
                DivideVertically(grid, row, column, height, width);
            }
        }

        //This one has a bias towards creating more rooms than passages
        private void DivideWithBias(IGrid grid, int row, int column, int height, int width)
        {
            //Will stop the divide if the zone is smaller than a given threshold or randomly
            //Changing the || in the RoomSize condition with a && will create many more passages than rooms.
            if (height <= 1 || width <= 1 || (height <= RoomSize.y || width <= RoomSize.x) && 4.Sample() == 0) return;

            //Although we could divide randomly, dividing based on the aspect ratio of the region tends
            //to give good results, and avoids producing areas with lots of long vertical or
            //horizontal passages.
            if (height > width)
            {
                DivideHorizontally(grid, row, column, height, width);
            }
            else
            {
                DivideVertically(grid, row, column, height, width);
            }
        }

        private void DivideHorizontally(IGrid grid, int row, int column, int height, int width)
        {
            int divideSouthOf = (height - 1).Sample();
            int passageAt = width.Sample();

            for (int x = 0; x < width; x++)
            {
                if (passageAt == x) continue;

                Cell cell = grid[row + divideSouthOf, column + x];
                cell.Unlink(cell.South);
            }

            if (BiasTowardsRooms)
            {
                DivideWithBias(grid, row, column, divideSouthOf + 1, width);
                DivideWithBias(grid, row + divideSouthOf + 1, column, height - divideSouthOf - 1, width);
            }
            else
            {
                Divide(grid, row, column, divideSouthOf + 1, width);
                Divide(grid, row + divideSouthOf + 1, column, height - divideSouthOf - 1, width);
            }
        }

        private void DivideVertically(IGrid grid, int row, int column, int height, int width)
        {
            int divideEastOf = (width - 1).Sample();
            int passageAt = height.Sample();

            for (int y = 0; y < height; y++)
            {
                if (passageAt == y) continue;

                Cell cell = grid[row + y, column + divideEastOf];
                cell.Unlink(cell.East);
            }

            if (BiasTowardsRooms)
            {
                DivideWithBias(grid, row, column, height, divideEastOf + 1);
                DivideWithBias(grid, row, column + divideEastOf + 1, height, width - divideEastOf - 1);
            }
            else
            {
                Divide(grid, row, column, height, divideEastOf + 1);
                Divide(grid, row, column + divideEastOf + 1, height, width - divideEastOf - 1);
            }
        }

        #endregion

        #region Async

        private IEnumerator DivideAsync(IGrid grid, int row, int column, int height, int width)
        {
            //Will stop the divide if the zone is smaller than a given threshold or randomly
            //Warning : The room needs to be that exact RoomSize for the divide to stop.
            //Changing the && in that condition with a || will create many more rooms than passages.
            if (height <= 1 || width <= 1 || height <= RoomSize.y && width <= RoomSize.x && 4.Sample() == 0) yield break;

            //Although we could divide randomly, dividing based on the aspect ratio of the region tends
            //to give good results, and avoids producing areas with lots of long vertical or
            //horizontal passages.
            if (height > width)
            {
                yield return DivideHorizontallyAsync(grid, row, column, height, width);
            }
            else
            {
                yield return DivideVerticallyAsync(grid, row, column, height, width);
            }
        }

        //This one has a bias towards creating more rooms than passages
        private IEnumerator DivideWithBiasAsync(IGrid grid, int row, int column, int height, int width)
        {
            //Will stop the divide if the zone is smaller than a given threshold or randomly
            //Changing the || in the RoomSize condition with a && will create many more passages than rooms.
            if (height <= 1 || width <= 1 || (height <= RoomSize.y || width <= RoomSize.x) && 4.Sample() == 0) yield break;

            //Although we could divide randomly, dividing based on the aspect ratio of the region tends
            //to give good results, and avoids producing areas with lots of long vertical or
            //horizontal passages.
            if (height > width)
            {
                yield return DivideHorizontallyAsync(grid, row, column, height, width);
            }
            else
            {
                yield return DivideVerticallyAsync(grid, row, column, height, width);
            }
        }

        private IEnumerator DivideHorizontallyAsync(IGrid grid, int row, int column, int height, int width)
        {
            int divideSouthOf = (height - 1).Sample();
            int passageAt = width.Sample();

            for (int x = 0; x < width; x++)
            {
                if (passageAt == x) continue;

                Cell cell = grid[row + divideSouthOf, column + x];
                cell.Unlink(cell.South);

                _unlinkedCells.Add(cell);
                _report.ProgressPercentage = (float)((grid.Size() - _unlinkedCells.Count) * 100 / grid.Size()) / 100f;
                _report.UpdateTrackTime(Time.deltaTime);
                _progress.Report(_report);
                yield return null;

            }

            if (BiasTowardsRooms)
            {
                DivideWithBias(grid, row, column, divideSouthOf + 1, width);
                DivideWithBias(grid, row + divideSouthOf + 1, column, height - divideSouthOf - 1, width);
            }
            else
            {
                Divide(grid, row, column, divideSouthOf + 1, width);
                Divide(grid, row + divideSouthOf + 1, column, height - divideSouthOf - 1, width);
            }
        }

        private IEnumerator DivideVerticallyAsync(IGrid grid, int row, int column, int height, int width)
        {
            int divideEastOf = (width - 1).Sample();
            int passageAt = height.Sample();

            for (int y = 0; y < height; y++)
            {
                if (passageAt == y) continue;

                Cell cell = grid[row + y, column + divideEastOf];
                cell.Unlink(cell.East);

                _unlinkedCells.Add(cell);
                _report.ProgressPercentage = (float)((grid.Size() - _unlinkedCells.Count) * 100 / grid.Size()) / 100f;
                _report.UpdateTrackTime(Time.deltaTime);
                _progress.Report(_report);
                yield return null;
            }

            if (BiasTowardsRooms)
            {
                DivideWithBias(grid, row, column, height, divideEastOf + 1);
                DivideWithBias(grid, row, column + divideEastOf + 1, height, width - divideEastOf - 1);
            }
            else
            {
                Divide(grid, row, column, height, divideEastOf + 1);
                Divide(grid, row, column + divideEastOf + 1, height, width - divideEastOf - 1);
            }
        }

        #endregion
    }
}