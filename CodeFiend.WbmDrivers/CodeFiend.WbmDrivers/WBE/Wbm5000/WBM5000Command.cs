namespace CodeFiend.WbmDrivers.WBE.Wbm5000
{
	public class Wbm5000Commands
	{
		public static readonly byte[] EjectCardForward = new byte[] { 0x33, 0x30 };
		public static readonly byte[] EjectCardBackword = new byte[] { 0x33, 0x31 };
		public static readonly byte[] ReadTrack2 = new byte[] { 0x36, 0x32 };
		public static readonly byte[] ReadAllTracks = new byte[] { 0x36, 0x34 };
		public static readonly byte[] CleanUpMemory = new byte[] { 0x36, 0x36 };
		public static readonly byte[] InitializeEquipmentEjectForward = new byte[] { 0x30, 0x30 };
	}
}