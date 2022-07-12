using UnityEngine;
using UnityEngine.UI;

namespace Project.Procedural.MazeGeneration
{
    //Draws square-shaped mazes (gamma-type mazes).
    //Used for 2D & 3D.
    public static class OrthogonalMaze
    {
        private static Canvas _canvas;
        private static RectTransform _bg;
        private static Transform _imgHolder;

        private static Canvas Canvas
        {
            get
            {
                if (!_canvas) _canvas = Object.FindObjectOfType<Canvas>();
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
        private static Transform ImgHolder
        {
            get
            {
                if (!_imgHolder) _imgHolder = Bg.parent.GetChild(1);
                return _imgHolder;
            }
        }

        #region 3D Mesh

        public static void DisplayOnMesh(Grid grid, float inset = 0f)
        {
            Debug.Log("3D");
        }

        #endregion


        #region UI

        public static void DisplayOnUI(Grid grid, float inset = 0f)
        {
            //cleanup pooler.
            //stored temp. in an array to avoid Bg's resizing
            Transform[] children = new Transform[Bg.childCount];
            for (int i = 0; i < Bg.childCount; i++)
            {
                children[i] = Bg.GetChild(i);
            }
            for (int i = 0; i < children.Length; i++)
            {
                Transform child = children[i];
                child.SetParent(ImgHolder);
                child.gameObject.SetActive(false);
                DemoPrefabPoolers.UIImagePooler.ReturnToPool(child.gameObject, child.name.Replace("(Clone)", ""));
            }





            float maxCellSize = Mathf.Min(Bg.rect.width / grid.Columns, Bg.rect.height / grid.Rows);
            inset = maxCellSize * inset;

            #region Spawn cell imgs

            for (int i = 0; i < grid.Rows; i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    Cell cell = grid[i, j];

                    float x = cell.Column * maxCellSize;
                    float y = cell.Row * maxCellSize;

                    if (!Mathf.Approximately(inset, 0f))
                    {
                        DisplayCellImgWithInset(grid, cell, maxCellSize, i, j, x, y, inset);
                    }
                    else
                    {
                        DisplayCellImgWithoutInset(grid, cell, maxCellSize, i, j);
                    }
                }
            }

            #endregion
            
            
            #region Spawn line imgs

            for (int i = 0; i < grid.Rows; i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    Cell cell = grid[i, j];

                    float x = cell.Column * maxCellSize;
                    float y = cell.Row * maxCellSize;

                    if (!Mathf.Approximately(inset, 0f))
                    {
                        DisplayLineImgWithInset(cell, maxCellSize, x, y, inset);
                    }
                    else
                    {

                        DisplayLineImgWithoutInset(cell, maxCellSize);
                    }
                }
            }

#endregion

            //DisplayOnUIWIthoutInset(grid);
        }




        private static float[] CellCoordsWithInset(float x, float y, float cellSize, float inset)
        {
            float x1 = x;
            float x4 = x + cellSize;
            float x2 = x1 + inset;
            float x3 = x4 - inset;

            float y1 = y;
            float y4 = y + cellSize;
            float y2 = y1 + inset;
            float y3 = y4 - inset;

            return new float []{x1, x2, x3, x4, y1, y2, y3, y4};
        }



        private static void DisplayCellImgWithInset(Grid grid, Cell cell, float cellSize, int i, int j, float x, float y, float inset)
        {
            float[] coords = CellCoordsWithInset(x, y, cellSize, inset);

            DrawCell(new Vector2(cellSize, cellSize),
                new Vector3(cellSize * j, -cellSize * i, 0),
                (cell is null) ? Color.black : grid.BackgroundColorFor(cell));
        }


        private static void DisplayLineImgWithInset(Cell cell, float cellSize, float x, float y, float inset)
        {

        }
        
        
        private static void DisplayCellImgWithoutInset(Grid grid, Cell cell, float cellSize, int i, int j)
        {
            DrawCell(new Vector2(cellSize, cellSize),
                new Vector3(cellSize * j, -cellSize * i, 0),
                (cell is null) ? Color.black : grid.BackgroundColorFor(cell));
        }
        
        
        private static void DisplayLineImgWithoutInset(Cell cell, float cellSize)
        {
            if (cell is null) return;

            //Scale shortest dimension to not overlap too much with cell img
            float lineX = .5f;
            float lineY = .5f;
            float lineWidth = 5f;
            float lineHeight = 5f;


            Vector2 anchorH = new(0f, 1f);
            Vector2 pivotH = new(0f, 1f);
            Vector2 anchorV = new(0f, 1f);
            Vector2 pivotV = new(0.5f, 1f);

            if (cell.North is null)
            {
                //Line North
                DrawLine(anchorH, pivotH,
                    new Vector2(cellSize, lineHeight), 
                    new Vector3(cellSize * cell.Column, -cellSize * cell.Row - lineY, 0));
            }

            if (cell.West is null)
            {
                //Line West
                DrawLine(anchorV, pivotV,
                    new Vector2(lineWidth, cellSize),
                    new Vector3(cellSize * cell.Column - lineX, -cellSize * cell.Row, 0));
            }

            if (!cell.IsLinked(cell.East))
            {
                //Line East
                DrawLine(anchorV, pivotV,
                    new Vector2(lineWidth, cellSize),
                    new Vector3(cellSize * (cell.Column + 1) - lineX, -cellSize * cell.Row, 0));
            }

            if (!cell.IsLinked(cell.South))
            {
                //Line South
                DrawLine(anchorH, pivotH,
                    new Vector2(cellSize, lineHeight),
                    new Vector3(cellSize * cell.Column, -cellSize * (cell.Row + 1) - lineY, 0));
            }
        }





        private static void DrawCell(Vector2 size, Vector3 anchoredPos, Color col)
        {
            RectTransform cellImg = DemoPrefabPoolers.UIImagePooler.GetFromPool<GameObject>("cell ui img").GetComponent<RectTransform>();
            cellImg.SetParent(Bg);
            cellImg.gameObject.SetActive(true);

            cellImg.pivot = cellImg.anchorMin = cellImg.anchorMax = new Vector2(0f, 1f);
            cellImg.anchoredPosition = anchoredPos;
            cellImg.localScale = Vector3.one;

            cellImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            cellImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);

            cellImg.GetComponent<Image>().color = col;
        }

        private static void DrawLine(Vector2 anchor, Vector2 pivot, Vector2 size, Vector3 anchoredPos)
        {
            RectTransform line = DemoPrefabPoolers.UIImagePooler.GetFromPool<GameObject>("line ui img").GetComponent<RectTransform>();
            line.SetParent(Bg);
            line.gameObject.SetActive(true);

            line.anchorMin = line.anchorMax = anchor;
            line.pivot = pivot;
            line.anchoredPosition = anchoredPos;
            line.localScale = Vector3.one;

            line.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            line.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        }
        #endregion
    }
}