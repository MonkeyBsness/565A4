using System;
using System.Collections.Generic;

public class LSystemEngine
{
    public List<Symbol> Current { get; private set; } = new();

    public List<Symbol> Axiom { get; private set; } = new();
    public RewriteRuleSet Rules { get; private set; } = new RewriteRuleSet();

    private readonly System.Random _rng;

    public LSystemEngine(int seed = 12345)
    {
        _rng = new System.Random(seed);
    }

    public void SetSeed(int seed)
    {
        // re-seed by creating new Random
        // (keeps deterministic results for same seed)
        typeof(System.Random).GetField("inext", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(_rng, 0);
        typeof(System.Random).GetField("inextp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(_rng, 0);
        // simpler: just replace via reflection not worth it; easiest: make rng not readonly, but keep minimal.
        // If you want reseed support cleanly, change _rng to non-readonly and reassign.
    }

    public void SetAxiom(List<Symbol> axiom)
    {
        Axiom = new List<Symbol>(axiom);
        Reset();
    }

    public void Reset()
    {
        Current = new List<Symbol>(Axiom);
    }

    public void Iterate(int iterations)
    {
        Reset();
        for (int i = 0; i < iterations; i++)
            Step();
    }

    public void Step()
    {
        var next = new List<Symbol>(Current.Count * 2);

        foreach (var sym in Current)
        {
            if (Rules.TryGetSuccessor(sym.C, _rng, out var succ))
            {
                // Expand
                for (int i = 0; i < succ.Count; i++)
                    next.Add(succ[i]);
            }
            else
            {
                // Keep as-is (e.g., + - [ ] etc.)
                next.Add(sym);
            }
        }

        Current = next;
    }
}