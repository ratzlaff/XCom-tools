using System;
using System.Drawing;
using System.Collections;
using XCom;
using System.Windows.Forms;
using System.IO;

namespace XCom
{
	public delegate void BufferChangedDelegate(xConsole.Node current);

	public class xConsole
	{
		private static Node currLine = null;
		public static event BufferChangedDelegate BufferChanged;

		private static int numNodes=0;
		private static StreamWriter sw;

		private xConsole()
		{
		}

		public static Node CurrNode
		{
			get { return currLine; }
		}

		public static int NumNodes
		{
			get { return numNodes; }
		}

		private static void makeNodes(int numLines)
		{
			if (currLine == null) {
				currLine = new Node("");
				currLine.Next = new Node("");

				Node curr = currLine.Next;
				Node last = currLine;
				curr.Last = last;
				for (int i = 2; i < numLines; i++) {
					curr.Next = new Node("");
					curr = curr.Next;
					curr.Last = last.Next;
					last = last.Next;
				}

				curr.Next = currLine;
				currLine.Last = curr;
			} else {
				if (numLines > numNodes) {
					Node curr = currLine;
					Node last = currLine.Last;

					for (int i = 0; i < numLines - numNodes; i++) {
						Node n = new Node("");
						n.Next = curr;
						n.Last = last;
						last.Next = n;
						curr.Last = n;
						last = n;
					}
				} else {
					for (int i = 0; i < numNodes - numLines; i++) {
						currLine.Last = currLine.Last.Last;
						currLine.Last.Next = currLine;
					}
				}
			}
		}

		public static void Init(int numLines)
		{
			makeNodes(numLines);
			numNodes = numLines;
		}

		public static void AddLine(string s)
		{
			if (numNodes == 0)
				Init(20);

			currLine = currLine.Last;
			currLine.Str = s;

			if (BufferChanged != null)
				BufferChanged(currLine);

			if (sw != null)
				sw.WriteLine(s);
		}

		public static void SetLine(string s)
		{
			currLine.Str = s;
			if (BufferChanged != null)
				BufferChanged(currLine);

			if (sw != null)
				sw.WriteLine("-+ " + s);
		}

		public static void AddToLine(string s)
		{
			currLine.Str += s;
			if (BufferChanged != null)
				BufferChanged(currLine);

			if (sw != null)
				sw.Write(s);
		}

		public static void LogToFile(string filename)
		{
			if (sw != null) {
				sw.Flush();
				sw.Close();
			}

			sw = new StreamWriter(File.Open(filename, FileMode.Create));
		}

		public class Node
		{
			public Node Last { get; set; }
			public Node Next { get; set; }
			public string Str { get; set; }

			public Node(string str)
			{
				Str = str;
			}
		}
	}
}