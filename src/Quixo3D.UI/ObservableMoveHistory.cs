using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Quixo3D.Framework;

namespace Quixo3D.UI
{
	/// <summary>
	/// An observable collection used to bind list controls to MoveHistoryInformation objects.
	/// </summary>
	public class ObservableMoveCollection : ObservableCollection<MoveHistoryInformation>
	{
		/// <summary>
		/// Creates a new instance of the class.
		/// </summary>
		/// <param name="controller"></param>
		public ObservableMoveCollection(GameBoardController controller)
			: base()
		{
			controller.MoveMade += new EventHandler<MoveMadeEventArgs>(controller_MoveMade);
			controller.GameReset += new EventHandler<EventArgs>(controller_GameReset);

			if(controller.Board.MoveHistory.Count > 0)
			{
				Players player = Players.X;
				foreach(Move move in controller.Board.MoveHistory)
				{
					this.Add(new MoveHistoryInformation(player, move));
					if(player == Players.X)
					{
						player = Players.O;
					}
					else
					{
						player = Players.X;
					}
				}
			}
		}

		void controller_GameReset(object sender, EventArgs e)
		{
			this.Clear();
		}

		void controller_MoveMade(object sender, MoveMadeEventArgs e)
		{
			MoveHistoryInformation info = new MoveHistoryInformation(e.Player, e.Move);
			this.Add(info);
		}
	}
}
