using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseScriptObject : ScriptableObject
{
    public string id;
    protected void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(id))
        {
            AssignNewUID();
        }
#endif
    }

    protected void Reset()
    {
        AssignNewUID();
    }

    protected void AssignNewUID()
    {
        id = System.Guid.NewGuid().ToString();
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    [Button(ButtonSizes.Gigantic)]
    private void ResetId()
    {
        AssignNewUID();
    }
}
