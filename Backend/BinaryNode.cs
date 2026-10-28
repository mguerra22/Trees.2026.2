namespace Backend;

public class BinaryNode<T>
{
    public BinaryNode(T value, BinaryNode<T>? left = null, BinaryNode<T>? right = null)
    {
        Value = value;
        Left = left;
        Right = right;
    }

    public T? Value { get; set; }
    public BinaryNode<T>? Left { get; set; }
    public BinaryNode<T>? Right { get; set; }

    public bool IsLeaf => Left == null && Right == null;
}