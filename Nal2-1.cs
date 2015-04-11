//LinqPad C# code 

void Main()
{
	//var root =testTree();

	var inOrder = new int[]{2,4,5,6,7,8,15,23,45,47,50,71};
	var preOrder = new int[]{15,6,4,2,5,7,8,23,71,50,45,47};
	//Draw naloga c
	var inOrderC = new int[]{22, 16, 14, 10, 2, 8, 25, 7, 1, 6, 42, 5, 19, 40, 3, 33, 15, 39};
	var preOrderC= new int[]{7, 16, 22, 8, 14, 2, 10, 25, 1, 19, 42, 6, 5, 33, 40, 3, 39, 15};
	var root = BuildTree(preOrderC,inOrderC);
//	var root = BuildTree(preOrder,inOrder);
	Draw<int>(root,@"C:\Users\BostjanSkok\Documents\New folder (4)\BinaryTreeNode.dgml");
}
private BinaryTreeNode<int> BuildTree(int[] preOrd, int[] inOrd)
{
  return BuildTreeR(inOrd, 0, inOrd.Length - 1, preOrd, 0);
}

private BinaryTreeNode<int> BuildTreeR(int[] inOrd, int inStart, int inEnd, int[] preOrd, int preStart)
{
   //found leaf return node without children
    if (inStart == inEnd) return new BinaryTreeNode<int>(inOrd[inStart]);
    // handled all nodes in subtree
    if (inStart > inEnd) return null;

    int indexOfRoot;
    int root = preOrd[preStart];
    //search for root index in inOrder table
    for (indexOfRoot = inStart; indexOfRoot < inEnd; indexOfRoot++) 
        if (inOrd[indexOfRoot] == root) break;

    return new BinaryTreeNode<int>
    {
        Value = root,
        //Build left subtree 
        //inOrder elements to handle form 0 up to index of found root
        // preOrder move start of virtual table up by 1 so preOrd[preStart] will be the next root
        Left = BuildTreeR(inOrd, inStart, indexOfRoot - 1, preOrd, preStart + 1),
        //Build Right subtree
        //inOrder elements to handle run from found root index +1 to end of table
        //preOrder move start up by the number of elements in left subTree using index of root
        Right = BuildTreeR(inOrd, indexOfRoot + 1, inEnd, preOrd,
        preStart + indexOfRoot - inStart + 1),
    };
}

// Define other methods and classes here
 class BinaryTreeNode<T> 
    {
        public BinaryTreeNode<T>  Left {get;set;}
        public BinaryTreeNode<T>  Right{get;set;}
        public T Value{get;set;}
		
		public BinaryTreeNode (T data){
		this.Value= data;
		}
		public BinaryTreeNode(){}
    }
    
//Not my code   
//The Little Engineer That Could http://www.weiwang.info/?p=2151
void Draw<T>(BinaryTreeNode<T> node, string path) where T : IComparable<T>
{
	if (node == null)
	{
		throw new ArgumentNullException("node");
	}
 
	var nodes = new List<T>();
	var links = new List<Tuple<T, T, bool>>();
	var queue = new Queue<BinaryTreeNode<T>>();
	queue.Enqueue(node);
 
	while (queue.Count > 0)
	{
		BinaryTreeNode<T> current = queue.Dequeue();
		nodes.Add(current.Value);
 
		if (current.Left != null)
		{
			links.Add(Tuple.Create(current.Value, current.Left.Value, true));
			queue.Enqueue(current.Left);
		}
		if (current.Right != null)
		{
			links.Add(Tuple.Create(current.Value, current.Right.Value, false));
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
 
	root.Save(path);
}
