        //LinqPad C# code
         void Main()
        {
            NalogaA();
            NalogaC();
        }

        private void NalogaA()
        {
            var list = new SkipList<int>(0, 1000, new RandomFlip());
            list.Add(16);
            list.Add(14);
            list.Add(23);
            list.Add(11);
            list.Add(16);
        //    Draw<int>(list, @"C:\Users\BostjanSkok\Documents\New folder (4)\NalogaA.dgml");
        }

        private void NalogaC()
        {
            var list = new SkipList<int>(0, 1000, new PreSetFlips());
            list.Add(14);
            list.Add(6);
            list.Add(45);
            list.Add(2);
            list.Add(74);
            list.Add(45);
            list.Add(44);
            list.Add(43);
            list.Add(12);
            list.Add(27);

            list.Delete(45);
            list.Delete(6);
            list.Add(38);
            list.Add(8);
			
            Draw<int>(list, @"C:\Users\BostjanSkok\Documents\New folder (4)\NalogaC1.dgml");

            list.Add(2);
            list.Add(43);
            list.Delete(6);
            list.Add(8);

            Draw<int>(list, @"C:\Users\BostjanSkok\Documents\New folder (4)\NalogaC2.dgml");
        }

        private
            void Draw<T>(SkipList<T> sList, string path) where T : IComparable
        {
            if (sList == null)
                throw new ArgumentNullException("sList");
            var nodes = new List<string>();
            var links = new List<Tuple<string, string, bool>>();
            SkipNode<T> nodeH = sList.Header;

            int h = sList.HeaderHeight;
            while (nodeH != null)
            {
                SkipNode<T> node = nodeH;
                while (node != null)
                {
                    nodes.Add(SkipNodeToString(h, node));
                    if (node.Next != null)
                        links.Add(Tuple.Create(SkipNodeToString(h, node), SkipNodeToString(h, node.Next), false));
                    if (node.Down != null)
                        links.Add(Tuple.Create(SkipNodeToString(h, node), SkipNodeToString(h - 1, node.Down), true));
                    node = node.Next;
                }
                nodeH = nodeH.Down;
                h--;
            }

            XNamespace ns = "http://schemas.microsoft.com/vs/2009/dgml";
            var root =
                new XElement(ns + "DirectedGraph",
                    new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                    new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                    new XElement(
                        ns + "Nodes",
                        from nodeValue in nodes
                        select new XElement(ns + "Node", new XAttribute("Id", nodeValue))
                        ),
                    new XElement(ns + "Links",
                        from tuple in links
                        select new XElement(ns + "Link",
                            new XAttribute("Source", tuple.Item1),
                            new XAttribute("Target", tuple.Item2),
                            new XAttribute("Label", tuple.Item3 ? "Down" : "Next")
                            )
                        )
                    );

            root.Save(path);
        }

        private static string SkipNodeToString<T>(int h, SkipNode<T> node) where T : IComparable
        {
            return  "Value:" + node.Value
			//+ " Height:" + h
			+" (Id:" + node.ID+")"  ;
        }

    internal interface ICoinFlip
    {
        bool IsFace();
    }

    internal class RandomFlip : ICoinFlip
    {
        private readonly Random _random;

        public RandomFlip()
        {
            _random = new Random((int) DateTime.Now.Ticks);
        }

        public bool IsFace()
        {
            return _random.Next(2) == 1;
        }
    }

    internal class PreSetFlips : ICoinFlip
    {
        private readonly Queue<bool> _flips =
            new Queue<bool>(new[]
            {
                true, true, false, true, false, true, false, false, false, false, true, true, true, false, true, false,
                false, false, true, true, false, false, false, true, true, true, false, true, true, false, false, false,
                true, false, true, false, true, false, true, true, true, true, true, false, false, false, true, false,
                true, false, true, false, false, false, true, false, false, true, true, true, true, false
            });

        public bool IsFace()
        {
            return _flips.Dequeue();
        }
    }

    // Define other methods and classes here
    internal class SkipNode<T> where T : IComparable
    {
        //Id only used by diagram to separate duplicate value nodes 
        public SkipNode(T value, int id)
        {
            ID = id;
            Value = value;
        }

        public int ID { get; private set; }

        public bool IsSentinel { get; set; }
        public SkipNode<T> Next { get; set; }
        public SkipNode<T> Down { get; set; }
        public T Value { get; private set; }
    }

    internal class SkipList<T> where T : IComparable
    {
        private readonly ICoinFlip _coin;
        private readonly T _maxValue;
        private readonly T _minValue;
        private int _nextNodeId;

        /// <summary>
        ///     Only constructor for SkipList
        /// </summary>
        /// <param name="minValue">Lowest value allowed</param>
        /// <param name="maxValue">Highest value allowed </param>
        /// <param name="coin">Random coin flip generator</param>
        public SkipList(T minValue, T maxValue, ICoinFlip coin)
        {
            HeaderHeight = 0;
            _minValue = minValue;
            _maxValue = maxValue;
            _coin = coin;
            // add header virtual -infinity
            Header = new SkipNode<T>(minValue, _nextNodeId++);
            Header.IsSentinel = true;
            //Add footer virtual +infinity
            Footer = new SkipNode<T>(maxValue, _nextNodeId++);
            Footer.IsSentinel = true;
            Header.Next = Footer;
        }

        //Top most header node
        public SkipNode<T> Header { get; private set; }
        //Top most footer
        public SkipNode<T> Footer { get; private set; }

        public int HeaderHeight { get; private set; }


          public virtual void Add(T value)
        {
            if (value.CompareTo(_maxValue) >= 0 || value.CompareTo(_minValue) <= 0)
                throw new Exception("Value outside of allowed range");
            int height = GetRandomHeight(); //determine height of value's tower

            //If Header lower than new height  add  nodes
            RaiseSentinels(height);


            //Store path to insertion point on stack
            var pathStack = new Stack<SkipNode<T>>();
            SkipNode<T> current = Header;
            while (current != null)
            {
                while (value.CompareTo(current.Next.Value) >= 0)
                {
                    current = current.Next;
                }
                pathStack.Push(current);
                current = current.Down;
            }

            //Insert element starting from height 0 to height of new element
            SkipNode<T> skipNode = pathStack.Pop();
            SkipNode<T> nodeUnder = null;
            while (height >= 0 && skipNode != null)
            {
                var toAdd = new SkipNode<T>(value, _nextNodeId++);
                toAdd.Next = skipNode.Next;
                skipNode.Next = toAdd;
                toAdd.Down = nodeUnder;
                nodeUnder = toAdd;
                skipNode = pathStack.Count > 0 ? pathStack.Pop() : null;
                height--;
            }
        }

        public bool Delete(T value)
        {
            SkipNode<T> prev, current, first;
            first = Header;
            prev = null;

            bool found = false;
            while (first != null)
            {
                current = first;
                while (value.CompareTo(current.Next.Value) >= 0)
                {
                    prev = current;
                    current = current.Next;
                    if (current.Value.CompareTo(value) == 0)
                    {
                        found = true;
                        prev.Next = current.Next;
                        break;
                    }
                }
                first = first.Down;
                prev = null;
            }
            return found;
        }

        private void RaiseSentinels(int height)
        {
            if (HeaderHeight < height)
            {
                int toAdd = height - HeaderHeight;
                while (toAdd > 0)
                {
                    var headerNew = new SkipNode<T>(_minValue, _nextNodeId++);
                    headerNew.Down = Header;
                    headerNew.IsSentinel = true;
                    Header = headerNew;

                    var footerNew = new SkipNode<T>(_maxValue, _nextNodeId++);
                    footerNew.Down = Footer;
                    footerNew.IsSentinel = true;
                    Footer = footerNew;

                    Header.Next = Footer;

                    toAdd--;
                    HeaderHeight++;
                }
            }
        }

        private int GetRandomHeight()
        {
            int height = 0;
            while (_coin.IsFace())
            {
                height++;
            }
            return height;
        }
    }
