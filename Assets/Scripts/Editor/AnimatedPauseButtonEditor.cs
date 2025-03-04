using Runner.Core;
using Runner.UI;
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(AnimatedPauseButton))]
public class AnimatedPauseButtonEditor : ButtonEditor
{
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();

        var playIdleSprite = new PropertyField(serializedObject.FindProperty(AnimatedPauseButton.PlaySpriteFieldName));
        var pauseIdleSprite = new PropertyField(serializedObject.FindProperty(AnimatedPauseButton.PauseSpriteFieldName));
        var buttonImage = new PropertyField(serializedObject.FindProperty(AnimatedPauseButton.ButtonImageFieldName));
        var label = new Label("Animation Settings");

        root.Add(new IMGUIContainer(OnInspectorGUI));
        root.Add(label);
        root.Add(playIdleSprite);
        root.Add(pauseIdleSprite);
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
