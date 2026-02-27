using System.Collections.Generic;
using UnityEngine;

public class Turtle3D : MonoBehaviour
{
    [Header("Materials")]
    public Material branchMaterial;
    public Material leafMaterial;

    [Header("Turtle Params")]
    public float stepLength = 1.0f;
    public float branchRadius = 0.05f;
    public float turnAngleDeg = 25f;

    [Header("Debug")]
    public bool drawLeaves = true;

    //Captures the turtles current interpretation state
    private struct TurtleState
    {
        public Vector3 pos;
        public Quaternion rot;
        public float step;
        public float radius;
        public Transform parent;
    }

    private readonly Stack<TurtleState> _stack = new();

    public Transform GenerateRoot { get; private set; }

    // Destroys the current root object
    public void ClearGenerated()
    {
        if (GenerateRoot != null)
        {
            Destroy(GenerateRoot.gameObject);
            GenerateRoot = null;
        }
    }

    // Interprets a sequence of L-System and generates geometry
    public void Interpret(List<Symbol> symbols)
    {
        ClearGenerated();

        // Create new root
        var root = new GameObject("LSystem_GeneratedRoot");
        GenerateRoot = root.transform;
        GenerateRoot.SetParent(transform, false);

        _stack.Clear();

        // Initialize turtle state
        TurtleState state = new TurtleState
        {
            pos = transform.position,
            rot = transform.rotation,
            step = stepLength,
            radius = branchRadius,
            parent = GenerateRoot
        };

        for (int i = 0; i < symbols.Count; i++)
        {
            char c = symbols[i].C;

            switch (c)
            {
                case 'F':  // Draw a branch
                {
                    Vector3 start = state.pos;
                    Vector3 dir = state.rot * Vector3.up; // turtle forward = up
                    Vector3 end = start + dir * state.step;

                    var cyl = LGeom.Cylinder(start, end, state.radius, branchMaterial, "Branch");
                    cyl.transform.SetParent(state.parent, true);

                    state.pos = end;
                    break;
                }

                case '+': // yaw right
                    state.rot = state.rot * Quaternion.AngleAxis(+turnAngleDeg, Vector3.forward);
                    break;
                case '-': // yaw left
                    state.rot = state.rot * Quaternion.AngleAxis(-turnAngleDeg, Vector3.forward);
                    break;

                case '&': // pitch down
                    state.rot = state.rot * Quaternion.AngleAxis(+turnAngleDeg, Vector3.right);
                    break;
                case '^': // pitch up
                    state.rot = state.rot * Quaternion.AngleAxis(-turnAngleDeg, Vector3.right);
                    break;

                case '\\': // roll
                    state.rot = state.rot * Quaternion.AngleAxis(+turnAngleDeg, Vector3.up);
                    break;
                case '/': // roll opposite
                    state.rot = state.rot * Quaternion.AngleAxis(-turnAngleDeg, Vector3.up);
                    break;

                case '[':
                    _stack.Push(state);
                    var branchNode = new GameObject("BranchNode");
                    branchNode.transform.SetParent(state.parent, true);
                    branchNode.transform.position = state.pos;
                    branchNode.transform.rotation = state.rot;
                    state.parent = branchNode.transform;
                    break;

                case ']':
                    if (_stack.Count > 0)
                        state = _stack.Pop();
                    break;

                case 'L': // leaf
                    if (drawLeaves)
                    {
                        var pts = new List<Vector3>
                        {
                            state.pos,
                            state.pos + (state.rot * Vector3.right) * (state.step * 0.3f),
                            state.pos + (state.rot * Vector3.up)    * (state.step * 0.5f),
                            state.pos + (state.rot * Vector3.left)  * (state.step * 0.3f)
                        };
                        var leaf = LGeom.FilledPolygon_Fan(pts, leafMaterial, "Leaf");
                        if (leaf != null)
                        {
                            leaf.transform.SetParent(state.parent, true);
                        }
                    }
                    break;

                default:
                    // ignore unknown symbol
                    break;
            }
        }
    }
}