namespace CodeFiend.WbmDrivers.WBE
{
	/// <summary>
	/// This is an enumeration of read errors or lack of errors that can be reported on a read opeartion 
	/// </summary>
	public enum ReadErrorCode : byte
	{
		/// <summary>
		/// Everything is good (this is the anti error)
		/// </summary>
		NormalExecution = 0x30,

		/// <summary>
		/// The track is empty 
		/// </summary>
		BlankError = 0x31,

		/// <summary>
		/// Something wrong with header
		/// </summary>
		PreambleError = 0x32,

		/// <summary>
		/// Something wrong with footer
		/// </summary>
		PostambleError = 0x33,

		/// <summary>
		/// Something is wrong with the parity data on the mag card vs what was actually read by the reader
		/// </summary>
		ParityError = 0x34,

		/// <summary>
		/// something happened with the communication protocol of the reader
		/// </summary>
		LrcError = 0x35
	}
}