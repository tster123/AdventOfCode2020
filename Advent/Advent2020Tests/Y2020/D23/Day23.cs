using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
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
            GiveAnswer(Problem2(GetData()));
        }

        private object Problem2(Game game)
        {
            return null;
        }
    }

    public class Node
    {
        public int Value;
        public Node Prev, Next;

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
        private bool log = true;
        public readonly int MaxInput;

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
            }

            Current.Prev = thisNode;
            thisNode.Next = Current;
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
            Node closest = current;
            Node candidate = current;
            do
            {
                if (closest.Value >= current.Value && candidate.Value < current.Value)
                {
                    // closest is higher, candidate is lower
                    closest = candidate;
                }
                else if (closest.Value < current.Value && candidate.Value < current.Value)
                {
                    // both lower
                    if (candidate.Value > closest.Value) closest = candidate;
                }
                else if (closest.Value >= current.Value && candidate.Value >= current.Value)
                {
                    // both higher
                    if (candidate.Value > closest.Value) closest = candidate;
                }
                candidate = candidate.Next;
            } while (candidate != current);

            return closest;
        }

        private void Place(Node destination, Tuple<Node, Node> pickedUp)
        {
            Node afterDest = destination.Next;
            destination.Next = pickedUp.Item1;
            pickedUp.Item1.Prev = destination;
            pickedUp.Item2.Next = afterDest;
            afterDest.Prev = pickedUp.Item2;
        }

        private Tuple<Node, Node> PickUp(Node current, int num)
        {
            Node f = current.Next;
            Node l = f;
            for (int i = 1; i < num; i++)
            {
                l = l.Next;
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
    }
}
