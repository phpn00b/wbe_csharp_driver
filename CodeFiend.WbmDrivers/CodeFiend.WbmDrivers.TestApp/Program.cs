using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFiend.WbmDrivers.Devices.CardReaders;
using CodeFiend.WbmDrivers.external;
using CodeFiend.WbmDrivers.WBE;

namespace CodeFiend.WbmDrivers.TestApp
{
	class Program
	{

		static void Main(string[] args)
		{
			AppDomain.CurrentDomain.ProcessExit += Shutdown;
			PickComPortToUse();
			bool keepGoing = true;
			while (keepGoing)
			{
				string value = Console.ReadLine();
				if (!string.IsNullOrWhiteSpace(value))
					if (value.ToLower() == "q")
						keepGoing = false;
			}

			Console.WriteLine("exit");
			if (driver != null)
				driver.StopDevice();
		}

		private static IMagneticCardReader driver;
		private static string devicePort;
		private static void Shutdown(object sender, EventArgs e)
		{
			if (driver != null)
				driver.StopDevice();
		}

		private static void OnCardRemoved(object sender, MagneticCardEventArgs e)
		{
			// for the WBM 5000 remove is not called till you actually remove the card from the device. when it is inserted it will pull in then pop back out. (in the driver there is the posibility to change how it operates to not return the card right away or eject it from the back ie take it from the person that entered it and drop it inside the kiosk/atm)
			Console.WriteLine("{0} {1}", e.MagneticCardEvent, e.MagneticCard);
		}

		private static void OnCardInserted(object sender, MagneticCardEventArgs e)
		{
			Console.WriteLine("{0} {1}", e.MagneticCardEvent, e.MagneticCard);
		}

		private static void PickComPortToUse()
		{
			string[] ports = SerialPort.GetPortNames();
			Console.WriteLine("We have detected the following com ports please pick the one you want to use:");
			foreach (var port in ports)
			{
				Console.WriteLine("  " + port);
			}
			string requestedPort = Console.ReadLine();
			bool isValid = false;
			if (!string.IsNullOrWhiteSpace(requestedPort))
			{
				foreach (var port in ports)
				{
					if (requestedPort.ToLower() == port.ToLower())
					{
						isValid = true;
					}
				}
			}
			if (!isValid)
			{
				Console.WriteLine("{0} is an unknown com port please enter a valid com port", requestedPort);
				PickComPortToUse();
				return;
			}
			devicePort = requestedPort.ToUpper();
			PickDeviceToActivate();

		}

		private static void PickDeviceToActivate()
		{
			PrintHelp();
			string activateOption = Console.ReadLine();
			string driverName = "";
			switch (activateOption)
			{
				case "1":
					driverName = Wbm9870Driver.NAME;
					Activate98xx();
					break;
				case "2":
					driverName = Wbm5000Driver.NAME;
					Activate5000();
					break;
				case "3":
					driverName = Wbm1370Driver.NAME;
					Activate1300();
					break;
				default:
					Console.WriteLine("{0} is not a valid option for a driver to activate please enter a number 1-3", activateOption);
					PickDeviceToActivate();
					return;
			}
			Console.WriteLine("Activated driver {0} press 'q' and hit enter to exit", driverName);

		}

		private static void Activate98xx()
		{
			driver = new Wbm9870Driver();
			driver.OpenPort(devicePort);
			driver.CardInserted += OnCardInserted;
			driver.CardRemoved += OnCardRemoved;
			driver.StartDevice();
		}

		private static void Activate5000()
		{
			driver = new Wbm5000Driver();
			driver.OpenPort(devicePort);
			driver.CardInserted += OnCardInserted;
			driver.CardRemoved += OnCardRemoved;
			driver.StartDevice();
		}

		private static void Activate1300()
		{
			driver = new Wbm1370Driver();
			driver.OpenPort(devicePort);
			driver.CardInserted += OnCardInserted;
			driver.CardRemoved += OnCardRemoved;
			driver.StartDevice();
		}

		private static void PrintHelp()
		{
			Console.WriteLine("Please Select a Driver to activate:");
			Console.WriteLine(" 1. WBM-98xx Hybrid");
			Console.WriteLine(" 2. WBM-5000 Moterized");
			Console.WriteLine(" 3. WBM-1300 Partial Insertion");
		}
	}
}
