using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public enum Preset { D0L_Plant = 0, Stochastic_Birch = 1, Stochastic_Bush = 2, Stochastic_Oak = 3}

    [Header("References")]
    public Turtle3D turtle;

    [Header("Preset")]
    public Preset preset = Preset.D0L_Plant;

    [Header("Controls")]
    [Range(0, 8)] public int iterations = 4;
    [Range(0.1f, 3f)] public float stepLength = 1.0f;
    [Range(0.01f, 0.3f)] public float radius = 0.05f;
    [Range(0f, 90f)] public float angleDeg = 25f;

    private LSystemEngine _engine;

    private void Awake()
    {
        _engine = new LSystemEngine(seed: 1337);
        if (turtle == null) turtle = FindFirstObjectByType<Turtle3D>();
    }

    private void Start()
    {
        Regenerate();
    }

    public void Regenerate()
    {
        if (turtle == null) return;

        // apply params
        turtle.stepLength = stepLength;
        turtle.branchRadius = radius;
        turtle.turnAngleDeg = angleDeg;

        // build ruleset
        switch (preset)
        {
            case Preset.D0L_Plant:
                PlantPresets.BuildPlantA(_engine);
                break;
                break;
            case Preset.Stochastic_Birch:
                PlantPresets.BuildBirch(_engine);
                break;
            case Preset.Stochastic_Bush:
                PlantPresets.BuildBush(_engine);
                break;
            case Preset.Stochastic_Oak:
                PlantPresets.BuildOak(_engine);
                break;
        }

        _engine.Iterate(iterations);
        turtle.Interpret(_engine.Current);
    }

    // --- UI hooks ---
    public void UI_SetPreset(int presetIndex)
    {
        preset = (Preset)Mathf.Clamp(presetIndex, 0, 1);
    }

    public void UI_SetIterations(float value)
    {
        iterations = Mathf.RoundToInt(value);
    }

    public void UI_SetAngle(float value)
    {
        angleDeg = value;
    }

    public void UI_SetStep(float value)
    {
        stepLength = value;
    }

    public void UI_SetRadius(float value)
    {
        radius = value;
    }
}