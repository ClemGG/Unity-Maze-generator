using UnityEngine;
using UnityEditor;

//Necessary to display the Scriptable Object inside it
[CanEditMultipleObjects]
[CustomEditor(typeof(MonoBehaviour), true)]
public class MonoBehaviourEditor : Editor
{
}
