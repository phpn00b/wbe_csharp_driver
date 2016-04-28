using System;

namespace CodeFiend.WbmDrivers.Devices.CardReaders
{
	/// <summary>
	/// Defines the interface for a magnetic card reader 
	/// </summary>
	public interface IMagneticCardReader : ISerialDeviceDriver
	{
		/// <summary>
		/// Gets or sets IsSwipeReader
		/// </summary>
		/// <remarks>
		/// It can be useful to know that a reader is insertion verses a insertion reader as a swipe reader will only send a card inserted and we will have no idea if the user is still present after swipe
		/// </remarks>
		bool IsSwipeReader { get; }

		/// <summary>
		/// Gets or sets SupportsInsertNotification
		/// </summary>
		/// <remarks>
		/// does this support notification when a card is inserted
		/// </remarks>
		bool SupportsInsertNotification { get; }

		/// <summary>
		/// Gets or sets SupportsRemovalNotification
		/// </summary>
		/// <remarks>
		/// does this support notification when a card is removed
		/// </remarks>
		bool SupportsRemovalNotification { get; }

		/// <summary>
		/// this is an event that fires when we have a card inserted
		/// </summary>
		event EventHandler<MagneticCardEventArgs> CardInserted;

		/// <summary>
		/// this is an event that is fired when we have a card removed 
		/// </summary>
		event EventHandler<MagneticCardEventArgs> CardRemoved;

		/// <summary>
		/// This is an event that is fired when the device gets a read error
		/// </summary>
		event EventHandler<MagneticCardEventArgs> ReadError;
	}
}