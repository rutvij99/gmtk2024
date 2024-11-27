using GravityWell.Core.Config;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GraphicsPreset", menuName = "GravityWell/Settings/GraphicsPreset")]
public class GraphicsPresetSO : ScriptableObject
{
    public GraphicsPresets presetType;
    public GraphicsSettings settings;
}
