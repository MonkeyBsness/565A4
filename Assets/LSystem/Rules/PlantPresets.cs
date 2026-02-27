using System.Collections.Generic;

public static class PlantPresets
{
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

    public static void BuildOak(LSystemEngine eng)
    {
        eng.Rules.Clear();
        eng.SetAxiom(Sym("X"));

        // Trunk elongation
        eng.Rules.AddDeterministic('F', Sym("FF"));


        eng.Rules.AddStochastic('X',
            Opt(0.35f, "F[+X]F[-X]FXL"),
            Opt(0.25f, "F[&+X]F[^-X]FXL"),
            Opt(0.20f, "F[\\+X]F[/ -X]FXL".Replace(" ", "")),
            Opt(0.20f, "F[+X]F[&-X]F[^X]L")
        );
    }

    public static void BuildBirch(LSystemEngine eng)
    {
        eng.Rules.Clear();
        eng.SetAxiom(Sym("X"));

        // Slender trunk elongation
        eng.Rules.AddDeterministic('F', Sym("FF"));

        // Mostly upward pitch
        eng.Rules.AddStochastic('X',
            Opt(0.50f, "F[^X]F[+^X]F[-^X]L"),
            Opt(0.30f, "F[^X]F[&+X]L"),
            Opt(0.20f, "F[^X]F[\\+^X]F[/ -^X]L".Replace(" ", ""))
        );
    }

    public static void BuildBush(LSystemEngine eng)
    {
        eng.Rules.Clear();
        eng.SetAxiom(Sym("X"));

        // Short segments
        eng.Rules.AddDeterministic('F', Sym("F"));

        // Dense branch with leaves everywhere
        eng.Rules.AddStochastic('X',
            Opt(0.40f, "F[+X]F[-X]F[&X]L"),
            Opt(0.30f, "F[+&X]F[-^X]F[\\X]L"),
            Opt(0.30f, "F[+X]F[/X]F[\\X]LL")
        );
    }
    public static void BuildPlantA(LSystemEngine eng)
    {
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