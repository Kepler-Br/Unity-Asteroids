using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class CustomTimer
{
    [SerializeField]
    float _timer;
    float _nextTime;

    public float GetTime()
    {
        return _timer;
    }

    public float GetNormalizedRemainingTime()
    {
        var currentTime = Time.fixedTime;
        var timeLeft = _nextTime - currentTime;
        if (timeLeft <= 0.0f) return 1.0f;
        return 1.0f - timeLeft / _timer;
    }

    public float GetRemainingTime()
    {
        var currentTime = Time.fixedTime;
        var timeLeft = _nextTime - currentTime;
        return timeLeft;
    }

    public CustomTimer(float timer)
    {
        _timer = timer;
        var currentTime = Time.fixedTime;
        _nextTime = currentTime + _timer;
    }

    public bool TryReset()
    {
        var currentTime = Time.fixedTime;
        if (currentTime >= _nextTime)
        {
            _nextTime = currentTime + _timer;
            return true;
        }
        return false;
    }

    public void Reset()
    {
        _nextTime = Time.fixedTime + _timer;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(CustomTimer))]
internal class CooldownTimerDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var icon = EditorGUIUtility.IconContent("SpeedScale").image;
        position.width -= 18;
        var prop = property.FindPropertyRelative("_timer");
        EditorGUI.PropertyField(position, prop, new GUIContent(label.text, icon, "Cooldown measured in seconds."));

        position.x = position.width + 13 - EditorGUI.indentLevel * 15;
        EditorGUI.LabelField(position, "sec");
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 16;
    }
}
#endif
