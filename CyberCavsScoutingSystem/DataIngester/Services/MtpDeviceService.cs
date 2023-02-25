using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using MediaDevices;
using UtilitiesLibrary.Collections;
using Timer = System.Timers.Timer;

namespace DataIngester.Services;



public interface IMtpDeviceService {

	public ReadOnlyList<MediaDevice> GetDevices();

}




public class MtpDeviceService : IMtpDeviceService {

	private static readonly TimeSpan DevicePollingFrequency = new(0, 0, 1);

	private readonly List<MediaDevice> ConnectedDevices = new();

	public MtpDeviceService() {

		Timer timer = new(DevicePollingFrequency);
		timer.Elapsed += TimerElapsedHandler;
		timer.Start();
	}

	private void TimerElapsedHandler(object? sender, ElapsedEventArgs e) {

		IEnumerable<MediaDevice> devices = MediaDevice.GetDevices().ToArray();

		ConnectedDevices.PruneToUnionAndDispose(devices);

		foreach (MediaDevice device in devices) {

			if (ConnectedDevices.Contains(device)) {
				continue;
			}

			try {
				device.Connect();

			} catch { /**/ }

			ConnectedDevices.Add(device);
		}
	}



	public ReadOnlyList<MediaDevice> GetDevices() {

		return ConnectedDevices.ToReadOnly();
	}

}