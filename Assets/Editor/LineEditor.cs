using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LineCreator))]
public class LineEditor : Editor
{
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
        if (creator.autoupdate && guiEvent.type == EventType.Repaint)
            creator.UpdateMesh();

        HandleUtility.AddDefaultControl(0);

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

            float lineWidth = creator.lineWidth;
            normal *= lineWidth / 2.0f;
            Handles.DrawLine(pointOne + normal, pointOne - normal);
            Handles.DrawLine(pointOne + normal, pointTwo + normal);
            Handles.DrawLine(pointOne - normal, pointTwo - normal);
            Handles.DrawLine(pointTwo + normal, pointTwo - normal);
        }
        for (int i = 0; i < lines.Count - 2; i++)
        {
            Vector3 pointOne = lines[i];
            Vector3 pointTwo = lines[i + 1];
            Vector3 pointThree = lines[i + 2];

            Vector3 normalOne = pointTwo - pointOne;
            Vector3 normalTwo = pointThree - pointTwo;
            // normalOne.Normalize();
            // normalTwo.Normalize();
            normalOne = new Vector3(-normalOne.y, normalOne.x, 0.0f);
            normalTwo = new Vector3(-normalTwo.y, normalTwo.x, 0.0f);
            normalOne.Normalize();
            normalTwo.Normalize();
            Vector3 result = normalOne + normalTwo;
            result.Normalize();

            Handles.color = Color.red;
            Handles.DrawLine(pointTwo+result, pointTwo);

            Handles.color = Color.blue;
            Handles.DrawLine(pointTwo+normalOne, pointTwo);

            Handles.color = Color.magenta;
            Handles.DrawLine(pointTwo+normalTwo, pointTwo);
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
}
