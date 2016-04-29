namespace CodeFiend.WbmDrivers
{
	/// <summary>
	/// This is our simple resperentation of a magnetic card in the most simple form with each track enumerated
	/// </summary>
	public class MagneticCard
	{
		/// <summary>
		/// Gets or sets OverallStatus
		/// </summary>
		/// <remarks>
		/// Not all card readers will support track by track read status flags and some card readers might use tracks and status of track combinations to indicate other 
		/// information about "true status" this overall status is here to clearify the nature of the read result either good / bad / error
		/// </remarks>
		public MagneticCardStatus OverallStatus { get; set; }

		/// <summary>
		/// Gets or sets Track1
		/// </summary>
		/// <remarks>
		/// This is the raw ASCII data read from the card
		/// </remarks>
		public string Track1 { get; set; }

		/// <summary>
		/// Gets or sets Track1Status
		/// </summary>
		/// <remarks>
		/// This is the general status flags based on the driver read operation abstracted to provide consistant
		/// upstack programming for all devices.
		/// </remarks>
		public MagneticCardStatus Track1Status { get; set; }

		/// <summary>
		/// Gets or sets Track2
		/// </summary>
		/// <remarks>
		/// This is the raw ASCII data read from the card
		/// </remarks>
		public string Track2 { get; set; }

		/// <summary>
		/// Gets or sets Track2Status
		/// </summary>
		/// <remarks>
		/// This is the general status flags based on the driver read operation abstracted to provide consistant
		/// upstack programming for all devices.
		/// </remarks>
		public MagneticCardStatus Track2Status { get; set; }

		/// <summary>
		/// Gets or sets Track3
		/// </summary>
		/// <remarks>
		/// This is the raw ASCII data read from the card
		/// </remarks>
		public string Track3 { get; set; }

		/// <summary>
		/// Gets or sets Track3Status
		/// </summary>
		/// <remarks>
		/// This is the general status flags based on the driver read operation abstracted to provide consistant
		/// upstack programming for all devices.
		/// </remarks>
		public MagneticCardStatus Track3Status { get; set; }

		public override string ToString()
		{
			return string.Format(
				"Status: {0}\r\n" +
				"\tTrack 1: {1} - {2}\r\n" +
				"\tTrack 2: {3} - {4}\r\n" +
				"\tTrack 3: {5} - {6}\r\n",
				OverallStatus,
				Track1Status, Track1,
				Track2Status, Track2,
				Track3Status, Track3);
		}
	}
}