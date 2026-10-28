// Backend/BinaryTreeFile.cs
namespace Backend;

/// <summary>Loads and saves binary trees using a flat text file (txt, csv, dat...).</summary>
public static class BinaryTreeFile
{
    /// <summary>
    /// Loads a binary tree from any flat file where each line describes one node:
    ///   value[sep]leftChild[sep]rightChild      ("-" or empty = no child)
    /// The first node line is the root. Blank lines and lines starting with '#' are ignored.
    /// A child that has no line of its own becomes a leaf.
    /// </summary>
    public static BinaryNode<T> Load<T>(string path, Func<string, T> parse, char separator = ',')
    {
        var nodes = new Dictionary<string, BinaryNode<T>>();
        var links = new List<(string Parent, string? Left, string? Right)>();
        string? rootKey = null;

        // Pass 1: create every node that has a line, remembering who its children are.
        foreach (string rawLine in File.ReadLines(path))
        {
            string line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith('#')) continue;

            string[] fields = line.Split(separator);
            if (fields.Length != 3)
            {
                throw new FormatException($"Expected 3 fields but found {fields.Length}: '{rawLine}'");
            }

            string key = fields[0].Trim();
            if (nodes.ContainsKey(key))
            {
                throw new FormatException($"Node '{key}' is defined twice.");
            }

            rootKey ??= key;
            nodes[key] = new BinaryNode<T>(parse(key));
            links.Add((key, ChildKey(fields[1]), ChildKey(fields[2])));
        }

        if (rootKey == null)
        {
            throw new FormatException("The file contains no node lines.");
        }

        // Pass 2: connect parents to children (creating missing leaves on the fly).
        foreach (var (parentKey, leftKey, rightKey) in links)
        {
            if (leftKey != null) nodes[parentKey].Left = GetOrCreate(nodes, leftKey, parse);
            if (rightKey != null) nodes[parentKey].Right = GetOrCreate(nodes, rightKey, parse);
        }

        return nodes[rootKey];
    }

    /// <summary>Convenience overload for trees whose values are plain strings.</summary>
    public static BinaryNode<string> Load(string path, char separator = ',')
    {
        return Load(path, text => text, separator);
    }

    /// <summary>Writes the tree in the same format, one node per line in pre-order.</summary>
    public static void Save<T>(BinaryNode<T> root, string path, char separator = ',')
    {
        using var writer = new StreamWriter(path);
        writer.WriteLine($"# value{separator}left{separator}right   ('-' = no child)");
        WriteNode(root, writer, separator);
    }

    /// <summary>Pre-order walk: write this node, then its left subtree, then its right subtree.</summary>
    private static void WriteNode<T>(BinaryNode<T>? node, StreamWriter writer, char separator)
    {
        if (node == null) return; // Base case: empty subtree.

        string left = node.Left == null ? "-" : node.Left.Value!.ToString()!;
        string right = node.Right == null ? "-" : node.Right.Value!.ToString()!;
        writer.WriteLine($"{node.Value}{separator}{left}{separator}{right}");

        WriteNode(node.Left, writer, separator);
        WriteNode(node.Right, writer, separator);
    }

    private static string? ChildKey(string field)
    {
        string key = field.Trim();
        return key.Length == 0 || key == "-" ? null : key;
    }

    private static BinaryNode<T> GetOrCreate<T>(Dictionary<string, BinaryNode<T>> nodes, string key, Func<string, T> parse)
    {
        if (!nodes.TryGetValue(key, out var node))
        {
            node = new BinaryNode<T>(parse(key)); // Referenced but never defined: it is a leaf.
            nodes[key] = node;
        }
        return node;
    }
}