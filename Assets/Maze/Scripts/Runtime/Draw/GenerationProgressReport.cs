
/* This class holds the progress status for the current async task.
 * Used to report the progress rate for the drawing of the maze.
 */
using UnityEngine;

public class GenerationProgressReport
{
    //From 0 to 1
    public float ProgressPercentage { get; set; } = 0f;
}
