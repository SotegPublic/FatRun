using Runner.Core;
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(AnimatedButton))]
public class AnimatedButtonEditor : ButtonEditor
{
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();

        var idleSprite = new PropertyField(serializedObject.FindProperty(AnimatedButton.IdleSpriteFieldName));
        var buttonDownSprite = new PropertyField(serializedObject.FindProperty(AnimatedButton.ButtonDownSpriteFieldName));
        var buttonImage = new PropertyField(serializedObject.FindProperty(AnimatedButton.ButtonImageFieldName));
        var label = new Label("Animation Settings");

        root.Add(new IMGUIContainer(OnInspectorGUI));
        root.Add(label);
        root.Add(idleSprite);
        root.Add(buttonDownSprite);
        root.Add(buttonImage);

        return root;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        serializedObject.ApplyModifiedProperties();
    }
}
