using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public class BinaryTreeNode<T>
{
    public T DataInNode { get; set; }
    public BinaryTreeNode<T> LeftLowerNode { get; set; }
    public BinaryTreeNode<T> RightLowerNode { get; set; }
    public BinaryTreeNode<T> Ancestor { get; set; }

    public int StepsToNode { get; set; }
}

public class BinaryTree<T> : IEnumerable<T>
{
    private BinaryTreeNode<T> _rootOfTree;
    private BinaryTreeNode<T> _mainRoot;
    private BinaryTreeNode<T> _currentNode;

    public BinaryTree(BinaryTreeNode<T> treeNode)
    {
        _rootOfTree = treeNode;
        _mainRoot = _rootOfTree;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return InOrderTraversal(_rootOfTree).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private List<T> InOrderTraversal(BinaryTreeNode<T> inputNode)
    {
        List<T> resultList = new List<T>();
        InOrderTraversalRecursive(inputNode, resultList);
        return resultList;
    }

    private void InOrderTraversalRecursive(BinaryTreeNode<T> inputNode, List<T> resultList)
    {
        if (inputNode != null)
        {
            if (inputNode.LeftLowerNode != null && inputNode.RightLowerNode != null)
            {
                InOrderTraversalRecursive(inputNode.LeftLowerNode, resultList);
                resultList.Add(inputNode.DataInNode);
                InOrderTraversalRecursive(inputNode.RightLowerNode, resultList);
                resultList.Add(inputNode.DataInNode);
            }
            else if (inputNode.LeftLowerNode == null)
            {
                InOrderTraversalRecursive(inputNode.RightLowerNode, resultList);
                resultList.Add(inputNode.DataInNode);
            }
            else
            {
                InOrderTraversalRecursive(inputNode.LeftLowerNode, resultList);
                resultList.Add(inputNode.DataInNode);
            }

        }
    }

    public BinaryTreeNode<T> Next(BinaryTreeNode<T> inputNode)
    {
        if (inputNode == null)
        {
            return null;
        }

        if (inputNode.LeftLowerNode == null)
        {
            return inputNode;
        }
        else
        {
            if (inputNode.RightLowerNode == null)
            {
                BinaryTreeNode<T> temporaryNode = inputNode.LeftLowerNode;
                while (temporaryNode.LeftLowerNode != null)
                {
                    temporaryNode = temporaryNode.LeftLowerNode;
                    ++temporaryNode.StepsToNode;
                }
                return temporaryNode;
            }
            else
            {
                BinaryTreeNode<T> firstSplitNode = this.Next(inputNode.LeftLowerNode);
                BinaryTreeNode<T> secondSplitNode = this.Next(inputNode.RightLowerNode);
                if (firstSplitNode.StepsToNode <= secondSplitNode.StepsToNode)
                {
                    return firstSplitNode;
                }
                else
                {
                    return secondSplitNode;
                }


            }
        }
    }

    public BinaryTreeNode<T> Previous(BinaryTreeNode<T> inputNode)
    {
        if (inputNode == null)
        {
            return null;
        }

        if (inputNode.RightLowerNode == null)
        {
            return inputNode;
        }
        else
        {
            if (inputNode.LeftLowerNode == null)
            {
                BinaryTreeNode<T> temporaryNode = inputNode.RightLowerNode;
                while (temporaryNode.LeftLowerNode != null)
                {
                    temporaryNode = temporaryNode.LeftLowerNode;
                    ++temporaryNode.StepsToNode;
                }
                return temporaryNode;
            }
            else
            {
                BinaryTreeNode<T> firstSplitNode = this.Previous(inputNode.RightLowerNode);
                BinaryTreeNode<T> secondSplitNode = this.Previous(inputNode.LeftLowerNode);
                if (firstSplitNode.StepsToNode <= secondSplitNode.StepsToNode)
                {
                    return firstSplitNode;
                }
                else
                {
                    return secondSplitNode;
                }


            }
        }
    }

    // Внешний итератор для центрального обхода дерева на основе лямбда-операторов
    public IEnumerable<T> GetSortedNodes(Func<T, T, int> comparison)
    {
        List<T> nodeList = new List<T>();
        InOrderTraversalForExternal(this._rootOfTree, nodeList, comparison);
        return nodeList;
    }

    private void InOrderTraversalForExternal(BinaryTreeNode<T> inputNode, List<T> nodeList, Func<T, T, int> comparison)
    {
        if (inputNode != null)
        {
            InOrderTraversalForExternal(inputNode.LeftLowerNode, nodeList, comparison);
            nodeList.Add(inputNode.DataInNode);
            InOrderTraversalForExternal(inputNode.RightLowerNode, nodeList, comparison);
        }
    }
    public void AddValue(T value)
    {
        _rootOfTree = AddValueRecursive(_rootOfTree, value);
    }

    public BinaryTreeNode<T> GetCurrentNode()
    {
        return _currentNode;
    }

    private BinaryTreeNode<T> AddValueRecursive(BinaryTreeNode<T> _currentNode, T value)
    {
        if (_currentNode == null)
        {
            return new BinaryTreeNode<T> { DataInNode = value };
        }

        // Проверка, в какое поддерево добавить новое значение, влево или вправо
        IComparable<T> comparable = value as IComparable<T>;
        if (comparable.CompareTo(_currentNode.DataInNode) < 0)
        {
            _currentNode.LeftLowerNode = AddValueRecursive(_currentNode.LeftLowerNode, value);
        }
        else if (comparable.CompareTo(_currentNode.DataInNode) > 0)
        {
            _currentNode.RightLowerNode = AddValueRecursive(_currentNode.RightLowerNode, value);
        }

        return _currentNode;
    }
    public void Reset()
    {
        // Установить текущий узел на самый левый узел
        _currentNode = GetLeftMostNode(_mainRoot);
    }

    private BinaryTreeNode<T> GetLeftMostNode(BinaryTreeNode<T> inputNode)
    {
        if (inputNode == null)
        {
            return null;
        }

        while (inputNode.LeftLowerNode != null)
        {
            inputNode = inputNode.LeftLowerNode;
        }

        return inputNode;
    }
}

namespace Testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte choice = 0;
            int number;
            bool isRunning = true;
            BinaryTreeNode<int> outputNode;
            Console.WriteLine("Create tree with adding number");
            number = Convert.ToInt32(Console.ReadLine());
            BinaryTreeNode<int> treeNode = new BinaryTreeNode<int> { DataInNode = number };

            BinaryTree<int> binaryTree = new BinaryTree<int>(treeNode);
            while (isRunning)
            {
                Console.WriteLine("Choose action: 1 = add, 2 = next, 3 = reset, 4 = previous, 5 = foreach, 6 = external operator, 7 = current, 8 - quit");
                choice = Convert.ToByte(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter number");
                        number = Convert.ToInt32(Console.ReadLine());
                        binaryTree.AddValue(number);
                        break;
                    case 2:
                        binaryTree.Reset();
                        outputNode = binaryTree.Next(treeNode);
                        Console.WriteLine("Branch without left element: " + outputNode.DataInNode.ToString());
                        break;
                    case 3:
                        binaryTree.Reset();
                        Console.WriteLine("Reset. Current Position:" + treeNode.DataInNode.ToString());
                        break;
                    case 4:
                        binaryTree.Reset();
                        binaryTree.Previous(treeNode);
                        Console.WriteLine("Branch without right element: " + treeNode.DataInNode.ToString());
                        break;
                    case 5:
                        binaryTree.Reset();
                        // Пример использования в цикле foreach
                        foreach (int value in binaryTree)
                        {
                            Console.WriteLine(value);
                        }
                        break;
                    case 6:
                        binaryTree.Reset();
                        var sortedNodes = binaryTree.GetSortedNodes((x, y) => x.CompareTo(y));
                        foreach (var value in sortedNodes)
                        {
                            Console.WriteLine(value);
                        }
                        break;
                    case 7:
                        binaryTree.GetCurrentNode();
                        Console.WriteLine("Current Position:" + treeNode.DataInNode.ToString());
                        break;
                    case 8:
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Wrong operation type");
                        break;
                }
            }

            Console.ReadKey();
        }
    }
}
