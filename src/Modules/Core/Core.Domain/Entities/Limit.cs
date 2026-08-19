namespace Core.Domain.Entities;

public sealed class SentrixLimit
{

  public int Door { get; set; }
  public int Device { get; set; }
  public int User { get; set; }
  public int Operator { get; set; }
  public int Input { get; set; }
  public int Output { get; set; }
  public int InputGroup { get; set; }
  public int Group { get; set; }
  public int Location { get; set; }
  public int Timezone { get; set; }
  public int Holiday { get; set; }
  public bool Trigger { get; set; }
  public bool Visitor { get; set; }
  public bool TimeAttendance { get; set; }
  public bool Guard { get; set; }

  public SentrixLimit(int door, int device, int user, int @operator, int input, int output, int inputGroup, int group, int location, int timezone, int holiday, bool trigger, bool visitor, bool timeAttendance, bool guard)
  {
    Door = door;
    Device = device;
    User = user;
    Operator = @operator;
    Input = input;
    Output = output;
    InputGroup = inputGroup;
    Group = group;
    Location = location;
    Timezone = timezone;
    Holiday = holiday;
    Trigger = trigger;
    Visitor = visitor;
    TimeAttendance = timeAttendance;
    Guard = guard;
  }




}