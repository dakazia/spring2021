using System;

namespace Tasks
{
    public class Node<T>
    {
        public T Data;
        public Node<T> Prev;
        public Node<T> Next;

        // Constructor to create a new node
        // Next and Prev is by default initialized as null
        public Node(T d)
        {
            Data = d;
        }
     }
}
