using Backend;

var root = BinaryTreeFile.Load("tree.txt");

Console.WriteLine("Pre-Order =>");
BinaryTreeOps.PreOrder(root, x => Console.Write($"{x} "));
Console.WriteLine("\n\nIn-Order =>");
BinaryTreeOps.InOrder(root, x => Console.Write($"{x} "));
Console.WriteLine("\n\nPost-Order =>");
BinaryTreeOps.PostOrder(root, x => Console.Write($"{x} "));
Console.WriteLine("\n\nLevelOrder =>");
BinaryTreeOps.LevelOrder(root, x => Console.Write($"{x} "));
Console.WriteLine("\n\nPrint Sideways =>");
BinaryTreeOps.PrintSideways(root, 0);
