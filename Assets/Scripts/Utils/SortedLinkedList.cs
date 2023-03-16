#nullable enable
namespace Utils
{
    public class SortedLinkedList<T>
    {
        public LinkedNode<T>? Head { get; private set; }

        public LinkedNode<T> AddNode(T value, int priority)
        {
            var node = new LinkedNode<T>
            {
                Value = value,
                Priority = priority
            };

            if (Head == null)
            {
                Head = node;
                return node;
            }

            var next = Head!;
            while (next != null)
            {
                if (next.Priority > node.Priority)
                    break;
                next = next.Next;
            }

            node.Next = next;
            if (next!.Prev != null)
            {
                node.Prev = next.Prev;
                node.Prev.Next = node;
            }

            next.Prev = node;

            return node;
        }
    }

    public class LinkedNode<T>
    {
        public LinkedNode<T>? Next;
        public LinkedNode<T>? Prev;
        public int Priority { get; init; }

        public T? Value { get; init; }

        public void RemoveNode()
        {
            if (Next != null) Next.Prev = Prev;
            if (Prev != null) Prev.Next = Next;
        }
    }
}