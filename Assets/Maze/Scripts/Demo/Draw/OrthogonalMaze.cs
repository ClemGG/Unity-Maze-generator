using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Procedural.MazeGeneration
{
    //Draws square-shaped mazes (gamma-type mazes).
    //Used for 2D & 3D.
    public static class OrthogonalMaze
    {

        #region UI Fields
        private static Canvas _canvas;
        private static RectTransform _bg;
        private static Transform _tiles;
        private static Transform _lines;
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

        #endregion

        #region Mesh Fields

        private static Transform _mazeObj;
        
        private static Transform MazeObj
        {
            get
            {
                if (!_mazeObj) _mazeObj = GameObject.Find("Maze").transform;
                return _mazeObj;
            }
        }

        //Quick hack to set the size directly from the Inspector.
        //TODO : Refactor to remove this
        public static Vector2 MeshCellSize = new(5f, 5f);


        private static readonly List<Vector3> _newVertices = new();
        private static readonly List<Vector2> _newUVs = new();
        private static readonly List<int> _newTriangles = new();

        #endregion





        private static (Vector4, Vector4) CellCoordsWithInset(float x, float y, float cellSize, float inset)
        {
            float x1 = x;
            float x4 = x + cellSize;
            float x2 = x1 + inset;
            float x3 = x4 - inset;

            float y1 = y;
            float y4 = y + cellSize;
            float y2 = y1 + inset;
            float y3 = y4 - inset;

            return (new(x1, x2, x3, x4), new(y1, y2, y3, y4));
        }





        #region 3D Mesh

        public static void CleanupMesh()
        {
            for (int i = 0; i < MazeObj.childCount; i++)
            {
                MeshFilter mf = MazeObj.GetChild(i).GetComponent<MeshFilter>();
                MeshCollider mc = MazeObj.GetChild(i).GetComponent<MeshCollider>();

                if (mf.sharedMesh is not null)
                    Object.DestroyImmediate(mf.sharedMesh);

                //The Floor & Ceiling meshes do not have a MeshCollider for better performances
                if (mc == null) continue;
                if (mc.sharedMesh is not null)
                        Object.DestroyImmediate(mc.sharedMesh);
            }
        }


        public static void DisplayOnMesh(Grid grid, float inset = 0f)
        {
            //We create 1 Mesh for each surface
            //so that none reach the limit of triangles allowed by Unity.
            Mesh[] meshes = new Mesh[4];
            for (int i = 0; i < meshes.Length; i++)
            {
                meshes[i] = new Mesh
                {
                    name = "Generated maze mesh"
                };


                _newVertices.Clear();
                _newUVs.Clear();
                _newTriangles.Clear();

                GenerateMesh(i, grid, inset);



                meshes[i].vertices = _newVertices.ToArray();
                meshes[i].uv = _newUVs.ToArray();
                meshes[i].SetTriangles(_newTriangles, 0);

                meshes[i].RecalculateNormals();

                MeshFilter mf = MazeObj.GetChild(i).GetComponent<MeshFilter>();
                MeshCollider mc = MazeObj.GetChild(i).GetComponent<MeshCollider>();
                mf.mesh = meshes[i];

                //The Floor & Ceiling meshes do not have a MeshCollider for better performances
                if(mc != null)
                    mc.sharedMesh = meshes[i];
            }

            //cleanup memory
            _newVertices.Clear();
            _newUVs.Clear();
            _newTriangles.Clear();
        }

        private static void GenerateMesh(int meshID, Grid grid, float inset)
        {
            float cellWidth = MeshCellSize.x;
            float cellHeight = MeshCellSize.y;
            inset = cellWidth * inset;

            for (int i = 0; i < grid.Rows; i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    Cell cell = grid[i, j];

                    if (cell is null) continue;


                    if (!Mathf.Approximately(inset, 0f) && !Mathf.Approximately(inset, .5f * cellWidth))
                    {
                        float x = cell.Column * cellWidth;
                        float z = (cell.Row - grid.Rows + 1) * cellWidth;

                        switch (meshID)
                        {
                            case 0:
                                AddFloorWithInset(cell, cellWidth, x, z, inset);
                                break;
                            case 1:
                                AddCeilingWithInset(cell, cellWidth, x, z, inset);
                                break;
                            case 2:
                                AddWallsWithInset(cell, cellWidth, cellHeight, x, z, inset);
                                break;
                            case 3:
                                AddBackWallsWithInset(cell, cellWidth, cellHeight, x, z, inset);
                                break;
                        }
                    }
                    else
                    {
                        switch (meshID)
                        {
                            case 0:
                                AddFloorWithoutInset(cell, cellWidth, cellHeight, i - grid.Rows + 1, j);
                                break;
                            case 1:
                                AddCeilingWithoutInset(cell, cellWidth, cellHeight, i - grid.Rows + 1, j);
                                break;
                            case 2:
                                AddWallsWithoutInset(cell, cellWidth, cellHeight, i - grid.Rows + 1, j);
                                break;
                            case 3:
                                AddBackWallsWithoutInset(cell, cellWidth, cellHeight, i - grid.Rows + 1, j);
                                break;
                        }
                    }
                }
            }
        }





        #region With Inset

        private static void AddFloorWithInset(Cell cell, float cellSize, float x, float z, float inset)
        {
            (Vector4 xc, Vector4 zc) = CellCoordsWithInset(x, z, cellSize, inset);
            float x1 = xc.x;
            float x2 = xc.y;
            float x3 = xc.z;
            float x4 = xc.w;

            float z1 = zc.x;
            float z2 = zc.y;
            float z3 = zc.z;
            float z4 = zc.w;

            cellSize -= inset * 2f;
            float halfCs = cellSize / 2f;
            float halfI = inset / 2f;

            // center
            AddQuad(
                Matrix4x4.TRS(new Vector3(x2, 0, -z2),
                              Quaternion.LookRotation(Vector3.up),
                              new Vector3(cellSize, cellSize, 1)));

            //Draws 4 imgs to fill the outer regions of the cell
            if (cell.IsLinked(cell.North))
            {
                AddQuad(
                Matrix4x4.TRS(new Vector3(x2, 0, -z1 + halfCs - halfI),
                              Quaternion.LookRotation(Vector3.up),
                              new Vector3(cellSize, inset, 1)));
            }
            if (cell.IsLinked(cell.West))
            {
                AddQuad(
                Matrix4x4.TRS(new Vector3(x1 - halfCs + halfI, 0, -z2),
                              Quaternion.LookRotation(Vector3.up),
                              new Vector3(inset, cellSize, 1)));
            }
            if (cell.IsLinked(cell.East))
            {
                AddQuad(
                Matrix4x4.TRS(new Vector3(x3 - halfCs + halfI, 0, -z2),
                              Quaternion.LookRotation(Vector3.up),
                              new Vector3(inset, cellSize, 1)));
            }
            if (cell.IsLinked(cell.South))
            {
                AddQuad(
                Matrix4x4.TRS(new Vector3(x2, 0, -z3 + halfCs - halfI),
                              Quaternion.LookRotation(Vector3.up),
                              new Vector3(cellSize, inset, 1)));
            }
        }

        private static void AddCeilingWithInset(Cell cell, float cellSize, float x, float y, float inset)
        {

        }

        private static void AddWallsWithInset(Cell cell, float cellWidth, float cellHeight, float x, float y, float inset)
        {

        }

        private static void AddBackWallsWithInset(Cell cell, float cellWidth, float cellHeight, float x, float y, float inset)
        {

        }

        #endregion




        #region Without Inset

        private static void AddFloorWithoutInset(Cell cell, float cellWidth, float cellHeight, int i, int j)
        {
            // floor
            AddQuad(
                Matrix4x4.TRS(new Vector3(j * cellWidth, 0, -i * cellWidth),
                              Quaternion.LookRotation(Vector3.up),
                              new Vector3(cellWidth, cellWidth, 1)));
        }

        private static void AddCeilingWithoutInset(Cell cell, float cellWidth, float cellHeight, int i, int j)
        {
            // ceiling
            AddQuad(
                Matrix4x4.TRS(new Vector3(j * cellWidth, cellHeight, -i * cellWidth),
                              Quaternion.LookRotation(Vector3.down),
                              new Vector3(cellWidth, cellWidth, 1)));
        }


        private static void AddWallsWithoutInset(Cell cell, float cellWidth, float cellHeight, int i, int j)
        {
            float halfH = cellHeight / 2f;

            if (cell.North is null)
            {
                //Wall North
                AddQuad(Matrix4x4.TRS(
                        new Vector3(j * cellWidth, halfH, (-i + 1 - .5f) * cellWidth),
                        Quaternion.LookRotation(Vector3.forward),
                        new Vector3(cellWidth, cellHeight, 1)));

            }

            if (cell.West is null)
            {
                //Wall West
                AddQuad(Matrix4x4.TRS(
                        new Vector3((j - 1 + .5f) * cellWidth, halfH, -i * cellWidth),
                        Quaternion.LookRotation(Vector3.left),
                        new Vector3(cellWidth, cellHeight, 1)));
            }

            if (!cell.IsLinked(cell.East))
            {
                //Wall East
                AddQuad(Matrix4x4.TRS(
                        new Vector3((j + 1 - .5f) * cellWidth, halfH, -i * cellWidth),
                        Quaternion.LookRotation(Vector3.right),
                        new Vector3(cellWidth, cellHeight, 1)));
            }

            if (!cell.IsLinked(cell.South))
            {
                //Wall South
                AddQuad(Matrix4x4.TRS(
                        new Vector3(j * cellWidth, halfH, (-i - 1 + .5f) * cellWidth),
                        Quaternion.LookRotation(Vector3.back),
                        new Vector3(cellWidth, cellHeight, 1)));
            }
        }

        private static void AddBackWallsWithoutInset(Cell cell, float cellWidth, float cellHeight, int i, int j)
        {
            float halfH = cellHeight / 2f;

            //We need to create the walls twice to create backfaces for the collider to work bothways.
            //The backfaces have a cutout material to hide them, the normal walls have a double-sided one.

            if (cell.North is null)
            {
                //backface North
                AddQuad(Matrix4x4.TRS(
                        new Vector3(j * cellWidth, halfH, (-i + 1 - .5f) * cellWidth),
                        Quaternion.LookRotation(Vector3.back),
                        new Vector3(cellWidth, cellHeight, 1)));

            }

            if (cell.West is null)
            {
                //backface West
                AddQuad(Matrix4x4.TRS(
                        new Vector3((j - 1 + .5f) * cellWidth, halfH, -i * cellWidth),
                        Quaternion.LookRotation(Vector3.right),
                        new Vector3(cellWidth, cellHeight, 1)));
            }

            if (!cell.IsLinked(cell.East))
            {
                //backface East
                AddQuad(Matrix4x4.TRS(
                        new Vector3((j + 1 - .5f) * cellWidth, halfH, -i * cellWidth),
                        Quaternion.LookRotation(Vector3.left),
                        new Vector3(cellWidth, cellHeight, 1)));
            }

            if (!cell.IsLinked(cell.South))
            {
                //backface South
                AddQuad(Matrix4x4.TRS(
                        new Vector3(j * cellWidth, halfH, (-i - 1 + .5f) * cellWidth),
                        Quaternion.LookRotation(Vector3.forward),
                        new Vector3(cellWidth, cellHeight, 1)));
            }
        }

        #endregion





        private static void AddQuad(Matrix4x4 matrix)
        {
            int index = _newVertices.Count;

            // corners before transforming
            Vector3 vert1 = new(-.5f, -.5f, 0);
            Vector3 vert2 = new(-.5f, .5f, 0);
            Vector3 vert3 = new(.5f, .5f, 0);
            Vector3 vert4 = new(.5f, -.5f, 0);


            //The matrix allows us to rotate the quads to fit walls or ceiling
            _newVertices.Add(matrix.MultiplyPoint3x4(vert1));
            _newVertices.Add(matrix.MultiplyPoint3x4(vert2));
            _newVertices.Add(matrix.MultiplyPoint3x4(vert3));
            _newVertices.Add(matrix.MultiplyPoint3x4(vert4));

            _newUVs.Add(new(1, 0));
            _newUVs.Add(new(1, 1));
            _newUVs.Add(new(0, 1));
            _newUVs.Add(new(0, 0));

            _newTriangles.Add(index + 2);
            _newTriangles.Add(index + 1);
            _newTriangles.Add(index);
            
            _newTriangles.Add(index + 3);
            _newTriangles.Add(index + 2);
            _newTriangles.Add(index);
        }

        #endregion


        #region UI


        public static void CleanupUI()
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


        public static void DisplayOnUI(Grid grid, float inset = 0f)
        {
            //cleanup pooler.
            //stored temp. in an array to avoid Bg's resizing
            int nbCells = Tiles.childCount;
            int nbLines = Lines.childCount;
            List<Transform> children = new(nbCells + nbLines);
            for (int i = 0; i < nbCells; i++)
            {
                children.Add(Tiles.GetChild(i));
            }
            for (int i = 0; i < nbLines; i++)
            {
                children.Add(Lines.GetChild(i));
            }
            for (int i = 0; i < children.Count; i++)
            {
                Transform child = children[i];
                child.SetParent(ImgHolder);
                child.gameObject.SetActive(false);
                MazePrefabs.UIImagePooler.ReturnToPool(child.gameObject, child.name.Replace("(Clone)", ""));
            }



            float cellSize = Mathf.Min(Bg.rect.width / grid.Columns, Bg.rect.height / grid.Rows);
            inset = cellSize * inset;

            //Used to resize the lineUIs if they get too big for the grid resolution
            float gridLongestSide = Mathf.Max(grid.Columns, grid.Rows);

            for (int i = 0; i < grid.Rows; i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    Cell cell = grid[i, j];

                    if (cell is null) continue;

                    if (!Mathf.Approximately(inset, 0f) && !Mathf.Approximately(inset, .5f * cellSize))
                    {
                        float x = cell.Column * cellSize;
                        float y = cell.Row * cellSize;

                        Color color = grid.BackgroundColorFor(cell);
                        DisplayCellImgWithInset(cell, cellSize, x, y, inset, color);
                        DisplayLineImgWithInset(cell, cellSize, x, y, inset, gridLongestSide);
                    }
                    else
                    {
                        Color color = grid.BackgroundColorFor(cell);
                        DisplayCellImgWithoutInset(i, j, cellSize, color);
                        DisplayLineImgWithoutInset(cell, cellSize, gridLongestSide);
                    }
                }
            }


        }



        private static void DisplayCellImgWithInset(Cell cell, float cellSize, float x, float y, float inset, Color color)
        {
            (Vector4 xc, Vector4 yc) = CellCoordsWithInset(x, y, cellSize, inset);
            float x1 = xc.x;
            float x2 = xc.y;
            float x3 = xc.z;
            float x4 = xc.w;

            float y1 = yc.x;
            float y2 = yc.y;
            float y3 = yc.z;
            float y4 = yc.w;

            cellSize -= inset * 2f;

            //Draws the img for the center of the cell
            DrawCell(new Vector2(cellSize, cellSize),
                new Vector3(x2, -y2, 0),
                //Color.black);
                color);


            //Draws 4 imgs to fill the outer regions of the cell
            if (cell.IsLinked(cell.North))
            {
                DrawCell(new Vector2(cellSize, inset),
                new Vector3(x2, -y1, 0),
                //Color.red);
                color);
            }
            if (cell.IsLinked(cell.West))
            {
                DrawCell(new Vector2(inset, cellSize),
                new Vector3(x1, -y2, 0),
                //Color.blue);
                color);
            }
            if (cell.IsLinked(cell.East))
            {
                DrawCell(new Vector2(inset, cellSize),
                new Vector3(x3, -y2, 0),
                //Color.yellow);
                color);
            }
            if (cell.IsLinked(cell.South))
            {
                DrawCell(new Vector2(cellSize, inset),
                new Vector3(x2, -y3, 0),
                //Color.green);
                color);
            }
        }


        private static void DisplayLineImgWithInset(Cell cell, float cellSize, float x, float y, float inset, float gridLongestSize)
        {
            //width and height of the UI Image in pixels
            //TODO : Scale these sizes for smaller cells
            float lineThickness = Mathf.Lerp(5f, 1f, inset / cellSize / 0.5f);
            lineThickness = Mathf.Min(lineThickness, Mathf.Lerp(5f, 1f, gridLongestSize / 100f));

            Vector2 anchorH = new(0f, 1f);
            Vector2 pivotH = new(0f, 1f);
            Vector2 anchorV = new(0f, 1f);
            Vector2 pivotV = new(0.5f, 1f);

            (Vector4 xc, Vector4 yc) = CellCoordsWithInset(x, y, cellSize, inset);
            float x1 = xc.x;
            float x2 = xc.y;
            float x3 = xc.z;
            float x4 = xc.w;

            float y1 = yc.x;
            float y2 = yc.y;
            float y3 = yc.z;
            float y4 = yc.w;

            cellSize -= inset * 2f;

            Vector3 pos, size;

            if (cell.IsLinked(cell.North))
            {
                size = new(lineThickness, inset + lineThickness);
                pos = new(x2, -y1);

                //Line V
                DrawLine(anchorV, pivotV, size, pos);

                pos = new(x3, -y1);

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
            if (cell.IsLinked(cell.South))
            {
                size = new(lineThickness, inset + lineThickness);
                pos = new(x2, -y3);

                //Line V
                DrawLine(anchorV, pivotV, size, pos);

                pos = new(x3, -y3);

                //Line V
                DrawLine(anchorV, pivotV, size, pos);
            }
            else
            {

                size = new(cellSize, lineThickness);
                pos = new(x2, -y3);

                //Line H
                DrawLine(anchorH, pivotH, size, pos);
            }
            if (cell.IsLinked(cell.West))
            {
                size = new(inset, lineThickness);
                pos = new(x1, -y2);

                //Line H
                DrawLine(anchorH, pivotH, size, pos);

                pos = new(x1, -y3);

                //Line H
                DrawLine(anchorH, pivotH, size, pos);
            }
            else
            {

                size = new(lineThickness, cellSize + lineThickness);
                pos = new(x2, -y2);

                //Line V
                DrawLine(anchorV, pivotV, size, pos);
            }
            if (cell.IsLinked(cell.East))
            {
                size = new(inset, lineThickness);
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
        }


        private static void DisplayCellImgWithoutInset(int i, int j, float cellSize, Color color)
        {
            DrawCell(new Vector2(cellSize, cellSize),
                new Vector3(cellSize * j, -cellSize * i, 0),
                color);
        }
        
        
        private static void DisplayLineImgWithoutInset(Cell cell, float cellSize, float gridLongestSize)
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



        private static void DrawCell(Vector2 size, Vector3 anchoredPos, Color col)
        {
            RectTransform cellImg = MazePrefabs.UIImagePooler.GetFromPool<GameObject>("cell ui img").GetComponent<RectTransform>();
            cellImg.SetParent(Tiles);
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
            RectTransform line = MazePrefabs.UIImagePooler.GetFromPool<GameObject>("line ui img").GetComponent<RectTransform>();
            line.SetParent(Lines);
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