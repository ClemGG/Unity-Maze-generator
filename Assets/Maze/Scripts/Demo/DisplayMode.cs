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

            //cleanup pooler
            for (int i = 0; i < bg.childCount; i++)
            {
                Transform child = bg.GetChild(i);
                DemoPrefabPoolers.UIImagePooler.ReturnToPool(child.gameObject, child.name.Replace("(Clone)", ""));
            }

            float imgWidth = bg.rect.width / grid.Columns;
            float imgHeight = bg.rect.height / grid.Rows;

            for (int i = 0; i < grid.Rows; i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    Cell curCell = grid[i, j];

                    RectTransform cell = DemoPrefabPoolers.UIImagePooler.GetFromPool<GameObject>("cell ui img").GetComponent<RectTransform>();
                    cell.SetParent(bg);
                    //cell.name = $"cell #{cell.GetSiblingIndex()}";

                    cell.anchorMin = cell.anchorMax = new Vector2(0f, 1f);
                    cell.pivot = new Vector2(0f, 1f);
                    cell.anchoredPosition = new Vector3(imgWidth * j, -imgHeight * i, 0);

                    cell.localScale = Vector3.one;

                    cell.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imgWidth);
                    cell.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imgHeight);
                }
                
            }
        }
    }
}