using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public interface IDraw
    {
        void Draw(Grid grid);
    }
}