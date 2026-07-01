using HID.Aero.ScpdNet.Wrapper;
using System.Text.Json;

namespace Titan.Controllers;

public class ScpdRead
{
    private bool shutdownFlag;
    private readonly WebConfig webConfig = new();

    public event Action<string>? OnTransactionReceived;
    public event Action<int, string>? OnStatusUpdate;
    public event Action<int, int, string>? OnSIOStatusUpdate;
    public event Action<int, string, int, int>? OnAccessGrantedEvent;
    public event Action<int, int, string>? OnReaderStatusUpdate;
    public event Action<int, int, bool>? OnDoorStatusUpdate;

    public ScpdRead()
    {
        shutdownFlag = false;
    }

    public void SetShutdownFlag()
    {
        SCPDLL.scpDebugSet((int)enSCPDebugLevel.enSCPDebugToFile);
        Thread.Sleep(100);
        shutdownFlag = true;
    }

    public void GetScpdMessagesUntilShutdown()
    {
        while (!shutdownFlag)
        {
            GetScpdMessage();
        }
    }

    private void GetScpdMessage()
    {
        SCPReplyMessage message = new();
        if (message.GetMessage())
        {
            ProcessMessage(message);
        }
    }

    private void ProcessMessage(SCPReplyMessage message)
    {
        switch (message.ReplyType)
        {
            case (int)enSCPReplyType.enSCPReplyCommStatus:
                string status = message.comm.status == (int)enSCPComm.enSCPCommOk ? "Online" : "Offline";
                OnStatusUpdate?.Invoke(message.SCPId, status);
                break;

            case (int)enSCPReplyType.enSCPReplyTransaction:
                ProcessTransactionEvent(message);
                break;

            case (int)enSCPReplyType.enSCPReplyWebConfigHostCommPrim:
                webConfig.ProcessWebConfigMessage(message);
                break;
        }
    }

    private void ProcessTransactionEvent(SCPReplyMessage message)
    {
        // Proses status SIO
        if (message.tran.tran_type == 2)
        {
            string? sioStatus = message.tran.tran_code switch
            {
                2 => "Offline",
                5 => "Online",
                _ => null
            };
            if (sioStatus != null) OnSIOStatusUpdate?.Invoke(message.SCPId, message.tran.source_number, sioStatus);
        }

        // Proses akses diizinkan
        if (message.tran.tran_type == 6 && message.tran.tran_code == 13)
        {
            long cardHolderId = message.tran.c_id.cardholder_id < 0
                ? message.tran.c_id.cardholder_id + 4294967296L
                : message.tran.c_id.cardholder_id;
            OnAccessGrantedEvent?.Invoke(message.SCPId, cardHolderId.ToString(), message.tran.source_type, message.tran.source_number);
        }

        // Proses status pembaca
        if (message.tran.tran_type == 7 && message.tran.source_type == 10)
        {
            string? readerStatus = message.tran.tran_code switch
            {
                5 => "Offline",
                3 => "Online",
                _ => null
            };
            if (readerStatus != null) OnReaderStatusUpdate?.Invoke(message.SCPId, message.tran.source_number, readerStatus);
        }

        // Proses status pintu
        if (message.tran.tran_type == 9 && (message.tran.tran_code == 3 || message.tran.tran_code == 4))
        {
            bool isLocked = message.tran.door.door_status != 1;
            OnDoorStatusUpdate?.Invoke(message.SCPId, message.tran.source_number, isLocked);
        }

        // Proses data transaksi umum
        try
        {
            string cardData = "0";
            if (message.tran.c_bin.bit_count != 0)
            {
                var hex = Utilities.ConvertHexStringToByteArray(message.tran.c_bin.bit_array ?? Array.Empty<byte>());
                cardData = hex[..Math.Min(32, hex.Length)];
            }

            long chId = message.tran.c_id.cardholder_id < 0
                ? message.tran.c_id.cardholder_id + 4294967296L
                : message.tran.c_id.cardholder_id;

            var payload = new
            {
                ScpId = message.SCPId,
                SerNum = message.tran.ser_num,
                Time = message.tran.time,
                SourceType = message.tran.source_type,
                SourceNumber = message.tran.source_number,
                TranType = message.tran.tran_type,
                TranCode = message.tran.tran_code,
                CardNumber = chId.ToString(),
                Extra = $"BitCount:{message.tran.c_bin.bit_count}|CardData:{cardData}"
            };

            OnTransactionReceived?.Invoke(JsonSerializer.Serialize(payload));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing transaksi: {ex.Message}");
        }
    }
}