﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace Assets.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SkillImpactSpawner))]
    class SkillImpactSpawnerEditor : UnityEditor.Editor
    {
        bool editOffset = false;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            editOffset = GUILayout.Toggle(editOffset, "Edit Offset", "Button");
        }
        

        private void OnSceneGUI()
        {
            var spawner = target as SkillImpactSpawner;
            if (editOffset)
            {
                Tools.current = Tool.None;

                var pos = Handles.PositionHandle(spawner.transform.position + spawner.SpawnOffset.ToVector3(), Quaternion.identity);
                var n = GridSystem.Instance.GridPerPixel;
                pos = new Vector3(Mathf.Round(pos.x / n) * n, Mathf.Round(pos.y / n) * n, pos.z);
                spawner.SpawnOffset = pos - spawner.transform.position;
            }
        }
    }
}
