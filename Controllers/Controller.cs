using HID.Aero.ScpdNet.Wrapper;

namespace Titan.Controllers;

public class HidController
{
    private readonly ScpdWrite _scpdWrite;

    public HidController(ScpdWrite scpdWrite)
    {
        _scpdWrite = scpdWrite;
    }

    public bool SendWebConfigRead(short scpNumber, short readType)
    {
        var webConfig = new CC_WEB_CONFIG_READ
        {
            scp_number = scpNumber,
            read_type = readType
        };
        return _scpdWrite.WriteCommand(900, webConfig);
    }

    public bool SendReset(short scpNumber)
    {
        var reset = new CC_RESET { scp_number = scpNumber };
        return _scpdWrite.WriteCommand((short)enCfgCmnd.enCcReset, reset);
    }

    public bool SendFirmware(short scpNumber, string filePath)
    {
        var firmware = new CC_FIRMWARE { scp_number = scpNumber };
        filePath.CopyTo(0, firmware.file_name, 0, Math.Min(filePath.Length, 199));
        return _scpdWrite.WriteCommand((short)enCfgCmnd.enCcFirmwareDown, firmware);
    }
}