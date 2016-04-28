using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Timers;
using CodeFiend.WbmDrivers.Devices.CardReaders;
using CuttingEdge.Conditions;

namespace CodeFiend.WbmDrivers.WBE
{
	/// <summary>
	/// This class handles working with the World Bridge Electronics WBM-9870 Partial Insert Serial Card Reader
	/// </summary>
	
	public class Wbm9870Driver :  IMagneticCardReader
	{
		// ReSharper disable InconsistentNaming
		public const string MANUFACTURER = "World Bridge Electronics";
		public const string MODEL = "WBM-9870";
		public const string NAME = "WBM-9870 Serial Driver v1";
		private const byte START_HEADER = 0x60;
		// ReSharper restore InconsistentNaming

		private bool _keepReading;
		private readonly SerialPort _serialPort;
		private readonly Thread _readThread;

		private volatile short _readsSinceHeader;

		private bool _isActive;
		private string _comPortName;

		#region WMB-9870 Implemention

		private MagneticCard _insertedCard;
		private readonly System.Timers.Timer _timer;

		/// <summary>
		/// Default Ctor
		/// </summary>
		public Wbm9870Driver()
		{
			_serialPort = new SerialPort();
			//_keepReading = true;
			_readThread = new Thread(ReaderThreadMain);
			_timer = new System.Timers.Timer(300);
			_timer.Elapsed += OnCheckStatusTick;
		}

		private void OnCheckStatusTick(object sender, ElapsedEventArgs e)
		{
			if (IsActive)
				RequestCardInsertionStatus();
		}

		/// <summary>
		/// This handles running the read thread
		/// </summary>
		private void ReaderThreadMain()
		{
			// this is if we are currently on a message
			bool parseingMessage = false;

			byte[] messageBytes = null;
			byte[] lengthBytes = null;
			short? messageLength = null;
			while (_keepReading)
			{
				if (_serialPort.IsOpen)
				{
					byte b = (byte)_serialPort.ReadByte();
					if (!parseingMessage && b == START_HEADER)
					{
						//	Log("0x60 start");
						parseingMessage = true;
						messageBytes = null;
						messageLength = null;
						lengthBytes = new byte[2];
						_readsSinceHeader = 0;
					}
					else if (parseingMessage && !messageLength.HasValue)
					{
						if (_readsSinceHeader == 1)
						{
							lengthBytes[1] = b;
							//	Log("length b1");
						}
						else if (_readsSinceHeader == 2)
						{
							lengthBytes[0] = b;
							messageLength = BitConverter.ToInt16(lengthBytes, 0);
							//Log(string.Format("length={0}", messageLength));
							if (messageLength == 0)
							{
								//Log("length 0 clear");
								parseingMessage = false;
							}
							else
							{
								messageBytes = new byte[messageLength.Value + 4];
								//Log(string.Format("[{0}]", messageBytes.Length));
								messageBytes[0] = START_HEADER;
								messageBytes[1] = lengthBytes[0];
								messageBytes[2] = lengthBytes[1];
							}
						}
					}
					else if (parseingMessage)
					{
						if (_readsSinceHeader == messageLength.Value + 3)
						{
							//	Log("end of message");
							messageBytes[_readsSinceHeader] = b;
							ProcessMessage(messageBytes);
							parseingMessage = false;
						}
						else
						{
							//	Log(string.Format("b#{0}={1:X2}", readsSinceHeader, b));
							messageBytes[_readsSinceHeader] = b;
						}
					}
					_readsSinceHeader++;
				}
				else
				{
					Thread.Sleep(10);
				}
			}
		}

		/// <summary>
		/// This will process a message and figure out what to do with it once the read thread has finished capturing it 
		/// </summary>
		/// <param name="messageBytes"></param>
		private void ProcessMessage(byte[] messageBytes)
		{
			Message m = new Message(messageBytes);
			if (m.Length == 1)
			{
				// reporting status or something like that
				if (EnableDebug)
					Console.WriteLine("status reported");
			}
			else if (m.Length == 4)
			{
				// this is likely the results of a request card status operation
				InsertionStatusMessage insertionStatus = new InsertionStatusMessage(messageBytes);
				if (_insertedCard != null)
				{
					// we have a card 
					if (!insertionStatus.Inserted)
					{
						// we didn't see a remove but there is no card so clear it 
						OnCardRemoved();
					}
				}
				m = insertionStatus;
			}
			else
			{
				CardReadMessage cardRead = new CardReadMessage(messageBytes);
				m = cardRead;
				if (cardRead.ErrorCode == ReadErrorCode.NormalExecution)
				{
					if (cardRead.Tracks.Count == 2)
					{
						// likely a good insert read
						_insertedCard = new MagneticCard();
						var track1 = cardRead.Tracks.FirstOrDefault(o => o.Track == MagneticTrack.Track1);
						var track2 = cardRead.Tracks.FirstOrDefault(o => o.Track == MagneticTrack.Track2);
						var track3 = cardRead.Tracks.FirstOrDefault(o => o.Track == MagneticTrack.Track3);
						if (track1 != null)
						{
							_insertedCard.Track1 = track1.Data;
							_insertedCard.Track1Status = ConvertToGenericStatus(track1.ErrorCode);
						}
						if (track2 != null)
						{
							_insertedCard.Track2 = track2.Data;
							_insertedCard.Track2Status = ConvertToGenericStatus(track2.ErrorCode);
						}
						if (track3 != null)
						{
							_insertedCard.Track3 = track3.Data;
							_insertedCard.Track3Status = ConvertToGenericStatus(track3.ErrorCode);
						}
						OnCardInserted();
					}
					else if (cardRead.Tracks.Count == 1 &&
							 cardRead.Tracks.Any(o =>
								 o.Track == MagneticTrack.Track1 &&
								 o.ErrorCode == ReadErrorCode.BlankError))
					{
						// this is a card removed send the request of status to confirm
						_insertedCard = null;
						// send out message
						OnCardRemoved();
					}
				}
			}
			if (EnableDebug)
				Console.WriteLine(m);
		}

		/// <summary>
		/// This helper method will convert from the status that this device supports to the FoxHorn Generalized 
		/// status messages 
		/// </summary>
		/// <param name="code">the device unique code</param>
		/// <returns>the generalize equivlient of the device unique code</returns>
		private MagneticCardStatus ConvertToGenericStatus(ReadErrorCode code)
		{
			switch (code)
			{
				case ReadErrorCode.NormalExecution:
				case ReadErrorCode.BlankError:
					return MagneticCardStatus.GoodRead;
					break;
				case ReadErrorCode.PreambleError:
				case ReadErrorCode.PostambleError:
				case ReadErrorCode.ParityError:
				case ReadErrorCode.LrcError:
				default:
					return MagneticCardStatus.ReadError;
			}
		}

		/// <summary>
		/// This is called to send out the card removed 
		/// </summary>
		protected virtual void OnCardRemoved()
		{
			if (EnableDebug)
				Console.WriteLine("Card Removed");
			if (CardRemoved != null)
				CardRemoved(this, new MagneticCardEventArgs(MagneticCardEvent.Remove, null));
		}

		/// <summary>
		/// This is called to send out the card removed 
		/// </summary>
		protected virtual void OnCardInserted()
		{
			if (EnableDebug)
				Console.WriteLine("Card Inserted");
			if (CardInserted != null)
				CardInserted(this, new MagneticCardEventArgs(MagneticCardEvent.Insert, _insertedCard));
		}

		/// <summary>
		/// Tells the device to read cards when they are removed
		/// </summary>
		private void ReadBackwords()
		{
			_serialPort.Write(new byte[] { 0x60, 0x00, 0x02, 0x4d, 0x32, 0x1d }, 0, 6);
		}

		/// <summary>
		/// Tells the card reader to read cards when they are inserted
		/// </summary>
		private void ReadForward()
		{
			_serialPort.Write(new byte[] { 0x60, 0x00, 0x02, 0x4d, 0x31, 0x1e }, 0, 6);
		}

		/// <summary>
		/// Tells the card reader to init its API and prepare for normal operation
		/// </summary>
		private void InitApi()
		{
			_serialPort.Write(new byte[] { 0x60, 0x00, 0x02, 0x43, 0x33, 0x12 }, 0, 6);
		}

		private void RequestCardInsertionStatus()
		{
			_serialPort.Write(new byte[] { 0x60, 0x00, 0x02, 0x43, 0x32, 0x13 }, 0, 6);
		}


		#endregion





		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			_keepReading = false;
			_readThread.Join();
			if (_serialPort.IsOpen)
				_serialPort.Close();
			_serialPort.Dispose();
			_timer.Stop();
			_timer.Dispose();
		}

		#endregion

		#region Implementation of IDevice

		/// <summary>
		/// The name of the company that makes the device 
		/// </summary>
		public string Manufacturer
		{
			get { return MANUFACTURER; }
		}

		/// <summary>
		/// The model of this device
		/// </summary>
		public string Model
		{
			get { return MODEL; }
		}

		/// <summary>
		/// The name of the serial device driver that that is implementing this interface
		/// </summary>
		public string Name
		{
			get { return NAME; }
		}

		#endregion

		#region Implementation of IDeviceDriver

		/// <summary>
		/// This is invoked to start the device normal operation 
		/// </summary>
		public void StartDevice()
		{
			Condition.Requires(_comPortName, "Serial COM Port Name").IsNotNullOrWhiteSpace();
			Condition.Requires(_serialPort.IsOpen, "Serial Port Open").IsTrue();
			_keepReading = true;

			_readThread.Start();
			Thread.Sleep(50);
			InitApi();
			_isActive = true;
			Thread.Sleep(100);
			ReadForward();
			Thread.Sleep(150);
			_timer.Start();
		}

		/// <summary>
		/// used to report if this device can auto detect the port it is connected to
		/// </summary>
		/// <remarks>
		/// Not supported yet
		/// </remarks>
		public bool SupportsConnectionDetection
		{
			get { return false; }
		}

		/// <summary>
		/// Used to return if the device is currently running 
		/// </summary>
		public bool IsActive
		{
			get { return _isActive; }
		}

		/// <summary>
		/// This is used to enable or disable dumping debug info to the console for the driver
		/// </summary>
		public bool EnableDebug { get; set; }

		public void StopDevice()
		{
			ClosePort();
			Dispose();
		}

		#endregion

		#region Implementation of ISerialDeviceDriver

		/// <summary>
		/// The name of the serial port that the device is running on 
		/// </summary>
		public string ActiveComPort
		{
			get { return _comPortName; }
		}

		/// <summary>
		/// Used to open the serial port that will handle the operation. 
		/// </summary>
		/// <remarks>
		/// Note that this does not actually start the device operating that is done by StartDevice
		/// </remarks>
		/// <param name="comPortName"></param>
		public void OpenPort(string comPortName)
		{
			_serialPort.PortName = comPortName;
			_serialPort.DataBits = 8;
			_serialPort.BaudRate = 9600;
			_serialPort.StopBits = StopBits.One;
			_serialPort.Parity = Parity.None;
			_serialPort.Open();
			_comPortName = comPortName;
		}

		/// <summary>
		/// Used to shutdown the device 
		/// </summary>
		public void ClosePort()
		{
			_keepReading = false;
			if (_serialPort.IsOpen)
				_serialPort.Close();
			_isActive = false;
			_readThread.Join();
			_timer.Stop();
		}

		#endregion

		#region Implementation of IMagneticCardReader

		/// <summary>
		/// Gets or sets IsSwipeReader
		/// </summary>
		/// <remarks>
		/// It can be useful to know that a reader is insertion verses a insertion reader as a swipe reader will only send a card inserted and we will have no idea if the user is still present after swipe
		/// </remarks>
		public bool IsSwipeReader
		{
			get { return false; }
		}

		/// <summary>
		/// Gets or sets SupportsInsertNotification
		/// </summary>
		/// <remarks>
		/// does this support notification when a card is inserted
		/// </remarks>
		public bool SupportsInsertNotification
		{
			get { return true; }
		}

		/// <summary>
		/// Gets or sets SupportsRemovalNotification
		/// </summary>
		/// <remarks>
		/// does this support notification when a card is removed
		/// </remarks>
		public bool SupportsRemovalNotification
		{
			get { return true; }
		}

		public event EventHandler<MagneticCardEventArgs> CardInserted;
		public event EventHandler<MagneticCardEventArgs> CardRemoved;
		public event EventHandler<MagneticCardEventArgs> ReadError;

		#endregion
	}
}
