using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LineCreator))]
public class LineEditor : Editor
{
    [Range(0.0f, 10.0f)]
    public float lineWidth = 1.0f;
    LineCreator creator;
    List<Vector3> lines;

    const float controlPointDiameter = .1f;

    void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Vector3 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        mousePos.z = 0.0f;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
            OnInputAddPoint(mousePos);
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
            OnInputDeletePoint(mousePos);

    }

    void OnInputAddPoint(Vector3 mousePos)
    {
        Undo.RecordObject(creator, "Add segment");
        lines.Add(mousePos);
    }

    void OnInputDeletePoint(Vector3 mousePos)
    {
        if (lines.Count == 2)
            return;
        int closestAnchorIndex = -1;
        float minDstToControlPoint = controlPointDiameter * .5f;
        for (int i = 0; i < lines.Count; i++)
        {
            float distance = Vector2.Distance(mousePos, lines[i]);
            if (distance < minDstToControlPoint)
            {
                minDstToControlPoint = distance;
                closestAnchorIndex = i;
            }
        }
        if (closestAnchorIndex != -1)
        {
            Undo.RecordObject(creator, "Delete segment");
            lines.RemoveAt(closestAnchorIndex);
        }
    }

    void Draw()
    {
        for (int i = 0; i < lines.Count - 1; i++)
        {
            Vector3 pointOne = lines[i];
            Vector3 pointTwo = lines[i + 1];
            
            Vector3 normal = pointTwo - pointOne;
            normal.Normalize();
            normal = new Vector3(-normal.y, normal.x, 0.0f);
            Handles.color = Color.black;
            Handles.DrawLine(pointOne, pointTwo);
            Handles.color = Color.yellow;
            Handles.DrawLine(pointOne, pointOne+normal);
        }
        Handles.color = Color.red;
        for (int i = 0; i < lines.Count; i++)
        {
            Vector3 newPos = Handles.FreeMoveHandle(lines[i], Quaternion.identity, controlPointDiameter, Vector3.zero, Handles.CylinderHandleCap);
            if (lines[i] != newPos)
            {
                Undo.RecordObject(creator, "Move point");
                lines[i] = newPos;
            }
        }
    }

    void OnEnable()
    {
        creator = (LineCreator)target;
        if (creator.lines == null)
        {
            creator.CreateLines();
        }
        lines = creator.lines;
    }


    Vector3 VectorFromPoints(Vector3 pointOne, Vector3 pointTwo)
    {
        return pointTwo - pointOne;
    }

    Vector3 GetPerpendicularVector(Vector3 vector)
    {
        Vector3 rotatedVector = Vector3.zero;
        const float angle = Mathf.PI / 2;
        rotatedVector.x = vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle);
        rotatedVector.y = vector.x * Mathf.Sin(angle) - vector.y * Mathf.Cos(angle);
        return rotatedVector;
    }
    void OnDrawGizmos()
    {
        Vector3 pointOne = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 pointTwo = new Vector3(3.0f, 10.0f, 0.0f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pointOne, 0.1f);
        Gizmos.DrawSphere(pointTwo, 0.1f);
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(pointOne, pointTwo);

        Vector3 vec = GetPerpendicularVector(VectorFromPoints(pointOne, pointTwo));
        vec.Normalize();
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pointOne, vec * lineWidth);
    }
}
