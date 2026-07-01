using HID.Aero.ScpdNet.Wrapper;

namespace Titan.Controllers;

public class Transaction
{
    private readonly LogMessages log = new();

    public void ProcessTransactionLogEvent(SCPReplyMessage message)
    {
        switch (message.tran.tran_type)
        {
            case (short)tranType.tranTypeCardBin:
                ProcessTransactionCardBin(message);
                break;
        }
    }

    private void ProcessTransactionCardBin(SCPReplyMessage message)
    {
        log.AppendLine($"Seri: {message.tran.ser_num} | Tipe: {message.tran.tran_type}:{message.tran.tran_code}");
        log.AppendLine($"Bit Count: {message.tran.c_bin.bit_count} | Data: {Utilities.ConvertHexStringToByteArray(message.tran.c_bin.bit_array)}");
    }
}