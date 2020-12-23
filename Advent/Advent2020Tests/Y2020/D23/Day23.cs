using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Advent2020Tests.Common;
using AdventLibrary;

namespace Advent2020Tests.Y2020.D23
{
    [TestClass]
    public class Day23 : AdventTest
    {

        public Game GetData()
        {
            //return new Game(10, "389125467");
            return new Game(100, "362981754");
        }

        [TestMethod]
        public void Problem1()
        {
            GiveAnswer("24798635", Problem1(GetData()));
        }

        private object Problem1(Game game)
        {
            for (int i = 0; i < game.Rounds; i++)
            {
                game.Tick();
            }

            return game.GetOrder(1);
        }

        [TestMethod]
        public void Problem2()
        {
            GiveAnswer(12757828710, Problem2(GetData()));
        }

        private object Problem2(Game game)
        {
            game.AddGame2Cups();
            for (int i = 0; i < 10000000; i++)
            {
                game.Tick();
            }

            Node n = game.GetNode(1);
            return ((long)n.Next.Value) * n.Next.Next.Value;
        }
    }

    public class Node
    {
        public int Value;
        public Node Prev, Next;
        public bool PickedUp = false;

        public Node(int value, Node prev, Node next)
        {
            Value = value;
            Prev = prev;
            Next = next;
        }
    }

    public class Game
    {
        public int Rounds;
        public Node Current;
        public int MaxInput;
        public int NodeCount = 0;
        private readonly Dictionary<int, Node> nodeHash = new Dictionary<int, Node>();
        public Game(int rounds, string start)
        {
            Rounds = rounds;
            Node thisNode = null;
            MaxInput = -1;
            foreach (int i in start.Select(c => int.Parse(c + "")))
            {
                MaxInput = Math.Max(MaxInput, i);
                if (Current == null)
                {
                    Current = new Node(i, null, null);
                    thisNode = Current;
                }
                else
                {
                    Node next = new Node(i, thisNode, null);
                    thisNode.Next = next;
                    thisNode = next;
                }

                nodeHash[i] = thisNode;
                NodeCount++;
            }

            Current.Prev = thisNode;
            thisNode.Next = Current;
        }

        public void AddGame2Cups()
        {
            int next = MaxInput + 1;
            Node lastNode = Current.Prev;
            while (NodeCount < 1000000)
            {
                Node n = new Node(next, lastNode, Current);
                Current.Prev = n;
                lastNode.Next = n;
                lastNode = n;

                NodeCount++;
                MaxInput = next;
                nodeHash[next] = n;
                next++;
            }
        }

        public void Tick()
        {
            var pickedUp = PickUp(Current, 3);
            Node destination = GetDestination(Current);
            Place(destination, pickedUp);
            Current = Current.Next;
        }

        private Node GetDestination(Node current)
        {
            int tryNum = current.Value;
            while (true)
            {
                tryNum--;
                if (tryNum < 0) tryNum = MaxInput;

                if (nodeHash.TryGetValue(tryNum, out var n) && !n.PickedUp) return n;
            }
        }

        private void Place(Node destination, Tuple<Node, Node> pickedUp)
        {
            Node f = pickedUp.Item1;
            while (f != null)
            {
                f.PickedUp = false;
                f = f.Next;
            }
            Node afterDest = destination.Next;
            destination.Next = pickedUp.Item1;
            pickedUp.Item1.Prev = destination;
            pickedUp.Item2.Next = afterDest;
            afterDest.Prev = pickedUp.Item2;
        }

        private Tuple<Node, Node> PickUp(Node current, int num)
        {
            Node f = current.Next;
            f.PickedUp = true;
            Node l = f;
            for (int i = 1; i < num; i++)
            {
                l = l.Next;
                l.PickedUp = true;
            }

            current.Next = l.Next;
            l.Next.Prev = current;
            f.Prev = null;
            l.Next = null;
            return new Tuple<Node, Node>(f, l);
        }

        public string GetOrder(int from)
        {
            Node start = Current;
            while (start.Value != from)
                start = start.Next;
            string r = "";
                Node c = start.Next;
            while (c != start)
            {
                r += c.Value;
                c = c.Next;
            }

            return r;
        }

        public Node GetNode(int value) => nodeHash[value];
    }
}
