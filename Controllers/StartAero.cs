using HID.Aero.ScpdNet.Wrapper;
using System.Runtime.InteropServices;

namespace Titan.Controllers;

public static class StartAero
{
    private static ScpdWrite? _scpdWrite;
    private static readonly ScpdRead _scpdRead = new();
    private static Thread? _readThread;

    public static event Action<string> OnTransaction
    {
        add => _scpdRead.OnTransactionReceived += value;
        remove => _scpdRead.OnTransactionReceived -= value;
    }

    public static event Action<int, string> OnControllerStatus
    {
        add => _scpdRead.OnStatusUpdate += value;
        remove => _scpdRead.OnStatusUpdate -= value;
    }

    public static void Communication()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            SCPDLL.IsLinux = true;
            var startupResult = SCPDLL.scpStartup("mpl_drv", 1);
            Console.WriteLine($"Inisialisasi driver Linux: {startupResult}");
        }

        _scpdWrite = new ScpdWrite();
        _scpdWrite.TurnOnDebug();
        _scpdWrite.StartIPClientConnection();

        _readThread = new Thread(_scpdRead.GetScpdMessagesUntilShutdown)
        {
            IsBackground = true
        };
        _readThread.Start();

        Console.WriteLine("Koneksi HID Aero berhasil diinisialisasi!");
    }
}