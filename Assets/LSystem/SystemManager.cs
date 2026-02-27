using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public enum Preset 
    { 
        /// <summary>Deterministic 0L-system “Plant A”.</summary>
        D0L_Plant = 0, 
        /// <summary>Stochastic birch-like plant ruleset.</summary>
        Stochastic_Birch = 1, 
        /// <summary>Stochastic bush-like plant ruleset.</summary>
        Stochastic_Bush = 2, 
        /// <summary>Stochastic oak-like plant ruleset.</summary>
        Stochastic_Oak = 3}

    [Header("References")]
    public Turtle3D turtle;

    [Header("Preset")]
    /// <summary>
    /// Which preset ruleset to build
    /// </summary>
    public Preset preset = Preset.D0L_Plant;

    [Header("Controls")]
    /// <summary>
    /// Number of rewrite iterations
    /// </summary>
    [Range(0, 8)] public int iterations = 4;
    /// <summary>
    /// Forward step length
    /// </summary>
    [Range(0.1f, 3f)] public float stepLength = 1.0f;
    /// <summary>
    /// Base branch radius  when generating branches
    [Range(0.01f, 0.3f)] public float radius = 0.05f;
    /// <summary>
    /// Turn angle used for rotations
    /// </summary>
    [Range(0f, 90f)] public float angleDeg = 25f;

    private LSystemEngine _engine;

    /// <summary>
    /// Creates the engine
    /// </summary>
    private void Awake()
    {
        _engine = new LSystemEngine(seed: 1337);
        if (turtle == null) turtle = FindFirstObjectByType<Turtle3D>();
    }

    /// <summary>
    /// Generates the initial plant
    /// </summary>
    private void Start()
    {
        Regenerate();
    }

    /// <summary>
    /// Builds the LSystem and renders it
    /// </summary>
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
        // render 
        _engine.Iterate(iterations);
        turtle.Interpret(_engine.Current);
    }

}