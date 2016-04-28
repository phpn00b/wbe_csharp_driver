namespace CodeFiend.WbmDrivers.Devices.CardReaders
{
	/// <summary>
	/// This is an enumeration of events in the life cycle of a magnetic card 
	/// </summary>
	public enum MagneticCardEvent
	{
		/// <summary>
		/// Something else happened
		/// </summary>
		Other = 0,

		/// <summary>
		/// The card is inserted
		/// </summary>
		Insert = 1,

		/// <summary>
		/// The card is removed
		/// </summary>
		Remove = 2,

		/// <summary>
		/// An error occcured
		/// </summary>
		Error = 3
	}
}