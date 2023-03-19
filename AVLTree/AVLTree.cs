using System.Xml.Linq;

namespace AVLTree
{
    public class AVLTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Ссылка на корень дерева
        /// </summary>
        public Node<TKey, TValue>? _root { get; private set; }

        /// <summary>
        /// Количество элементов в дереве
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Изменение ссылок родителей узлов участвующих в повороте
        /// </summary>
        /// <param name="node">Старый узел</param>
        /// <param name="newNode">Новый узел</param>
        /// <param name="direction">Направление поворота</param>
        private void SwapParentNodesDuringRotate(Node<TKey, TValue> node, Node<TKey, TValue>? newNode, Direction direction)
        {
            newNode.parent = node.parent;
            if (direction == Direction.Right)
            {
                if (newNode.right != null)
                    newNode.right.parent = node;
            }
            else
            {
                if (newNode.left != null)
                    newNode.left.parent = node;
            }

            SwapLinkParentNewNode(newNode, node);
            node.parent = newNode;
        }

        /// <summary>
        /// Исправление ссылки в родителе для узла, в котором выполняется поворот
        /// </summary>
        /// <param name="newNode"></param>
        /// <param name="currentNode"></param>
        private void SwapLinkParentNewNode(Node<TKey, TValue> newNode, Node<TKey, TValue> currentNode)
        {
            if (newNode.parent != null)
            {
                if (newNode.parent.left == currentNode)
                    newNode.parent.left = newNode;
                else
                    newNode.parent.right = newNode;
            }
            else
                _root = newNode;
        }

        /// <summary>
        /// Правый малый поворот
        /// </summary>
        /// <param name="node">Узел в котором выполняется поворот</param>
        /// <returns></returns>
        private Node<TKey, TValue> MakeRightSmallRotate(Node<TKey, TValue> node)
        {
            var newNode = node.left;
            SwapParentNodesDuringRotate(node, newNode, Direction.Right);
            node.left = newNode.right;
            newNode.right = node;
            node.FixHeight();
            newNode.FixHeight();

            return newNode;
        }

        /// <summary>
        /// Левый малый поворот
        /// </summary>
        /// <param name="node">Узел в котором выполняется поворот</param>
        /// <returns></returns>
        private Node<TKey, TValue> MakeLeftSmallRotate(Node<TKey, TValue> node)
        {
            var newNode = node.right;
            SwapParentNodesDuringRotate(node, newNode, Direction.Left);
            node.right = newNode.left;
            newNode.left = node;
            node.FixHeight();
            newNode.FixHeight();

            return newNode;
        }

        /// <summary>
        /// Балансировка дерева(идем до корня дерева и убираем дисбаланс)
        /// </summary>
        /// <param name="node">Узел с которого будет идти поиск дисбаланса</param>
        private void BalanceTree(Node<TKey, TValue> node)
        {
            var currentNode = node;
            while (currentNode != null)
            {
                currentNode.FixHeight();
                if (currentNode.GetBalance() == 2)
                {
                    //Большой левый поворот
                    if (currentNode.right.GetBalance() < 0)
                        MakeRightSmallRotate(currentNode.right);
                    currentNode = MakeLeftSmallRotate(currentNode);
                }
                if (currentNode.GetBalance() == -2)
                {
                    //Большой правый поворот
                    if (currentNode.left.GetBalance() > 0)
                        MakeLeftSmallRotate(currentNode.left);
                    currentNode = MakeRightSmallRotate(currentNode);
                }
                currentNode = currentNode.parent;
            }
        }

        /// <summary>
        /// Изменение ссылок узлов при удаление узла
        /// </summary>
        /// <param name="currentNode">Удаляемый узел</param>
        /// <param name="minNode">Мин. узел в правом поддереве для удаляемого узла</param>
        private void SwapLinksDuringRemoveNode(Node<TKey, TValue> currentNode, Node<TKey, TValue> minNode)
        {
            var parentNode = currentNode.parent;
            minNode.left = currentNode.left;

            if (minNode.right != null)
            {
                minNode.right.parent = minNode.parent;
                minNode.parent.left = minNode.right;
            }
            else
                minNode.parent.left = null;

            if (currentNode.right != minNode)
            {
                minNode.right = currentNode.right;
            }

            if (minNode.left != null)
                minNode.left.parent = minNode;
            if (minNode.right != null)
                minNode.right.parent = minNode;

            minNode.parent = parentNode;
        }

        /// <summary>
        /// Поиск мин. узла в правом поддереве
        /// </summary>
        /// <param name="node">Узел для которого ищется минимальный</param>
        /// <returns></returns>
        private static Node<TKey, TValue> SearchMinNodeInRightTree(Node<TKey, TValue> node)
        {
            var currentNode = node;
            var parentNode = node;
            while (currentNode != null)
            {
                parentNode = currentNode;
                currentNode = currentNode.left;
            }
            return parentNode;
        }

        /// <summary>
        /// Удаление узла, когда у него нет потомков
        /// </summary>
        /// <param name="currentNode">Узел который следует удалить</param>
        private void RemoveNodeNotSubtree(Node<TKey, TValue> currentNode)
        {
            var parentNode = currentNode.parent;
            //Если узел не является корнем
            if (parentNode != null)
            {
                //Проверяем где находится узел
                if (parentNode.left == currentNode)
                {
                    parentNode.left = null;
                    currentNode.parent = null;
                }
                else
                {
                    parentNode.right = null;
                    currentNode.parent = null;
                }
                BalanceTree(parentNode);
            }
            else
                _root = null;
        }

        /// <summary>
        /// Удаление узла, когда нет правого поддерева
        /// </summary>
        /// <param name="currentNode">Узел который следует удалить</param>
        private void RemoveNodeNotRightSubtree(Node<TKey, TValue> currentNode)
        {
            var parentNode = currentNode.parent;
            //Если узел не является корнем
            if (parentNode != null)
            {
                //Проверяем где находится узел
                if (parentNode.left == currentNode)
                {
                    parentNode.left = currentNode.left;
                    currentNode.left.parent = parentNode;
                    BalanceTree(parentNode.left);
                }
                else
                {
                    parentNode.right = currentNode.left;
                    currentNode.left.parent = parentNode;
                    BalanceTree(parentNode.right);
                }
            }
            else
            {
                _root = _root.left;
                _root.parent = null;
                BalanceTree(_root);
            }
        }

        /// <summary>
        /// Удаление узла, у которого есть правое поддерево
        /// </summary>
        /// <param name="currentNode">Узел который следует удалить</param>
        private void RemoveNodeHaveRightSubtree(Node<TKey, TValue> currentNode)
        {
            var parentNode = currentNode.parent;
            var minNode = SearchMinNodeInRightTree(currentNode.right);
            var parentMinNode = minNode.parent;
            //Если узел не является корнем
            if (parentNode != null)
            {
                //Проверяем где находится узел
                if (parentNode.left == currentNode)
                    parentNode.left = minNode;
                else
                    parentNode.right = minNode;
                SwapLinksDuringRemoveNode(currentNode, minNode);
            }
            else
            {
                minNode.left = _root.left;

                if (minNode.right != null)
                {
                    minNode.right.parent = minNode.parent;
                    minNode.parent.left = minNode.right;
                }
                else
                    minNode.parent.left = null;

                if (_root.right != minNode)
                {
                    minNode.right = _root.right;
                }

                if (minNode.left != null)
                    minNode.left.parent = minNode;
                if (minNode.right != null)
                    minNode.right.parent = minNode;

                _root = minNode;
                _root.parent = null;
            }
            if (parentMinNode != currentNode)
                BalanceTree(parentMinNode);
            else
                BalanceTree(minNode);
        }

        /*/// <summary>
        /// Рекурсивный обход дерева и вывод его в консоль
        /// </summary>
        /// <param name="startNode">Начальный узел</param>
        /// <param name="indent">Отступ для узлов</param>
        /// <param name="side">Тип узла</param>
        private void PrintTree(Node<TKey, TValue> startNode, string indent = "", Direction? side = null)
        {
            if (startNode != null)
            {
                var nodeSide = side == null ? "+" : side == Direction.Left ? "L" : "R";
                Console.WriteLine($"{indent} [{nodeSide}]- {startNode.Value}");
                indent += new string(' ', 3);
                PrintTree(startNode.left, indent, Direction.Left);
                PrintTree(startNode.right, indent, Direction.Right);
            }
        }*/

        /// <summary>
        /// Получить узел по ключу
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Node<TKey, TValue>? GetNode(TKey key)
        {
            var currentNode = _root;
            while (currentNode != null)
            {
                if (currentNode.Key.CompareTo(key) > 0)
                    currentNode = currentNode.left;
                else if (currentNode.Key.CompareTo(key) < 0)
                    currentNode = currentNode.right;
                else
                    return currentNode;
            }
            return null;
        }

        public AVLTree(TKey key, TValue value)
        {
            _root = new(key, value);
        }

        public AVLTree() { }

        //Индексатор
        public TValue this[TKey index]
        {
            get
            {
                var node = GetNode(index);
                if (node == null)
                    throw new ArgumentNullException("Элемента по заданному ключу не существует");
                return node.Value;
            }
        }

        /// <summary>
        /// Добавить элемент в дерево
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Add(TKey key, TValue value)
        {
            var node = new Node<TKey, TValue>(key, value);
            if (_root == null)
            {
                _root = node;
                Count++;
                return;
            }
            var currentNode = _root;
            var parentNode = _root;
            while (currentNode != null)
            {
                parentNode = currentNode;
                if (currentNode.Key.CompareTo(key) > 0)
                    currentNode = currentNode.left;
                else if (currentNode.Key.CompareTo(key) < 0)
                    currentNode = currentNode.right;
                else
                    throw new ArgumentException("Such key is already added");
            }
            if (parentNode.Key.CompareTo(node.Key) > 0)
                parentNode.left = node;
            if (parentNode.Key.CompareTo(node.Key) < 0)
                parentNode.right = node;
            Count++;
            node.parent = parentNode;
            BalanceTree(node.parent);
        }

        /// <summary>
        /// Поиск элемента по ключу
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true если элемент был найден, иначе false</returns>
        public bool Contains(TKey key)
        {
            if (GetNode(key) == null)
                return false;
            return true;
        }

        /// <summary>
        /// Удаление элемента по ключу
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true если элемент был удален, иначе false</returns>
        public bool Remove(TKey key)
        {
            var currentNode = GetNode(key);
            if (currentNode != null)
            {
                if (currentNode.left == null && currentNode.right == null)
                    RemoveNodeNotSubtree(currentNode);
                else if (currentNode.left != null && currentNode.right == null)
                    RemoveNodeNotRightSubtree(currentNode);
                else
                    RemoveNodeHaveRightSubtree(currentNode);
                Count--;
                return true;
            }
            return false;
        }
    }
    
}