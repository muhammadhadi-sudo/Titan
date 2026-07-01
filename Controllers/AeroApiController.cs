using Microsoft.AspNetCore.Mvc;

namespace Titan.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class AeroApiController : ControllerBase
{
    private readonly SendMessage _sendMessage = new();

    /// <summary>
    /// Mendapatkan informasi dasar API Titan
    /// </summary>
    [HttpGet("info")]
    public IActionResult GetInfo()
    {
        return Ok(new
        {
            Nama = "Titan HID Aero Web API",
            VersiNet = ".NET 10",
            Status = "Berjalan Normal",
            Waktu = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });
    }

    /// <summary>
    /// Mengirim perintah mentah ke perangkat HID Aero
    /// </summary>
    /// <param name="command">String perintah sesuai dokumentasi HID</param>
    [HttpPost("kirim")]
    public IActionResult KirimPerintah([FromQuery] string command)
    {
        if (string.IsNullOrWhiteSpace(command))
            return BadRequest("Perintah tidak boleh kosong");

        var hasil = _sendMessage.SendRawCommand(command);
        return Ok(new { Berhasil = true, Perintah = hasil });
    }
}