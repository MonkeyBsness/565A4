using System.Collections.Generic;

public static class PlantPresets
{
    // ========= Helpers =========
    private static List<Symbol> Sym(string s)
    {
        var list = new List<Symbol>(s.Length);
        foreach (char c in s)
            list.Add(new Symbol(c));
        return list;
    }
    private static RewriteOption Opt(float w, string successor)
        => new RewriteOption { weight = w, successor = Sym(successor) };

    // ========= Presets =========
    // - X non-drawing growth symbol
    // - F draws branch segment
    // - L creates leaf
    // - + - & ^ \ / rotations
    // - [ ] push/pop

    /// <summary>
    /// Broadleaf / Oak-like: thick trunk feel, wide crown, lots of asymmetry.
    /// Suggest params: iterations 5-6, angle 18-30, step 0.8-1.2, radius 0.05-0.10
    /// </summary>
    public static void BuildOak(LSystemEngine eng)
    {
        eng.Rules.Clear();
        eng.SetAxiom(Sym("X"));

        // Trunk elongation
        eng.Rules.AddDeterministic('F', Sym("FF"));

        // X produces branching structure.
        // Mix yaw/pitch/roll to make real 3D crown, plus leaf placement at tips.
        eng.Rules.AddStochastic('X',
            // Classic 3-branch crown with leaves near tips
            Opt(0.35f, "F[+X]F[-X]FXL"),
            // Slight pitch variation
            Opt(0.25f, "F[&+X]F[^-X]FXL"),
            // Add roll -> more 3D spread
            Opt(0.20f, "F[\\+X]F[/ -X]FXL".Replace(" ", "")),
            // A bit denser crown option
            Opt(0.20f, "F[+X]F[&-X]F[^X]L")
        );
    }

    /// <summary>
    /// Birch-like: slender, mostly upward growth, fewer branches, airy canopy.
    /// Suggest params: iterations 5-6, angle 12-22, step 0.8-1.3, radius 0.02-0.05
    /// </summary>
    public static void BuildBirch(LSystemEngine eng)
    {
        eng.Rules.Clear();
        eng.SetAxiom(Sym("X"));

        // Slender trunk elongation
        eng.Rules.AddDeterministic('F', Sym("FF"));

        // Mostly upward pitch (^), lighter branching than oak.
        eng.Rules.AddStochastic('X',
            Opt(0.50f, "F[^X]F[+^X]F[-^X]L"),
            Opt(0.30f, "F[^X]F[&+X]L"),
            Opt(0.20f, "F[^X]F[\\+^X]F[/ -^X]L".Replace(" ", ""))
        );
    }

    /// <summary>
    /// Bush / shrub: lots of short branching, leaf-dense.
    /// Suggest params: iterations 4-6, angle 20-40, step 0.5-0.9, radius 0.03-0.06
    /// </summary>
    public static void BuildBush(LSystemEngine eng)
    {
        eng.Rules.Clear();
        eng.SetAxiom(Sym("X"));

        // Short segments (less elongation)
        eng.Rules.AddDeterministic('F', Sym("F"));

        // Dense multi-branch with leaves everywhere
        eng.Rules.AddStochastic('X',
            Opt(0.40f, "F[+X]F[-X]F[&X]L"),
            Opt(0.30f, "F[+&X]F[-^X]F[\\X]L"),
            Opt(0.30f, "F[+X]F[/X]F[\\X]LL")
        );
    }
    public static void BuildPlantA(LSystemEngine eng)
    {
        // Classic branching plant:
        // Axiom: F
        // Rule: F -> F[+\\F]F[-/F]L
        eng.Rules.Clear();
        eng.SetAxiom(new List<Symbol> { new Symbol('F') });

        eng.Rules.AddDeterministic('F', new List<Symbol>
        {
            new Symbol('F'),
            new Symbol('['), new Symbol('+'), new Symbol('\\'), new Symbol('F'), new Symbol(']'),
            new Symbol('F'),
            new Symbol('['), new Symbol('-'), new Symbol('/'), new Symbol('F'), new Symbol(']'),
            new Symbol('F'),
        });
    }

}