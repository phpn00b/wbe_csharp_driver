
namespace CodeFiend.WbmDrivers.WBE
{
	/// <summary>
	/// This message is used to understand the insertion status message from the card reader
	/// </summary>
	public class InsertionStatusMessage1370 : Message
	{
		/// <summary>
		/// Status of Position 1 Sensor
		/// </summary>
		public bool Position1 { get; private set; }

		/// <summary>
		/// Status of Position 2 Sensor
		/// </summary>
		public bool Position2 { get; private set; }



		/// <summary>
		/// Is a card inserted like at all even just as far as the first sensor
		/// </summary>
		public bool Inserted { get { return IsInserted(); } }

		/// <summary>
		/// The error (or lack of) code 
		/// </summary>
		public CommandErrorCode ErrorCode { get { return (CommandErrorCode)Data[3]; } }

		/// <summary>
		/// Default ctor that parses the raw read data and makes it consumable
		/// </summary>
		/// <param name="data"></param>
		public InsertionStatusMessage1370(byte[] data)
			: base(data)
		{
			Position1 = data[4] == 0x31;
			Position2 = data[5] == 0x31;
		}

		/// <summary>
		/// Used to check if a card is registering at any of the 3 sensors
		/// </summary>
		/// <returns></returns>
		private bool IsInserted()
		{
			return Position1 || Position2;
		}

		/// <summary>
		/// Makes the contents of this message dump to a easy to understand string 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(
				"{0} - {1:1;0;0}{2:1;0;0}",
				ErrorCode,
				Position1.GetHashCode(),
				Position2.GetHashCode());
		}
	}
}