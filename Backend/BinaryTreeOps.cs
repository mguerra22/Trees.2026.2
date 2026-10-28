namespace Backend;

/// <summary>Generic algorithms over any binary tree (not necessarily a search tree).</summary>
public static class BinaryTreeOps
{
    // ---- Depth-first traversals (recursive) ----

    public static void PreOrder<T>(BinaryNode<T>? node, Action<T> visit)
    {
        if (node == null) return;      // Base case: empty subtree.
        visit(node.Value);             // Root first...
        PreOrder(node.Left, visit);    // ...then the left subtree...
        PreOrder(node.Right, visit);   // ...then the right subtree.
    }

    public static void InOrder<T>(BinaryNode<T>? node, Action<T> visit)
    {
        if (node == null) return;
        InOrder(node.Left, visit);     // Left subtree first...
        visit(node.Value);             // ...then the root...
        InOrder(node.Right, visit);    // ...then the right subtree.
    }

    public static void PostOrder<T>(BinaryNode<T>? node, Action<T> visit)
    {
        if (node == null) return;
        PostOrder(node.Left, visit);   // Left subtree...
        PostOrder(node.Right, visit);  // ...right subtree...
        visit(node.Value);             // ...and the root last.
    }

    // ---- Breadth-first traversal (uses a queue, not recursion) ----

    public static void LevelOrder<T>(BinaryNode<T>? root, Action<T> visit)
    {
        if (root == null) return;

        var queue = new Queue<BinaryNode<T>>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            visit(node.Value);

            // Children are enqueued so they are visited after the whole current level.
            if (node.Left != null) queue.Enqueue(node.Left);
            if (node.Right != null) queue.Enqueue(node.Right);
        }
    }

    /// <summary>Same as LevelOrder, but groups the values of each level in its own list.</summary>
    public static List<List<T>> LevelOrderByLevels<T>(BinaryNode<T>? root)
    {
        var levels = new List<List<T>>();
        if (root == null) return levels;

        var queue = new Queue<BinaryNode<T>>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            int nodesInLevel = queue.Count; // Everything currently queued belongs to one level.
            var current = new List<T>();

            for (int i = 0; i < nodesInLevel; i++)
            {
                var node = queue.Dequeue();
                current.Add(node.Value);
                if (node.Left != null) queue.Enqueue(node.Left);
                if (node.Right != null) queue.Enqueue(node.Right);
            }

            levels.Add(current);
        }

        return levels;
    }

    // ---- In-order without recursion: the stack is explicit ----

    public static void InOrderIterative<T>(BinaryNode<T>? root, Action<T> visit)
    {
        var stack = new Stack<BinaryNode<T>>();
        var current = root;

        while (current != null || stack.Count > 0)
        {
            // Go as far left as possible, remembering the path.
            while (current != null)
            {
                stack.Push(current);
                current = current.Left;
            }

            // Visit the deepest pending node, then move to its right subtree.
            current = stack.Pop();
            visit(current.Value);
            current = current.Right;
        }
    }

    // ---- Metrics ----

    /// <summary>Height measured in edges: empty tree = -1, single node = 0.</summary>
    public static int Height<T>(BinaryNode<T>? node)
    {
        if (node == null) return -1;
        return 1 + Math.Max(Height(node.Left), Height(node.Right));
    }

    public static int Count<T>(BinaryNode<T>? node)
    {
        if (node == null) return 0;
        return 1 + Count(node.Left) + Count(node.Right);
    }

    public static int CountLeaves<T>(BinaryNode<T>? node)
    {
        if (node == null) return 0;
        if (node.IsLeaf) return 1;
        return CountLeaves(node.Left) + CountLeaves(node.Right);
    }

    /// <summary>Linear search: works on any binary tree, O(n).</summary>
    public static bool Contains<T>(BinaryNode<T>? node, T value)
    {
        if (node == null) return false;
        if (EqualityComparer<T>.Default.Equals(node.Value, value)) return true;
        return Contains(node.Left, value) || Contains(node.Right, value);
    }

    public static T Max<T>(BinaryNode<T> node) where T : IComparable<T>
    {
        T best = node.Value;
        if (node.Left != null)
        {
            T leftMax = Max(node.Left);
            if (leftMax.CompareTo(best) > 0) best = leftMax;
        }
        if (node.Right != null)
        {
            T rightMax = Max(node.Right);
            if (rightMax.CompareTo(best) > 0) best = rightMax;
        }
        return best;
    }

    /// <summary>Returns a new tree where every left and right child are swapped.</summary>
    public static BinaryNode<T>? Mirror<T>(BinaryNode<T>? node)
    {
        if (node == null) return null;
        return new BinaryNode<T>(node.Value, Mirror(node.Right), Mirror(node.Left));
    }

    /// <summary>Prints the tree rotated 90 degrees: root at the left, right subtree above.</summary>
    public static void PrintSideways<T>(BinaryNode<T>? node, int depth = 0)
    {
        if (node == null) return;
        PrintSideways(node.Right, depth + 1);
        Console.WriteLine(new string(' ', depth * 4) + node.Value);
        PrintSideways(node.Left, depth + 1);
    }
}