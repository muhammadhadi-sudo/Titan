using HID.Aero.ScpdNet.Wrapper;

namespace Titan.Controllers;

public class HidReader
{
    private readonly ScpdWrite _scpdWrite;

    public HidReader(ScpdWrite scpdWrite)
    {
        _scpdWrite = scpdWrite;
    }

    public bool SendOsdpPassthrough(short scpNumber, short acrNumber, short dataLength, string hexData)
    {
        if (string.IsNullOrWhiteSpace(hexData)) return false;

        var osdp = new CC_ACR_OSDP_PASSTHROUGH
        {
            scp_number = scpNumber,
            acr_number = acrNumber,
            sequence_num = 1,
            reader_role = 0,
            msg_type = 0,
            data_len = dataLength
        };

        Utilities.ConvertHexStringToByteArray(hexData, osdp.data, 1024);
        return _scpdWrite.WriteCommand((short)enCfgCmnd.enCcAcrOsdpPassthrough, osdp);
    }
}