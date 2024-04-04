using System;
using System.Collections;
using System.Collections.Generic;
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
    private BinaryTreeNode<T> _nodeInTree;
    private BinaryTreeNode<T> _rootOfTree;
    private BinaryTreeNode<T> _currentNode;

    public BinaryTree(BinaryTreeNode<T> inputTreeNode)
    {
        _nodeInTree = inputTreeNode;
        _rootOfTree = _nodeInTree;
    }

    public BinaryTreeNode<T> GetNextNode(BinaryTreeNode<T> inputTreeNode)
    {
        if (inputTreeNode.LeftLowerNode != null)
        {
            _currentNode = inputTreeNode.LeftLowerNode;
        }
        else if (inputTreeNode.RightLowerNode != null)
        {
            _currentNode = inputTreeNode.RightLowerNode;
        }
        else
        {
            _currentNode = inputTreeNode;
        }
        return _currentNode;
    }

    public BinaryTreeNode<T> GetAncestor(BinaryTreeNode<T> inputTreeNode)
    {
        if (inputTreeNode.Ancestor != null)
        {
            _currentNode = inputTreeNode.Ancestor;
        }
        else
        {
            _currentNode = inputTreeNode;
        }
        return _currentNode;
    }
    public static BinaryTree<T> operator ++(BinaryTree<T> inputTree)
    {
        inputTree.GetNextNode(inputTree._rootOfTree);
        return inputTree;
    }
    public static BinaryTree<T> operator --(BinaryTree<T> inputTree)
    {
        inputTree.GetAncestor(inputTree._rootOfTree);
        return inputTree;
    }

    //Для возможности использования foreach
    public IEnumerator<T> GetEnumerator()
    {
        return InOrderTraversal(_nodeInTree).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    // Для перебора в цикле foreach
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

    // Метод поиска узла без левого дочернего элемента
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

    // Метод поиска узла без правого дочернего элемента
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

    // Внешний итератор 
    public IEnumerable<T> GetSortedNodes(Func<T, T, int> comparison)
    {
        List<T> nodeList = new List<T>();
        InOrderTraversalForExternal(this._nodeInTree, nodeList, comparison);
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

    //Добавление элемента в дерево
    public void AddValue(T inputValue)
    {
        _nodeInTree = AddValueRecursive(_nodeInTree, inputValue);
    }

    //Получение текущего узла дерева
    public BinaryTreeNode<T> GetCurrentNode()
    {
        return _currentNode;
    }

    private BinaryTreeNode<T> AddValueRecursive(BinaryTreeNode<T> _currentNode, T inputValue)
    {
        if (_currentNode == null)
        {
            return new BinaryTreeNode<T> { DataInNode = inputValue };
        }

        // Проверка, в какое поддерево добавить новое значение, влево или вправо
        IComparable<T> comparable = inputValue as IComparable<T>; //Суровая необходимость - без этого дерево не поймёт, что к нему пришло и что с этим делать
        if (comparable.CompareTo(_currentNode.DataInNode) < 0)
        {
            _currentNode.LeftLowerNode = AddValueRecursive(_currentNode.LeftLowerNode, inputValue);
        }
        else if (comparable.CompareTo(_currentNode.DataInNode) > 0)
        {
            _currentNode.RightLowerNode = AddValueRecursive(_currentNode.RightLowerNode, inputValue);
        }

        return _currentNode;
    }
    public void Reset()
    {
        // Установить текущий узел в начало
        _currentNode = GetRoot(_rootOfTree);
    }

    private BinaryTreeNode<T> GetRoot(BinaryTreeNode<T> inputNode)
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
            byte userChoice = 0;
            int inputNumber;
            bool isRunning = true;
            BinaryTreeNode<int> outputNode; //Объявлен для вывода результатов некоторых операций

            Console.WriteLine("Create tree with adding number");
            inputNumber = Convert.ToInt32(Console.ReadLine());

            BinaryTreeNode<int> inputTreeNode = new BinaryTreeNode<int> { DataInNode = inputNumber };

            BinaryTree<int> binaryTree = new BinaryTree<int>(inputTreeNode);

            while (isRunning)
            {
                Console.WriteLine("Choose action: 1 = add, 2 = node without left part, 3 = reset, 4 = without right part," +
                                  " 5 = get all elements, 6 = get all elements (another way), 7 = current node, 8 - get next node, 9 - get previous node, 10 - quit ");

                userChoice = Convert.ToByte(Console.ReadLine());

                switch (userChoice)
                {
                    case 1:
                        Console.WriteLine("Enter number");
                        inputNumber = Convert.ToInt32(Console.ReadLine());
                        binaryTree.AddValue(inputNumber);
                        break;
                    case 2:
                        binaryTree.Reset();
                        outputNode = binaryTree.Next(inputTreeNode);
                        Console.WriteLine("Branch without left element: " + outputNode.DataInNode.ToString());
                        break;
                    case 3:
                        binaryTree.Reset();
                        Console.WriteLine("Reset. Current Position:" + inputTreeNode.DataInNode.ToString());
                        break;
                    case 4:
                        binaryTree.Reset();
                        binaryTree.Previous(inputTreeNode);
                        Console.WriteLine("Branch without right element: " + inputTreeNode.DataInNode.ToString());
                        break;
                    case 5:
                        binaryTree.Reset();
                        // Использование в цикле foreach
                        foreach (int inputValue in binaryTree)
                        {
                            Console.WriteLine(inputValue);
                        }
                        break;
                    case 6:
                        binaryTree.Reset();
                        //То же, что и 5, но через внешний итератор + другой метод сортировки
                        var sortedNodes = binaryTree.GetSortedNodes((x, y) => x.CompareTo(y));
                        foreach (var inputValue in sortedNodes)
                        {
                            Console.WriteLine(inputValue);
                        }
                        break;
                    case 7:
                        binaryTree.GetCurrentNode();
                        Console.WriteLine("Current Position:" + inputTreeNode.DataInNode.ToString());
                        break;
                    case 8:
                        binaryTree = ++binaryTree;
                        inputTreeNode = binaryTree.GetCurrentNode();
                        Console.WriteLine("Current Position:" + inputTreeNode.DataInNode.ToString());
                        break;
                    case 9:
                        binaryTree = --binaryTree;
                        inputTreeNode = binaryTree.GetCurrentNode();
                        Console.WriteLine("Current Position:" + inputTreeNode.DataInNode.ToString());
                        break;
                    case 10:
                        isRunning = false;
                        Console.WriteLine("Press any key to exit");
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
