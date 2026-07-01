using HID.Aero.ScpdNet.Wrapper;

namespace Titan.Controllers;

public class ScpdWrite
{
    public bool TurnOnDebug()
    {
        return SCPDLL.scpDebugSet((int)enSCPDebugLevel.enSCPDebugToFile);
    }

    public bool WriteCommand(string command)
    {
        return SCPDLL.scpConfigCommand(command);
    }

    public bool WriteCommand(short command, IConfigCommand cfg)
    {
        SCPConfig scp = new();
        return scp.scpCfgCmndEx(command, cfg);
    }

    public void StartIPClientConnection()
    {
        SCPDLL.scpConfigCommand("11 512 512 0 0 1 0 0 0 0 0");
        SCPDLL.scpConfigCommand("012 1 7 3001 0 3000 15000 \"\" 0");
    }
}