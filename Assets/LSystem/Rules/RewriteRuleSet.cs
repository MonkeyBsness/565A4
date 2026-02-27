using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RewriteOption
{
    [Tooltip("Relative weight for stochastic selection.")]
    public float weight = 1f;

    [Tooltip("Successor symbols for this option.")]
    public List<Symbol> successor = new List<Symbol>();
}

[Serializable]
public class RewriteRuleSet
{
    // For each predecessor char, we can have multiple options (stochastic).
    // If only one option -> deterministic D0L.
    private readonly Dictionary<char, List<RewriteOption>> _rules = new();

    public void Clear() => _rules.Clear();

    public void AddDeterministic(char predecessor, List<Symbol> successor)
    {
        _rules[predecessor] = new List<RewriteOption>
        {
            new RewriteOption { weight = 1f, successor = successor }
        };
    }

    public void AddStochastic(char predecessor, params RewriteOption[] options)
    {
        _rules[predecessor] = new List<RewriteOption>(options);
    }

    public bool TryGetSuccessor(char predecessor, System.Random rng, out List<Symbol> successor)
    {
        successor = null;
        if (!_rules.TryGetValue(predecessor, out var options) || options == null || options.Count == 0)
            return false;

        if (options.Count == 1)
        {
            successor = options[0].successor;
            return true;
        }

        // Weighted random pick
        float sum = 0f;
        for (int i = 0; i < options.Count; i++) sum += Mathf.Max(0f, options[i].weight);

        if (sum <= 0f)
        {
            successor = options[0].successor;
            return true;
        }

        float r = (float)(rng.NextDouble() * sum);
        float acc = 0f;
        for (int i = 0; i < options.Count; i++)
        {
            acc += Mathf.Max(0f, options[i].weight);
            if (r <= acc)
            {
                successor = options[i].successor;
                return true;
            }
        }

        successor = options[^1].successor;
        return true;
    }
}