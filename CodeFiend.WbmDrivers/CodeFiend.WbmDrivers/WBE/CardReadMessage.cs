using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFiend.WbmDrivers.WBE
{
	/// <summary>
	/// This class handles all card read messages 
	/// </summary>
	public class CardReadMessage : Message
	{
		// ReSharper disable InconsistentNaming
		public const byte TRACK_1 = 0x25;
		public const byte TRACK_2 = 0x3f;
		public const byte TRACK_3 = 0x26;
		// ReSharper restore InconsistentNaming

		private readonly List<TrackInfo> _tracks = new List<TrackInfo>(3);

		public List<TrackInfo> Tracks
		{
			get { return _tracks; }
		}

		/// <summary>
		/// This class provides structure for each Magnetic track
		/// </summary>
		public class TrackInfo
		{
			private readonly byte[] _data;

			/// <summary>
			/// The state of the track read
			/// </summary>
			public ReadErrorCode ErrorCode
			{
				get { return (ReadErrorCode)_data[0]; }
			}

			/// <summary>
			/// The track that this instace resperents
			/// </summary>
			public MagneticTrack Track
			{
				get { return (MagneticTrack)_data[1]; }
			}

			/// <summary>
			/// The ASCII data off the track
			/// </summary>
			public string Data
			{
				get { return Encoding.ASCII.GetString(_data, 2, _data.Length - 2); }
			}

			/// <summary>
			/// Ctor to create an instance of a track
			/// </summary>
			/// <param name="bytes">raw bytes from the reader for the track</param>
			public TrackInfo(byte[] bytes)
			{
				_data = bytes;
			}

			/// <summary>
			/// Used to make the data that this class contains human readable / intelligable
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return string.Format("{0} - {1} = {2}", Track, ErrorCode, Data);
			}
		}

		/// <summary>
		/// This is the over all status for the read. Note that each track has a status code as well.
		/// </summary>
		public ReadErrorCode ErrorCode { get { return (ReadErrorCode)Data[3]; } }


		/// <summary>
		/// Default Ctor that also fully populates all the track data. 
		/// </summary>
		/// <param name="data"></param>
		public CardReadMessage(byte[] data)
			: base(data)
		{
			if (Length > 1)
			{
				int offset = 4;
				int cycleOffset = 0;
				while (offset < Length)
				{
					int endIndex = Data.Skip(offset).ToList().IndexOf(0x00);

					_tracks.Add(new TrackInfo(Data.Skip(offset).Take(endIndex).ToArray()));
					offset += endIndex + 1;
				}
			}
		}

		/// <summary>
		/// Used to return easy to understand data about the results of the read with all data spelled out
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(string.Format("[{0}] - {1}\r\n", Length, ErrorCode));
			foreach (var track in _tracks)
			{
				sb.Append(string.Format("  {0}\r\n", track));
			}
			return sb.ToString();
		}
	}
}