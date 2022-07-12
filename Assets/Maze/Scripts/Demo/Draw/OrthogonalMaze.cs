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


        public static void DisplayOnMesh(Grid grid, float inset = 0f)
        {
            Debug.Log("3D");
        }




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
                        //DisplayCellImgWithInset(cell, maxCellSize, x, y, inset, true);
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
                        //DisplayLineImgWithInset(cell, maxCellSize, x, y, inset, false);
                    }
                    else
                    {

                        DisplayLineImgWithoutInset(cell, maxCellSize, i, j);
                    }
                }
            }

#endregion

            //DisplayOnUIWIthoutInset(grid);
        }



        private static void DisplayCellImgWithInset()
        {

        }
        private static void DisplayLineImgWithInset()
        {

        }
        private static void DisplayCellImgWithoutInset(Grid grid, Cell cell, float cellSize, int i, int j)
        {
            RectTransform cellImg = DemoPrefabPoolers.UIImagePooler.GetFromPool<GameObject>("cell ui img").GetComponent<RectTransform>();
            cellImg.SetParent(Bg);
            cellImg.gameObject.SetActive(true);
            //cell.name = $"cell #{cell.GetSiblingIndex()}";

            cellImg.pivot = cellImg.anchorMin = cellImg.anchorMax = new Vector2(0f, 1f);
            cellImg.anchoredPosition = new Vector3(cellSize * j, -cellSize * i, 0);

            cellImg.localScale = Vector3.one;

            cellImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize);
            cellImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize);

            Color cellColor = (cell is null) ? Color.black : grid.BackgroundColorFor(cell);
            cellImg.GetComponent<Image>().color = cellColor;
        }
        private static void DisplayLineImgWithoutInset(Cell cell, float cellSize, int i, int j)
        {
            if (cell is null) return;

            //Scale shortest dimension to not overlap too much with cell img
            float lineX = .5f;
            float lineY = .5f;
            float lineWidth = 5f;
            float lineHeight = 5f;


            Vector2 anchorsH = new(0f, 1f);
            Vector2 pivotH = new(0f, 1f);
            Vector2 anchorsV = new(0f, 1f);
            Vector2 pivotV = new(0.5f, 1f);

            if (cell.North is null)
            {
                RectTransform lineN = DemoPrefabPoolers.UIImagePooler.GetFromPool<GameObject>("line ui img").GetComponent<RectTransform>();
                lineN.SetParent(Bg);
                lineN.gameObject.SetActive(true);

                lineN.anchorMin = lineN.anchorMax = anchorsH;
                lineN.pivot = pivotH;
                lineN.anchoredPosition = new Vector3(cellSize * cell.Column, -cellSize * cell.Row - lineY, 0);
                lineN.localScale = Vector3.one;

                lineN.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize);
                lineN.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lineHeight);
            }

            if (cell.West is null)
            {

                //Line West
                RectTransform lineW = DemoPrefabPoolers.UIImagePooler.GetFromPool<GameObject>("line ui img").GetComponent<RectTransform>();
                lineW.SetParent(Bg);
                lineW.gameObject.SetActive(true);

                lineW.anchorMin = lineW.anchorMax = anchorsV;
                lineW.pivot = pivotV;
                lineW.anchoredPosition = new Vector3(cellSize * cell.Column - lineX, -cellSize * cell.Row, 0);
                lineW.localScale = Vector3.one;

                lineW.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);
                lineW.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize);
            }

            if (!cell.IsLinked(cell.East))
            {
                //Line East
                RectTransform lineE = DemoPrefabPoolers.UIImagePooler.GetFromPool<GameObject>("line ui img").GetComponent<RectTransform>();
                lineE.SetParent(Bg);
                lineE.gameObject.SetActive(true);

                lineE.anchorMin = lineE.anchorMax = anchorsV;
                lineE.pivot = pivotV;
                lineE.anchoredPosition = new Vector3(cellSize * (cell.Column + 1) - lineX, -cellSize * cell.Row, 0);
                lineE.localScale = Vector3.one;

                lineE.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);
                lineE.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize);
            }

            if (!cell.IsLinked(cell.South))
            {
                //Line South
                RectTransform lineS = DemoPrefabPoolers.UIImagePooler.GetFromPool<GameObject>("line ui img").GetComponent<RectTransform>();
                lineS.SetParent(Bg);
                lineS.gameObject.SetActive(true);

                lineS.anchorMin = lineS.anchorMax = anchorsH;
                lineS.pivot = pivotH;
                lineS.anchoredPosition = new Vector3(cellSize * cell.Column, -cellSize * (cell.Row + 1) - lineY, 0);
                lineS.localScale = Vector3.one;

                lineS.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize);
                lineS.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lineHeight);
            }
        }

        //public static void DisplayOnUI(Grid grid)
        //{



        //    float maxCellSize = Mathf.Min(Bg.rect.width / grid.Columns, Bg.rect.height / grid.Rows);
        //    float cellWidth = maxCellSize;
        //    float cellHeight = maxCellSize;

        //    //Scale shortest dimension to not overlap too much with cell img
        //    float lineX = .5f;
        //    float lineY = .5f;
        //    float lineWidth = 5f;
        //    float lineHeight = 5f;


        //    Vector2 anchorsH = new(0f, 1f);
        //    Vector2 pivotH = new(0f, 1f);
        //    Vector2 anchorsV = new(0f, 1f);
        //    Vector2 pivotV = new(0.5f, 1f);

        //    for (int i = 0; i < grid.Rows; i++)
        //    {
        //        for (int j = 0; j < grid.Columns; j++)
        //        {
        //            Cell cell = grid[i, j];

        //            Color cellColor = (cell is null) ? Color.black : grid.BackgroundColorFor(cell);

        //            #region Spawn cell imgs

        //            RectTransform cellImg = DemoPrefabPoolers.UIImagePooler.GetFromPool<GameObject>("cell ui img").GetComponent<RectTransform>();
        //            cellImg.SetParent(Bg);
        //            cellImg.gameObject.SetActive(true);
        //            //cell.name = $"cell #{cell.GetSiblingIndex()}";

        //            cellImg.anchorMin = cellImg.anchorMax = new Vector2(0f, 1f);
        //            cellImg.pivot = new Vector2(0f, 1f);
        //            cellImg.anchoredPosition = new Vector3(cellWidth * j, -cellHeight * i, 0);

        //            cellImg.localScale = Vector3.one;

        //            cellImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellWidth);
        //            cellImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellHeight);

        //            cellImg.GetComponent<Image>().color = cellColor;

        //            #endregion
        //        }
        //    }

        //    for (int i = 0; i < grid.Rows; i++)
        //    {
        //        for (int j = 0; j < grid.Columns; j++)
        //        {
        //            Cell cell = grid[i, j];
        //            if (cell is null) continue;

        //            if (cell.North is null)
        //            {
        //                RectTransform lineN = DemoPrefabPoolers.UIImagePooler.GetFromPool<GameObject>("line ui img").GetComponent<RectTransform>();
        //                lineN.SetParent(Bg);
        //                lineN.gameObject.SetActive(true);

        //                lineN.anchorMin = lineN.anchorMax = anchorsH;
        //                lineN.pivot = pivotH;
        //                lineN.anchoredPosition = new Vector3(cellWidth * cell.Column, -cellHeight * cell.Row - lineY, 0);
        //                lineN.localScale = Vector3.one;

        //                lineN.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellWidth);
        //                lineN.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lineHeight);
        //            }

        //            if (cell.West is null)
        //            {

        //                //Line West
        //                RectTransform lineW = DemoPrefabPoolers.UIImagePooler.GetFromPool<GameObject>("line ui img").GetComponent<RectTransform>();
        //                lineW.SetParent(Bg);
        //                lineW.gameObject.SetActive(true);

        //                lineW.anchorMin = lineW.anchorMax = anchorsV;
        //                lineW.pivot = pivotV;
        //                lineW.anchoredPosition = new Vector3(cellWidth * cell.Column - lineX, -cellHeight * cell.Row, 0);
        //                lineW.localScale = Vector3.one;

        //                lineW.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);
        //                lineW.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellHeight);
        //            }

        //            if (!cell.IsLinked(cell.East))
        //            {
        //                //Line East
        //                RectTransform lineE = DemoPrefabPoolers.UIImagePooler.GetFromPool<GameObject>("line ui img").GetComponent<RectTransform>();
        //                lineE.SetParent(Bg);
        //                lineE.gameObject.SetActive(true);

        //                lineE.anchorMin = lineE.anchorMax = anchorsV;
        //                lineE.pivot = pivotV;
        //                lineE.anchoredPosition = new Vector3(cellWidth * (cell.Column + 1) - lineX, -cellHeight * cell.Row, 0);
        //                lineE.localScale = Vector3.one;

        //                lineE.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);
        //                lineE.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellHeight);
        //            }

        //            if (!cell.IsLinked(cell.South))
        //            {
        //                //Line South
        //                RectTransform lineS = DemoPrefabPoolers.UIImagePooler.GetFromPool<GameObject>("line ui img").GetComponent<RectTransform>();
        //                lineS.SetParent(Bg);
        //                lineS.gameObject.SetActive(true);

        //                lineS.anchorMin = lineS.anchorMax = anchorsH;
        //                lineS.pivot = pivotH;
        //                lineS.anchoredPosition = new Vector3(cellWidth * cell.Column, -cellHeight * (cell.Row + 1) - lineY, 0);
        //                lineS.localScale = Vector3.one;

        //                lineS.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellWidth);
        //                lineS.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lineHeight);
        //            }
        //        }
        //    }

        //}
    }
}