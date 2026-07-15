namespace Adapter.Amico.Helper;

public static class EventHelper
{
      public static string EventMapper(int eventId)
{
    return eventId switch
    {
        1 => "Invalid reader",
        2 => "Invalid identification rule parameters",
        3 => "Not identified",
        4 => "Pending identification",
        5 => "Identification time expired",
        6 => "Access denied",
        7 => "Access granted",
        8 => "Pending access",
        9 => "User is not administrator",
        10 => "Non-identified access",
        11 => "Access via pushbutton switch",
        12 => "Access through web interface",
        13 => "No response",
        _ => "Unknown"
    };
}
}