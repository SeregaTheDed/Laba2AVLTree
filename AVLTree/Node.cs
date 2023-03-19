namespace AVLTree
{
    public class Node<TKey, TValue>
    {
        public TKey? Key { get; private set; }
        public TValue? Value { get; set; }
        public int Height { get; private set; }

        public Node<TKey, TValue>? left;
        public Node<TKey, TValue>? right;
        public Node<TKey, TValue>? parent;

        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Height = 1;
        }

        private static int GetHeight(Node<TKey, TValue>? node)
        {
            return node is not null ? node.Height : 0;
        }

        public int GetBalance()
        {
            return GetHeight(right) - GetHeight(left);
        }

        public void FixHeight()
        {
            var leftHeight = GetHeight(left);
            var rightHeight = GetHeight(right);
            Height = (leftHeight > rightHeight ? leftHeight : rightHeight) + 1;
        }
    }
}
