namespace CodeFiend.WbmDrivers
{
	/// <summary>
	/// This defines the interface for interacting with serial devices
	/// </summary>
	public interface ISerialDeviceDriver 
	{
		/// <summary>
		/// This is invoked to start the device normal operation 
		/// </summary>
		void StartDevice();

		/// <summary>
		/// used to report if this device can auto detect the port it is connected to
		/// </summary>
		/// <remarks>
		/// Not supported yet
		/// </remarks>
		bool SupportsConnectionDetection { get; }

		/// <summary>
		/// Used to return if the device is currently running 
		/// </summary>
		bool IsActive { get; }

		/// <summary>
		/// This is used to enable or disable dumping debug info to the console for the driver
		/// </summary>
		bool EnableDebug { get; set; }

		/// <summary>
		/// Used to shutdown the device
		/// </summary>
		void StopDevice();
		/// <summary>
		/// The name of the serial port that the device is running on 
		/// </summary>
		string ActiveComPort { get; }

		/// <summary>
		/// Used to open the serial port that will handle the operation. 
		/// </summary>
		/// <remarks>
		/// Note that this does not actually start the device operating that is done by StartDevice
		/// </remarks>
		/// <param name="comPortName"></param>
		void OpenPort(string comPortName);

		/// <summary>
		/// Used to shutdown the device 
		/// </summary>
		void ClosePort();
	}
}