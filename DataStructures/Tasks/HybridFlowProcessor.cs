using System;
using Tasks.DoNotChange;

namespace Tasks
{
    public class HybridFlowProcessor<T> : IHybridFlowProcessor<T>
    {
        private readonly DoublyLinkedList<T> list;

        public HybridFlowProcessor()
        {
            list = new DoublyLinkedList<T>();
        }

        public T Dequeue()
        {
            VerifyListHasElement(list);

            return list.RemoveAt(0);
        }

        public void Enqueue(T item)
        {
            list.Add(item);
        }

        public T Pop()
        {
            VerifyListHasElement(list);

            return list.RemoveAt(list.Length - 1);
        }

        public void Push(T item)
        {
            list.Add(item);
        }

        private void VerifyListHasElement(DoublyLinkedList<T> list)
        {
            if (list.Length == 0)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
