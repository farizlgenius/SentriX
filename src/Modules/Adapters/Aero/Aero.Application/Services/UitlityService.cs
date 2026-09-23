using Adapter.Contract.Interfaces;
using Aero.Application.Helpers;

namespace Aero.Application.Services;

public sealed class UtilityService(CommandDecoder decoder) : IUtilityAdapter
{
      public string DecodeCommand(string ascii)
      {
            // Optional: pick the command dialect (defaults to Aero)
        decoder.CurrentProductFamily = CommandDecoder.ProductFamily.Aero;
        // or .Mercury / .Honeywell

        var lines = decoder.Decode(ascii);

        // Join the decoded lines into a single string to return to the caller.
        // (Color info is dropped here since it's console-only; expose it separately
        // via decoder.Lines if a caller needs per-line color.)
        return string.Join(Environment.NewLine, lines.Select(l => l.Text));
      }

      public IReadOnlyList<object> DecodeCommandWithColor(string ascii)
    {
        decoder.CurrentProductFamily = CommandDecoder.ProductFamily.Aero;
        return decoder.Decode(ascii);
    }
}

