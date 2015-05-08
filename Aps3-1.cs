//LinqPad koda 

void Main()
{
	//test();
	//NalogaPrimer();
	//NalogaA();
	//NalogaB(3,1000);
	NalogaC(5,30)
}

void test(){
var a  =  new RankTree<int>();
	
	a.Insert(1);
	a.Insert(2);
	a.Insert(3);
	a.Insert(4);
	a.Insert(5);
	a.Insert(6);
	a.Insert(7);
	  a.Insert(8);
	a.SelectRank(4).Dump();
//	a.Delete(4);
//	a.Delete(6);
	a.DeleteByRank(4);
	a.DeleteByRank(4);	
  //  a.DeleteByRank(4);
	a.SelectRank(4).Dump();
	
	
	
	Draw<int>(a.Root);
}

void NalogaPrimer()
		{
			var tree = new RankTree<int>();
			for (int i = 1; i <= 10; i++)
			{
				tree.Insert(i);
			}
			int step = 3;
		//	Draw<int>(tree.Root);
			int j = step;
			
			while (tree.Root!=null)
			{
				int rank =j%10;
			 //    Console.Out.Write("["+rank+"]");
				
				var rezNode =tree.DeleteByRank(rank);
				int rez = rezNode == null?-1:rezNode.Key;
			    if (rez > 0){
			        Console.Out.Write(","+rez);
					//j++;
					}
			    j += step;

			}
		}
		
void NalogaA(){
	
			var tree = new RankTree<int>();
			for (int i = 1; i <= 30; i++)
			{
				tree.Insert(i);
			}
			int step = 7;
			Draw<int>(tree.Root);
			int j = step;
			while (tree.Root!=null)
			{
				int rank =j%30;
			 //   Console.Out.Write(rank+";");
				
			var rezNode =tree.DeleteByRank(rank);
				int rez = rezNode == null?-1:rezNode.Key;
			   if (rez > 0)
			       Console.Out.Write(","+rez);
			    j += step;

			}
		
}

void NalogaB(int m ,int n){
	
			var tree = new RankTree<int>();
			for (int i = 1; i <= n; i++)
			{
				tree.Insert(i);
			}
			int step = m;
		//	Draw<int>(tree.Root);
			int j = step;
			while (tree.Root!=null)
			{
				int rank =j%n;
			//     Console.Out.Write(","+rank);
				
			var rezNode =tree.DeleteByRank(rank);
				int rez = rezNode == null?-1:rezNode.Key;
			    if (rez > 0)
			        Console.Out.Write(","+rez);
			    j += step;

			}
		
}

void NalogaC(int m ,int n){
	
			var tree = new RankTree<int>();
			for (int i = 1; i <= n; i++)
			{
				tree.Insert(i);
			}
			int j = m;
			while (tree.Root!=null)
			{
				int rank =j%n;
			     Console.Out.Write(","+rank);
				
			var rezNode =tree.DeleteByRank(rank);
				int rez = rezNode == null?-1:rezNode.Key;
			    if (rez > 0){
			        Console.Out.Write(","+rez);
					m=rez;
					}
			    j += m;
			}
}

// Define other methods and classes here
  public class RankTreeNode<T>   where T : IComparable
    {
        private int _height;
        public int Size { get; set; }


        public RankTreeNode(T key) 
        {
            Key = key;
            Size = 1;
        }

        public T Key { get; set; }
        public  RankTreeNode<T> Parent { get; set; }
        public  RankTreeNode<T> Left { get; set; }
        public RankTreeNode<T> Right { get; set; }

        public void UpdateHeight()
        {
            _height = Math.Max(GetHeight(Left), GetHeight(Right)) + 1;
        }

        public static int GetHeight(RankTreeNode<T> node)
        {
            if (node == null)
                return -1;
            return node._height;
        }

        public int LeftSize()
        {
            return Left != null ? Left.Size : 0;
        }

        public int RightSize()
        {
            return Right != null ? Right.Size : 0;
        }

        public void UpdateSize()
        {
            Size = RightSize() + LeftSize() + 1;
        }

        public RankTreeNode<T> Find(T key)
        {
            if (key.CompareTo(Key) == 0)
                return this;
            if (key.CompareTo(Key) < 0)
                return Left == null ? null : Left.Find(key);
            return Right == null ? null : Right.Find(key);
        }

        public RankTreeNode<T> FindRank(int rank)
        {
            var r = LeftSize() + 1;
            if (r==rank)
                return this;
            if (rank < r)
                return Left == null ? null : Left.FindRank(rank);
            return Right == null ? null : Right.FindRank(rank-r);
        }

        //Returns left most child
        public RankTreeNode<T> GetMin()
        {
            var result = this;
            while (result.Left != null)
            {
                result = result.Left;
            }
            return result;
        }

        public RankTreeNode<T> GetNextlarger()
        {
            if (Right != null)
                return Right.GetMin();
            //We walk up the tree until we are the left child of our parent or become root 
            // when we are the left child of a parent that parent is our next larger
            var result = this;
            while (result.Parent != null && result == result.Parent.Right)
            {
                result = result.Parent;
            }
            return result.Parent;
        }

        public void Insert(RankTreeNode<T> node)
        {
            if (node == null)
                return;
            if (Key.CompareTo(node.Key) > 0)
            {
                if (Left == null)
                {
                    node.Parent = this;
                    Left = node;
                }
                else
                    Left.Insert(node);
            }
            else if (Key.CompareTo(node.Key) < 0)
            {
                if (Right == null)
                {
                    node.Parent = this;
                    Right = node;
                }
                else

                    Right.Insert(node);
            }
            else
            {
                //TODO handle duplicates
                throw new NotImplementedException("Duplicate key");
            }
        }

        //Deletes self from tree
        public RankTreeNode<T> Delete()
        {
            if (Left == null || Right == null)
            {
                if (this == Parent.Left)
                {
                    Parent.Left = Left ?? Right;
                    if (Parent.Left != null)
                        Parent.Left.Parent = Parent;
                }
                else
                {
                    Parent.Right = Left ?? Right;
                    if (Parent.Right != null)
                        Parent.Right.Parent = Parent;
                }
                return this;
            }
            //TODO test this because python code strange
            var s = GetNextlarger();
            var tempKey = Key;
            Key = s.Key;
            s.Key = tempKey ;
            return s.Delete();
        }

        public void CheckInvariant()
        {
            if (Left != null)
            {
                if (Left.Key.CompareTo(Key) > 0)
                    throw new Exception("Binary Search Tree invariant violated by left node key");
                if (Left.Parent != this)
                    throw new Exception("Binary Search Tree invariant violated by left node parent pointer");
                Left.CheckInvariant();
            }
            if (Right != null)
            {
                if (Right.Key.CompareTo(Key) < 0)
                    throw new Exception("Binary Search Tree invariant violated by Right node key");
                if (Right.Parent != this)
                    throw new Exception("Binary Search Tree invariant violated by right node parent pointer");
                Right.CheckInvariant();
            }
        }
    }
	
	
	 public class RankTree<T> where T : IComparable
    {
        public RankTreeNode<T> Root { get; private set; }

        public void Insert(T key)
        {
            var node = new RankTreeNode<T>(key);
            if (Root == null)
                Root = node;
            else
                Root.Insert(node);
            Rebalance(node);
        }

        public RankTreeNode<T> Delete(T key)
        {
            var node = Root.Find(key);
            if (node == null)
                return null;
            if (node == Root)
            {
                var pseudoroot = new RankTreeNode<T>(default(T));
                pseudoroot.Left = Root;
                Root.Parent = pseudoroot;
                var deleted = Root.Delete();
                Root = pseudoroot.Left;
                if (Root != null)
                    Root.Parent = null;
            	Rebalance(deleted.Parent);
                return deleted;
            }
            var a = node.Delete();
            Rebalance(a.Parent);
            return a;
        }

        public T SelectRank(int rank)
        {
            return SelectRank(Root, rank);
        }


        private T SelectRank(RankTreeNode<T> node, int rank)
        {
            int r = node.LeftSize() + 1;
            if (r == rank)
                return node.Key;
            if (rank < r)
                return SelectRank(node.Left, rank);
            return SelectRank(node.Right, rank - r);
        }

   /*     public T DeleteByRank(int rank)
        {
           // lastDeleted = default(T);
            DeleteByRank(Root, rank);
           // return lastDeleted;

        }
        */
        public RankTreeNode<T> DeleteByRank(int rank)
        {
            var node = Root.FindRank(rank);
            if (node == null)
                return null;
            if (node == Root)
            {
                var pseudoroot = new RankTreeNode<T>(default(T));
                pseudoroot.Left = Root;
                Root.Parent = pseudoroot;
                var deleted = Root.Delete();
                Root = pseudoroot.Left;
                if (Root != null)
                    Root.Parent = null;
	            Rebalance(deleted.Parent);
                return deleted;
            }
            var a = node.Delete();
            Rebalance(a.Parent);
            return a;
        }


        private void Rebalance(RankTreeNode<T> node)
        {
            while (node != null)
            {
                node.UpdateHeight();
                node.UpdateSize();
				
				
                if (RankTreeNode<T>.GetHeight(node.Left) >= 2 + RankTreeNode<T>.GetHeight(node.Right))
                {
                    if (RankTreeNode<T>.GetHeight(node.Left.Left) >= RankTreeNode<T>.GetHeight(node.Left.Right))
                    {
                        RightRotate(node);
                    }
                    else
                    {
                        LeftRotate(node.Left);
                        RightRotate(node);
                    }
                }
                else if (RankTreeNode<T>.GetHeight(node.Right) >= 2 + RankTreeNode<T>.GetHeight(node.Left))
                {
                    if (RankTreeNode<T>.GetHeight(node.Right.Right) >= RankTreeNode<T>.GetHeight(node.Right.Left))
                    {
                        LeftRotate(node);
                    }
                    else
                    {
                        RightRotate(node.Right);
                        LeftRotate(node);
                    }
                }
                node = node.Parent;
            }
        }

        private void LeftRotate(RankTreeNode<T> x)
        {
            var y = x.Right;
            y.Parent = x.Parent;
            if (y.Parent == null)
                Root = y;
            else
            {
                if (y.Parent.Left == x)
                    y.Parent.Left = y;
                else if (y.Parent.Right == x)
                    y.Parent.Right = y;
            }
            x.Right = y.Left;
            if (x.Right != null)
                x.Right.Parent = x;
            y.Left = x;
            x.Parent = y;
            x.UpdateHeight();
            x.UpdateSize();
            y.UpdateHeight();
            y.UpdateSize();
        }

        private void RightRotate(RankTreeNode<T> x)
        {
            var y = x.Left;
            y.Parent = x.Parent;
            if (y.Parent == null)
                Root = y;
            else
            {
                if (y.Parent.Left == x)
                    y.Parent.Left = y;
                else if (y.Parent.Right == x)
                    y.Parent.Right = y;
            }
            x.Left = y.Right;
            if (x.Left != null)
                x.Left.Parent = x;
            y.Right = x;
            x.Parent = y; 
            x.UpdateHeight();
            x.UpdateSize();
            y.UpdateHeight();
            y.UpdateSize();
        }
    }



void Draw<T>(RankTreeNode<T> node) where T : IComparable
{
	if (node == null)
	{
		throw new ArgumentNullException("node");
	}
 
	var nodes = new List<string>();
	var links = new List<Tuple<string, string, bool>>();
	var queue = new Queue<RankTreeNode<T>>();
	queue.Enqueue(node);
 
	while (queue.Count > 0)
	{
		RankTreeNode<T> current = queue.Dequeue();
		nodes.Add(current.Key+"--"+current.Size);
 
		if (current.Left != null)
		{
			links.Add(Tuple.Create(current.Key+"--"+current.Size, current.Left.Key+"--"+current.Left.Size, true));
			queue.Enqueue(current.Left);
		}
		if (current.Right != null)
		{
			links.Add(Tuple.Create(current.Key+"--"+current.Size, current.Right.Key+"--"+current.Right.Size, false));
			queue.Enqueue(current.Right);
		}
	}
 
	XNamespace ns = "http://schemas.microsoft.com/vs/2009/dgml";
	var root =
		new XElement
		(
			ns + "DirectedGraph",
			new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
			new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
			new XElement
			(
				ns + "Nodes",
				from nodeValue in nodes
				select new XElement
				(
					ns + "Node",
					new XAttribute("Id", nodeValue)
				)
			),
			new XElement
			(
				ns + "Links",
				from tuple in links
				select new XElement
				(
					ns + "Link",
					new XAttribute("Source", tuple.Item1),
					new XAttribute("Target", tuple.Item2),
					new XAttribute("Label", tuple.Item3 ? "Left" : "Right")
				)
			)
		);
 
	root.Save(@"C:\Users\Boštjan\Documents\RankTree.dgml");
}
