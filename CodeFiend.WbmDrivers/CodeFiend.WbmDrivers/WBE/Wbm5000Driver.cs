using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using CodeFiend.WbmDrivers.Devices.CardReaders;
using CodeFiend.WbmDrivers.external;

namespace CodeFiend.WbmDrivers.WBE
{
	public class Wbm5000Driver : IMagneticCardReader
	{
		private string _serialPort;
		public const string MANUFACTURER = "World Bridge Electronics";
		public const string MODEL = "WBM-5000";
		public const string NAME = "WBM-5000 Serial Driver v1";
		private Thread _readerThread;
		private bool _keepAlive;
		private byte[] sensorBytes = new byte[8];
		private class MoveType
		{
			public const byte Front = 0x30;
			public const byte Back = 0x33;
		}
		private class CardInType
		{
			public const byte Locked = 0x31;
			public const byte OnlyMagnetic = 0x32;
		}
		private IntPtr handle;
		private bool _hasCard;
		private MagneticCard _currentCard;
		private void RunReadThread()
		{
			byte[] cardData = new byte[100];
			byte length;
			byte error;
			while (_keepAlive)
			{
				WBM5000.WBM5000_SensorState(handle, sensorBytes, out error);

				if (sensorBytes.Count(o => o == 0x31) >= 1 && !_hasCard)
				{
					_hasCard = true;
					WBM5000.WBM5000_MagCardReadData(handle, 0x30, 0x32, out length, cardData);
					string cardString = Encoding.ASCII.GetString(cardData, 0, length);
					cardString = cardString.Replace("O", "").Replace("?Y", "");
					cardString = Regex.Replace(cardString, @"[^\u0020-\u007E]", string.Empty);
					//	Console.WriteLine();
					if (CardInserted != null)
					{
						_currentCard = new MagneticCard
										{
											Track2 = cardString
										};
						CardInserted(this, new MagneticCardEventArgs(MagneticCardEvent.Insert, _currentCard));
					}
					//	WBM5000.WBM5000_SensorState(handle, sensorBytes, out error);
					PerformForwardEject();
				}
				// we have no sensors active and we believe that we had a card send our card removed
				else if (sensorBytes.Count(o => o == 0x31) == 0 && _hasCard)
				{
					if (_hasCard)
					{
						if (CardRemoved != null)
							CardRemoved(this, new MagneticCardEventArgs(MagneticCardEvent.Remove, _currentCard));
						_currentCard = null;
						_hasCard = false;
					}
				}
				Thread.Sleep(500);
			}
		}

		private void PerformForwardEject()
		{
			byte error;

			int status;
			status = WBM5000.WBM5000_CardMove(handle, MoveType.Front, out error);
			while (status != 0 && status != -1)
			{
				status = WBM5000.WBM5000_CardMove(handle, MoveType.Front, out error);
			}
			WBM5000.WBM5000_SensorState(handle, sensorBytes, out error);

		}

		#region Implementation of IDisposable

		public void Dispose()
		{

		}

		#endregion

		#region Implementation of IDevice

		public string Manufacturer
		{
			get { return MANUFACTURER; }
		}

		public string Model
		{
			get { return MODEL; }
		}

		public string Name
		{
			get { return NAME; }
		}

		#endregion

		#region Implementation of IDeviceDriver

		public void StartDevice()
		{
			_keepAlive = true;
			_readerThread = new Thread(RunReadThread);
			_readerThread.Start();

		}

		public bool SupportsConnectionDetection
		{
			get { return false; }
		}

		public bool IsActive
		{
			get { return _keepAlive; }
		}

		public bool EnableDebug { get; set; }
		public void StopDevice()
		{
			ClosePort();
			Dispose();
		}

		#endregion

		#region Implementation of ISerialDeviceDriver

		public string ActiveComPort
		{
			get { return _serialPort; }
		}

		public void OpenPort(string comPortName)
		{
			_serialPort = comPortName;
			handle = WBM5000.CommOpen(comPortName);
			byte error;
			WBM5000.WBM5000_CardMove(handle, MoveType.Front, out error);
			WBM5000.WBM5000_CardIN(handle, CardInType.OnlyMagnetic, 0x30, out error);

		}

		public void ClosePort()
		{
			_keepAlive = false;
			_readerThread.Join();
			WBM5000.CommClose(handle);
		}

		#endregion

		#region Implementation of IMagneticCardReader

		public bool IsSwipeReader
		{
			get { return false; }
		}

		public bool SupportsInsertNotification
		{
			get { return true; }
		}

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
