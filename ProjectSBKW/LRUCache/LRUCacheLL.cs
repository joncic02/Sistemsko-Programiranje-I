using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSBKW.LRUCache
{
    public class LRUCacheLL
    {
        private DoublyLinkedList list;
        private Dictionary<string, Node> hash;
        private int capacity;
        private static readonly ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        private int count;

        public LRUCacheLL(int capacity)
        {
            this.capacity = capacity;
            this.list = new DoublyLinkedList();
            this.hash = new Dictionary<string, Node>(); // Inicijalizacija recnika uz pomoc konstruktora
            this.count = 0;
        }

        public string Get(string key)
        {
            cacheLock.EnterReadLock();
            try
            {
                Node node;
                if (hash.TryGetValue(key, out node))
                {
                    list.MoveToFront(node);
                    return node.Value;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;

            }
            finally { cacheLock.ExitReadLock(); }
        }

        public void Put(string key, string value)
        {
            cacheLock.EnterWriteLock();
            try
            {
                Node node;
                if (hash.TryGetValue(key, out node))
                {
                    node.Value = value;
                    list.MoveToFront(node);
                }
                else
                {
                    if (count >= capacity)
                    {
                        Node tail = list.Tail;
                        list.Remove(tail);
                        hash.Remove(tail.Key);
                        count--;
                    }
                    node = new Node(key, value);
                    list.AddToFront(node);
                    hash.Add(key, node);
                    count++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;

            }
            finally { cacheLock.ExitWriteLock(); }
        }

        public void Remove(string key)
        {
            try
            {
                Node node;
                if (hash.TryGetValue(key, out node))
                {
                    list.Remove(node);
                    hash.Remove(key);
                    count--;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;

            }
        }

        public void WriteOutCatcheToConsole()
        {
            try
            {
                Node node = list.Head;
                Console.WriteLine("Trenutni sadrzaj Cache memorije: ");
                
                do
                {
                    Console.WriteLine($"Keyword: '{node.Key}' - Cached Response: {node.Value}");
                    node = node.Next;
                } while (node != list.Head);
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;

            }
        }
    }
}
