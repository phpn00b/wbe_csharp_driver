using System;

namespace CodeFiend.WbmDrivers.WBE
{
	/// <summary>
	/// Base class for understanding messages sent from the device
	/// </summary>
	public class Message
	{
		private readonly byte[] _data;

		/// <summary>
		/// This is the raw data for this message
		/// </summary>
		protected byte[] Data { get { return _data; } }

		/// <summary>
		/// Ctor used to create a message all classes need to handle this and should do any required processing in the ctor that invokes this
		/// </summary>
		/// <param name="data"></param>
		public Message(byte[] data)
		{
			_data = data;
		}

		/// <summary>
		/// The length of the message
		/// </summary>
		public short Length
		{
			get { return BitConverter.ToInt16(_data, 1); }
		}


		/// <summary>
		/// Used to show the Header with length of the message. As practice this should be overridden to provide more info 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("H[{0}]\r\n", Length);
		}
	}
}
