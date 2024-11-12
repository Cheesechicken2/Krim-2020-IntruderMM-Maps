#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NudgeZone))]
public class NudgeZoneEditor : Editor
{
    // Custom textures
    private Texture2D buttonNormal;
    private Texture2D buttonHover;
    private Texture2D buttonActive;
    private Texture2D boxTexture;
    private GUIStyle buttonStyle;
    private GUIStyle titleLabelStyle;
    private Font customFont;

    private void OnEnable()
    {
        // Load assets, but do not use GUI functions here
        buttonNormal = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/IntruderMM/Scripts/Extension/Editor/GUI/button.png");
        buttonHover = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/IntruderMM/Scripts/Extension/Editor/GUI/buttonHover.png");
        buttonActive = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/IntruderMM/Scripts/Extension/Editor/GUI/buttonSelect.png");
        boxTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/IntruderMM/Scripts/Extension/Editor/GUI/box.png");

        // Load the custom font
        customFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/IntruderMM/Scripts/Extension/Editor/GUI/Font/ShareTechMono-Regular.ttf");
    }

    public override void OnInspectorGUI()
    {
        NudgeZone nudgeZone = (NudgeZone)target;

        // Initialize custom button styles here
        if (buttonStyle == null)
        {
            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                font = customFont,
                fontSize = 12,
                normal = { background = buttonNormal, textColor = Color.white },
                hover = { background = buttonHover, textColor = Color.white },
                active = { background = buttonActive, textColor = Color.white },
                alignment = TextAnchor.MiddleCenter,
                border = new RectOffset(12, 12, 12, 12) // Padding around button
            };
        }

        // Title label style
        if (titleLabelStyle == null)
        {
            titleLabelStyle = new GUIStyle(GUI.skin.label)
            {
                font = customFont,
                fontSize = 16,
                normal = { textColor = Color.white },
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(0, 0, 10, 10) // Spacing around the label
            };
        }

        // Box background
        GUIStyle boxStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { background = boxTexture },
            border = new RectOffset(10, 10, 10, 10),
            padding = new RectOffset(10, 10, 10, 10)
        };

        // Begin vertical layout for the entire editor
        GUILayout.BeginVertical(boxStyle);

        // Title label
        GUILayout.Space(10);
        GUILayout.Label("Nudge Zone Settings", titleLabelStyle);
        GUILayout.Space(10);

        // Nudge Settings Section
        GUILayout.BeginVertical(boxStyle);
        GUILayout.Label("Nudge Settings", EditorStyles.boldLabel);
        nudgeZone.speed = EditorGUILayout.FloatField("Nudge Speed", nudgeZone.speed);
        nudgeZone.lerpSpeed = EditorGUILayout.FloatField("Lerp Speed", nudgeZone.lerpSpeed);
        nudgeZone.directionTransform = (Transform)EditorGUILayout.ObjectField("Direction Transform", nudgeZone.directionTransform, typeof(Transform), true);
        GUILayout.EndVertical();

        GUILayout.Space(10);

        // Ragdoll Settings Section
        GUILayout.BeginVertical(boxStyle);
        GUILayout.Label("Ragdoll Settings", EditorStyles.boldLabel);
        nudgeZone.ragSpeed = EditorGUILayout.FloatField("Ragdoll Speed", nudgeZone.ragSpeed);
        nudgeZone.ragTorque = EditorGUILayout.Vector3Field("Ragdoll Torque", nudgeZone.ragTorque);
        nudgeZone.ragDirectionTransform = (Transform)EditorGUILayout.ObjectField("Ragdoll Direction Transform", nudgeZone.ragDirectionTransform, typeof(Transform), true);
        GUILayout.EndVertical();

        GUILayout.Space(10);

        // Options Section
        GUILayout.BeginVertical(boxStyle);
        GUILayout.Label("Options", EditorStyles.boldLabel);
        nudgeZone.twoWay = EditorGUILayout.Toggle("Two Way", nudgeZone.twoWay);

        if (nudgeZone.twoWay)
        {
            nudgeZone.twoWayDirectionTransform = (Transform)EditorGUILayout.ObjectField("Two Way Direction Transform", nudgeZone.twoWayDirectionTransform, typeof(Transform), true);
        }

        nudgeZone.setYVelocity = EditorGUILayout.Toggle("Set Y Velocity", nudgeZone.setYVelocity);
        if (nudgeZone.setYVelocity)
        {
            EditorGUILayout.HelpBox("Set Y Velocity is enabled. Ensure that the appropriate behavior is implemented.", MessageType.Info);
        }

        nudgeZone.allowPlayerMovement = EditorGUILayout.Toggle("Allow Player Movement", nudgeZone.allowPlayerMovement);
        GUILayout.EndVertical();

        GUILayout.EndVertical();
    }

    // Custom Scene GUI to draw rays in Scene view
    private void OnSceneGUI()
    {
        NudgeZone nudgeZone = (NudgeZone)target;

        if (nudgeZone.directionTransform != null)
        {
            Handles.color = Color.magenta;
            Handles.DrawLine(nudgeZone.directionTransform.position, nudgeZone.directionTransform.position + nudgeZone.directionTransform.forward * nudgeZone.speed * 0.25f);

            if (nudgeZone.twoWay)
            {
                Handles.color = Color.yellow;
                if (nudgeZone.twoWayDirectionTransform != null)
                {
                    Handles.DrawLine(nudgeZone.twoWayDirectionTransform.position, nudgeZone.twoWayDirectionTransform.position + nudgeZone.twoWayDirectionTransform.forward * nudgeZone.speed * 0.25f);
                }
                else
                {
                    Handles.DrawLine(nudgeZone.directionTransform.position, nudgeZone.directionTransform.position - nudgeZone.directionTransform.forward * nudgeZone.speed * 0.25f);
                }
            }
        }
    }
}
#endif
