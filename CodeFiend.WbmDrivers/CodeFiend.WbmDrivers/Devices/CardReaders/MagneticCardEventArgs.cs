using System;

namespace CodeFiend.WbmDrivers.Devices.CardReaders
{
	/// <summary>
	/// This is the arguments used for card events 
	/// </summary>
	public class MagneticCardEventArgs : EventArgs
	{
		private readonly MagneticCard _magneticCard;
		private readonly MagneticCardEvent _magneticCardEvent;

		/// <summary>
		/// Default ctor
		/// </summary>
		/// <param name="cardEvent">type of event</param>
		/// <param name="card">the card for the event</param>
		public MagneticCardEventArgs(MagneticCardEvent cardEvent, MagneticCard card)
		{
			_magneticCardEvent = cardEvent;
			_magneticCard = card;
		}

		/// <summary>
		/// Gets or sets MagneticCardEvent
		/// </summary>
		public MagneticCardEvent MagneticCardEvent
		{
			get { return _magneticCardEvent; }
		}

		/// <summary>
		/// Gets or sets MagneticCard
		/// </summary>
		public MagneticCard MagneticCard
		{
			get { return _magneticCard; }
		}
	}
}