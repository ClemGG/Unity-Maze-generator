using System.Collections;
using System.Collections.Generic;
using Project.Pool;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Procedural.MazeGeneration
{
    public class UIImageDraw : IDrawMethod<Color>, IDrawMethodAsync<Color>
    {

        #region UI Fields

        //No need to clean the pooler, we recreate it before each generation
        private readonly ClassPooler<GameObject> UIImagePooler = new
            (
                new Pool<GameObject>("cell ui img", 10000, () => Object.Instantiate(Resources.Load<GameObject>("Maze Prefabs/cell ui img"))),
                new Pool<GameObject>("line ui img", 40000, () => Object.Instantiate(Resources.Load<GameObject>("Maze Prefabs/line ui img")))
            );

        private static Canvas _canvas;
        private static RectTransform _bg;
        private static Transform _tiles;
        private static Transform _lines;
        private static Transform _imgHolder;

        private static Canvas Canvas
        {
            get
            {
                if (!_canvas) _canvas = GameObject.Find("Maze").GetComponent<Canvas>();
                return _canvas;
            }
        }
        private static RectTransform Bg
        {
            get
            {
                if (!_bg) _bg = (RectTransform)Canvas.transform.GetChild(0);
                return _bg;
            }
        }
        private static Transform Tiles
        {
            get
            {
                if (!_tiles) _tiles = Bg.transform.GetChild(0);
                return _tiles;
            }
        }
        private static Transform Lines
        {
            get
            {
                if (!_lines) _lines = Bg.transform.GetChild(1);
                return _lines;
            }
        }
        private static Transform ImgHolder
        {
            get
            {
                if (!_imgHolder) _imgHolder = Bg.parent.GetChild(1);
                return _imgHolder;
            }
        }


        private float _inset = 0f;


        #endregion


        public UIImageDraw(GenerationSettingsSO settings)
        {
            _inset = settings.Inset;
        }

        public void Cleanup()
        {
            //cleanup pooler.
            //stored temp. in an array to avoid Bg's resizing
            int nbCells = Tiles.childCount;
            int nbLines = Lines.childCount;
            int nbHeld = ImgHolder.childCount;
            List<Transform> children = new(nbCells + nbLines + nbHeld);
            for (int i = 0; i < nbCells; i++)
            {
                children.Add(Tiles.GetChild(i));
            }
            for (int i = 0; i < nbLines; i++)
            {
                children.Add(Lines.GetChild(i));
            }
            for (int i = 0; i < nbHeld; i++)
            {
                children.Add(ImgHolder.GetChild(i));
            }
            for (int i = 0; i < children.Count; i++)
            {
                Object.DestroyImmediate(children[i].gameObject);
            }
        }


        public IEnumerator DrawAsync(IDrawableGrid<Color> grid, System.IProgress<GenerationProgressReport> progress)
        {
            Cleanup();

            float cellSize = Mathf.Min(Bg.rect.width / grid.Columns, Bg.rect.height / grid.Rows);
            _inset = cellSize * _inset;

            //Used to resize the lineUIs if they get too big for the grid resolution
            float gridLongestSide = Mathf.Max(grid.Columns, grid.Rows);


            List<Cell> completedCells = new(grid.Size());
            GenerationProgressReport report = new();


            for (int i = 0; i < grid.Rows; i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    Cell cell = grid[i, j];

                    if (cell is not null)
                    {
                        completedCells.Add(cell);
                        Color color = grid.Draw(cell);

                        if (!Mathf.Approximately(_inset, 0f) && !Mathf.Approximately(_inset, .5f * cellSize))
                        {
                            float x = cell.Column * cellSize;
                            float y = cell.Row * cellSize;

                            DisplayCellImgWithInset(cell, cellSize, x, y, _inset, color);
                            DisplayLineImgWithInset(cell, cellSize, x, y, _inset, gridLongestSide);

                        }
                        else
                        {
                            DisplayCellImgWithoutInset(i, j, cellSize, color);
                            DisplayLineImgWithoutInset(cell, cellSize, gridLongestSide);
                        }

                        report.ProgressPercentage = (float)(completedCells.Count * 100 / grid.Size()) / 100f;
                        report.UpdateTrackTime(Time.deltaTime);
                        progress.Report(report);
                        yield return null;
                    }
                }
            }


        }


        public void DrawSync(IDrawableGrid<Color> grid)
        {
            Cleanup();

            float cellSize = Mathf.Min(Bg.rect.width / grid.Columns, Bg.rect.height / grid.Rows);
            _inset = cellSize * _inset;

            //Used to resize the lineUIs if they get too big for the grid resolution
            float gridLongestSide = Mathf.Max(grid.Columns, grid.Rows);

            for (int i = 0; i < grid.Rows; i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    Cell cell = grid[i, j];

                    if (cell is null) continue;
                    Color color = grid.Draw(cell);

                    if (!Mathf.Approximately(_inset, 0f) && !Mathf.Approximately(_inset, .5f * cellSize))
                    {
                        float x = cell.Column * cellSize;
                        float y = cell.Row * cellSize;

                        DisplayCellImgWithInset(cell, cellSize, x, y, _inset, color);
                        DisplayLineImgWithInset(cell, cellSize, x, y, _inset, gridLongestSide);
                    }
                    else
                    {
                        DisplayCellImgWithoutInset(i, j, cellSize, color);
                        DisplayLineImgWithoutInset(cell, cellSize, gridLongestSide);
                    }
                }
            }



        }



        private (Vector4, Vector4) CellCoordsWithInset(float x, float y, float cellSize)
        {
            float x1 = x;
            float x4 = x + cellSize;
            float x2 = x1 + _inset;
            float x3 = x4 - _inset;

            float y1 = y;
            float y4 = y + cellSize;
            float y2 = y1 + _inset;
            float y3 = y4 - _inset;

            return (new(x1, x2, x3, x4), new(y1, y2, y3, y4));
        }





        private void DisplayCellImgWithInset(Cell cell, float cellSize, float x, float y, float inset, Color color)
        {
            (Vector4 xc, Vector4 yc) = CellCoordsWithInset(x, y, cellSize);
            float x1 = xc.x;
            float x2 = xc.y;
            float x3 = xc.z;
            float x4 = xc.w;

            float y1 = yc.x;
            float y2 = yc.y;
            float y3 = yc.z;
            float y4 = yc.w;

            float doubleI = inset * 2f;
            cellSize -= doubleI;

            //Draws the img for the center of the cell
            DrawCell(new Vector2(cellSize, cellSize),
                new Vector3(x2, -y2, 0),
                //Color.black);
                color);


            //Draws 4 imgs to fill the outer regions of the cell
            if (cell.IsLinked(cell.North))
            {
                DrawCell(new Vector2(cellSize, doubleI),
                new Vector3(x2, -y1 + inset, 0),
                //Color.red);
                color);
            }
            if (cell.IsLinked(cell.East))
            {
                DrawCell(new Vector2(doubleI, cellSize),
                new Vector3(x3, -y2, 0),
                //Color.yellow);
                color);
            }
        }


        private void DisplayLineImgWithInset(Cell cell, float cellSize, float x, float y, float inset, float gridLongestSize)
        {
            //width and height of the UI Image in pixels
            //TODO : Scale these sizes for smaller cells
            float lineThickness = Mathf.Lerp(5f, 1f, inset / cellSize / 0.4f);
            lineThickness = Mathf.Min(lineThickness, Mathf.Lerp(5f, 1f, gridLongestSize / 50f));

            Vector2 anchorH = new(0f, 1f);
            Vector2 pivotH = new(0f, 1f);
            Vector2 anchorV = new(0f, 1f);
            Vector2 pivotV = new(0.5f, 1f);

            (Vector4 xc, Vector4 yc) = CellCoordsWithInset(x, y, cellSize);
            float x1 = xc.x;
            float x2 = xc.y;
            float x3 = xc.z;
            float x4 = xc.w;

            float y1 = yc.x;
            float y2 = yc.y;
            float y3 = yc.z;
            float y4 = yc.w;

            float doubleI = inset * 2f;
            cellSize -= doubleI;

            Vector3 pos, size;

            if (cell.IsLinked(cell.North))
            {
                size = new(lineThickness, doubleI + lineThickness);
                pos = new(x2, -y1 + inset);

                //Line V
                DrawLine(anchorV, pivotV, size, pos);

                pos = new(x3, -y1 + inset);

                //Line V
                DrawLine(anchorV, pivotV, size, pos);
            }
            else
            {
                size = new(cellSize, lineThickness);
                pos = new(x2, -y2);

                //Line H
                DrawLine(anchorH, pivotH, size, pos);
            }




            if (cell.IsLinked(cell.East))
            {
                size = new(doubleI, lineThickness);
                pos = new(x3, -y2);

                //Line H
                DrawLine(anchorH, pivotH, size, pos);

                pos = new(x3, -y3);

                //Line H
                DrawLine(anchorH, pivotH, size, pos);
            }
            else
            {

                size = new(lineThickness, cellSize + lineThickness);
                pos = new(x3, -y2);

                //Line V
                DrawLine(anchorV, pivotV, size, pos);
            }



            if (!cell.IsLinked(cell.South))
            {
                size = new(cellSize, lineThickness);
                pos = new(x2, -y3);

                //Line H
                DrawLine(anchorH, pivotH, size, pos);
            }



            if (!cell.IsLinked(cell.West))
            {
                size = new(lineThickness, cellSize + lineThickness);
                pos = new(x2, -y2);

                //Line V
                DrawLine(anchorV, pivotV, size, pos);
            }
        }


        private void DisplayCellImgWithoutInset(int i, int j, float cellSize, Color color)
        {
            DrawCell(new Vector2(cellSize, cellSize),
                new Vector3(cellSize * j, -cellSize * i, 0),
                color);
        }


        private void DisplayLineImgWithoutInset(Cell cell, float cellSize, float gridLongestSize)
        {
            //width and height of the UI Image in pixels
            float lineThickness = Mathf.Lerp(5f, 1f, gridLongestSize / 100f);

            Vector2 anchorH = new(0f, 1f);
            Vector2 pivotH = new(0f, 0.5f);
            Vector2 anchorV = new(0f, 1f);
            Vector2 pivotV = new(0.5f, 1f);

            if (cell.North is null)
            {
                //Line North
                DrawLine(anchorH, pivotH,
                    new Vector2(cellSize, lineThickness),
                    new Vector3(cellSize * cell.Column, -cellSize * cell.Row, 0));
            }

            if (cell.West is null)
            {
                //Line West
                DrawLine(anchorV, pivotV,
                    new Vector2(lineThickness, cellSize + lineThickness),
                    new Vector3(cellSize * cell.Column, -cellSize * cell.Row + lineThickness / 2f, 0));
            }

            if (!cell.IsLinked(cell.East))
            {
                //Line East
                DrawLine(anchorV, pivotV,
                    new Vector2(lineThickness, cellSize + lineThickness),
                    new Vector3(cellSize * (cell.Column + 1), -cellSize * cell.Row + lineThickness / 2f, 0));
            }

            if (!cell.IsLinked(cell.South))
            {
                //Line South
                DrawLine(anchorH, pivotH,
                    new Vector2(cellSize, lineThickness),
                    new Vector3(cellSize * cell.Column, -cellSize * (cell.Row + 1), 0));
            }
        }



        private void DrawCell(Vector2 size, Vector3 anchoredPos, Color col)
        {
            RectTransform cellImg = UIImagePooler.GetFromPool<GameObject>("cell ui img").GetComponent<RectTransform>();
            cellImg.SetParent(Tiles);
            cellImg.gameObject.SetActive(true);

            cellImg.pivot = cellImg.anchorMin = cellImg.anchorMax = new Vector2(0f, 1f);
            cellImg.anchoredPosition = anchoredPos;
            cellImg.localScale = Vector3.one;

            cellImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            cellImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);

            cellImg.GetComponent<Image>().color = col;
        }

        private void DrawLine(Vector2 anchor, Vector2 pivot, Vector2 size, Vector3 anchoredPos)
        {
            RectTransform line = UIImagePooler.GetFromPool<GameObject>("line ui img").GetComponent<RectTransform>();
            line.SetParent(Lines);
            line.gameObject.SetActive(true);

            line.anchorMin = line.anchorMax = anchor;
            line.pivot = pivot;
            line.anchoredPosition = anchoredPos;
            line.localScale = Vector3.one;

            line.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            line.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        }
    }
}