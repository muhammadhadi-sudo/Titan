namespace Titan.Controllers;

public class SendMessage
{
    public string SendRawCommand(string command)
    {
        if (!string.IsNullOrWhiteSpace(command))
        {
            HID.Aero.ScpdNet.Wrapper.SCPDLL.scpConfigCommand(command);
            Console.WriteLine($"Kirim perintah: {command}");
        }
        return command ?? string.Empty;
    }
}