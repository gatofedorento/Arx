﻿using AnimatorSequencer.States.ControlStates;
using CommonEditors.Nodes.Framework;
using CommonEditors.Nodes.Framework.CanvasSaveObjects;
using CommonEditors.Nodes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer.Nodes.Nodes.ActionSequence.Control
{
    [Serializable]
    [Node(false, "Control/Wait")]
    public class WaitNode : BaseActionSequenceNode<WaitState>
    {
        public const string ID = "waitNode";
        public override string GetID { get { return ID; } }

        public float value = 1f;

        public WaitNode() : base(CreateInstance<WaitState>())
        {
        }

        public override Node Create(Vector2 pos)
        { // This function has to be registered in Node_Editor.ContextCallback
            var node = CreateInstance<WaitNode>();

            node.name = "Wait Node";
            node.rect = new Rect(pos.x, pos.y, 200, 50); ;

            NodeInput.Create(node, "Value", "Float");
            NodeOutput.Create(node, "Value", "Float");

            return node;
        }

        protected override void NodeGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            float val = 0;

            if (Inputs[0].connection != null)
                GUILayout.Label(Inputs[0].name);
            else
                val = RTEditorGUI.FloatField(GUIContent.none, val);
            InputKnob(0);

            GUILayout.EndVertical();
            GUILayout.BeginVertical();

            Outputs[0].DisplayLayout();

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            if (GUI.changed)
                NodeEditor.RecalculateFrom(this);
        }

        public override bool Calculate()
        {
            Outputs[0].SetValue<float>(value);
            return true;
        }
    }
}
