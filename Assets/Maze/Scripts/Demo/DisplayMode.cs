using UnityEngine;
using UnityEngine.UI;

namespace Project.Procedural.MazeGeneration
{

    public enum DisplayMode : byte
    {
        Print,
        UIAscii,
        UIImage,
        Sprite,
        ThreeD,
    }


    public static class GridDisplayer
    {
        public static void DisplayGrid(this Grid grid, DisplayMode mode)
        {
            switch (mode)
            {
#if UNITY_EDITOR
                case DisplayMode.Print:
                    ClearConsole();
                    Debug.Log(grid.ToString());
                    break;
#endif
                case DisplayMode.UIImage:
                    DisplayOnUI(grid);
                    break;
            }
        }



#if UNITY_EDITOR
        static void ClearConsole()
        {
            // This simply does "LogEntries.Clear()" the long way:
            var logEntries = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            clearMethod.Invoke(null, null);
        }
#endif


        private static void DisplayOnUI(Grid grid)
        {
            Canvas canvas = Object.FindObjectOfType<Canvas>();
            RectTransform bg = (RectTransform)canvas.transform.GetChild(0);

            //cleanup pooler.
            //stored temp. in an array to avoid bg's resizing
            Transform[] children = new Transform[bg.childCount];
            Transform imgHolder = bg.parent.GetChild(1);
            for (int i = 0; i < bg.childCount; i++)
            {
                children[i] = bg.GetChild(i);
            }
            for (int i = 0; i < children.Length; i++)
            {
                Transform child = children[i];
                child.SetParent(imgHolder);
                child.gameObject.SetActive(false);
                DemoPrefabPoolers.UIImagePooler.ReturnToPool(child.gameObject, child.name.Replace("(Clone)", ""));
            }

            float cellWidth = bg.rect.width / grid.Columns;
            float cellHeight = bg.rect.height / grid.Rows;

            //Scale shortest dimension to not overlap too much with cell img
            float lineX = .5f;
            float lineY = .5f;
            float lineWidth = 5f;
            float lineHeight = 5f;

            //Spawn cell imgs
            for (int i = 0; i <= grid.Rows; i++)
            {
                for (int j = 0; j <= grid.Columns; j++)
                {
                    Cell curCell = grid[i, j];

                    if (j < grid.Columns && i < grid.Rows)
                    {

                        RectTransform cell = DemoPrefabPoolers.UIImagePooler.GetFromPool<GameObject>("cell ui img").GetComponent<RectTransform>();
                        cell.SetParent(bg);
                        cell.gameObject.SetActive(true);
                        //cell.name = $"cell #{cell.GetSiblingIndex()}";

                        cell.anchorMin = cell.anchorMax = new Vector2(0f, 1f);
                        cell.pivot = new Vector2(0f, 1f);
                        cell.anchoredPosition = new Vector3(cellWidth * j, -cellHeight * i, 0);

                        cell.localScale = Vector3.one;

                        cell.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellWidth);
                        cell.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellHeight);
                    }



                    if (j < grid.Columns)
                    {
                        //bool shouldDrawLineH = curCell.North is null || !curCell.IsLinked(curCell.South);

                        //Horizontal lines
                        RectTransform lineH = DemoPrefabPoolers.UIImagePooler.GetFromPool<GameObject>("line ui img").GetComponent<RectTransform>();
                        lineH.SetParent(bg);
                        lineH.gameObject.SetActive(true);

                        lineH.anchorMin = lineH.anchorMax = new Vector2(0f, 1f);
                        lineH.pivot = new Vector2(0f, 0.5f);
                        lineH.anchoredPosition = new Vector3(cellWidth * j, -cellHeight * i - lineY, 0);

                        lineH.localScale = Vector3.one;

                        lineH.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellWidth);
                        lineH.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lineHeight);
                    }

                    if (i < grid.Rows)
                    {
                        //bool shouldDrawLineV = curCell.West is null || !curCell.IsLinked(curCell.East);

                        //Vertical lines
                        RectTransform lineV = DemoPrefabPoolers.UIImagePooler.GetFromPool<GameObject>("line ui img").GetComponent<RectTransform>();
                        lineV.SetParent(bg);
                        lineV.gameObject.SetActive(true);

                        lineV.anchorMin = lineV.anchorMax = new Vector2(0f, 1f);
                        lineV.pivot = new Vector2(0.5f, 1f);
                        lineV.anchoredPosition = new Vector3(cellWidth * j - lineX, -cellHeight * i, 0);

                        lineV.localScale = Vector3.one;

                        lineV.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);
                        lineV.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellHeight);
                    }
                }

            }

        }
    }
}