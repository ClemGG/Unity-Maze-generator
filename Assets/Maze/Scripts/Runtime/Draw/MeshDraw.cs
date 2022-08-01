using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class MeshDraw : IDrawMethod<Color>, IDrawMethodAsync<Color>
    {

        #region Mesh Fields

        private Transform _mazeObj;

        private Transform MazeObj
        {
            get
            {
                if (!_mazeObj) _mazeObj = GameObject.Find("Maze").transform;
                return _mazeObj;
            }
        }

        private readonly Vector2 _meshCellSize = new(5f, 5f);
        private float _inset = 0f;

        private readonly List<Vector3> _newVertices = new();
        private readonly List<Vector2> _newUVs = new();
        private readonly List<int> _newTriangles = new();

        #endregion


        public MeshDraw(GenerationSettingsSO settings)
        {
            _meshCellSize = settings.MeshCellSize;
            _inset = settings.Inset;
        }

        public void Cleanup()
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


        public IEnumerator DrawAsync(IDrawableGrid<Color> grid, System.IProgress<GenerationProgressReport> progress)
        {
            GenerationProgressReport report = new();

            //We create 1 Mesh for each surface
            //so that none reach the limit of triangles allowed by Unity.
            Mesh[] meshes = new Mesh[4];
            for (int i = 0; i < meshes.Length; i++)
            {
                meshes[i] = new Mesh
                {
                    name = $"Maze mesh {i + 1}",
                    indexFormat = UnityEngine.Rendering.IndexFormat.UInt32  //Allows us to get more triangles
                };

                _newVertices.Clear();
                _newUVs.Clear();
                _newTriangles.Clear();

                GenerateMesh(i, grid);



                meshes[i].vertices = _newVertices.ToArray();
                meshes[i].uv = _newUVs.ToArray();
                meshes[i].SetTriangles(_newTriangles, 0);

                meshes[i].RecalculateNormals();

                MeshFilter mf = MazeObj.GetChild(i).GetComponent<MeshFilter>();
                MeshCollider mc = MazeObj.GetChild(i).GetComponent<MeshCollider>();
                mf.mesh = meshes[i];

                //The Floor & Ceiling meshes do not have a MeshCollider for better performances
                if (mc != null)
                    mc.sharedMesh = meshes[i];


                report.ProgressPercentage = (float)((i+1) * 100 / meshes.Length) / 100f;
                report.UpdateTrackTime(Time.deltaTime);
                progress.Report(report);

                yield return null;
            }

            //cleanup memory
            _newVertices.Clear();
            _newUVs.Clear();
            _newTriangles.Clear();
        }


        public void DrawSync(IDrawableGrid<Color> grid)
        {
            //We create 1 Mesh for each surface
            //so that none reach the limit of triangles allowed by Unity.
            Mesh[] meshes = new Mesh[4];
            for (int i = 0; i < meshes.Length; i++)
            {
                meshes[i] = new Mesh
                {
                    name = $"Maze mesh {i+1}",
                    indexFormat = UnityEngine.Rendering.IndexFormat.UInt32  //Allows us to get more triangles
                };

                _newVertices.Clear();
                _newUVs.Clear();
                _newTriangles.Clear();

                GenerateMesh(i, grid);



                meshes[i].vertices = _newVertices.ToArray();
                meshes[i].uv = _newUVs.ToArray();
                meshes[i].SetTriangles(_newTriangles, 0);

                meshes[i].RecalculateNormals();

                MeshFilter mf = MazeObj.GetChild(i).GetComponent<MeshFilter>();
                MeshCollider mc = MazeObj.GetChild(i).GetComponent<MeshCollider>();
                mf.mesh = meshes[i];

                //The Floor & Ceiling meshes do not have a MeshCollider for better performances
                if (mc != null)
                    mc.sharedMesh = meshes[i];
            }

            //cleanup memory
            _newVertices.Clear();
            _newUVs.Clear();
            _newTriangles.Clear();
        }




        private void GenerateMesh(int meshID, IDrawableGrid<Color> grid)
        {
            float cellWidth = _meshCellSize.x;
            float cellHeight = _meshCellSize.y;
            _inset = cellWidth * _inset;

            for (int i = 0; i < grid.Rows; i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    Cell cell = grid[i, j];

                    if (cell is null) continue;


                    if (!Mathf.Approximately(_inset, 0f) && !Mathf.Approximately(_inset, .5f * cellWidth))
                    {
                        float x = cell.Column * cellWidth;
                        float z = (cell.Row - grid.Rows + 1) * cellWidth;

                        switch (meshID)
                        {
                            case 0:
                                AddFloorWithInset(cell, cellWidth, x, z);
                                break;
                            case 1:
                                AddCeilingWithInset(cell, cellWidth, cellHeight, x, z);
                                break;
                            case 2:
                                AddWallsWithInset(cell, cellWidth, cellHeight, x, z);
                                break;

                                //No need for back walls when using _inset, the collisions work properly
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


        private (Vector4, Vector4) CellCoordsWithInset(float x, float z, float cellSize)
        {
            float x1 = x;
            float x4 = x + cellSize;
            float x2 = x1 + _inset;
            float x3 = x4 - _inset;

            float z1 = z;
            float z4 = z + cellSize;
            float z2 = z1 + _inset;
            float z3 = z4 - _inset;

            return (new(x1, x2, x3, x4), new(z1, z2, z3, z4));
        }




        #region With Inset

        private void AddFloorWithInset(Cell cell, float cellSize, float x, float z)
        {
            (Vector4 xc, Vector4 zc) = CellCoordsWithInset(x, z, cellSize);
            float x1 = xc.x;
            float x2 = xc.y;
            float x3 = xc.z;
            float x4 = xc.w;

            float z1 = zc.x;
            float z2 = zc.y;
            float z3 = zc.z;
            float z4 = zc.w;

            cellSize -= _inset * 2f;
            float halfCs = cellSize / 2f;
            float halfI = _inset / 2f;
            Quaternion rot = Quaternion.LookRotation(Vector3.up);

            // center
            AddQuad(
                Matrix4x4.TRS(new Vector3(x2, 0, -z2),
                              rot,
                              new Vector3(cellSize, cellSize, 1)));

            //Draws 4 imgs to fill the outer regions of the cell
            if (cell.IsLinked(cell.North))
            {
                AddQuad(
                Matrix4x4.TRS(new Vector3(x2, 0, -z1 + halfCs),
                              rot,
                              new Vector3(cellSize, _inset * 2f, 1)));
            }
            if (cell.IsLinked(cell.East))
            {
                AddQuad(
                Matrix4x4.TRS(new Vector3(x3 - halfCs + _inset, 0, -z2),
                              rot,
                              new Vector3(_inset * 2f, cellSize, 1)));
            }
        }

        private void AddCeilingWithInset(Cell cell, float cellSize, float cellHeight, float x, float z)
        {
            (Vector4 xc, Vector4 zc) = CellCoordsWithInset(x, z, cellSize);
            float x1 = xc.x;
            float x2 = xc.y;
            float x3 = xc.z;
            float x4 = xc.w;

            float z1 = zc.x;
            float z2 = zc.y;
            float z3 = zc.z;
            float z4 = zc.w;

            cellSize -= _inset * 2f;
            float halfCs = cellSize / 2f;
            float halfI = _inset / 2f;
            Quaternion rot = Quaternion.LookRotation(Vector3.down);


            // center
            AddQuad(
                Matrix4x4.TRS(new Vector3(x2, cellHeight, -z2),
                              rot,
                              new Vector3(cellSize, cellSize, 1)));

            //Draws 4 imgs to fill the outer regions of the cell
            if (cell.IsLinked(cell.North))
            {
                AddQuad(
                Matrix4x4.TRS(new Vector3(x2, cellHeight, -z1 + halfCs),
                              rot,
                              new Vector3(cellSize, _inset * 2f, 1)));
            }
            if (cell.IsLinked(cell.East))
            {
                AddQuad(
                Matrix4x4.TRS(new Vector3(x3 - halfCs + _inset, cellHeight, -z2),
                              rot,
                              new Vector3(_inset * 2f, cellSize, 1)));
            }
        }

        private void AddWallsWithInset(Cell cell, float cellWidth, float cellHeight, float x, float z)
        {
            (Vector4 xc, Vector4 zc) = CellCoordsWithInset(x, z, cellWidth);
            float x1 = xc.x;
            float x2 = xc.y;
            float x3 = xc.z;
            float x4 = xc.w;

            float z1 = zc.x;
            float z2 = zc.y;
            float z3 = zc.z;
            float z4 = zc.w;

            float doubleI = _inset * 2f;
            float cellSize = cellWidth - doubleI;
            float halfH = cellHeight / 2f;
            float halfCs = cellSize / 2f;

            Vector3 pos, size;

            if (cell.IsLinked(cell.North))
            {
                size = new(doubleI, cellHeight, 1f);
                pos = new(x2 - halfCs, halfH, -z1 + halfCs);

                //Line V
                AddQuad(Matrix4x4.TRS(
                        pos,
                        Quaternion.LookRotation(Vector3.right),
                        size));

                pos = new(x3 - halfCs, halfH, -z1 + halfCs);

                //Line V
                AddQuad(Matrix4x4.TRS(
                        pos,
                        Quaternion.LookRotation(Vector3.left),
                        size));
            }
            else
            {
                size = new(cellSize, cellHeight, 1f);
                pos = new(x2, halfH, -z2 + halfCs);

                //Line H
                AddQuad(Matrix4x4.TRS(
                        pos,
                        Quaternion.LookRotation(Vector3.back),
                        size));
            }

            if (cell.IsLinked(cell.East))
            {
                size = new(doubleI, cellHeight, 1f);
                pos = new(x3 - halfCs + _inset, halfH, -z2 + halfCs);

                //Line H
                AddQuad(Matrix4x4.TRS(
                        pos,
                        Quaternion.LookRotation(Vector3.back),
                        size));

                pos = new(x3 - halfCs + _inset, halfH, -z3 + halfCs);

                //Line H
                AddQuad(Matrix4x4.TRS(
                        pos,
                        Quaternion.LookRotation(Vector3.forward),
                        size));
            }
            else
            {
                size = new(cellSize, cellHeight, 1f);
                pos = new(x3 - halfCs, halfH, -z2);

                //Line V
                AddQuad(Matrix4x4.TRS(
                        pos,
                        Quaternion.LookRotation(Vector3.left),
                        size));
            }

            if (!cell.IsLinked(cell.South))
            {
                size = new(cellSize, cellHeight, 1f);
                pos = new(x2, halfH, -z3 + halfCs);

                //Line H
                AddQuad(Matrix4x4.TRS(
                        pos,
                        Quaternion.LookRotation(Vector3.forward),
                        size));
            }

            if (!cell.IsLinked(cell.West))
            {
                size = new(cellSize, cellHeight, 1f);
                pos = new(x2 - halfCs, halfH, -z2);

                //Line V
                AddQuad(Matrix4x4.TRS(
                        pos,
                        Quaternion.LookRotation(Vector3.right),
                        size));
            }
        }

        #endregion




        #region Without Inset

        private void AddFloorWithoutInset(Cell cell, float cellWidth, float cellHeight, int i, int j)
        {
            // floor
            AddQuad(
                Matrix4x4.TRS(new Vector3(j * cellWidth, 0, -i * cellWidth),
                              Quaternion.LookRotation(Vector3.up),
                              new Vector3(cellWidth, cellWidth, 1)));
        }

        private void AddCeilingWithoutInset(Cell cell, float cellWidth, float cellHeight, int i, int j)
        {
            // ceiling
            AddQuad(
                Matrix4x4.TRS(new Vector3(j * cellWidth, cellHeight, -i * cellWidth),
                              Quaternion.LookRotation(Vector3.down),
                              new Vector3(cellWidth, cellWidth, 1)));
        }


        private void AddWallsWithoutInset(Cell cell, float cellWidth, float cellHeight, int i, int j)
        {
            float halfH = cellHeight / 2f;


            if (cell.North is null)
            {
                //Wall North
                AddQuad(Matrix4x4.TRS(
                        new Vector3(j * cellWidth, halfH, (-i + .5f) * cellWidth),
                        Quaternion.LookRotation(Vector3.back),
                        new Vector3(cellWidth, cellHeight, 1)));

            }

            if (cell.West is null)
            {
                //Wall West
                AddQuad(Matrix4x4.TRS(
                        new Vector3((j - .5f) * cellWidth, halfH, -i * cellWidth),
                        Quaternion.LookRotation(Vector3.right),
                        new Vector3(cellWidth, cellHeight, 1)));
            }

            if (!cell.IsLinked(cell.East))
            {
                //Wall East
                AddQuad(Matrix4x4.TRS(
                        new Vector3((j + .5f) * cellWidth, halfH, -i * cellWidth),
                        Quaternion.LookRotation(Vector3.left),
                        new Vector3(cellWidth, cellHeight, 1)));
            }

            if (!cell.IsLinked(cell.South))
            {
                //Wall South
                AddQuad(Matrix4x4.TRS(
                        new Vector3(j * cellWidth, halfH, (-i - .5f) * cellWidth),
                        Quaternion.LookRotation(Vector3.forward),
                        new Vector3(cellWidth, cellHeight, 1)));
            }
        }

        private void AddBackWallsWithoutInset(Cell cell, float cellWidth, float cellHeight, int i, int j)
        {
            float halfH = cellHeight / 2f;

            //We need to create the walls twice to create backfaces for the collider to work bothways.
            //The backfaces have a cutout material to hide them, the normal walls have a double-sided one.

            if (cell.North is null)
            {
                //backface North
                AddQuad(Matrix4x4.TRS(
                        new Vector3(j * cellWidth, halfH, (-i + 1 - .5f) * cellWidth),
                        Quaternion.LookRotation(Vector3.forward),
                        new Vector3(cellWidth, cellHeight, 1)));

            }

            if (cell.West is null)
            {
                //backface West
                AddQuad(Matrix4x4.TRS(
                        new Vector3((j - 1 + .5f) * cellWidth, halfH, -i * cellWidth),
                        Quaternion.LookRotation(Vector3.left),
                        new Vector3(cellWidth, cellHeight, 1)));
            }

            if (!cell.IsLinked(cell.East))
            {
                //backface East
                AddQuad(Matrix4x4.TRS(
                        new Vector3((j + 1 - .5f) * cellWidth, halfH, -i * cellWidth),
                        Quaternion.LookRotation(Vector3.right),
                        new Vector3(cellWidth, cellHeight, 1)));
            }

            if (!cell.IsLinked(cell.South))
            {
                //backface South
                AddQuad(Matrix4x4.TRS(
                        new Vector3(j * cellWidth, halfH, (-i - 1 + .5f) * cellWidth),
                        Quaternion.LookRotation(Vector3.back),
                        new Vector3(cellWidth, cellHeight, 1)));
            }
        }

        #endregion





        private void AddQuad(Matrix4x4 matrix)
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


    }
}