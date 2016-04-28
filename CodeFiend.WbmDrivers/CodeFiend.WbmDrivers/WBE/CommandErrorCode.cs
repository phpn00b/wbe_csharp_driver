namespace CodeFiend.WbmDrivers.WBE
{
	/// <summary>
	/// This is an enumeration of all the errors or lack of error that can happen when we issue the reader a supported command
	/// </summary>
	public enum CommandErrorCode : byte
	{
		/// <summary>
		/// Command accepted all ok
		/// </summary>
		NormalExecution = 0x30,

		/// <summary>
		/// our LRC data didn't match what the device expected us to send 
		/// </summary>
		CommunicationLrcError = 0x31,

		/// <summary>
		/// There was an error in the command we issued other than LRC mismatch
		/// </summary>
		CommandError = 0x32,

		/// <summary>
		/// Something wrong with our data
		/// </summary>
		DataFormError = 0x33,

		/// <summary>
		/// ?
		/// </summary>
		DoNotExecuteLocker = 0x34,

		/// <summary>
		/// There is no card inserted
		/// </summary>
		NoCardInModule = 0x36,

		/// <summary>
		/// ?
		/// </summary>
		CardOperateError = 0x37
	}
}