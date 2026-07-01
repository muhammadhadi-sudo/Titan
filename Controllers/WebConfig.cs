using HID.Aero.ScpdNet.Wrapper;

namespace Titan.Controllers;

public class WebConfig
{
    public void ProcessWebConfigMessage(SCPReplyMessage message)
    {
        switch (message.ReplyType)
        {
            case (int)enSCPReplyType.enSCPReplyWebConfigHostCommPrim:
                ProcessWebConfigHostCommPrim(message);
                break;
        }
    }

    private void ProcessWebConfigHostCommPrim(SCPReplyMessage message)
    {
        SCPDLL.scpDebug((int)enSCPDebugLevel.enSCPDebugToFile, "\n");
        SCPDLL.scpDebug((int)enSCPDebugLevel.enSCPDebugToFile, "********** Web Config Host Comm Prim **********\n");
        SCPDLL.scpDebug((int)enSCPDebugLevel.enSCPDebugToFile, $"SCP Number: {message.web_host_comm_prim.scp_number}\n");
        SCPDLL.scpDebug((int)enSCPDebugLevel.enSCPDebugToFile, $"Address: {message.web_host_comm_prim.address}\n");
        SCPDLL.scpDebug((int)enSCPDebugLevel.enSCPDebugToFile, $"Type: {message.web_host_comm_prim.cType}\n");

        if (message.web_host_comm_prim.cType == 1)
        {
            SCPDLL.scpDebug((int)enSCPDebugLevel.enSCPDebugToFile, $"IP Server Port: {message.web_host_comm_prim.ipserver.nPort}\n");
        }
        else if (message.web_host_comm_prim.cType == 2)
        {
            SCPDLL.scpDebug((int)enSCPDebugLevel.enSCPDebugToFile, $"IP Client Host: {message.web_host_comm_prim.ipclient.cHostName}\n");
            SCPDLL.scpDebug((int)enSCPDebugLevel.enSCPDebugToFile, $"IP Client Port: {message.web_host_comm_prim.ipclient.nPort}\n");
        }
    }
}