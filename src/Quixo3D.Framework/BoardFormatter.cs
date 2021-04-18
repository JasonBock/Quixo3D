using Spackle.Extensions;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.XPath;

namespace Quixo3D.Framework
{
	/// <summary>
	/// Performs custom serialization for a <see cref="Board" />.
	/// </summary>
	public sealed class BoardFormatter : IFormatter
	{
		/// <summary>
		/// Creates a new <see cref="BoardFormatter" /> instance.
		/// </summary>
		public BoardFormatter()
			: base()
		{
		}
		
		private static void AddCoordinate(XmlNode coordinateNode, Coordinate coordinate)
		{
			var xNode = coordinateNode.AppendChild(coordinateNode.OwnerDocument.CreateElement("X"));
			xNode.InnerText = coordinate.X.ToString(CultureInfo.CurrentCulture);
			var yNode = coordinateNode.AppendChild(coordinateNode.OwnerDocument.CreateElement("Y"));
			yNode.InnerText = coordinate.Y.ToString(CultureInfo.CurrentCulture);
			var zNode = coordinateNode.AppendChild(coordinateNode.OwnerDocument.CreateElement("Z"));
			zNode.InnerText = coordinate.Z.ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Returns a <see cref="Board"/> object based on the 
		/// serialized data.
		/// </summary>
		/// <param name="serializationStream">A serialized version of a <see cref="Board"/>.</param>
		/// <returns>A new <see cref="Board"/> object.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="serializationStream"/> is <code>null</code>.</exception>
		/// <exception cref="InvalidMoveException">Thrown if a move error occurred during deserialization.</exception>
		/// <exception cref="XmlException">Thrown if an XML error occurred during deserialization.</exception>
		public object Deserialize(Stream serializationStream)
		{
			serializationStream.CheckParameterForNull("serializationStream");

			var board = new Board();
			
			var document = new XmlDocument();
			document.Load(serializationStream);
			
			foreach(XmlNode moveNode in document.DocumentElement.ChildNodes)
			{
				board.MovePiece(BoardFormatter.GetCoordinate(moveNode["Source"]),
					BoardFormatter.GetCoordinate(moveNode["Destination"]));
			}

			return board;
		}

		private static Coordinate GetCoordinate(XmlNode coordinateNode)
		{
			return new Coordinate(byte.Parse(coordinateNode["X"].InnerText, CultureInfo.CurrentCulture),
				byte.Parse(coordinateNode["Y"].InnerText, CultureInfo.CurrentCulture),
				byte.Parse(coordinateNode["Z"].InnerText, CultureInfo.CurrentCulture));		
		}
		
		/// <summary>
		/// Serializes the given <see cref="Board"/> into the stream.
		/// </summary>
		/// <param name="serializationStream">The stream to serialize the given <see cref="Board"/>.</param>
		/// <param name="graph">A <see cref="Board"/> object to serialize.</param>
		/// <exception cref="ArgumentNullException">Thrown if either <paramref name="serializationStream"/> or <paramref name="graph"/> are <code>null</code>.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="graph"/> is not a <see cref="Board"/>.</exception>
		public void Serialize(Stream serializationStream, object graph)
		{
			graph.CheckParameterForNull("graph");
			serializationStream.CheckParameterForNull("serializationStream");

			var board = graph as Board;

			if(board == null)
			{
				throw new ArgumentException("The given object must be a Board.", "graph");
			}

			var document = new XmlDocument();

			document.AppendChild(document.CreateElement("Board"));

			foreach(var move in board.MoveHistory)
			{
				var moveNode = document.DocumentElement.AppendChild(document.CreateElement("Move"));
				var sourceNode = moveNode.AppendChild(document.CreateElement("Source"));
				BoardFormatter.AddCoordinate(sourceNode, move.Source);
				var destinationNode = moveNode.AppendChild(document.CreateElement("Destination"));
				BoardFormatter.AddCoordinate(destinationNode, move.Destination);
			}

			document.Save(serializationStream);
		}

		/// <summary>
		/// Gets or sets the binder.
		/// </summary>
		public SerializationBinder Binder
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the context.
		/// </summary>
		public StreamingContext Context
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the surrogate selector.
		/// </summary>
		public ISurrogateSelector SurrogateSelector
		{
			get;
			set;
		}
	}
}
