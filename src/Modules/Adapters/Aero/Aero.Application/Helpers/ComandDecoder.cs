// Converted from the decompiled WinForms class WindowsFormsApplication1.MainForm
// (originally from HIDAeroCommandDecoder.exe) into a plain, UI-independent C# class.
// All WinForms-specific plumbing (Form base class, controls, InitializeComponent,
// system-menu P/Invoke, WndProc, MessageBox popups) has been removed or replaced
// with plain members: Decode(string), CurrentProductFamily, Lines, Clear().

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

#nullable disable
namespace Aero.Application.Helpers;

/// <summary>
/// A minimal, dependency-free stand-in for System.Drawing.Color, so this file has
/// no dependency on System.Drawing.Common (which is Windows-only / deprecated on
/// modern, cross-platform .NET). Only the handful of named colors actually used
/// by the original WinForms RichTextBox output are provided.
/// </summary>
public readonly struct LineColor : IEquatable<LineColor>
{
  public string Name { get; }

  private LineColor(string name)
  {
    this.Name = name;
  }

  public static readonly LineColor Black = new LineColor(nameof (Black));
  public static readonly LineColor White = new LineColor(nameof (White));
  public static readonly LineColor Red = new LineColor(nameof (Red));
  public static readonly LineColor Green = new LineColor(nameof (Green));

  public bool Equals(LineColor other) => this.Name == other.Name;
  public override bool Equals(object obj) => obj is LineColor other && this.Equals(other);
  public override int GetHashCode() => this.Name?.GetHashCode() ?? 0;
  public override string ToString() => this.Name;
}

/// <summary>
/// A single decoded output line together with the color it would have been
/// rendered in by the original WinForms RichTextBox (used to flag errors, e.g.
/// <see cref="LineColor.Red"/>, and wildcards, e.g. <see cref="LineColor.Green"/>).
/// </summary>
public class DecodedLine
{
  public string Text { get; }
  public LineColor Color { get; }

  public DecodedLine(string text, LineColor color)
  {
    this.Text = text;
    this.Color = color;
  }

  public override string ToString() => this.Text;
}

public sealed class CommandDecoder
{
  public const int maximumNumOfSIOs = 96 /*0x60*/;
  public const int maximumNumOfACRs = 128 /*0x80*/;
  public const int maximumNumOfACRsPrec = 64 /*0x40*/;
  public const int maximumNumOfCPs = 2048 /*0x0800*/;
  public const int maximumNumOfMPs = 2048 /*0x0800*/;
  public const int maximumNumOfAccessLevels = 32000;
  public const int maximumNumOfTrgrs = 8196;
  public const int maximumNumOfProcs = 8196;
  public const int maximumNumOfTZs = 255 /*0xFF*/;
  public const int maximumNumOfHolidays = 255 /*0xFF*/;
  public const int maximumNumOfMPGs = 128 /*0x80*/;
  public const int maximumNumOfElAlvl = 256 /*0x0100*/;
  public const int maximumNumOfFloors = 128 /*0x80*/;
  public const int maximumNumOfTVs = 128 /*0x80*/;
  public const int maxScpID = 16383 /*0x3FFF*/;
  public const int maxSioNumberReaders = 16 /*0x10*/;
  public const int maximumNumIPSGroups = 256 /*0x0100*/;
  public const int maximumBioTemplateSize = 1024 /*0x0400*/;
  public const int maximumApbAreas = 128 /*0x80*/;
  public const int maximumNumAssetGroups = 65535 /*0xFFFF*/;
  public const int maximumOperatingModes = 8;
  private const string wildCardConstant = "$";
  private string currentLine = "";
  private const int maxNumOfQueues = 15;
  public static readonly List<string> ControllerModelList = new List<string>((IEnumerable<string>) new List<string>()
  {
    "scp2",
    "pw3k",
    "pro2200",
    "scpc",
    "pw3Kaes",
    "pro2200aes",
    "scpe",
    "scp2aes",
    "scpcaes",
    "pw5k",
    "scpeaes",
    "pw5kaes",
    "ep2500",
    "lp2500",
    "pw6k",
    "ep1502",
    "lp1502",
    "ep1501",
    "lp1501",
    "mpl_icd",
    "pim4001501",
    "pro32ic",
    "m5-ic",
    "kerinxt2",
    "kerinxt4",
    "mirs4",
    "mixl16",
    "ep4502",
    "lp4502",
    "kerinxt1",
    "rkarm",
    "msics",
    "aliro",
    "ssc",
    "acx4",
    "acx8",
    "x1100"
  });
  private LineColor foregroundColor;
  private LineColor backgroundColor;
  private LineColor textColor;
  private LineColor errorFoundTextColor = LineColor.Red;
  private LineColor wildCardColor = LineColor.Green;
  private List<Dictionary<int, Action<string[], int, bool>>> commandMapList = new List<Dictionary<int, Action<string[], int, bool>>>(Enum.GetNames(typeof (CommandDecoder.ProductFamily)).Length);
  private Dictionary<int, Action<string[], int, bool>> activeCommandMap;
  private Dictionary<System.Type, Dictionary<int, Dictionary<string, string>>> structureTranslationDict = new Dictionary<System.Type, Dictionary<int, Dictionary<string, string>>>();
  private static string wrapperQualifiedName = "HID.Aero.ScpdNet.Wrapper";
  private string SCPReplyMessageQualifiedName = CommandDecoder.wrapperQualifiedName + ".SCPReplyMessage+";
  private int productFamily = 2;
  private bool processCommandOnly = true;
  private int num_records;


  private bool loadCommandMap()
  {
    this.commandMapList.Insert(0, new Dictionary<int, Action<string[], int, bool>>()
    {
      {
        1,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcBatch(words, numWords, honeywellCommands))
      },
      {
        11,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSystem(words, numWords, honeywellCommands))
      },
      {
        12,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCreateChannel(words, numWords, honeywellCommands))
      },
      {
        13,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCreateScp(words, numWords, honeywellCommands))
      },
      {
        14,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDeleteChannel(words, numWords, honeywellCommands))
      },
      {
        15,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDeleteScp(words, numWords, honeywellCommands))
      },
      {
        16 /*0x10*/,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDaylight(words, numWords, honeywellCommands))
      },
      {
        17,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcPeerCertificate(words, numWords, honeywellCommands))
      },
      {
        18,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpClientDefaults(words, numWords, honeywellCommands))
      },
      {
        19,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpClientAssignment(words, numWords, honeywellCommands))
      },
      {
        101,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIcvt(words, numWords, honeywellCommands))
      },
      {
        102,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCfmt(words, numWords, honeywellCommands))
      },
      {
        103,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTimezone(words, numWords, honeywellCommands))
      },
      {
        104,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcHoliday(words, numWords, honeywellCommands))
      },
      {
        105,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAdbSpec(words, numWords, honeywellCommands))
      },
      {
        106,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAdbCardFile(words, numWords, honeywellCommands))
      },
      {
        107,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScp(words, numWords, honeywellCommands))
      },
      {
        108,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMsp1(words, numWords, honeywellCommands))
      },
      {
        109,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSio(words, numWords, honeywellCommands))
      },
      {
        110,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcInput(words, numWords, honeywellCommands))
      },
      {
        111,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcOutput(words, numWords, honeywellCommands))
      },
      {
        112 /*0x70*/,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcReader(words, numWords, honeywellCommands))
      },
      {
        113,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMP(words, numWords, honeywellCommands))
      },
      {
        114,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCP(words, numWords, honeywellCommands))
      },
      {
        115,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcACR(words, numWords, honeywellCommands))
      },
      {
        116,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAlvl(words, numWords, honeywellCommands))
      },
      {
        117,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTrgr(words, numWords, honeywellCommands))
      },
      {
        118,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcProc(words, numWords, honeywellCommands))
      },
      {
        119,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcActnRem(words, numWords, honeywellCommands))
      },
      {
        120,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMpg(words, numWords, honeywellCommands))
      },
      {
        121,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcArea(words, numWords, honeywellCommands))
      },
      {
        122,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRledSpc(words, numWords, honeywellCommands))
      },
      {
        123,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRTxtSpc(words, numWords, honeywellCommands))
      },
      {
        124,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAlvlSpc(words, numWords, honeywellCommands))
      },
      {
        125,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcPointName(words, numWords, honeywellCommands))
      },
      {
        (int) sbyte.MaxValue,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioAesControl(words, numWords, honeywellCommands))
      },
      {
        128 /*0x80*/,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioNetwork(words, numWords, honeywellCommands))
      },
      {
        129,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcLanguageCodePages(words, numWords, honeywellCommands))
      },
      {
        130,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrExtendedLedDefault(words, numWords, honeywellCommands))
      },
      {
        151,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrLogDeny(words, numWords, honeywellCommands))
      },
      {
        203,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnScpDown(words, numWords, honeywellCommands))
      },
      {
        206,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcFirmwareDown(words, numWords, honeywellCommands))
      },
      {
        207,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAttachScp(words, numWords, honeywellCommands))
      },
      {
        208 /*0xD0*/,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDetachScp(words, numWords, honeywellCommands))
      },
      {
        209,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcConfigSave(words, numWords, honeywellCommands))
      },
      {
        210,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcConfigDelta(words, numWords, honeywellCommands))
      },
      {
        211,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDualPortControl(words, numWords, honeywellCommands))
      },
      {
        212,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAesControl(words, numWords, honeywellCommands))
      },
      {
        213,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAesTest(words, numWords, honeywellCommands))
      },
      {
        214,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcPollMode(words, numWords, honeywellCommands))
      },
      {
        215,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcFileDownload(words, numWords, honeywellCommands))
      },
      {
        217,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcHexOutInternal(words, numWords, honeywellCommands))
      },
      {
        218,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDeleteFile(words, numWords, honeywellCommands))
      },
      {
        219,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcGetFileInfo(words, numWords, honeywellCommands))
      },
      {
        301,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseSimpleCommand(words, numWords, honeywellCommands))
      },
      {
        302,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTime(words, numWords, honeywellCommands))
      },
      {
        303,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTranIndex(words, numWords, honeywellCommands))
      },
      {
        304,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAdbCard304(words, numWords, honeywellCommands))
      },
      {
        305,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCardDelete(words, numWords, honeywellCommands))
      },
      {
        306,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMpMask(words, numWords, honeywellCommands))
      },
      {
        307,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCpCtl(words, numWords, honeywellCommands))
      },
      {
        308,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrMode(words, numWords, honeywellCommands))
      },
      {
        309,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcForcedOpenMask(words, numWords, honeywellCommands))
      },
      {
        310,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcHeldOpenMask(words, numWords, honeywellCommands))
      },
      {
        311,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcUnlock(words, numWords, honeywellCommands))
      },
      {
        312,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcProcedure(words, numWords, honeywellCommands))
      },
      {
        313,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTVCommand(words, numWords, honeywellCommands))
      },
      {
        314,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTzCommand(words, numWords, honeywellCommands))
      },
      {
        315,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrLedMode(words, numWords, honeywellCommands))
      },
      {
        316,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcOemCode(words, numWords, honeywellCommands))
      },
      {
        317,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcPassword(words, numWords, honeywellCommands))
      },
      {
        318,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpID(words, numWords, honeywellCommands))
      },
      {
        319,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcApbFreePass(words, numWords, honeywellCommands))
      },
      {
        320,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcHexOut(words, numWords, honeywellCommands))
      },
      {
        321,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMpgSet(words, numWords, honeywellCommands))
      },
      {
        322,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAreaSet(words, numWords, honeywellCommands))
      },
      {
        323,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcUseLimit(words, numWords, honeywellCommands))
      },
      {
        324,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpOfflineTime(words, numWords, honeywellCommands))
      },
      {
        325,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRLedTemp(words, numWords, honeywellCommands))
      },
      {
        326,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcLcdText(words, numWords, honeywellCommands))
      },
      {
        327,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioDc(words, numWords, honeywellCommands))
      },
      {
        328,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioHexLoad(words, numWords, honeywellCommands))
      },
      {
        329,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcHostResponse(words, numWords, honeywellCommands))
      },
      {
        330,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcNvArgSet(words, numWords, honeywellCommands))
      },
      {
        331,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCardSim(words, numWords, honeywellCommands))
      },
      {
        332,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpsSet(words, numWords, honeywellCommands))
      },
      {
        333,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDiag(words, numWords, honeywellCommands))
      },
      {
        334,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTempAcrMode(words, numWords, honeywellCommands))
      },
      {
        335,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcOperatingMode(words, numWords, honeywellCommands))
      },
      {
        336,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioRdrHexLoad(words, numWords, honeywellCommands))
      },
      {
        337,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrOsdpPassthrough(words, numWords, honeywellCommands))
      },
      {
        338,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrOfflineAccessList(words, numWords, honeywellCommands))
      },
      {
        339,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcKeySim(words, numWords, honeywellCommands))
      },
      {
        340,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcOsdpReaderTransfer(words, numWords, honeywellCommands))
      },
      {
        341,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcControlReboot(words, numWords, honeywellCommands))
      },
      {
        401,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseSimpleCommand(words, numWords, honeywellCommands))
      },
      {
        402,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseSimpleCommand(words, numWords, honeywellCommands))
      },
      {
        403,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMsp1Srq(words, numWords, honeywellCommands))
      },
      {
        404,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioSrq(words, numWords, honeywellCommands))
      },
      {
        405,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMpSrq(words, numWords, honeywellCommands))
      },
      {
        406,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCpSrq(words, numWords, honeywellCommands))
      },
      {
        407,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrSrq(words, numWords, honeywellCommands))
      },
      {
        408,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTzSrq(words, numWords, honeywellCommands))
      },
      {
        409,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTvSrq(words, numWords, honeywellCommands))
      },
      {
        410,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseSimpleCommand(words, numWords, honeywellCommands))
      },
      {
        411,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMpgSrq(words, numWords, honeywellCommands))
      },
      {
        412,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAreaSrq(words, numWords, honeywellCommands))
      },
      {
        413,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpsSrq(words, numWords, honeywellCommands))
      },
      {
        414,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpsSrqPts(words, numWords, honeywellCommands))
      },
      {
        415,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioRelayCtSrq(words, numWords, honeywellCommands))
      },
      {
        416,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcElevRelayStatus(words, numWords, honeywellCommands))
      },
      {
        501,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcElAlvlSpc(words, numWords, honeywellCommands))
      },
      {
        502,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcElAlvl(words, numWords, honeywellCommands))
      },
      {
        503,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcElFloorConfig(words, numWords, honeywellCommands))
      },
      {
        504,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcElCabAccess(words, numWords, honeywellCommands))
      },
      {
        601,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioCommand(words, numWords, honeywellCommands))
      },
      {
        900,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigRead(words, numWords, honeywellCommands))
      },
      {
        901,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigNotes(words, numWords, honeywellCommands))
      },
      {
        902,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigNetwork(words, numWords, honeywellCommands))
      },
      {
        903,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigHostCommPrim(words, numWords, honeywellCommands))
      },
      {
        904,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigHostCommAlt(words, numWords, honeywellCommands))
      },
      {
        905,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigSessionTmr(words, numWords, honeywellCommands))
      },
      {
        906,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigWebConn(words, numWords, honeywellCommands))
      },
      {
        907,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigAutoSave(words, numWords, honeywellCommands))
      },
      {
        908,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigNetworkDiag(words, numWords, honeywellCommands))
      },
      {
        909,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigTimeServer(words, numWords, honeywellCommands))
      },
      {
        910,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigCentralStation(words, numWords, honeywellCommands))
      },
      {
        911,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigCardDBSize(words, numWords, honeywellCommands))
      },
      {
        912,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigDiagnostics(words, numWords, honeywellCommands))
      },
      {
        913,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigApplyReboot(words, numWords, honeywellCommands))
      },
      {
        1013,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCreateScpLn(words, numWords, honeywellCommands))
      },
      {
        1101,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpIcvt(words, numWords, honeywellCommands))
      },
      {
        1102,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpCfmt(words, numWords, honeywellCommands))
      },
      {
        1103,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpTimezone(words, numWords, honeywellCommands))
      },
      {
        1104,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpHoliday(words, numWords, honeywellCommands))
      },
      {
        1105,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpAdbSpec(words, numWords, honeywellCommands))
      },
      {
        1107,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpScp(words, numWords, honeywellCommands))
      },
      {
        1116,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpDaylight(words, numWords, honeywellCommands))
      },
      {
        1117,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTrgr128(words, numWords, honeywellCommands))
      },
      {
        1121,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAreaSpc(words, numWords, honeywellCommands))
      },
      {
        1123,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpAsdbSpec(words, numWords, honeywellCommands))
      },
      {
        1124,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpAsAdd(words, numWords, honeywellCommands))
      },
      {
        1125,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpAsDel(words, numWords, honeywellCommands))
      },
      {
        1126,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpAsGroup(words, numWords, honeywellCommands))
      },
      {
        1131,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpBioDbSpec1(words, numWords, honeywellCommands))
      },
      {
        1132,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpBioDbAdd1(words, numWords, honeywellCommands))
      },
      {
        1141,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpUCmnd(words, numWords, honeywellCommands))
      },
      {
        1142,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcUCmndMacro(words, numWords, honeywellCommands))
      },
      {
        1143,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpUCmndBurgX(words, numWords, honeywellCommands))
      },
      {
        1144,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpUCmndBkgd(words, numWords, honeywellCommands))
      },
      {
        1220,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAccException(words, numWords, honeywellCommands))
      },
      {
        1304,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAdbCard1304(words, numWords, honeywellCommands))
      },
      {
        1701,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdScp(words, numWords, honeywellCommands))
      },
      {
        1702,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdDaylight(words, numWords, honeywellCommands))
      },
      {
        1703,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdAdbSpec(words, numWords, honeywellCommands))
      },
      {
        1704,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdIcvt(words, numWords, honeywellCommands))
      },
      {
        1705,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdCfmt(words, numWords, honeywellCommands))
      },
      {
        1706,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdRledSpc(words, numWords, honeywellCommands))
      },
      {
        1707,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdTimezone(words, numWords, honeywellCommands))
      },
      {
        1708,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdHoliday(words, numWords, honeywellCommands))
      },
      {
        1709,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdAdbCard(words, numWords, honeywellCommands))
      },
      {
        1801,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdMsp1(words, numWords, honeywellCommands))
      },
      {
        1802,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdSio(words, numWords, honeywellCommands))
      },
      {
        1803,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdInput(words, numWords, honeywellCommands))
      },
      {
        1804,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdOutput(words, numWords, honeywellCommands))
      },
      {
        1805,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdReader(words, numWords, honeywellCommands))
      },
      {
        1811,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdMP(words, numWords, honeywellCommands))
      },
      {
        1812,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdCP(words, numWords, honeywellCommands))
      },
      {
        1813,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdACR(words, numWords, honeywellCommands))
      },
      {
        1814,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdAlvl(words, numWords, honeywellCommands))
      },
      {
        1815,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdTrgr(words, numWords, honeywellCommands))
      },
      {
        1816,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdProc(words, numWords, honeywellCommands))
      },
      {
        1817,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdspare(words, numWords, honeywellCommands))
      },
      {
        1818,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdMpg(words, numWords, honeywellCommands))
      },
      {
        1819,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdArea(words, numWords, honeywellCommands))
      },
      {
        1851,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMemRead(words, numWords, honeywellCommands))
      },
      {
        1852,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRMS(words, numWords, honeywellCommands))
      },
      {
        1853,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcStrSRq(words, numWords, honeywellCommands))
      },
      {
        1854,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcPkgInfo(words, numWords, honeywellCommands))
      },
      {
        1855,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdBioTemplate(words, numWords, honeywellCommands))
      },
      {
        1856,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCertInfo(words, numWords, honeywellCommands))
      },
      {
        2103,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpTimezoneEx(words, numWords, honeywellCommands))
      },
      {
        2116,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAlvlEx(words, numWords, honeywellCommands))
      },
      {
        2220,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpsAlloc(words, numWords, honeywellCommands))
      },
      {
        2221,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpsConfig(words, numWords, honeywellCommands))
      },
      {
        2222,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpsPoints(words, numWords, honeywellCommands))
      },
      {
        2223,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpsPointsSia(words, numWords, honeywellCommands))
      },
      {
        2250,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpLogin(words, numWords, honeywellCommands))
      },
      {
        2251,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpLoginResources(words, numWords, honeywellCommands))
      },
      {
        2252,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpLoginUsers(words, numWords, honeywellCommands))
      },
      {
        2304 /*0x0900*/,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAdbCard2304(words, numWords, honeywellCommands))
      },
      {
        2305,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCardDeleteDbl(words, numWords, honeywellCommands))
      },
      {
        2319,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcApbFreePassDbl(words, numWords, honeywellCommands))
      },
      {
        2323,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcUseLimitDbl(words, numWords, honeywellCommands))
      },
      {
        3103,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpTimezoneExAct(words, numWords, honeywellCommands))
      },
      {
        3124,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpAsAddI64(words, numWords, honeywellCommands))
      },
      {
        3132,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpBioDbAdd1I64(words, numWords, honeywellCommands))
      },
      {
        3133,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpBioDbAddBin(words, numWords, honeywellCommands))
      },
      {
        3304,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAdbCard3304(words, numWords, honeywellCommands))
      },
      {
        3305,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCardDeleteI64(words, numWords, honeywellCommands))
      },
      {
        3319,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcApbFreePassI64(words, numWords, honeywellCommands))
      },
      {
        3323,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcUseLimitI64(words, numWords, honeywellCommands))
      },
      {
        4304,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAdbCard4304(words, numWords, honeywellCommands))
      },
      {
        5304,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAdbCard5304(words, numWords, honeywellCommands))
      },
      {
        6304,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAdbCard6304(words, numWords, honeywellCommands))
      },
      {
        7304,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAdbCard7304(words, numWords, honeywellCommands))
      },
      {
        8304,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAdbCard8304(words, numWords, honeywellCommands))
      },
      {
        9331,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseNvToolsGetAuthorizationCode(words, numWords, honeywellCommands))
      },
      {
        9332,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseNvToolsGetAuthorizationNumber(words, numWords, honeywellCommands))
      }
    });
    this.commandMapList.Insert(1, new Dictionary<int, Action<string[], int, bool>>()
    {
      {
        11,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcBatch(words, numWords, honeywellCommands))
      },
      {
        17,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcPeerCertificate(words, numWords, honeywellCommands))
      },
      {
        18,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpClientDefaults(words, numWords, honeywellCommands))
      },
      {
        19,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpClientAssignment(words, numWords, honeywellCommands))
      },
      {
        107,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScp(words, numWords, honeywellCommands))
      },
      {
        209,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcConfigSave(words, numWords, honeywellCommands))
      },
      {
        210,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcConfigDelta(words, numWords, honeywellCommands))
      },
      {
        304,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAdbCard304(words, numWords, honeywellCommands))
      },
      {
        305,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCardDelete(words, numWords, honeywellCommands))
      },
      {
        319,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcApbFreePass(words, numWords, honeywellCommands))
      },
      {
        323,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcUseLimit(words, numWords, honeywellCommands))
      },
      {
        900,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigRead(words, numWords, honeywellCommands))
      },
      {
        901,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigNotes(words, numWords, honeywellCommands))
      },
      {
        902,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigNetwork(words, numWords, honeywellCommands))
      },
      {
        903,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigHostCommPrim(words, numWords, honeywellCommands))
      },
      {
        904,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigHostCommAlt(words, numWords, honeywellCommands))
      },
      {
        905,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigSessionTmr(words, numWords, honeywellCommands))
      },
      {
        906,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigWebConn(words, numWords, honeywellCommands))
      },
      {
        907,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigAutoSave(words, numWords, honeywellCommands))
      },
      {
        908,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigNetworkDiag(words, numWords, honeywellCommands))
      },
      {
        909,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigTimeServer(words, numWords, honeywellCommands))
      },
      {
        910,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigCentralStation(words, numWords, honeywellCommands))
      },
      {
        911,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigCardDBSize(words, numWords, honeywellCommands))
      },
      {
        912,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigDiagnostics(words, numWords, honeywellCommands))
      },
      {
        913,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigApplyReboot(words, numWords, honeywellCommands))
      },
      {
        1001,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSystem(words, numWords, honeywellCommands))
      },
      {
        1002,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnScpDown(words, numWords, honeywellCommands))
      },
      {
        1011,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCreateChannel(words, numWords, honeywellCommands))
      },
      {
        1012,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDeleteChannel(words, numWords, honeywellCommands))
      },
      {
        1021,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCreateScp(words, numWords, honeywellCommands))
      },
      {
        1022,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDeleteScp(words, numWords, honeywellCommands))
      },
      {
        1023 /*0x03FF*/,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAttachScp(words, numWords, honeywellCommands))
      },
      {
        1024 /*0x0400*/,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDetachScp(words, numWords, honeywellCommands))
      },
      {
        1025,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcFirmwareDown(words, numWords, honeywellCommands))
      },
      {
        1026,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDualPortControl(words, numWords, honeywellCommands))
      },
      {
        1027,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAesControl(words, numWords, honeywellCommands))
      },
      {
        1028,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAesTest(words, numWords, honeywellCommands))
      },
      {
        1029,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcPollMode(words, numWords, honeywellCommands))
      },
      {
        1030,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcFileDownload(words, numWords, honeywellCommands))
      },
      {
        1032,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcHexOutInternal(words, numWords, honeywellCommands))
      },
      {
        1033,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDeleteFile(words, numWords, honeywellCommands))
      },
      {
        1034,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcGetFileInfo(words, numWords, honeywellCommands))
      },
      {
        1101,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpScp(words, numWords, honeywellCommands))
      },
      {
        1102,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpDaylight(words, numWords, honeywellCommands))
      },
      {
        1103,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpAdbSpec(words, numWords, honeywellCommands))
      },
      {
        1104,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpIcvt(words, numWords, honeywellCommands))
      },
      {
        1105,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpCfmt(words, numWords, honeywellCommands))
      },
      {
        1106,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRledSpc(words, numWords, honeywellCommands))
      },
      {
        1107,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpTimezone(words, numWords, honeywellCommands))
      },
      {
        1108,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpHoliday(words, numWords, honeywellCommands))
      },
      {
        1109,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAdbCardFile(words, numWords, honeywellCommands))
      },
      {
        1124,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpAsAdd(words, numWords, honeywellCommands))
      },
      {
        1132,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpBioDbAdd1(words, numWords, honeywellCommands))
      },
      {
        1201,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMsp1(words, numWords, honeywellCommands))
      },
      {
        1202,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSio(words, numWords, honeywellCommands))
      },
      {
        1203,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcInput(words, numWords, honeywellCommands))
      },
      {
        1204,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcOutput(words, numWords, honeywellCommands))
      },
      {
        1205,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcReader(words, numWords, honeywellCommands))
      },
      {
        1211,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMP(words, numWords, honeywellCommands))
      },
      {
        1212,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCP(words, numWords, honeywellCommands))
      },
      {
        1213,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcACR(words, numWords, honeywellCommands))
      },
      {
        1214,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAlvl(words, numWords, honeywellCommands))
      },
      {
        1215,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTrgr(words, numWords, honeywellCommands))
      },
      {
        1216,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcProc(words, numWords, honeywellCommands))
      },
      {
        1217,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcActnRem(words, numWords, honeywellCommands))
      },
      {
        1218,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMpg(words, numWords, honeywellCommands))
      },
      {
        1219,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcArea(words, numWords, honeywellCommands))
      },
      {
        1220,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAccException(words, numWords, honeywellCommands))
      },
      {
        1221,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrLogDeny(words, numWords, honeywellCommands))
      },
      {
        1222,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpUCmnd(words, numWords, honeywellCommands))
      },
      {
        1223,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcUCmndMacro(words, numWords, honeywellCommands))
      },
      {
        1224,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpUCmndBurgX(words, numWords, honeywellCommands))
      },
      {
        1225,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpUCmndBkgd(words, numWords, honeywellCommands))
      },
      {
        1226,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcPointName(words, numWords, honeywellCommands))
      },
      {
        1227,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioAesControl(words, numWords, honeywellCommands))
      },
      {
        1228,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioNetwork(words, numWords, honeywellCommands))
      },
      {
        1229,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRTxtSpc(words, numWords, honeywellCommands))
      },
      {
        1230,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAlvlSpc(words, numWords, honeywellCommands))
      },
      {
        1231,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpBioDbSpec1(words, numWords, honeywellCommands))
      },
      {
        1232,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpBioDbAdd1I64(words, numWords, honeywellCommands))
      },
      {
        1233,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcLanguageCodePages(words, numWords, honeywellCommands))
      },
      {
        1234,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrExtendedLedDefault(words, numWords, honeywellCommands))
      },
      {
        1235,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpBioDbAddBin(words, numWords, honeywellCommands))
      },
      {
        1301,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseSimpleCommand(words, numWords, honeywellCommands))
      },
      {
        1302,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTime(words, numWords, honeywellCommands))
      },
      {
        1303,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTranIndex(words, numWords, honeywellCommands))
      },
      {
        1304,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAdbCard1304(words, numWords, honeywellCommands))
      },
      {
        1305,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCardDeleteI64(words, numWords, honeywellCommands))
      },
      {
        1306,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMpMask(words, numWords, honeywellCommands))
      },
      {
        1307,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCpCtl(words, numWords, honeywellCommands))
      },
      {
        1308,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrMode(words, numWords, honeywellCommands))
      },
      {
        1309,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcForcedOpenMask(words, numWords, honeywellCommands))
      },
      {
        1310,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcHeldOpenMask(words, numWords, honeywellCommands))
      },
      {
        1311,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcUnlock(words, numWords, honeywellCommands))
      },
      {
        1312,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcProcedure(words, numWords, honeywellCommands))
      },
      {
        1313,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTVCommand(words, numWords, honeywellCommands))
      },
      {
        1314,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTzCommand(words, numWords, honeywellCommands))
      },
      {
        1315,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrLedMode(words, numWords, honeywellCommands))
      },
      {
        1316,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcOemCode(words, numWords, honeywellCommands))
      },
      {
        1317,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcPassword(words, numWords, honeywellCommands))
      },
      {
        1318,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpID(words, numWords, honeywellCommands))
      },
      {
        1319,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcApbFreePassI64(words, numWords, honeywellCommands))
      },
      {
        1320,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcHexOut(words, numWords, honeywellCommands))
      },
      {
        1321,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMpgSet(words, numWords, honeywellCommands))
      },
      {
        1322,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAreaSet(words, numWords, honeywellCommands))
      },
      {
        1323,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcUseLimitI64(words, numWords, honeywellCommands))
      },
      {
        1324,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpOfflineTime(words, numWords, honeywellCommands))
      },
      {
        1325,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRLedTemp(words, numWords, honeywellCommands))
      },
      {
        1326,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcLcdText(words, numWords, honeywellCommands))
      },
      {
        1327,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioDc(words, numWords, honeywellCommands))
      },
      {
        1328,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioHexLoad(words, numWords, honeywellCommands))
      },
      {
        1329,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcHostResponse(words, numWords, honeywellCommands))
      },
      {
        1330,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcNvArgSet(words, numWords, honeywellCommands))
      },
      {
        1331,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCardSim(words, numWords, honeywellCommands))
      },
      {
        1332,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpsSet(words, numWords, honeywellCommands))
      },
      {
        1333,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDiag(words, numWords, honeywellCommands))
      },
      {
        1334,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTempAcrMode(words, numWords, honeywellCommands))
      },
      {
        1335,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcOperatingMode(words, numWords, honeywellCommands))
      },
      {
        1336,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioRdrHexLoad(words, numWords, honeywellCommands))
      },
      {
        1337,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrOsdpPassthrough(words, numWords, honeywellCommands))
      },
      {
        1338,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrOfflineAccessList(words, numWords, honeywellCommands))
      },
      {
        1339,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcKeySim(words, numWords, honeywellCommands))
      },
      {
        1340,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcOsdpReaderTransfer(words, numWords, honeywellCommands))
      },
      {
        1401,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseSimpleCommand(words, numWords, honeywellCommands))
      },
      {
        1402,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseSimpleCommand(words, numWords, honeywellCommands))
      },
      {
        1403,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMsp1Srq(words, numWords, honeywellCommands))
      },
      {
        1404,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioSrq(words, numWords, honeywellCommands))
      },
      {
        1405,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMpSrq(words, numWords, honeywellCommands))
      },
      {
        1406,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCpSrq(words, numWords, honeywellCommands))
      },
      {
        1407,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrSrq(words, numWords, honeywellCommands))
      },
      {
        1408,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTzSrq(words, numWords, honeywellCommands))
      },
      {
        1409,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTvSrq(words, numWords, honeywellCommands))
      },
      {
        1410,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseSimpleCommand(words, numWords, honeywellCommands))
      },
      {
        1411,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMpgSrq(words, numWords, honeywellCommands))
      },
      {
        1412,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAreaSrq(words, numWords, honeywellCommands))
      },
      {
        1413,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpsSrq(words, numWords, honeywellCommands))
      },
      {
        1414,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpsSrqPts(words, numWords, honeywellCommands))
      },
      {
        1415,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioRelayCtSrq(words, numWords, honeywellCommands))
      },
      {
        1416,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcElevRelayStatus(words, numWords, honeywellCommands))
      },
      {
        1501,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcElAlvlSpc(words, numWords, honeywellCommands))
      },
      {
        1502,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcElAlvl(words, numWords, honeywellCommands))
      },
      {
        1503,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcElFloorConfig(words, numWords, honeywellCommands))
      },
      {
        1504,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcElCabAccess(words, numWords, honeywellCommands))
      },
      {
        1601,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpAsdbSpec(words, numWords, honeywellCommands))
      },
      {
        1602,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpAsGroup(words, numWords, honeywellCommands))
      },
      {
        1603,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpAsAddI64(words, numWords, honeywellCommands))
      },
      {
        1604,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpAsDel(words, numWords, honeywellCommands))
      },
      {
        1701,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdScp(words, numWords, honeywellCommands))
      },
      {
        1702,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdDaylight(words, numWords, honeywellCommands))
      },
      {
        1703,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdAdbSpec(words, numWords, honeywellCommands))
      },
      {
        1704,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdIcvt(words, numWords, honeywellCommands))
      },
      {
        1705,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdCfmt(words, numWords, honeywellCommands))
      },
      {
        1706,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdRledSpc(words, numWords, honeywellCommands))
      },
      {
        1707,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdTimezone(words, numWords, honeywellCommands))
      },
      {
        1708,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdHoliday(words, numWords, honeywellCommands))
      },
      {
        1709,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdAdbCard(words, numWords, honeywellCommands))
      },
      {
        1801,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdMsp1(words, numWords, honeywellCommands))
      },
      {
        1802,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdSio(words, numWords, honeywellCommands))
      },
      {
        1803,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdInput(words, numWords, honeywellCommands))
      },
      {
        1804,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdOutput(words, numWords, honeywellCommands))
      },
      {
        1805,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdReader(words, numWords, honeywellCommands))
      },
      {
        1811,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdMP(words, numWords, honeywellCommands))
      },
      {
        1812,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdCP(words, numWords, honeywellCommands))
      },
      {
        1813,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdACR(words, numWords, honeywellCommands))
      },
      {
        1814,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdAlvl(words, numWords, honeywellCommands))
      },
      {
        1815,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdTrgr(words, numWords, honeywellCommands))
      },
      {
        1816,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdProc(words, numWords, honeywellCommands))
      },
      {
        1817,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdspare(words, numWords, honeywellCommands))
      },
      {
        1818,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdMpg(words, numWords, honeywellCommands))
      },
      {
        1819,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdArea(words, numWords, honeywellCommands))
      },
      {
        1851,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMemRead(words, numWords, honeywellCommands))
      },
      {
        1852,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRMS(words, numWords, honeywellCommands))
      },
      {
        1853,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcStrSRq(words, numWords, honeywellCommands))
      },
      {
        1854,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcPkgInfo(words, numWords, honeywellCommands))
      },
      {
        1855,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdBioTemplate(words, numWords, honeywellCommands))
      },
      {
        1901,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioCommand(words, numWords, honeywellCommands))
      },
      {
        2021,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCreateScpLn(words, numWords, honeywellCommands))
      },
      {
        2107,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpTimezoneEx(words, numWords, honeywellCommands))
      },
      {
        2215,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTrgr128(words, numWords, honeywellCommands))
      },
      {
        2219,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAreaSpc(words, numWords, honeywellCommands))
      },
      {
        2304 /*0x0900*/,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnNCcAdbCard2304(words, numWords, honeywellCommands))
      },
      {
        2305,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCardDeleteDbl(words, numWords, honeywellCommands))
      },
      {
        2319,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcApbFreePassDbl(words, numWords, honeywellCommands))
      },
      {
        2323,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcUseLimitDbl(words, numWords, honeywellCommands))
      },
      {
        3107,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpTimezoneExAct(words, numWords, honeywellCommands))
      },
      {
        3220,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpsAlloc(words, numWords, honeywellCommands))
      },
      {
        3221,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpsConfig(words, numWords, honeywellCommands))
      },
      {
        3222,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpsPoints(words, numWords, honeywellCommands))
      },
      {
        3223,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpsPointsSia(words, numWords, honeywellCommands))
      },
      {
        3250,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpLogin(words, numWords, honeywellCommands))
      },
      {
        3251,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpLoginResources(words, numWords, honeywellCommands))
      },
      {
        3252,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpLoginUsers(words, numWords, honeywellCommands))
      },
      {
        4304,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnNCcAdbCard4304(words, numWords, honeywellCommands))
      },
      {
        5304,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnNCcAdbCard5304(words, numWords, honeywellCommands))
      },
      {
        6304,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnNCcAdbCard6304(words, numWords, honeywellCommands))
      }
    });
    this.commandMapList.Insert(2, new Dictionary<int, Action<string[], int, bool>>()
    {
      {
        1,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcBatch(words, numWords, honeywellCommands))
      },
      {
        11,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSystem(words, numWords, honeywellCommands))
      },
      {
        12,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCreateChannel(words, numWords, honeywellCommands))
      },
      {
        13,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCreateScp(words, numWords, honeywellCommands))
      },
      {
        14,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDeleteChannel(words, numWords, honeywellCommands))
      },
      {
        15,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDeleteScp(words, numWords, honeywellCommands))
      },
      {
        17,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcPeerCertificate(words, numWords, honeywellCommands))
      },
      {
        18,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpClientDefaults(words, numWords, honeywellCommands))
      },
      {
        19,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcIpClientAssignment(words, numWords, honeywellCommands))
      },
      {
        108,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMsp1(words, numWords, honeywellCommands))
      },
      {
        109,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSio(words, numWords, honeywellCommands))
      },
      {
        110,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcInput(words, numWords, honeywellCommands))
      },
      {
        111,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcOutput(words, numWords, honeywellCommands))
      },
      {
        112 /*0x70*/,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcReader(words, numWords, honeywellCommands))
      },
      {
        113,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMP(words, numWords, honeywellCommands))
      },
      {
        114,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCP(words, numWords, honeywellCommands))
      },
      {
        115,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcACR(words, numWords, honeywellCommands))
      },
      {
        117,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTrgr(words, numWords, honeywellCommands))
      },
      {
        118,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcProc(words, numWords, honeywellCommands))
      },
      {
        119,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcActnRem(words, numWords, honeywellCommands))
      },
      {
        120,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMpg(words, numWords, honeywellCommands))
      },
      {
        122,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRledSpc(words, numWords, honeywellCommands))
      },
      {
        123,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRTxtSpc(words, numWords, honeywellCommands))
      },
      {
        124,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAlvlSpc(words, numWords, honeywellCommands))
      },
      {
        (int) sbyte.MaxValue,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioAesControl(words, numWords, honeywellCommands))
      },
      {
        151,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrLogDeny(words, numWords, honeywellCommands))
      },
      {
        203,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnScpDown(words, numWords, honeywellCommands))
      },
      {
        206,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcFirmwareDown(words, numWords, honeywellCommands))
      },
      {
        207,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAttachScp(words, numWords, honeywellCommands))
      },
      {
        208 /*0xD0*/,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDetachScp(words, numWords, honeywellCommands))
      },
      {
        209,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcConfigSave(words, numWords, honeywellCommands))
      },
      {
        210,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcConfigDelta(words, numWords, honeywellCommands))
      },
      {
        211,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDualPortControl(words, numWords, honeywellCommands))
      },
      {
        212,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAesControl(words, numWords, honeywellCommands))
      },
      {
        214,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcPollMode(words, numWords, honeywellCommands))
      },
      {
        215,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcFileDownload(words, numWords, honeywellCommands))
      },
      {
        301,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseSimpleCommand(words, numWords, honeywellCommands))
      },
      {
        302,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTime(words, numWords, honeywellCommands))
      },
      {
        303,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTranIndex(words, numWords, honeywellCommands))
      },
      {
        306,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMpMask(words, numWords, honeywellCommands))
      },
      {
        307,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCpCtl(words, numWords, honeywellCommands))
      },
      {
        308,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrMode(words, numWords, honeywellCommands))
      },
      {
        309,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcForcedOpenMask(words, numWords, honeywellCommands))
      },
      {
        310,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcHeldOpenMask(words, numWords, honeywellCommands))
      },
      {
        311,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcUnlock(words, numWords, honeywellCommands))
      },
      {
        312,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcProcedure(words, numWords, honeywellCommands))
      },
      {
        313,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTVCommand(words, numWords, honeywellCommands))
      },
      {
        314,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTzCommand(words, numWords, honeywellCommands))
      },
      {
        315,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrLedMode(words, numWords, honeywellCommands))
      },
      {
        317,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcPassword(words, numWords, honeywellCommands))
      },
      {
        318,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpID(words, numWords, honeywellCommands))
      },
      {
        321,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMpgSet(words, numWords, honeywellCommands))
      },
      {
        322,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAreaSet(words, numWords, honeywellCommands))
      },
      {
        324,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpOfflineTime(words, numWords, honeywellCommands))
      },
      {
        325,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRLedTemp(words, numWords, honeywellCommands))
      },
      {
        326,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcLcdText(words, numWords, honeywellCommands))
      },
      {
        328,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioHexLoad(words, numWords, honeywellCommands))
      },
      {
        329,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcHostResponse(words, numWords, honeywellCommands))
      },
      {
        331,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCardSim(words, numWords, honeywellCommands))
      },
      {
        333,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcDiag(words, numWords, honeywellCommands))
      },
      {
        334,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTempAcrMode(words, numWords, honeywellCommands))
      },
      {
        335,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcOperatingMode(words, numWords, honeywellCommands))
      },
      {
        337,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrOsdpPassthrough(words, numWords, honeywellCommands))
      },
      {
        339,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcKeySim(words, numWords, honeywellCommands))
      },
      {
        401,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseSimpleCommand(words, numWords, honeywellCommands))
      },
      {
        402,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseSimpleCommand(words, numWords, honeywellCommands))
      },
      {
        403,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMsp1Srq(words, numWords, honeywellCommands))
      },
      {
        404,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioSrq(words, numWords, honeywellCommands))
      },
      {
        405,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMpSrq(words, numWords, honeywellCommands))
      },
      {
        406,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCpSrq(words, numWords, honeywellCommands))
      },
      {
        407,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAcrSrq(words, numWords, honeywellCommands))
      },
      {
        408,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTzSrq(words, numWords, honeywellCommands))
      },
      {
        409,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTvSrq(words, numWords, honeywellCommands))
      },
      {
        411,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcMpgSrq(words, numWords, honeywellCommands))
      },
      {
        412,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAreaSrq(words, numWords, honeywellCommands))
      },
      {
        415,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcSioRelayCtSrq(words, numWords, honeywellCommands))
      },
      {
        416,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcElevRelayStatus(words, numWords, honeywellCommands))
      },
      {
        501,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcElAlvlSpc(words, numWords, honeywellCommands))
      },
      {
        502,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcElAlvl(words, numWords, honeywellCommands))
      },
      {
        900,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigRead(words, numWords, honeywellCommands))
      },
      {
        901,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigNotes(words, numWords, honeywellCommands))
      },
      {
        902,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigNetwork(words, numWords, honeywellCommands))
      },
      {
        903,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigHostCommPrim(words, numWords, honeywellCommands))
      },
      {
        905,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigSessionTmr(words, numWords, honeywellCommands))
      },
      {
        906,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigWebConn(words, numWords, honeywellCommands))
      },
      {
        907,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigAutoSave(words, numWords, honeywellCommands))
      },
      {
        908,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigNetworkDiag(words, numWords, honeywellCommands))
      },
      {
        909,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigTimeServer(words, numWords, honeywellCommands))
      },
      {
        912,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigDiagnostics(words, numWords, honeywellCommands))
      },
      {
        913,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcWebConfigApplyReboot(words, numWords, honeywellCommands))
      },
      {
        1013,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCreateScpLn(words, numWords, honeywellCommands))
      },
      {
        1101,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpIcvt(words, numWords, honeywellCommands))
      },
      {
        1102,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpCfmt(words, numWords, honeywellCommands))
      },
      {
        1104,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpHoliday(words, numWords, honeywellCommands))
      },
      {
        1105,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpAdbSpec(words, numWords, honeywellCommands))
      },
      {
        1107,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpScp(words, numWords, honeywellCommands))
      },
      {
        1116,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpDaylight(words, numWords, honeywellCommands))
      },
      {
        1117,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcTrgr128(words, numWords, honeywellCommands))
      },
      {
        1121,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAreaSpc(words, numWords, honeywellCommands))
      },
      {
        1141,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpUCmnd(words, numWords, honeywellCommands))
      },
      {
        1144,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpUCmndBkgd(words, numWords, honeywellCommands))
      },
      {
        1220,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAccException(words, numWords, honeywellCommands))
      },
      {
        1707,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdTimezone(words, numWords, honeywellCommands))
      },
      {
        1709,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcRdAdbCard(words, numWords, honeywellCommands))
      },
      {
        1853,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcStrSRq(words, numWords, honeywellCommands))
      },
      {
        1856,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCertInfo(words, numWords, honeywellCommands))
      },
      {
        2116,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAlvlEx(words, numWords, honeywellCommands))
      },
      {
        2250,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpLogin(words, numWords, honeywellCommands))
      },
      {
        2252,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpLoginUsers(words, numWords, honeywellCommands))
      },
      {
        3103,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcScpTimezoneExAct(words, numWords, honeywellCommands))
      },
      {
        3305,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcCardDeleteI64(words, numWords, honeywellCommands))
      },
      {
        3319,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcApbFreePassI64(words, numWords, honeywellCommands))
      },
      {
        3323,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcUseLimitI64(words, numWords, honeywellCommands))
      },
      {
        5304,
        (Action<string[], int, bool>) ((words, numWords, honeywellCommands) => this.ParseEnCcAdbCard5304(words, numWords, honeywellCommands))
      }
    });
    this.activeCommandMap = this.commandMapList[this.productFamily];
    return false;
  }


  public CommandDecoder()
  {
    this.foregroundColor = LineColor.Black;
    this.backgroundColor = LineColor.White;
    this.textColor = this.foregroundColor;
    this.productFamily = (int) ProductFamily.Aero;
    this.loadCommandMap();
  }

  /// <summary>
  /// The Mercury / Honeywell / Aero command dialect that incoming strings are decoded against.
  /// Replaces the old dialectComboBox selection from the WinForms UI.
  /// </summary>
  public ProductFamily CurrentProductFamily
  {
    get => (ProductFamily) this.productFamily;
    set
    {
      this.productFamily = (int) value;
      if (this.commandMapList.Count > 0)
        this.activeCommandMap = this.commandMapList[this.productFamily];
    }
  }

  /// <summary>
  /// The decoded output lines produced by the most recent call to Decode(...).
  /// Replaces the old textBox2 RichTextBox output from the WinForms UI.
  /// </summary>
  public List<DecodedLine> Lines { get; } = new List<DecodedLine>();

  /// <summary>Clears any previously decoded output.</summary>
  public void Clear()
  {
    this.Lines.Clear();
  }

  /// <summary>
  /// Decodes the given command string and returns the resulting output lines.
  /// Replaces the old Run_Click button-click handler from the WinForms UI.
  /// </summary>
  public List<DecodedLine> Decode(string commandString)
  {
    this.Lines.Clear();
    string text = commandString ?? string.Empty;
    this.currentLine = string.Copy(text);
    string input = text.Trim();
    string str1 = this.RemoveDuplicateWhiteSpace(input);
    int length = str1.Length;
    char[] separator = new char[1] { ' ' };
    string[] strArray = str1.Split(separator, StringSplitOptions.RemoveEmptyEntries);
    int num = ((IEnumerable<string>) strArray).Count<string>();
    if (num == 1 && (strArray[0][0] == '5' && strArray[0][1] == 'A' || strArray[0][0] == '5' && strArray[0][1] == '3') && strArray[0] != "5304")
    {
      string str2 = "";
      for (int index = 0; index < length - 1; index += 2)
      {
        string str3 = $"{strArray[0][index]}{strArray[0][index + 1]} ";
        str2 += str3;
      }
      strArray = str2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
      num = ((IEnumerable<string>) strArray).Count<string>();
    }
    if (num > 0)
    {
      try
      {
        this.ProcessCommands(strArray);
      }
      catch
      {
      }
    }
    else
      this.InsertLine("Invalid input string");
    return this.Lines;
  }



  private bool CheckForReply(string[] words)
  {
    return ((IEnumerable<string>) words).Count<string>() >= 1 && (words[0] == "5B" || words[0] == "59");
  }

  private bool CheckForMSP1Message(string[] words)
  {
    return ((IEnumerable<string>) words).Count<string>() >= 1 && (words[0] == "5A" || words[0] == "57");
  }

  private bool CheckForOSDPMessage(string[] words)
  {
    return ((IEnumerable<string>) words).Count<string>() >= 1 && words[0] == "53";
  }

  private bool CheckForTransaction(string[] words)
  {
    if (((IEnumerable<string>) words).Count<string>() >= 2)
    {
      switch (words[1])
      {
        case "Apr":
        case "Aug":
        case "Dec":
        case "Feb":
        case "Jan":
        case "Jul":
        case "Jun":
        case "Mar":
        case "May":
        case "Nov":
        case "Oct":
        case "Sep":
          return true;
      }
    }
    return false;
  }



  private void ProcessCommands(string[] words)
  {
    try
    {
      this.textColor = this.foregroundColor;
      this.processCommandOnly = true;
      int num1 = ((IEnumerable<string>) words).Count<string>();
      if (num1 >= 3)
      {
        if (words[2].Contains("enCc"))
          this.processCommandOnly = false;
        if (words[2].Contains("enNCc"))
          this.processCommandOnly = false;
      }
      if (!this.processCommandOnly && num1 < 4 || this.processCommandOnly && num1 < 1)
      {
        this.InsertLine("Invalid input string");
      }
      else
      {
        int cmdIndex = this.GetCmdIndex();
        string word = words[cmdIndex];
        try
        {
          this.activeCommandMap[int.Parse(words[cmdIndex])](words, num1, this.productFamily == 1);
        }
        catch (KeyNotFoundException ex)
        {
          this.textColor = this.errorFoundTextColor;
          this.InsertLine($"Command [{word}] Not Implemented!!!!!! Exception Info:{ex.GetType().ToString()}");
        }
        catch (FormatException ex)
        {
          this.textColor = this.errorFoundTextColor;
          this.InsertLine($"{ex.GetType().ToString()}: {ex.Message}\n\nPlease Verify Command Parameters");
        }
        catch (Exception ex)
        {
          this.textColor = this.errorFoundTextColor;
          this.InsertLine($"Unknown Error Occured: \n{ex.GetType().ToString()}: {ex.Message}\n\nPlease Check Command Parameters.");
        }
      }
    }
    catch (FormatException ex)
    {
      this.textColor = this.errorFoundTextColor;
      this.InsertLine($"Error parsing input string: \n{ex.GetType().ToString()}: {ex.Message}\n\n Please Check Command Parameters");
    }
  }

  private void InsertLine(string str)
  {
    this.InsertLine(str, this.textColor);
    this.textColor = this.foregroundColor;
  }

  private void InsertLine(string str, LineColor color)
  {
    this.Lines.Add(new DecodedLine(str, color));
  }

  private void InsertLine(string labelStr, string separatorStr, string[] words, int index)
  {
    if (index >= ((IEnumerable<string>) words).Count<string>())
      return;
    this.InsertLine(labelStr + separatorStr + words[index]);
  }

  private void InsertLineValidateRange(
    string labelStr,
    string separatorStr,
    string[] words,
    int index,
    int minVal,
    int maxVal)
  {
    if (index >= ((IEnumerable<string>) words).Count<string>())
      return;
    try
    {
      int num = int.Parse(words[index]);
      string str = labelStr + separatorStr + words[index];
      if (num < minVal || num > maxVal)
        this.textColor = this.errorFoundTextColor;
      this.InsertLine(str);
    }
    catch
    {
      this.textColor = this.errorFoundTextColor;
      this.InsertLine($"{labelStr}{separatorStr}Invalid Parameter");
      throw new ArgumentException($"Error Parsing {labelStr} to a 32-bit Signed Integer.");
    }
  }

  private void InsertLine(string labelStr, string separatorStr, string word)
  {
    if (!(word != ""))
      return;
    this.InsertLine(labelStr + separatorStr + word);
  }

  private void InsertLine(
    string labelStr,
    string separatorStr,
    string[] words,
    int index,
    int count)
  {
    if (index >= ((IEnumerable<string>) words).Count<string>())
      return;
    string str = labelStr + separatorStr;
    for (int index1 = 0; index1 < count; ++index1)
    {
      if (index + index1 < ((IEnumerable<string>) words).Count<string>())
        str = $"{str}{words[index + index1]} ";
    }
    this.InsertLine(str);
  }

  private void InsertLineValidateRange(
    string labelStr,
    string separatorStr,
    string[] words,
    int index,
    int count,
    int minVal,
    int maxVal)
  {
    if (index >= ((IEnumerable<string>) words).Count<string>())
      return;
    string str = labelStr + separatorStr;
    for (int index1 = 0; index1 < count; ++index1)
    {
      if (index + index1 < ((IEnumerable<string>) words).Count<string>())
      {
        int num = int.Parse(words[index + index1]);
        if (num < minVal || num > maxVal)
          this.textColor = this.errorFoundTextColor;
        str = $"{str}{words[index + index1]} ";
      }
    }
    this.InsertLine(str);
  }

  private int InsertLineWithText(string labelStr, string separatorStr, string[] words, int index)
  {
    int num1 = 0;
    string str = labelStr + separatorStr;
    bool flag = true;
    int num2 = 0;
    while (flag)
    {
      if (index + num1 < ((IEnumerable<string>) words).Count<string>())
      {
        str = str + words[index + num1] + " ";
        int num3 = words[index + num1].Split('"').Length - 1;
        num2 += num3;
        ++num1;
        if (num2 >= 2)
          flag = false;
      }
      else
        flag = false;
    }
    this.InsertLine(str);
    return num1;
  }

  private void ProcessTransactions(string[] words)
  {
    int index = 0;
    this.InsertLine("Date/Time", ": ", $"{words[1]} {words[2]} {words[3]}");
    string[] strArray1 = words[4].Split(':');
    this.InsertLine("SCP", ": ", strArray1[0]);
    this.InsertLine("Serial Number", ": ", strArray1[1]);
    if (words[5].Contains(",") && words[6].Contains(":"))
      index = 6;
    else if (words[6].Contains(",") && words[7].Contains(":"))
      index = 7;
    if (index <= 0)
      return;
    switch (index)
    {
      case 6:
        this.InsertLine("Source", ": ", words[5]);
        break;
      case 7:
        this.InsertLine("Source", ": ", $"{words[5]} {words[6]}");
        break;
    }
    string[] strArray2 = words[index].Split(':');
    this.ParseTransactionType(strArray2[0], strArray2[1], false);
  }

  private void ParseTransactionType(string tran_type_str, string tran_code_str, bool hexValues)
  {
    string word1 = "";
    int num1;
    int num2;
    if (hexValues)
    {
      num1 = int.Parse(tran_type_str, NumberStyles.HexNumber);
      num2 = int.Parse(tran_code_str, NumberStyles.HexNumber);
    }
    else
    {
      num1 = int.Parse(tran_type_str);
      num2 = int.Parse(tran_code_str);
    }
    string word2;
    switch (num1)
    {
      case 1:
        word1 = tran_type_str + " - tranTypeSys";
        switch (num2)
        {
          case 1:
            word2 = "1 - SCP power-up diagnostics";
            break;
          case 2:
            word2 = "2 - host comm, off-line --> formats to TypeSytsComm";
            break;
          case 3:
            word2 = "3 - host comm, on-line ---> formats to TypeSytsComm";
            break;
          case 4:
            word2 = "4 - Transaction count exceeds the preset limit";
            break;
          case 5:
            word2 = "5 - Autosave - Configuration save complete";
            break;
          case 6:
            word2 = "6 - Autosave - Database Complete";
            break;
          case 7:
            word2 = "7 - Card Database cleared due to SRAM buffer overflow";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 2:
        word1 = tran_type_str + " - tranTypeSioComm";
        switch (num2)
        {
          case 1:
            word2 = "1- comm disabled (result of host command)";
            break;
          case 2:
            word2 = "2 - off-line: timeout (no/bad response from unit)";
            break;
          case 3:
            word2 = "3 - off-line: invalid identification from SIO";
            break;
          case 4:
            word2 = "4 - off-line: Encryption could not be established";
            break;
          case 5:
            word2 = "5 - on-line: normal connection";
            break;
          case 6:
            word2 = "6 - hexLoad report: ser_num is address loaded (-1 == last record)";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 3:
        word1 = tran_type_str + " - tranTypeCardBin";
        word2 = num2 != 1 ? tran_code_str + " - UNKNOWN" : "1 - access denied, invalid card format";
        break;
      case 4:
        word1 = tran_type_str + " - tranTypeCardBcd";
        switch (num2)
        {
          case 1:
            word2 = "1 - access denied, invalid card format, forward read";
            break;
          case 2:
            word2 = "2 - access denied, invalid card format, reverse read";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 5:
      case 21:
      case 37:
      case 53:
      case 69:
        if (num1 <= 21)
        {
          if (num1 != 5)
          {
            if (num1 == 21)
              word1 = tran_type_str + " - tranTypeDblCardFull";
          }
          else
            word1 = tran_type_str + " - tranTypeCardFull";
        }
        else if (num1 != 37)
        {
          if (num1 != 53)
          {
            if (num1 == 69)
              word1 = tran_type_str + " - tranTypeI64CardFullAAM";
          }
          else
            word1 = tran_type_str + " - tranTypeI64CardFullIc32";
        }
        else
          word1 = tran_type_str + " - tranTypeI64CardFull";
        switch (num2)
        {
          case 1:
            word2 = "1 - request rejected: access point locked";
            break;
          case 2:
            word2 = "2 - request accepted: access point unlocked";
            break;
          case 3:
            word2 = "3 - request rejected: invalid facility code";
            break;
          case 4:
            word2 = "4 - request rejected: invalid f/c extension";
            break;
          case 5:
            word2 = "5 - request rejected: not in card file";
            break;
          case 6:
            word2 = "6 - request rejected: invalid issue code";
            break;
          case 7:
            word2 = "7 - request granted: f/c verified, not used";
            break;
          case 8:
            word2 = "8 - request granted: f/c verified, door used";
            break;
          case 9:
            word2 = "9 - access denied - asked for host approval, then timed out";
            break;
          case 10:
            word2 = "10 - reporting that this card is 'about to get access granted' (expecting C_329 Host Response)";
            break;
          case 11:
            word2 = "11 - access denied count exceeded";
            break;
          case 12:
            word2 = "12 - access denied - asked for host approval, then host denied";
            break;
          case 13:
            word2 = "13 - request rejected: Airlock is Busy";
            break;
          case 14:
            word2 = "14 - connection issue with AAM component.  Failed to connect or timeout";
            break;
          case 15:
            word2 = "15 - authentication or validation failure, specified in the nAuth* fields";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 6:
      case 22:
      case 38:
      case 54:
        if (num1 <= 22)
        {
          if (num1 != 6)
          {
            if (num1 == 22)
              word1 = tran_type_str + " - tranTypeDblCardID";
          }
          else
            word1 = tran_type_str + " - tranTypeCardID";
        }
        else if (num1 != 38)
        {
          if (num1 == 54)
            word1 = tran_type_str + " - tranTypeI64CardIDAAM";
        }
        else
          word1 = tran_type_str + " - tranTypeI64CardID";
        switch (num2)
        {
          case 1:
            word2 = "1 - request rejected: de-activated card";
            break;
          case 2:
            word2 = "2 - request rejected: before activation date";
            break;
          case 3:
            word2 = "3 - request rejected: after expiration date";
            break;
          case 4:
            word2 = "4 - request rejected: invalid time";
            break;
          case 5:
            word2 = "5 - request rejected: invalid PIN";
            break;
          case 6:
            word2 = "6 - request rejected: anti-passback violation";
            break;
          case 7:
            word2 = "7 - request granted:  apb violation, not used";
            break;
          case 8:
            word2 = "8 - request granted:  apb violation, used";
            break;
          case 9:
            word2 = "9 - request rejected: duress code detected";
            break;
          case 10:
            word2 = "10 - request granted:  duress, used";
            break;
          case 11:
            word2 = "11 - request granted:  duress, not used";
            break;
          case 12:
            word2 = "12 - request granted:  full test, not used";
            break;
          case 13:
            word2 = "13 - request granted:  full test, used";
            break;
          case 14:
            word2 = "14 - request denied:   never allowed at this reader (all Tz's == 0)";
            break;
          case 15:
            word2 = "15 - request denied:   no second card presented";
            break;
          case 16 /*0x10*/:
            word2 = "16 - request denied:   occupancy limit reached";
            break;
          case 17:
            word2 = "17 - request denied:   the area is NOT enabled";
            break;
          case 18:
            word2 = "18 - request denied:   use limit";
            break;
          case 19:
            word2 = "19 - request denied:   unauthorized assets";
            break;
          case 20:
            word2 = "20 - request denied:   biometric verification error";
            break;
          case 21:
            word2 = "21 - granting access:  used/not-used transaction will follow";
            break;
          case 22:
            word2 = "22 - request rejected: failed the bio test: no bio record";
            break;
          case 23:
            word2 = "23 - request rejected: failed the bio test: no bio device";
            break;
          case 24:
            word2 = "24 - request rejected: no escort card presented";
            break;
          case 25:
            word2 = "25 - request rejected: obsolete >> m1m/m2m last special user may not exit";
            break;
          case 26:
            word2 = "26 - request rejected: obsolete >> m1m/m2m area has no special users yet";
            break;
          case 27:
            word2 = "27 - request rejected: obsolete >> m1m/m2m supervisor approval timeout";
            break;
          case 28:
            word2 = "28 - request rejected: no asset present - asset is required";
            break;
          case 29:
            word2 = "29 - request rejected: Airlock is Busy";
            break;
          case 30:
            word2 = "30 - request rejected: Incomplete CARD & PIN sequence";
            break;
          case 31 /*0x1F*/:
            word2 = "31 - request granted: Double-card event.";
            break;
          case 32 /*0x20*/:
            word2 = "32 - request granted: Double-card event while in uncontrolled state (locked/unlocked).";
            break;
          case 33:
            word2 = "33 - request rejected: Elevators - floor not in floors served";
            break;
          case 34:
            word2 = "34 - request rejected: Elevators - floor request not authorized";
            break;
          case 35:
            word2 = "35 - request rejected: Elevators - timeout";
            break;
          case 36:
            word2 = "36 - request rejected: Elevators - unknown error";
            break;
          case 37:
            word2 = "37 - request granted: local verification - not used";
            break;
          case 38:
            word2 = "38 - request rejected: local verification - used";
            break;
          case 39:
            word2 = "39 - granting access: requires escort, pending escort card";
            break;
          case 40:
            word2 = "40 - request rejected: violates minimum occupancy count";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 7:
        word1 = tran_type_str + " - tranTypeCoS";
        switch (num2)
        {
          case 1:
            word2 = "1 - disconnected (from an input point ID)";
            break;
          case 2:
            word2 = "2 - unknown (off-line): no report from the ID";
            break;
          case 3:
            word2 = "3 - secure (or de-activate relay)";
            break;
          case 4:
            word2 = "4 - alarm (or activated relay: perm or temp)";
            break;
          case 5:
            word2 = "5 - fault";
            break;
          case 6:
            word2 = "6 - exit delay in progress";
            break;
          case 7:
            word2 = "7 - entry delay in progress";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 8:
        word1 = tran_type_str + " - tranTypeREX";
        switch (num2)
        {
          case 1:
            word2 = "1 - exit cycle: door use not verified";
            break;
          case 2:
            word2 = "2 - exit cycle: door not used";
            break;
          case 3:
            word2 = "3 - exit cycle: door used";
            break;
          case 4:
            word2 = "4 - host initiated request: door use not verified";
            break;
          case 5:
            word2 = "5 - host initiated request: door not used";
            break;
          case 6:
            word2 = "6 - host initiated request: door used";
            break;
          case 7:
            word2 = "7 -\texit request denied: Airlock Busy";
            break;
          case 8:
            word2 = "8 - host request: Cannot complete due to Airlock Busy.";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 9:
        word1 = tran_type_str + " - tranTypeCoSDoor";
        switch (num2)
        {
          case 1:
            word2 = "1 - disconnected";
            break;
          case 2:
            word2 = "2 - unknown (_RS bits: last known status)";
            break;
          case 3:
            word2 = "3 - secure";
            break;
          case 4:
            word2 = "4 - alarm (forced, held, or both)";
            break;
          case 5:
            word2 = "5 - fault (fault type is encoded in door_status byte";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 10:
        word1 = tran_type_str + " - tranTypeProcedure";
        switch (num2)
        {
          case 1:
            word2 = "1 - cancel (abort delay), see command enCcProcedure (312)";
            break;
          case 2:
            word2 = "2 - execute (start new), see command enCcProcedure (312)";
            break;
          case 3:
            word2 = "3 - resume, if paused, see command enCcProcedure (312)";
            break;
          case 4:
            word2 = "4 - execute (prefix  256 actions), see command enCcProcedure (312)";
            break;
          case 5:
            word2 = "5 - execute (prefix  512 actions), see command enCcProcedure (312)";
            break;
          case 6:
            word2 = "6 - execute (prefix 1024 actions), see command enCcProcedure (312)";
            break;
          case 7:
            word2 = "7 - resume  (prefix  256 actions), see command enCcProcedure (312)";
            break;
          case 8:
            word2 = "8 - resume  (prefix  512 actions), see command enCcProcedure (312) ";
            break;
          case 9:
            word2 = "9 - resume  (prefix 1024 actions), see command enCcProcedure (312)";
            break;
          case 10:
            word2 = "10 - command was issued to procedure with no actions - nop";
            break;
          case 11:
            word2 = "11 - regional I/O procedure action not acknowledged";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 11:
        word1 = tran_type_str + " - tranTypeUserCmnd";
        word2 = num2 != 1 ? tran_code_str + " - UNKNOWN" : "1 - command entered by the user...";
        break;
      case 12:
        word1 = tran_type_str + " - tranTypeActivate";
        switch (num2)
        {
          case 1:
            word2 = "1 - became inactive";
            break;
          case 2:
            word2 = "2 - became active";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 13:
        word1 = tran_type_str + " - tranTypeAcr";
        switch (num2)
        {
          case 1:
            word2 = "1 - disabled";
            break;
          case 2:
            word2 = "2 - unlocked";
            break;
          case 3:
            word2 = "3 - locked (exit request enabled)";
            break;
          case 4:
            word2 = "4 - facility code only";
            break;
          case 5:
            word2 = "5 - card only";
            break;
          case 6:
            word2 = "6 - PIN only";
            break;
          case 7:
            word2 = "7 - card and PIN";
            break;
          case 8:
            word2 = "8 - PIN or card";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 14:
        word1 = tran_type_str + " - tranTypeMpg";
        switch (num2)
        {
          case 1:
            word2 = "1 - first disarm command executed (mask_count was 0, all MPs got masked)";
            break;
          case 2:
            word2 = "2 - subsequent disarm command executed (mask_count incremented, MPs already masked)";
            break;
          case 3:
            word2 = "3 - override command: armed (mask_count cleard, all points unmasked)";
            break;
          case 4:
            word2 = "4 - override command: disarmed (mask_count set, unmasked all points)";
            break;
          case 5:
            word2 = "5 - force arm command, MPG armed, (may have active zones, mask_count is now zero)";
            break;
          case 6:
            word2 = "6 - force arm command, MPG not armed (mask_count decremented)";
            break;
          case 7:
            word2 = "7 - standard arm command, MPG armed (did not have active zones, mask_count is now zero)";
            break;
          case 8:
            word2 = "8 - standard arm command, MPG did not arm, (had active zones, mask_count unchanged)";
            break;
          case 9:
            word2 = "9 - standard arm command, MPG still armed, (mask_count decremented)";
            break;
          case 10:
            word2 = "10 - override arm command, MPG armed (mask_count is now zero)";
            break;
          case 11:
            word2 = "11 - override arm command, MPG did not arm, (mask_count decremented)";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 15:
        word1 = tran_type_str + " - tranTypeArea";
        switch (num2)
        {
          case 1:
            word2 = "1 - area disabled";
            break;
          case 2:
            word2 = "2 - area enabled";
            break;
          case 3:
            word2 = "3 - occupancy count reached zero";
            break;
          case 4:
            word2 = "4 - occupancy count reached the 'downward-limit'";
            break;
          case 5:
            word2 = "5 - occupancy count reached the 'upward-limit'";
            break;
          case 6:
            word2 = "6 - occupancy count reached the 'max-occupancy-limit'";
            break;
          case 7:
            word2 = "7 - multi-occupancy mode change";
            break;
          case 8:
            word2 = "8 - multi-occupancy mode change could not be made - the area is not empty";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 16 /*0x10*/:
      case 32 /*0x20*/:
        if (num1 != 16 /*0x10*/)
        {
          if (num1 == 32 /*0x20*/)
            word1 = tran_type_str + " - tranTypeAssetI64";
        }
        else
          word1 = tran_type_str + " - tranTypeAsset";
        switch (num2)
        {
          case 1:
            word2 = "1 - asset: no access request, no card";
            break;
          case 2:
            word2 = "2 - asset: no temp buffer to queue asset";
            break;
          case 3:
            word2 = "3 - asset: timeout, no card event";
            break;
          case 4:
            word2 = "4 - asset: have card, asset not in database";
            break;
          case 5:
            word2 = "5 - asset: have card, card is owner";
            break;
          case 6:
            word2 = "6 - asset: have card, card has class rights";
            break;
          case 7:
            word2 = "7 - asset: have card, card has no rights";
            break;
          case 8:
            word2 = "8 - asset: sent a TamperSet command";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 17:
      case 33:
      case 34:
        if (num1 != 17)
        {
          if (num1 != 33)
          {
            if (num1 == 34)
              word1 = tran_type_str + " - tranTypeBioExt";
          }
          else
            word1 = tran_type_str + " - tranTypeBio1I64";
        }
        else
          word1 = tran_type_str + " - tranTypeBio1";
        switch (num2)
        {
          case 1:
            word2 = "1 - verify/enroll operation failed, no card data available, no template data available";
            break;
          case 2:
            word2 = "2 - verify/enroll operation completed, no card data available, have template data";
            break;
          case 3:
            word2 = "3 - verify operation failed, have card data, no template data available";
            break;
          case 4:
            word2 = "4 - verify operation completed, did not pass, have card data, have template data";
            break;
          case 5:
            word2 = "5 - verify operation completed, passed, have card data, have template data";
            break;
          case 6:
            word2 = "6 - enroll operation complete";
            break;
          case 7:
            word2 = "7 - enroll operation complete, failed to save template to database";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 18:
        word1 = tran_type_str + " - tranTypeUserCmndX";
        switch (num2)
        {
          case 1:
            word2 = "1 - Executed the command";
            break;
          case 2:
            word2 = "2 - rejected user command: command is not allowed at this ACR */";
            break;
          case 3:
            word2 = "3 - rejected user command: not authorized at this ACR */";
            break;
          case 4:
            word2 = "4 - rejected user command: invalid command parameter(s) */";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 19:
        word1 = tran_type_str + " - tranTypeUseLimit";
        word2 = num2 != 1 ? tran_code_str + " - UNKNOWN" : "1 - use limit changed, reporting new limit";
        break;
      case 20:
        word1 = tran_type_str + " - tranTypeWebActivity";
        switch (num2)
        {
          case 1:
            word2 = "1 - Save Home Notes";
            break;
          case 2:
            word2 = "2 - Save Network Settings";
            break;
          case 3:
            word2 = "3 - Save Host Comm";
            break;
          case 4:
            word2 = "4 - Add User";
            break;
          case 5:
            word2 = "5 - Delete User";
            break;
          case 6:
            word2 = "6 - Modify User";
            break;
          case 7:
            word2 = "7 - Save Password Strength and Session Timer";
            break;
          case 8:
            word2 = "8 - Save Web Server Options";
            break;
          case 9:
            word2 = "9 - Save Time Server Settings";
            break;
          case 10:
            word2 = "10 - Auto Save Timer Settings";
            break;
          case 11:
            word2 = "11 - Load Certificate";
            break;
          case 12:
            word2 = "12 - Logged Out By Link";
            break;
          case 13:
            word2 = "13 - Logged Out By Timeout";
            break;
          case 14:
            word2 = "14 - Logged Out By User";
            break;
          case 15:
            word2 = "15 - Logged Out By Apply";
            break;
          case 16 /*0x10*/:
            word2 = "16 - Invalid Login";
            break;
          case 17:
            word2 = "17 - Successful Login";
            break;
          case 18:
            word2 = "18 - Network Diagnostic Saved";
            break;
          case 19:
            word2 = "19 - Card DB Size Saved";
            break;
          case 20:
            word2 = "20 - Central Station Settings Saved";
            break;
          case 21:
            word2 = "21 - Diagnostic Page Saved";
            break;
          case 22:
            word2 = "22 - Security Options Saved";
            break;
          case 23:
            word2 = "23 - Add-On Page Saved";
            break;
          case 24:
            word2 = "24 - Load File";
            break;
          case 25:
            word2 = "25 - Delete File";
            break;
          case 26:
            word2 = "26 - Transfer File";
            break;
          case 27:
            word2 = "27 - Invalid Login Limit Reached";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 23:
        word1 = tran_type_str + " - tranTypeMpgIps";
        switch (num2)
        {
          case 1:
            word2 = "1 - DISARMED, all points normal (no zones in fault/trouble or active state)";
            break;
          case 2:
            word2 = "2 - DISARMED, some points in fault state (supervisory fault or point is off-line)";
            break;
          case 3:
            word2 = "3 - ARMED_AWAY";
            break;
          case 4:
            word2 = "4 - ARMED_STAY";
            break;
          case 5:
            word2 = "5 - ARMED_INSTANT";
            break;
          case 6:
            word2 = "6 - ARMED, entry delay is in progress";
            break;
          case 7:
            word2 = "7 - ARMING, exit delay is in progress";
            break;
          case 8:
            word2 = "8 - NEW_ALARM";
            break;
          case 9:
            word2 = "9 - ALARM_CANCELLED";
            break;
          case 10:
            word2 = "10 - PointSet";
            break;
          case 11:
            word2 = "11 - DISARMED, but some points active.";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 24:
        word1 = tran_type_str + " - tranTypeOperatingMode";
        switch (num2)
        {
          case 1:
            word2 = "1 - Operating Mode 0";
            break;
          case 2:
            word2 = "2 - Operating Mode 1";
            break;
          case 3:
            word2 = "3 - Operating Mode 2";
            break;
          case 4:
            word2 = "4 - Operating Mode 3";
            break;
          case 5:
            word2 = "5 - Operating Mode 4";
            break;
          case 6:
            word2 = "6 - Operating Mode 5";
            break;
          case 7:
            word2 = "7 - Operating Mode 6";
            break;
          case 8:
            word2 = "8 - Operating Mode 7";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 25:
        word1 = tran_type_str + " - tranTypeOal";
        switch (num2)
        {
          case 1:
            word2 = "1 - OAL Enabled";
            break;
          case 2:
            word2 = "2 - OAL Disabled";
            break;
          case 3:
            word2 = "3 - Credential Added";
            break;
          case 4:
            word2 = "4 - Credential Deleted";
            break;
          case 5:
            word2 = "5 - List Clear";
            break;
          case 6:
            word2 = "6 - Count for local verification";
            break;
          case 7:
            word2 = "7 - Count for offline access list";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 26:
        word1 = tran_type_str + " - tranTypeCoSElevator";
        switch (num2)
        {
          case 1:
            word2 = "1 - Floor Status is Secure";
            break;
          case 2:
            word2 = "2 - Floor Status is Public";
            break;
          case 3:
            word2 = "3 - Floor Status is Disabled";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 27:
        word1 = tran_type_str + " - tranTypeFileDownloadStatus";
        switch (num2)
        {
          case 1:
            word2 = "1 - File transfer complete";
            break;
          case 2:
            word2 = "2 - File transfer aborted/error";
            break;
          case 3:
            word2 = "3 - File successfully deleted";
            break;
          case 4:
            word2 = "4 - File Delete Error";
            break;
          case 5:
            word2 = "5 - OSDP Primary File Transfer Success";
            break;
          case 6:
            word2 = "6 - OSDP Primary File Transfer Error";
            break;
          case 7:
            word2 = "7 - OSDP Alt File Transfer Success";
            break;
          case 8:
            word2 = "8 - OSDP Alt File Transfer Error";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 29:
        word1 = tran_type_str + " - tranTypeCoSElevatorAccess";
        word2 = num2 != 1 ? tran_code_str + " - UNKNOWN" : "1 - Elevator Access";
        break;
      case 64 /*0x40*/:
        word1 = tran_type_str + " - tranTypeAcrExtFeatureStls";
        word2 = num2 != 1 ? tran_code_str + " - UNKNOWN" : "1 - Extended Status Updated";
        break;
      case 65:
        word1 = tran_type_str + " - tranTypeAcrExtFeatureCoS";
        switch (num2)
        {
          case 1:
            word2 = "1 - disconnected";
            break;
          case 2:
            word2 = "2 - unknown (nStatus bits: last known status)";
            break;
          case 3:
            word2 = "3 - secure";
            break;
          case 4:
            word2 = "4 - alarm";
            break;
          case 5:
            word2 = "5 - fault (fault type is encoded in nStatus byte)";
            break;
          default:
            word2 = tran_code_str + " - UNKNOWN";
            break;
        }
        break;
      case 126:
        word1 = tran_type_str + " - tranTypeAsci";
        word2 = tran_code_str;
        break;
      case (int) sbyte.MaxValue:
        word1 = tran_type_str + " - tranTypeSioDiag";
        word2 = tran_code_str;
        break;
      default:
        word1 = tran_type_str + " - UNKNOWN";
        word2 = tran_code_str + " - UNKNOWN";
        break;
    }
    this.InsertLine("Transaction Type", ": ", word1);
    this.InsertLine("Transaction Code", ": ", word2);
  }

  private void ParseEnCcScp(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enCcScp (0107)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified: ", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("ser_num_low", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("ser_num_high", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("rev_major", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("rev_minor", ": ", words6, index6);
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLineValidateRange("nMsp1Port", ": ", words7, index7, 0, 10);
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLine("nTransactions", ": ", words8, index8);
    string[] words9 = words;
    int index9 = num9;
    int num10 = index9 + 1;
    this.InsertLineValidateRange("nSio", ": ", words9, index9, 0, 96 /*0x60*/);
    string[] words10 = words;
    int index10 = num10;
    int num11 = index10 + 1;
    this.InsertLineValidateRange("nMp", ": ", words10, index10, 0, 2048 /*0x0800*/);
    string[] words11 = words;
    int index11 = num11;
    int num12 = index11 + 1;
    this.InsertLineValidateRange("nCp", ": ", words11, index11, 0, 2048 /*0x0800*/);
    string[] words12 = words;
    int index12 = num12;
    int num13 = index12 + 1;
    this.InsertLineValidateRange("nAcr", ": ", words12, index12, 0, 128 /*0x80*/);
    string[] words13 = words;
    int index13 = num13;
    int num14 = index13 + 1;
    this.InsertLineValidateRange("nAlvl", ": ", words13, index13, 0, 32000);
    string[] words14 = words;
    int index14 = num14;
    int num15 = index14 + 1;
    this.InsertLineValidateRange("nTrgr", ": ", words14, index14, 0, 8196);
    string[] words15 = words;
    int index15 = num15;
    int num16 = index15 + 1;
    this.InsertLineValidateRange("nProc", ": ", words15, index15, 0, 8196);
  }

  private void ParseEnCcScpScp(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    int num2;
    if (honeywell)
    {
      this.InsertLine("enNCcMcbMcb (1101)");
      string[] words1 = words;
      int index1 = num1;
      int num3 = index1 + 1;
      this.InsertLine("lastModified: ", ": ", words1, index1);
      string[] words2 = words;
      int index2 = num3;
      int num4 = index2 + 1;
      this.InsertLineValidateRange("number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
      string[] words3 = words;
      int index3 = num4;
      int num5 = index3 + 1;
      this.InsertLine("ser_num_low", ": ", words3, index3);
      string[] words4 = words;
      int index4 = num5;
      int num6 = index4 + 1;
      this.InsertLine("ser_num_high", ": ", words4, index4);
      string[] words5 = words;
      int index5 = num6;
      int num7 = index5 + 1;
      this.InsertLine("rev_major", ": ", words5, index5);
      string[] words6 = words;
      int index6 = num7;
      int num8 = index6 + 1;
      this.InsertLine("rev_minor", ": ", words6, index6);
      string[] words7 = words;
      int index7 = num8;
      int num9 = index7 + 1;
      this.InsertLine("gmt_offset", ": ", this.ParseGmtOffset(words7, index7));
      string[] words8 = words;
      int index8 = num9;
      int num10 = index8 + 1;
      this.InsertLine("nDstID", ": ", words8, index8);
      string[] words9 = words;
      int index9 = num10;
      int num11 = index9 + 1;
      this.InsertLine("nTransactions", ": ", words9, index9);
      string[] words10 = words;
      int index10 = num11;
      int num12 = index10 + 1;
      this.InsertLineValidateRange("nMsp1Port", ": ", words10, index10, 0, 10);
      string[] words11 = words;
      int index11 = num12;
      int num13 = index11 + 1;
      this.InsertLineValidateRange("nSio", ": ", words11, index11, 0, 96 /*0x60*/);
      string[] words12 = words;
      int index12 = num13;
      int num14 = index12 + 1;
      this.InsertLineValidateRange("nMp", ": ", words12, index12, 0, 2048 /*0x0800*/);
      string[] words13 = words;
      int index13 = num14;
      int num15 = index13 + 1;
      this.InsertLineValidateRange("nCp", ": ", words13, index13, 0, 2048 /*0x0800*/);
      string[] words14 = words;
      int index14 = num15;
      int num16 = index14 + 1;
      this.InsertLineValidateRange("nAcr", ": ", words14, index14, 0, 128 /*0x80*/);
      string[] words15 = words;
      int index15 = num16;
      int num17 = index15 + 1;
      this.InsertLineValidateRange("nAlvl", ": ", words15, index15, 0, 32000);
      string[] words16 = words;
      int index16 = num17;
      int num18 = index16 + 1;
      this.InsertLineValidateRange("nTrgr", ": ", words16, index16, 0, 8196);
      string[] words17 = words;
      int index17 = num18;
      int num19 = index17 + 1;
      this.InsertLineValidateRange("nProc", ": ", words17, index17, 0, 8196);
      string[] words18 = words;
      int index18 = num19;
      int num20 = index18 + 1;
      this.InsertLineValidateRange("nTz", ": ", words18, index18, 0, (int) byte.MaxValue);
      string[] words19 = words;
      int index19 = num20;
      int num21 = index19 + 1;
      this.InsertLineValidateRange("nHol", ": ", words19, index19, 0, (int) byte.MaxValue);
      string[] words20 = words;
      int index20 = num21;
      int num22 = index20 + 1;
      this.InsertLineValidateRange("nMpg", ": ", words20, index20, 0, 128 /*0x80*/);
      string[] words21 = words;
      int index21 = num22;
      int num23 = index21 + 1;
      this.InsertLine("nTranLimit", ": ", words21, index21);
      string[] words22 = words;
      int index22 = num23;
      int num24 = index22 + 1;
      this.InsertLine("nAuthModType", ": ", this.ParseAuthMode(words22, index22));
      string[] words23 = words;
      int index23 = num24;
      int num25 = index23 + 1;
      this.InsertLineValidateRange("nOperModes", ": ", words23, index23, 0, 8);
      string[] words24 = words;
      int index24 = num25;
      int num26 = index24 + 1;
      this.InsertLine("oper_type", ": ", this.ParseOperType(words24, index24));
      string[] words25 = words;
      int index25 = num26;
      int num27 = index25 + 1;
      this.InsertLine("nLanguages", ": ", words25, index25);
      string[] words26 = words;
      int index26 = num27;
      num2 = index26 + 1;
      this.InsertLine("nSrvcType", ": ", this.ParseServiceTypeFlags(words26, index26));
    }
    else
    {
      this.InsertLine("enCcScpScp (1107)");
      string[] words27 = words;
      int index27 = num1;
      int num28 = index27 + 1;
      this.InsertLine("lastModified: ", ": ", words27, index27);
      string[] words28 = words;
      int index28 = num28;
      int num29 = index28 + 1;
      this.InsertLineValidateRange("number", ": ", words28, index28, 0, 16383 /*0x3FFF*/);
      string[] words29 = words;
      int index29 = num29;
      int num30 = index29 + 1;
      this.InsertLine("ser_num_low", ": ", words29, index29);
      string[] words30 = words;
      int index30 = num30;
      int num31 = index30 + 1;
      this.InsertLine("ser_num_high", ": ", words30, index30);
      string[] words31 = words;
      int index31 = num31;
      int num32 = index31 + 1;
      this.InsertLine("rev_major", ": ", words31, index31);
      string[] words32 = words;
      int index32 = num32;
      int num33 = index32 + 1;
      this.InsertLine("rev_minor", ": ", words32, index32);
      string[] words33 = words;
      int index33 = num33;
      int num34 = index33 + 1;
      this.InsertLineValidateRange("nMsp1Port", ": ", words33, index33, 0, 10);
      string[] words34 = words;
      int index34 = num34;
      int num35 = index34 + 1;
      this.InsertLine("nTransactions", ": ", words34, index34);
      string[] words35 = words;
      int index35 = num35;
      int num36 = index35 + 1;
      this.InsertLineValidateRange("nSio", ": ", words35, index35, 0, 96 /*0x60*/);
      string[] words36 = words;
      int index36 = num36;
      int num37 = index36 + 1;
      this.InsertLineValidateRange("nMp", ": ", words36, index36, 0, 2048 /*0x0800*/);
      string[] words37 = words;
      int index37 = num37;
      int num38 = index37 + 1;
      this.InsertLineValidateRange("nCp", ": ", words37, index37, 0, 2048 /*0x0800*/);
      string[] words38 = words;
      int index38 = num38;
      int num39 = index38 + 1;
      this.InsertLineValidateRange("nAcr", ": ", words38, index38, 0, 128 /*0x80*/);
      string[] words39 = words;
      int index39 = num39;
      int num40 = index39 + 1;
      this.InsertLineValidateRange("nAlvl", ": ", words39, index39, 0, 32000);
      string[] words40 = words;
      int index40 = num40;
      int num41 = index40 + 1;
      this.InsertLineValidateRange("nTrgr", ": ", words40, index40, 0, 8196);
      string[] words41 = words;
      int index41 = num41;
      int num42 = index41 + 1;
      this.InsertLineValidateRange("nProc", ": ", words41, index41, 0, 8196);
      string[] words42 = words;
      int index42 = num42;
      int num43 = index42 + 1;
      this.InsertLine("gmt_offset", ": ", this.ParseGmtOffset(words42, index42));
      string[] words43 = words;
      int index43 = num43;
      int num44 = index43 + 1;
      this.InsertLine("nDstID", ": ", words43, index43);
      string[] words44 = words;
      int index44 = num44;
      int num45 = index44 + 1;
      this.InsertLineValidateRange("nTz", ": ", words44, index44, 0, (int) byte.MaxValue);
      string[] words45 = words;
      int index45 = num45;
      int num46 = index45 + 1;
      this.InsertLineValidateRange("nHol", ": ", words45, index45, 0, (int) byte.MaxValue);
      string[] words46 = words;
      int index46 = num46;
      int num47 = index46 + 1;
      this.InsertLineValidateRange("nMpg", ": ", words46, index46, 0, 128 /*0x80*/);
      string[] words47 = words;
      int index47 = num47;
      int num48 = index47 + 1;
      this.InsertLine("nTranLimit", ": ", words47, index47);
      string[] words48 = words;
      int index48 = num48;
      int num49 = index48 + 1;
      this.InsertLine("nAuthModType", ": ", this.ParseAuthMode(words48, index48));
      string[] words49 = words;
      int index49 = num49;
      int num50 = index49 + 1;
      this.InsertLineValidateRange("nOperModes", ": ", words49, index49, 0, 8);
      string[] words50 = words;
      int index50 = num50;
      int num51 = index50 + 1;
      this.InsertLine("oper_type", ": ", this.ParseOperType(words50, index50));
      string[] words51 = words;
      int index51 = num51;
      int num52 = index51 + 1;
      this.InsertLine("nLanguages", ": ", words51, index51);
      string[] words52 = words;
      int index52 = num52;
      num2 = index52 + 1;
      this.InsertLine("nSrvcType", ": ", this.ParseServiceTypeFlags(words52, index52));
    }
  }

  private void ParseEnCcTimezone(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enCcScpTimezone (0103)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified: ", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("number", ": ", words2, index2, 0, (int) byte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int index4 = index3 + 1;
    this.InsertLine("mode", ": ", this.ParseTimezoneMode(words3, index3));
    int num4 = int.Parse(words[index4]);
    string[] words4 = words;
    int index5 = index4;
    int num5 = index5 + 1;
    this.InsertLineValidateRange("intervals", ": ", words4, index5, 0, 12);
    if (num4 > 12)
    {
      this.InsertLine("Invalid number of intervals");
      num4 = 12;
    }
    for (int index6 = 0; index6 < num4; ++index6)
    {
      this.InsertLine("");
      this.InsertLine("Interval: " + (object) (index6 + 1));
      this.InsertLine("i_days", ": ", this.ParseDayMask(words, num5 + index6 * 3));
      this.InsertLine("i_start", ": ", this.ParseStartEndTime(words, num5 + 1 + index6 * 3));
      this.InsertLine("i_end", ": ", this.ParseStartEndTime(words, num5 + 2 + index6 * 3));
    }
  }

  private void ParseEnCcScpTimezone(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpTimezone (1107)");
    else
      this.InsertLine("enCcScpTimezone (1103)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified: ", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("number", ": ", words3, index3, 0, (int) byte.MaxValue);
    string[] words4 = words;
    int index4 = num4;
    int index5 = index4 + 1;
    this.InsertLine("mode", ": ", this.ParseTimezoneMode(words4, index4));
    int num5 = int.Parse(words[index5]);
    string[] words5 = words;
    int index6 = index5;
    int num6 = index6 + 1;
    this.InsertLineValidateRange("intervals", ": ", words5, index6, 0, 12);
    if (num5 > 12)
    {
      this.InsertLine("Invalid number of intervals");
      num5 = 12;
    }
    for (int index7 = 0; index7 < num5; ++index7)
    {
      this.InsertLine("");
      this.InsertLine("Interval: " + (object) (index7 + 1));
      this.InsertLine("i_days", ": ", this.ParseDayMask(words, num6 + index7 * 3));
      this.InsertLine("i_start", ": ", this.ParseStartEndTime(words, num6 + 1 + index7 * 3));
      this.InsertLine("i_end", ": ", this.ParseStartEndTime(words, num6 + 2 + index7 * 3));
    }
  }

  private void ParseEnCcScpTimezoneEx(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpTimezoneEx (2107)");
    else
      this.InsertLine("enCcScpTimezoneEx (2103)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified: ", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("number", ": ", words3, index3, 0, (int) byte.MaxValue);
    string[] words4 = words;
    int index4 = num4;
    int index5 = index4 + 1;
    this.InsertLine("mode", ": ", this.ParseTimezoneMode(words4, index4));
    int num5 = int.Parse(words[index5]);
    string[] words5 = words;
    int index6 = index5;
    int index7 = index6 + 1;
    this.InsertLineValidateRange("intervals", ": ", words5, index6, 0, 12);
    if (num5 > 12)
    {
      this.InsertLine("Invalid number of intervals");
      num5 = 12;
    }
    for (int index8 = 0; index8 < num5; ++index8)
    {
      this.InsertLine("");
      this.InsertLine("Interval: " + (object) (index8 + 1));
      this.InsertLine("i_days", ": ", this.ParseDayMask(words, index7));
      this.InsertLine("i_start", ": ", this.ParseStartEndTime(words, index7 + 1));
      this.InsertLine("i_end", ": ", this.ParseStartEndTime(words, index7 + 2));
      index7 += 3;
    }
    this.InsertLine("");
    string[] words6 = words;
    int index9 = index7;
    int num6 = index9 + 1;
    this.InsertLine("expTest", ": ", words6, index9);
  }

  private void ParseEnCcScpTimezoneExAct(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpTimezoneEx (3107)");
    else
      this.InsertLine("enCcScpTimezoneEx (3103)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified: ", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("number", ": ", words3, index3, 0, (int) byte.MaxValue);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("mode", ": ", this.ParseTimezoneMode(words4, index4));
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("actTime", ": ", this.ParseTime(words5, index5, 1970));
    string[] words6 = words;
    int index6 = num6;
    int index7 = index6 + 1;
    this.InsertLine("deactTime", ": ", this.ParseTime(words6, index6, 1970));
    int num7 = int.Parse(words[index7]);
    string[] words7 = words;
    int index8 = index7;
    int index9 = index8 + 1;
    this.InsertLineValidateRange("intervals", ": ", words7, index8, 0, 12);
    if (num7 > 12)
    {
      this.InsertLine("Invalid number of intervals");
      num7 = 12;
    }
    for (int index10 = 0; index10 < num7; ++index10)
    {
      this.InsertLine("");
      this.InsertLine("Interval: " + (object) (index10 + 1));
      this.InsertLine("i_days", ": ", this.ParseDayMask(words, index9));
      this.InsertLine("i_start", ": ", this.ParseStartEndTime(words, index9 + 1));
      this.InsertLine("i_end", ": ", this.ParseStartEndTime(words, index9 + 2));
      index9 += 3;
    }
    this.InsertLine("");
    string[] words8 = words;
    int index11 = index9;
    int num8 = index11 + 1;
    this.InsertLine("expTest", ": ", words8, index11);
  }

  private void ParseEnCcSio(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    int num2;
    if (honeywell)
    {
      this.InsertLine("enNCcSio (1202)");
      string[] words1 = words;
      int index1 = num1;
      int num3 = index1 + 1;
      this.InsertLine("lastModified: ", ": ", words1, index1);
      string[] words2 = words;
      int index2 = num3;
      int num4 = index2 + 1;
      this.InsertLineValidateRange("mcb_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
      string[] words3 = words;
      int index3 = num4;
      int num5 = index3 + 1;
      this.InsertLineValidateRange("sio_number", ": ", words3, index3, 0, 95);
      string[] words4 = words;
      int index4 = num5;
      int num6 = index4 + 1;
      this.InsertLine("model", ": ", this.ParseSioModel(words4, index4, false));
      string[] words5 = words;
      int index5 = num6;
      int num7 = index5 + 1;
      this.InsertLine("revision", ": ", words5, index5);
      string[] words6 = words;
      int index6 = num7;
      int num8 = index6 + 1;
      this.InsertLine("ser_num_low", ": ", words6, index6);
      string[] words7 = words;
      int index7 = num8;
      int num9 = index7 + 1;
      this.InsertLine("ser_num_high", ": ", words7, index7);
      string[] words8 = words;
      int index8 = num9;
      int num10 = index8 + 1;
      this.InsertLine("nInputs", ": ", words8, index8);
      string[] words9 = words;
      int index9 = num10;
      int num11 = index9 + 1;
      this.InsertLine("nOutputs", ": ", words9, index9);
      string[] words10 = words;
      int index10 = num11;
      int num12 = index10 + 1;
      this.InsertLine("nReaders", ": ", words10, index10);
      string[] words11 = words;
      int index11 = num12;
      int num13 = index11 + 1;
      this.InsertLine("port", ": ", words11, index11);
      string[] words12 = words;
      int index12 = num13;
      int num14 = index12 + 1;
      this.InsertLine("channel_out", ": ", words12, index12);
      string[] words13 = words;
      int index13 = num14;
      int num15 = index13 + 1;
      this.InsertLine("channel_in", ": ", words13, index13);
      string[] words14 = words;
      int index14 = num15;
      int num16 = index14 + 1;
      this.InsertLineValidateRange("address", ": ", words14, index14, 0, 31 /*0x1F*/);
      string[] words15 = words;
      int index15 = num16;
      int num17 = index15 + 1;
      this.InsertLine("e_max", ": ", words15, index15);
      string[] words16 = words;
      int index16 = num17;
      int num18 = index16 + 1;
      this.InsertLine("enable", ": ", this.ParseSioEnable(words16, index16));
      string[] words17 = words;
      int index17 = num18;
      int num19 = index17 + 1;
      this.InsertLine("flags", ": ", this.ParseSioFlags(words17, index17));
      string[] words18 = words;
      int index18 = num19;
      int num20 = index18 + 1;
      this.InsertLineValidateRange("nSioNextIn", ": ", words18, index18, -1, 95);
      string[] words19 = words;
      int index19 = num20;
      int num21 = index19 + 1;
      this.InsertLineValidateRange("nSioNextOut", ": ", words19, index19, -1, 95);
      string[] words20 = words;
      int index20 = num21;
      int num22 = index20 + 1;
      this.InsertLineValidateRange("nSioNextRdr", ": ", words20, index20, -1, 95);
      string[] words21 = words;
      int index21 = num22;
      int num23 = index21 + 1;
      this.InsertLine("nSioConnectTest", ": ", this.ParseSioConnectTest(words21, index21));
      string[] words22 = words;
      int index22 = num23;
      int num24 = index22 + 1;
      this.InsertLine("nSioOemCode", ": ", this.ParseOEMCodes(words22, index22));
      string[] words23 = words;
      int index23 = num24;
      num2 = index23 + 1;
      this.InsertLine("nSioOemMask", ": ", words23, index23);
    }
    else
    {
      this.InsertLine("enCcSio (0109)");
      string[] words24 = words;
      int index24 = num1;
      int num25 = index24 + 1;
      this.InsertLine("lastModified: ", ": ", words24, index24);
      string[] words25 = words;
      int index25 = num25;
      int num26 = index25 + 1;
      this.InsertLineValidateRange("scp_number", ": ", words25, index25, 0, 16383 /*0x3FFF*/);
      string[] words26 = words;
      int index26 = num26;
      int num27 = index26 + 1;
      this.InsertLineValidateRange("sio_number", ": ", words26, index26, 0, 95);
      string[] words27 = words;
      int index27 = num27;
      int num28 = index27 + 1;
      this.InsertLine("nInputs", ": ", words27, index27);
      string[] words28 = words;
      int index28 = num28;
      int num29 = index28 + 1;
      this.InsertLine("nOutputs", ": ", words28, index28);
      string[] words29 = words;
      int index29 = num29;
      int num30 = index29 + 1;
      this.InsertLine("nReaders", ": ", words29, index29);
      string[] words30 = words;
      int index30 = num30;
      int num31 = index30 + 1;
      this.InsertLine("model", ": ", this.ParseSioModel(words30, index30, false));
      string[] words31 = words;
      int index31 = num31;
      int num32 = index31 + 1;
      this.InsertLine("revision", ": ", words31, index31);
      string[] words32 = words;
      int index32 = num32;
      int num33 = index32 + 1;
      this.InsertLine("ser_num_low", ": ", words32, index32);
      string[] words33 = words;
      int index33 = num33;
      int num34 = index33 + 1;
      this.InsertLine("ser_num_high", ": ", words33, index33);
      string[] words34 = words;
      int index34 = num34;
      int num35 = index34 + 1;
      this.InsertLine("enable", ": ", this.ParseSioEnable(words34, index34));
      string[] words35 = words;
      int index35 = num35;
      int num36 = index35 + 1;
      this.InsertLine("port", ": ", words35, index35);
      string[] words36 = words;
      int index36 = num36;
      int num37 = index36 + 1;
      this.InsertLine("channel_out", ": ", words36, index36);
      string[] words37 = words;
      int index37 = num37;
      int num38 = index37 + 1;
      this.InsertLine("channel_in", ": ", words37, index37);
      string[] words38 = words;
      int index38 = num38;
      int num39 = index38 + 1;
      this.InsertLineValidateRange("address", ": ", words38, index38, 0, 31 /*0x1F*/);
      string[] words39 = words;
      int index39 = num39;
      int num40 = index39 + 1;
      this.InsertLine("e_max", ": ", words39, index39);
      string[] words40 = words;
      int index40 = num40;
      int num41 = index40 + 1;
      this.InsertLine("flags", ": ", this.ParseSioFlags(words40, index40));
      string[] words41 = words;
      int index41 = num41;
      int num42 = index41 + 1;
      this.InsertLineValidateRange("nSioNextIn", ": ", words41, index41, -1, 95);
      string[] words42 = words;
      int index42 = num42;
      int num43 = index42 + 1;
      this.InsertLineValidateRange("nSioNextOut", ": ", words42, index42, -1, 95);
      string[] words43 = words;
      int index43 = num43;
      int num44 = index43 + 1;
      this.InsertLineValidateRange("nSioNextRdr", ": ", words43, index43, -1, 95);
      string[] words44 = words;
      int index44 = num44;
      int num45 = index44 + 1;
      this.InsertLine("nSioConnectTest", ": ", this.ParseSioConnectTest(words44, index44));
      string[] words45 = words;
      int index45 = num45;
      int num46 = index45 + 1;
      this.InsertLine("nSioOemCode", ": ", this.ParseOEMCodes(words45, index45));
      string[] words46 = words;
      int index46 = num46;
      num2 = index46 + 1;
      this.InsertLine("nSioOemMask", ": ", words46, index46);
    }
  }

  private void ParseEnCcACR(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcACR (1213)");
    else
      this.InsertLine("enCcACR (0115)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int index4 = index3 + 1;
    this.InsertLineValidateRange("acr_number", ": ", words3, index3, 0, (int) sbyte.MaxValue);
    int num4 = int.Parse(words[index4]);
    string[] words4 = words;
    int index5 = index4;
    int num5 = index5 + 1;
    this.InsertLine("access_cfg", ": ", this.ParseAccessCfg(words4, index5));
    int num6;
    if (num4 == 4 || num4 == 5)
    {
      string[] words5 = words;
      int index6 = num5;
      num6 = index6 + 1;
      this.InsertLine("pair_acr_number (standard override)", ": ", words5, index6);
    }
    else
    {
      string[] words6 = words;
      int index7 = num5;
      num6 = index7 + 1;
      this.InsertLine("pair_acr_number", ": ", words6, index7);
    }
    string[] words7 = words;
    int index8 = num6;
    int num7 = index8 + 1;
    this.InsertLineValidateRange("rdr_sio", ": ", words7, index8, -1, 95);
    string[] words8 = words;
    int index9 = num7;
    int num8 = index9 + 1;
    this.InsertLineValidateRange("rdr_number", ": ", words8, index9, -1, 15);
    int num9;
    if (num4 == 4 || num4 == 5)
    {
      string[] words9 = words;
      int index10 = num8;
      num9 = index10 + 1;
      this.InsertLine("strk_sio (SIO of first output)", ": ", words9, index10);
    }
    else
    {
      string[] words10 = words;
      int index11 = num8;
      num9 = index11 + 1;
      this.InsertLineValidateRange("strk_sio", ": ", words10, index11, -1, 95);
    }
    string[] words11 = words;
    int index12 = num9;
    int num10 = index12 + 1;
    this.InsertLine("strk_number", ": ", words11, index12);
    string[] words12 = words;
    int index13 = num10;
    int num11 = index13 + 1;
    this.InsertLineValidateRange("strike_t_min", ": ", words12, index13, 0, (int) byte.MaxValue);
    string[] words13 = words;
    int index14 = num11;
    int num12 = index14 + 1;
    this.InsertLineValidateRange("strike_t_max", ": ", words13, index14, 0, (int) byte.MaxValue);
    int num13;
    if (num4 == 4 || num4 == 5)
    {
      string[] words14 = words;
      int index15 = num12;
      num13 = index15 + 1;
      this.InsertLine("strike_mode (# of floors)", ": ", words14, index15);
    }
    else
    {
      string[] words15 = words;
      int index16 = num12;
      num13 = index16 + 1;
      this.InsertLine("strike_mode", ": ", this.ParseStrikeMode(words15, index16));
    }
    int num14;
    if (honeywell)
    {
      string[] words16 = words;
      int index17 = num13;
      int num15 = index17 + 1;
      this.InsertLineValidateRange("strk_t2", ": ", words16, index17, 0, (int) byte.MaxValue);
      int num16;
      if (num4 == 5)
      {
        string[] words17 = words;
        int index18 = num15;
        num16 = index18 + 1;
        this.InsertLine("door_sio (SIO of first input)", ": ", words17, index18);
      }
      else
      {
        string[] words18 = words;
        int index19 = num15;
        num16 = index19 + 1;
        this.InsertLineValidateRange("door_sio", ": ", words18, index19, -1, 95);
      }
      string[] words19 = words;
      int index20 = num16;
      int num17 = index20 + 1;
      this.InsertLine("door_number", ": ", words19, index20);
      string[] words20 = words;
      int index21 = num17;
      int num18 = index21 + 1;
      this.InsertLineValidateRange("dc_held", ": ", words20, index21, 0, (int) short.MaxValue);
      string[] words21 = words;
      int index22 = num18;
      int num19 = index22 + 1;
      this.InsertLineValidateRange("dc_held2", ": ", words21, index22, 0, (int) short.MaxValue);
      string[] words22 = words;
      int index23 = num19;
      int num20 = index23 + 1;
      this.InsertLine("pre_alarm", ": ", words22, index23);
      string[] words23 = words;
      int index24 = num20;
      int num21 = index24 + 1;
      this.InsertLineValidateRange("rex0_sio", ": ", words23, index24, -1, 95);
      string[] words24 = words;
      int index25 = num21;
      int num22 = index25 + 1;
      this.InsertLine("rex0_number", ": ", words24, index25);
      string[] words25 = words;
      int index26 = num22;
      int num23 = index26 + 1;
      this.InsertLineValidateRange("rex1_sio", ": ", words25, index26, -1, 95);
      string[] words26 = words;
      int index27 = num23;
      int index28 = index27 + 1;
      this.InsertLine("rex1_number", ": ", words26, index27);
      int num24;
      if (num4 == 4 || num4 == 5)
      {
        string[] words27 = words;
        int index29 = index28;
        int num25 = index29 + 1;
        this.InsertLine("rex_tzmask[0] (Offline EAL)", ": ", words27, index29);
        string[] words28 = words;
        int index30 = num25;
        num24 = index30 + 1;
        this.InsertLine("rex_tzmask[1] (F/C EAL)", ": ", words28, index30);
      }
      else
      {
        this.InsertLine("rex_tzmask[2]", ": ", words, index28, 2);
        num24 = index28 + 2;
      }
      string[] words29 = words;
      int index31 = num24;
      int num26 = index31 + 1;
      this.InsertLineValidateRange("altrdr_sio", ": ", words29, index31, -1, 95);
      string[] words30 = words;
      int index32 = num26;
      int num27 = index32 + 1;
      this.InsertLineValidateRange("altrdr_number", ": ", words30, index32, -1, 15);
      string[] words31 = words;
      int index33 = num27;
      int num28 = index33 + 1;
      this.InsertLine("altrdr_spec", ": ", this.ParseAltRdrSpec(words31, index33));
      string[] words32 = words;
      int index34 = num28;
      int num29 = index34 + 1;
      this.InsertLine("cd_format", ": ", words32, index34);
      string[] words33 = words;
      int index35 = num29;
      int num30 = index35 + 1;
      this.InsertLine("apb_mode", ": ", this.ParseApbMode(words33, index35));
      string[] words34 = words;
      int index36 = num30;
      int num31 = index36 + 1;
      this.InsertLineValidateRange("apb_in", ": ", words34, index36, -1, (int) sbyte.MaxValue);
      string[] words35 = words;
      int index37 = num31;
      int num32 = index37 + 1;
      this.InsertLineValidateRange("apb_to", ": ", words35, index37, -1, (int) sbyte.MaxValue);
      string[] words36 = words;
      int index38 = num32;
      int num33 = index38 + 1;
      this.InsertLineValidateRange("apb_delay", ": ", words36, index38, 0, (int) ushort.MaxValue);
      string[] words37 = words;
      int index39 = num33;
      int num34 = index39 + 1;
      this.InsertLine("actl_flags", ": ", this.ParseActlFlags(words37, index39));
      string[] words38 = words;
      int index40 = num34;
      int num35 = index40 + 1;
      this.InsertLine("offline_mode", ": ", this.ParseAcrMode(words38, index40));
      string[] words39 = words;
      int index41 = num35;
      int num36 = index41 + 1;
      this.InsertLine("default_mode", ": ", this.ParseAcrMode(words39, index41));
      string[] words40 = words;
      int index42 = num36;
      int num37 = index42 + 1;
      this.InsertLineValidateRange("default_led_mode", ": ", words40, index42, 0, 3);
      string[] words41 = words;
      int index43 = num37;
      num14 = index43 + 1;
      this.InsertLine("spare (extended actl_flags)", ": ", this.ParseExtActlFlags(words41, index43));
    }
    else
    {
      int num38;
      if (num4 == 5)
      {
        string[] words42 = words;
        int index44 = num13;
        num38 = index44 + 1;
        this.InsertLine("door_sio (SIO of first input)", ": ", words42, index44);
      }
      else
      {
        string[] words43 = words;
        int index45 = num13;
        num38 = index45 + 1;
        this.InsertLineValidateRange("door_sio", ": ", words43, index45, -1, 95);
      }
      string[] words44 = words;
      int index46 = num38;
      int num39 = index46 + 1;
      this.InsertLine("door_number", ": ", words44, index46);
      string[] words45 = words;
      int index47 = num39;
      int num40 = index47 + 1;
      this.InsertLineValidateRange("dc_held", ": ", words45, index47, 0, (int) short.MaxValue);
      string[] words46 = words;
      int index48 = num40;
      int num41 = index48 + 1;
      this.InsertLineValidateRange("rex0_sio", ": ", words46, index48, -1, 95);
      string[] words47 = words;
      int index49 = num41;
      int num42 = index49 + 1;
      this.InsertLine("rex0_number", ": ", words47, index49);
      string[] words48 = words;
      int index50 = num42;
      int num43 = index50 + 1;
      this.InsertLineValidateRange("rex1_sio", ": ", words48, index50, -1, 95);
      string[] words49 = words;
      int index51 = num43;
      int index52 = index51 + 1;
      this.InsertLine("rex1_number", ": ", words49, index51);
      int num44;
      if (num4 == 4 || num4 == 5)
      {
        string[] words50 = words;
        int index53 = index52;
        int num45 = index53 + 1;
        this.InsertLine("rex_tzmask[0] (Offline EAL)", ": ", words50, index53);
        string[] words51 = words;
        int index54 = num45;
        num44 = index54 + 1;
        this.InsertLine("rex_tzmask[1] (F/C EAL)", ": ", words51, index54);
      }
      else
      {
        this.InsertLine("rex_tzmask[2]", ": ", words, index52, 2);
        num44 = index52 + 2;
      }
      string[] words52 = words;
      int index55 = num44;
      int num46 = index55 + 1;
      this.InsertLineValidateRange("altrdr_sio", ": ", words52, index55, -1, 95);
      string[] words53 = words;
      int index56 = num46;
      int num47 = index56 + 1;
      this.InsertLineValidateRange("altrdr_number", ": ", words53, index56, -1, 15);
      string[] words54 = words;
      int index57 = num47;
      int num48 = index57 + 1;
      this.InsertLine("altrdr_spec", ": ", this.ParseAltRdrSpec(words54, index57));
      string[] words55 = words;
      int index58 = num48;
      int num49 = index58 + 1;
      this.InsertLine("cd_format", ": ", this.ParseCardFormatBits(words55, index58));
      string[] words56 = words;
      int index59 = num49;
      int num50 = index59 + 1;
      this.InsertLine("apb_mode", ": ", this.ParseApbMode(words56, index59));
      string[] words57 = words;
      int index60 = num50;
      int num51 = index60 + 1;
      this.InsertLineValidateRange("apb_in", ": ", words57, index60, -1, (int) sbyte.MaxValue);
      string[] words58 = words;
      int index61 = num51;
      int num52 = index61 + 1;
      this.InsertLineValidateRange("apb_to", ": ", words58, index61, -1, (int) sbyte.MaxValue);
      string[] words59 = words;
      int index62 = num52;
      int num53 = index62 + 1;
      this.InsertLine("spare (extended actl_flags)", ": ", this.ParseExtActlFlags(words59, index62));
      string[] words60 = words;
      int index63 = num53;
      int num54 = index63 + 1;
      this.InsertLine("actl_flags", ": ", this.ParseActlFlags(words60, index63));
      string[] words61 = words;
      int index64 = num54;
      int num55 = index64 + 1;
      this.InsertLine("offline_mode", ": ", this.ParseAcrMode(words61, index64));
      string[] words62 = words;
      int index65 = num55;
      int num56 = index65 + 1;
      this.InsertLine("default_mode", ": ", this.ParseAcrMode(words62, index65));
      string[] words63 = words;
      int index66 = num56;
      int num57 = index66 + 1;
      this.InsertLineValidateRange("default_led_mode", ": ", words63, index66, 0, 3);
      string[] words64 = words;
      int index67 = num57;
      int num58 = index67 + 1;
      this.InsertLine("pre_alarm", ": ", words64, index67);
      string[] words65 = words;
      int index68 = num58;
      int num59 = index68 + 1;
      this.InsertLineValidateRange("apb_delay", ": ", words65, index68, 0, (int) ushort.MaxValue);
      string[] words66 = words;
      int index69 = num59;
      int num60 = index69 + 1;
      this.InsertLineValidateRange("strk_t2", ": ", words66, index69, 0, (int) byte.MaxValue);
      string[] words67 = words;
      int index70 = num60;
      num14 = index70 + 1;
      this.InsertLineValidateRange("dc_held2", ": ", words67, index70, 0, (int) short.MaxValue);
    }
    string[] words68 = words;
    int index71 = num14;
    int num61 = index71 + 1;
    this.InsertLine("strk_follow_pulse", ": ", words68, index71);
    string[] words69 = words;
    int index72 = num61;
    int num62 = index72 + 1;
    this.InsertLineValidateRange("strk_follow_delay", ": ", words69, index72, 0, 63 /*0x3F*/);
    string[] words70 = words;
    int index73 = num62;
    int index74 = index73 + 1;
    this.InsertLine("nAuthModFlags", ": ", words70, index73);
    int num63 = 0;
    if (index74 < ((IEnumerable<string>) words).Count<string>())
      num63 = (int) short.Parse(words[index74]);
    int num64;
    if (((IEnumerable<string>) words).Count<string>() > index74)
    {
      string[] words71 = words;
      int index75 = index74;
      int num65 = index75 + 1;
      this.InsertLine("nExtFeatureType", ": ", this.ParseExtFeatureType(words71, index75));
      switch (num63)
      {
        case 1:
        case 2:
        case 3:
        case 4:
          string[] words72 = words;
          int index76 = num65;
          int num66 = index76 + 1;
          this.InsertLine("iIPB_sio", ": ", words72, index76);
          string[] words73 = words;
          int index77 = num66;
          int num67 = index77 + 1;
          this.InsertLine("iIPB_number", ": ", words73, index77);
          string[] words74 = words;
          int index78 = num67;
          int num68 = index78 + 1;
          this.InsertLine("iIPB_long_press", ": ", words74, index78);
          string[] words75 = words;
          int index79 = num68;
          int num69 = index79 + 1;
          this.InsertLine("iIPB_out_sio", ": ", words75, index79);
          string[] words76 = words;
          int index80 = num69;
          num65 = index80 + 1;
          this.InsertLine("iIPB_out_num", ": ", words76, index80);
          break;
        case 5:
        case 6:
        case 7:
        case 8:
          string[] words77 = words;
          int index81 = num65;
          int num70 = index81 + 1;
          this.InsertLine("ip_octet1", ": ", words77, index81);
          string[] words78 = words;
          int index82 = num70;
          int num71 = index82 + 1;
          this.InsertLine("ip_octet2", ": ", words78, index82);
          string[] words79 = words;
          int index83 = num71;
          int num72 = index83 + 1;
          this.InsertLine("ip_octet3", ": ", words79, index83);
          string[] words80 = words;
          int index84 = num72;
          num65 = index84 + 1;
          this.InsertLine("ip_octet4", ": ", words80, index84);
          break;
        case 9:
          string[] words81 = words;
          int index85 = num65;
          int num73 = index85 + 1;
          this.InsertLine("op_type", ": ", this.ParseKoneOpType(words81, index85));
          string[] words82 = words;
          int index86 = num73;
          int num74 = index86 + 1;
          this.InsertLine("op_id", ": ", words82, index86);
          string[] words83 = words;
          int index87 = num74;
          int num75 = index87 + 1;
          this.InsertLine("op_num", ": ", words83, index87);
          string[] words84 = words;
          int index88 = num75;
          int num76 = index88 + 1;
          this.InsertLine("op_calltypes", ": ", words84, index88);
          string[] words85 = words;
          int index89 = num76;
          num65 = index89 + 1;
          this.InsertLine("op_timeout", ": ", words85, index89);
          break;
        case 10:
          string[] words86 = words;
          int index90 = num65;
          int num77 = index90 + 1;
          this.InsertLine("es_group_id", ": ", words86, index90);
          string[] words87 = words;
          int index91 = num77;
          int num78 = index91 + 1;
          this.InsertLine("floor_id", ": ", words87, index91);
          string[] words88 = words;
          int index92 = num78;
          int num79 = index92 + 1;
          this.InsertLine("ob_id", ": ", words88, index92);
          string[] words89 = words;
          int index93 = num79;
          int num80 = index93 + 1;
          this.InsertLine("flags", ": ", this.ParseTKEFlags(words89, index93));
          string[] words90 = words;
          int index94 = num80;
          num65 = index94 + 1;
          this.InsertLine("dedMode", ": ", this.ParseTKEDedModeFlags(words90, index94));
          break;
        case 11:
          string[] words91 = words;
          int index95 = num65;
          int num81 = index95 + 1;
          this.InsertLine("bSeacDevNum", ": ", words91, index95);
          string[] words92 = words;
          int index96 = num81;
          int num82 = index96 + 1;
          this.InsertLine("bOpdcDevNum", ": ", words92, index96);
          string[] words93 = words;
          int index97 = num82;
          int num83 = index97 + 1;
          this.InsertLine("bFloorOffset", ": ", words93, index97);
          string[] words94 = words;
          int index98 = num83;
          int num84 = index98 + 1;
          this.InsertLine("bVerificationLoc", ": ", this.ParseMitsuVeriLoc(words94, index98));
          string[] words95 = words;
          int index99 = num84;
          int num85 = index99 + 1;
          this.InsertLine("bBoardingFrontRear", ": ", this.ParseMitsuBoardingEntrance(words95, index99));
          string[] words96 = words;
          int index100 = num85;
          int num86 = index100 + 1;
          this.InsertLine("wBoardingFloor", ": ", words96, index100);
          string[] words97 = words;
          int index101 = num86;
          num65 = index101 + 1;
          this.InsertLine("wCardReaderNum", ": ", words97, index101);
          break;
      }
      string[] words98 = words;
      int index102 = num65;
      num64 = index102 + 1;
      this.InsertLine("dfofFilterTime", ": ", words98, index102);
    }
    else
    {
      string[] words99 = words;
      int index103 = index74;
      num64 = index103 + 1;
      this.InsertLine("dfofFilterTime", ": ", words99, index103);
    }
  }

  private void ParseEnCcInput(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcInput (1203)");
    else
      this.InsertLine("enCcInput (0110)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("sio_number", ": ", words3, index3, 0, 95);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("input", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("icvt_num", ": ", this.ParseIcvtNum(words5, index5));
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLineValidateRange("debounce", ": ", words6, index6, 0, 15);
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("hold_time", ": ", words7, index7);
  }

  private void ParseEnCcMP(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcMP (1211)");
    else
      this.InsertLine("enCcMP (0113)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("mp_number", ": ", words3, index3, 0, 2047 /*0x07FF*/);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLineValidateRange("sio_number", ": ", words4, index4, 0, 95);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("ip_number", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("lf_code", ": ", this.ParseLogFunction(words6, index6));
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("mode", ": ", this.ParseMpMode(words7, index7));
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLineValidateRange("delay_entry", ": ", words8, index8, 0, (int) ushort.MaxValue);
    string[] words9 = words;
    int index9 = num9;
    int num10 = index9 + 1;
    this.InsertLineValidateRange("delay_exit", ": ", words9, index9, 0, (int) ushort.MaxValue);
  }

  private void ParseEnCcOutput(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcOutput (1204)");
    else
      this.InsertLine("enCcOutput (0111)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("sio_number", ": ", words3, index3, 0, 95);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("output", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("mode", ": ", this.ParseOutputDriveMode(words5, index5));
  }

  private void ParseEnCcCP(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcCP (1212)");
    else
      this.InsertLine("enCcCP (0114)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("cp_number", ": ", words3, index3, 0, 2047 /*0x07FF*/);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLineValidateRange("sio_number", ": ", words4, index4, 0, 95);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("op_number", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("dflt_pulse", ": ", words6, index6);
  }

  private void ParseEnCcSioCommand(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcSioCommand (1901)");
    else
      this.InsertLine("enCcSioCommand (0601)");
    string[] words1 = words;
    int index1 = num1;
    int index2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    int num2 = int.Parse(words[index2]);
    string[] words2 = words;
    int index3 = index2;
    int index4 = index3 + 1;
    this.InsertLine("nSioCommand", ": ", this.ParseSioCommand(words2, index3));
    int num3;
    switch (num2)
    {
      case 102:
        string[] words3 = words;
        int index5 = index4;
        num3 = index5 + 1;
        this.InsertLine("reply_delay", ": ", words3, index5);
        break;
      case 103:
        string[] words4 = words;
        int index6 = index4;
        int num4 = index6 + 1;
        this.InsertLine("nTable", ": ", words4, index6);
        string[] words5 = words;
        int index7 = num4;
        int num5 = index7 + 1;
        this.InsertLine("pri_5pr", ": ", words5, index7);
        string[] words6 = words;
        int index8 = num5;
        int num6 = index8 + 1;
        this.InsertLine("sts_5pr", ": ", words6, index8);
        this.InsertLine("");
        string[] words7 = words;
        int index9 = num6;
        int num7 = index9 + 1;
        this.InsertLine("priority", ": ", words7, index9);
        string[] words8 = words;
        int index10 = num7;
        int num8 = index10 + 1;
        this.InsertLine("status_code", ": ", words8, index10);
        string[] words9 = words;
        int index11 = num8;
        int num9 = index11 + 1;
        this.InsertLine("res_code_1", ": ", words9, index11);
        string[] words10 = words;
        int index12 = num9;
        int num10 = index12 + 1;
        this.InsertLine("res_code_2", ": ", words10, index12);
        this.InsertLine("");
        string[] words11 = words;
        int index13 = num10;
        int num11 = index13 + 1;
        this.InsertLine("priority", ": ", words11, index13);
        string[] words12 = words;
        int index14 = num11;
        int num12 = index14 + 1;
        this.InsertLine("status_code", ": ", words12, index14);
        string[] words13 = words;
        int index15 = num12;
        int num13 = index15 + 1;
        this.InsertLine("res_code_1", ": ", words13, index15);
        string[] words14 = words;
        int index16 = num13;
        int num14 = index16 + 1;
        this.InsertLine("res_code_2", ": ", words14, index16);
        this.InsertLine("");
        string[] words15 = words;
        int index17 = num14;
        int num15 = index17 + 1;
        this.InsertLine("priority", ": ", words15, index17);
        string[] words16 = words;
        int index18 = num15;
        int num16 = index18 + 1;
        this.InsertLine("status_code", ": ", words16, index18);
        string[] words17 = words;
        int index19 = num16;
        int num17 = index19 + 1;
        this.InsertLine("res_code_1", ": ", words17, index19);
        string[] words18 = words;
        int index20 = num17;
        int num18 = index20 + 1;
        this.InsertLine("res_code_2", ": ", words18, index20);
        this.InsertLine("");
        string[] words19 = words;
        int index21 = num18;
        int num19 = index21 + 1;
        this.InsertLine("priority", ": ", words19, index21);
        string[] words20 = words;
        int index22 = num19;
        int num20 = index22 + 1;
        this.InsertLine("status_code", ": ", words20, index22);
        string[] words21 = words;
        int index23 = num20;
        int num21 = index23 + 1;
        this.InsertLine("res_code_1", ": ", words21, index23);
        string[] words22 = words;
        int index24 = num21;
        int num22 = index24 + 1;
        this.InsertLine("res_code_2", ": ", words22, index24);
        this.InsertLine("");
        string[] words23 = words;
        int index25 = num22;
        int num23 = index25 + 1;
        this.InsertLine("priority", ": ", words23, index25);
        string[] words24 = words;
        int index26 = num23;
        int num24 = index26 + 1;
        this.InsertLine("status_code", ": ", words24, index26);
        string[] words25 = words;
        int index27 = num24;
        int num25 = index27 + 1;
        this.InsertLine("res_code_1", ": ", words25, index27);
        string[] words26 = words;
        int index28 = num25;
        int num26 = index28 + 1;
        this.InsertLine("res_code_2", ": ", words26, index28);
        this.InsertLine("");
        string[] words27 = words;
        int index29 = num26;
        int num27 = index29 + 1;
        this.InsertLine("priority", ": ", words27, index29);
        string[] words28 = words;
        int index30 = num27;
        int num28 = index30 + 1;
        this.InsertLine("status_code", ": ", words28, index30);
        string[] words29 = words;
        int index31 = num28;
        int num29 = index31 + 1;
        this.InsertLine("res_code_1", ": ", words29, index31);
        string[] words30 = words;
        int index32 = num29;
        int num30 = index32 + 1;
        this.InsertLine("res_code_2", ": ", words30, index32);
        this.InsertLine("");
        string[] words31 = words;
        int index33 = num30;
        int num31 = index33 + 1;
        this.InsertLine("priority", ": ", words31, index33);
        string[] words32 = words;
        int index34 = num31;
        int num32 = index34 + 1;
        this.InsertLine("status_code", ": ", words32, index34);
        string[] words33 = words;
        int index35 = num32;
        int num33 = index35 + 1;
        this.InsertLine("res_code_1", ": ", words33, index35);
        string[] words34 = words;
        int index36 = num33;
        int num34 = index36 + 1;
        this.InsertLine("res_code_2", ": ", words34, index36);
        this.InsertLine("");
        string[] words35 = words;
        int index37 = num34;
        int num35 = index37 + 1;
        this.InsertLine("priority", ": ", words35, index37);
        string[] words36 = words;
        int index38 = num35;
        int num36 = index38 + 1;
        this.InsertLine("status_code", ": ", words36, index38);
        string[] words37 = words;
        int index39 = num36;
        int num37 = index39 + 1;
        this.InsertLine("res_code_1", ": ", words37, index39);
        string[] words38 = words;
        int index40 = num37;
        num3 = index40 + 1;
        this.InsertLine("res_code_2", ": ", words38, index40);
        break;
      case 104:
        string[] words39 = words;
        int index41 = index4;
        int num38 = index41 + 1;
        this.InsertLine("nInput", ": ", words39, index41);
        string[] words40 = words;
        int index42 = num38;
        int num39 = index42 + 1;
        this.InsertLine("icvt_num", ": ", words40, index42);
        string[] words41 = words;
        int index43 = num39;
        int num40 = index43 + 1;
        this.InsertLineValidateRange("debounce", ": ", words41, index43, 0, 15);
        string[] words42 = words;
        int index44 = num40;
        int num41 = index44 + 1;
        this.InsertLine("hold_time", ": ", words42, index44);
        string[] words43 = words;
        int index45 = num41;
        num3 = index45 + 1;
        this.InsertLine("o_time", ": ", words43, index45);
        break;
      case 105:
        string[] words44 = words;
        int index46 = index4;
        int num42 = index46 + 1;
        this.InsertLine("nOutputs", ": ", words44, index46);
        string[] words45 = words;
        int index47 = num42;
        num3 = index47 + 1;
        this.InsertLine("cDriveMode", ": ", words45, index47);
        break;
      case 106:
        string[] words46 = words;
        int index48 = index4;
        int num43 = index48 + 1;
        this.InsertLine("nReader", ": ", words46, index48);
        string[] words47 = words;
        int index49 = num43;
        int num44 = index49 + 1;
        this.InsertLine("dt_fmt", ": ", words47, index49);
        string[] words48 = words;
        int index50 = num44;
        int num45 = index50 + 1;
        this.InsertLine("keypad_mode", ": ", words48, index50);
        string[] words49 = words;
        int index51 = num45;
        num3 = index51 + 1;
        this.InsertLine("led_drive_mode", ": ", words49, index51);
        break;
      case 108:
        string[] words50 = words;
        int index52 = index4;
        int num46 = index52 + 1;
        this.InsertLine("nReader", ": ", words50, index52);
        string[] words51 = words;
        int index53 = num46;
        int num47 = index53 + 1;
        this.InsertLine("nOlMode", ": ", this.ParseAcrMode(words51, index53));
        string[] words52 = words;
        int index54 = num47;
        int num48 = index54 + 1;
        this.InsertLine("nCfmtMsk", ": ", words52, index54);
        string[] words53 = words;
        int index55 = num48;
        int num49 = index55 + 1;
        this.InsertLine("nRexIn", ": ", words53, index55);
        string[] words54 = words;
        int index56 = num49;
        int num50 = index56 + 1;
        this.InsertLine("nStrkOut", ": ", words54, index56);
        string[] words55 = words;
        int index57 = num50;
        num3 = index57 + 1;
        this.InsertLine("nStrkTm", ": ", words55, index57);
        break;
      case 109:
        string[] words56 = words;
        int index58 = index4;
        int num51 = index58 + 1;
        this.InsertLine("nAction", ": ", this.ParseOfflineLEDActionColor(words56, index58));
        string[] words57 = words;
        int index59 = num51;
        int num52 = index59 + 1;
        this.InsertLine("on_color", ": ", this.ParseLEDColor(words57, index59));
        string[] words58 = words;
        int index60 = num52;
        int num53 = index60 + 1;
        this.InsertLine("off_color", ": ", this.ParseLEDColor(words58, index60));
        string[] words59 = words;
        int index61 = num53;
        int num54 = index61 + 1;
        this.InsertLine("on_time", ": ", words59, index61);
        string[] words60 = words;
        int index62 = num54;
        int num55 = index62 + 1;
        this.InsertLine("off_time", ": ", words60, index62);
        string[] words61 = words;
        int index63 = num55;
        int num56 = index63 + 1;
        this.InsertLine("repeat_count", ": ", words61, index63);
        string[] words62 = words;
        int index64 = num56;
        num3 = index64 + 1;
        this.InsertLine("beep_count", ": ", words62, index64);
        break;
      case 110:
        string[] words63 = words;
        int index65 = index4;
        num3 = index65 + 1;
        this.InsertLine("cHexLine", ": ", words63, index65);
        break;
      case 114:
        string[] words64 = words;
        int index66 = index4;
        int num57 = index66 + 1;
        this.InsertLine("nCmndId", ": ", words64, index66);
        string[] words65 = words;
        int index67 = num57;
        int num58 = index67 + 1;
        this.InsertLine("nDataLength", ": ", words65, index67);
        string[] words66 = words;
        int index68 = num58;
        num3 = index68 + 1;
        this.InsertLine("cData[100]", ": ", words66, index68);
        break;
      case 115:
        string[] words67 = words;
        int index69 = index4;
        int num59 = index69 + 1;
        this.InsertLine("nOutput", ": ", words67, index69);
        string[] words68 = words;
        int index70 = num59;
        int num60 = index70 + 1;
        this.InsertLine("nTmpCommand", ": ", words68, index70);
        string[] words69 = words;
        int index71 = num60;
        int num61 = index71 + 1;
        this.InsertLine("nTmpOnTime", ": ", words69, index71);
        string[] words70 = words;
        int index72 = num61;
        int num62 = index72 + 1;
        this.InsertLine("nTmpOffTime", ": ", words70, index72);
        string[] words71 = words;
        int index73 = num62;
        int num63 = index73 + 1;
        this.InsertLine("nTmpRepeat", ": ", words71, index73);
        string[] words72 = words;
        int index74 = num63;
        int num64 = index74 + 1;
        this.InsertLine("nPrmCommand", ": ", words72, index74);
        string[] words73 = words;
        int index75 = num64;
        int num65 = index75 + 1;
        this.InsertLine("nPrmOnTime", ": ", words73, index75);
        string[] words74 = words;
        int index76 = num65;
        num3 = index76 + 1;
        this.InsertLine("nPrmOffTime", ": ", words74, index76);
        break;
      case 116:
        string[] words75 = words;
        int index77 = index4;
        int num66 = index77 + 1;
        this.InsertLine("nReader", ": ", words75, index77);
        string[] words76 = words;
        int index78 = num66;
        int num67 = index78 + 1;
        this.InsertLine("nTmpCommand", ": ", words76, index78);
        string[] words77 = words;
        int index79 = num67;
        int num68 = index79 + 1;
        this.InsertLine("nTmpOnColor", ": ", words77, index79);
        string[] words78 = words;
        int index80 = num68;
        int num69 = index80 + 1;
        this.InsertLine("nTmpOffColor", ": ", words78, index80);
        string[] words79 = words;
        int index81 = num69;
        int num70 = index81 + 1;
        this.InsertLine("nTmpOnTime", ": ", words79, index81);
        string[] words80 = words;
        int index82 = num70;
        int num71 = index82 + 1;
        this.InsertLine("nTmpOffTime", ": ", words80, index82);
        string[] words81 = words;
        int index83 = num71;
        int num72 = index83 + 1;
        this.InsertLine("nTmpRepeat", ": ", words81, index83);
        string[] words82 = words;
        int index84 = num72;
        int num73 = index84 + 1;
        this.InsertLine("nPrmCommand", ": ", words82, index84);
        string[] words83 = words;
        int index85 = num73;
        int num74 = index85 + 1;
        this.InsertLine("nPrmOnColor", ": ", words83, index85);
        string[] words84 = words;
        int index86 = num74;
        int num75 = index86 + 1;
        this.InsertLine("nPrmOffColor", ": ", words84, index86);
        string[] words85 = words;
        int index87 = num75;
        int num76 = index87 + 1;
        this.InsertLine("nPrmOnTime", ": ", words85, index87);
        string[] words86 = words;
        int index88 = num76;
        num3 = index88 + 1;
        this.InsertLine("nPrmOffTime", ": ", words86, index88);
        break;
      case 117:
        string[] words87 = words;
        int index89 = index4;
        int num77 = index89 + 1;
        this.InsertLine("nReader", ": ", words87, index89);
        string[] words88 = words;
        int index90 = num77;
        num3 = index90 + 1;
        this.InsertLine("nBeeps", ": ", words88, index90);
        break;
      case 118:
        string[] words89 = words;
        int index91 = index4;
        int num78 = index91 + 1;
        this.InsertLine("nReader", ": ", words89, index91);
        string[] words90 = words;
        int index92 = num78;
        int num79 = index92 + 1;
        this.InsertLine("nTextType", ": ", words90, index92);
        string[] words91 = words;
        int index93 = num79;
        int num80 = index93 + 1;
        this.InsertLine("nTempTime", ": ", words91, index93);
        string[] words92 = words;
        int index94 = num80;
        int num81 = index94 + 1;
        this.InsertLine("nRow", ": ", words92, index94);
        string[] words93 = words;
        int index95 = num81;
        int num82 = index95 + 1;
        this.InsertLine("nColumn", ": ", words93, index95);
        string[] words94 = words;
        int index96 = num82;
        num3 = index96 + 1;
        this.InsertLine("cText", ": ", words94, index96);
        break;
      case 119:
        int num83 = int.Parse(words[index4]);
        string[] words95 = words;
        int index97 = index4;
        int num84 = index97 + 1;
        this.InsertLine("nOutputs", ": ", words95, index97);
        if (num83 > 16 /*0x10*/)
        {
          this.InsertLine("Invalid number for nOutputs");
          num83 = 16 /*0x10*/;
        }
        for (int index98 = 0; index98 < num83; ++index98)
        {
          this.InsertLine("");
          string[] words96 = words;
          int index99 = num84;
          int num85 = index99 + 1;
          this.InsertLine("nOutput", ": ", words96, index99);
          string[] words97 = words;
          int index100 = num85;
          int num86 = index100 + 1;
          this.InsertLine("nOutCommand", ": ", this.ParseSioOutCommand(words97, index100));
          string[] words98 = words;
          int index101 = num86;
          num84 = index101 + 1;
          this.InsertLine("nPulseTime", ": ", words98, index101);
        }
        break;
      case 121:
        string[] words99 = words;
        int index102 = index4;
        int num87 = index102 + 1;
        this.InsertLine("rdr_num", ": ", words99, index102);
        string[] words100 = words;
        int index103 = num87;
        int num88 = index103 + 1;
        this.InsertLine("override_type", ": ", words100, index103);
        string[] words101 = words;
        int index104 = num88;
        num3 = index104 + 1;
        this.InsertLine("nDuration", ": ", words101, index104);
        break;
      case 122:
        string[] words102 = words;
        int index105 = index4;
        int num89 = index105 + 1;
        this.InsertLine("nKeyId", ": ", words102, index105);
        string[] words103 = words;
        int index106 = num89;
        num3 = index106 + 1;
        this.InsertLine("cKey", ": ", words103, index106);
        break;
      case 124:
        string[] words104 = words;
        int index107 = index4;
        int num90 = index107 + 1;
        this.InsertLine("nReader", ": ", words104, index107);
        string[] words105 = words;
        int index108 = num90;
        int num91 = index108 + 1;
        this.InsertLine("e_sec", ": ", words105, index108);
        string[] words106 = words;
        int index109 = num91;
        num3 = index109 + 1;
        this.InsertLine("baseYear", ": ", words106, index109);
        break;
    }
  }

  private void ParseEnCcScpUCmnd(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpUCmnd (1222)");
    else
      this.InsertLine("enCcScpUCmnd (1141)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nUCmndId", ": ", this.ParseUCmnd(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("cCmndCode[MAX_UCMND_NAME+1]", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("cAcrList[MAX_ACR_PER_SCP+1]", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("nIdMode", ": ", this.ParseIdMode(words6, index6));
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("nAuthorityFlags", ": ", this.ParseUcmdAuthorityFlags(words7, index7));
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLine("nAccessDelay", ": ", words8, index8);
    string[] words9 = words;
    int index9 = num9;
    int num10 = index9 + 1;
    this.InsertLine("nUsrLevel", ": ", this.ParseBurgUsrLevel(words9, index9));
    string[] words10 = words;
    int index10 = num10;
    int index11 = index10 + 1;
    this.InsertLineValidateRange("nAccLevel", ": ", words10, index10, -1, (int) sbyte.MaxValue);
    this.InsertLine("nArgs[MAX_UCMND_ARGS_CONFIG]", ": ", words, index11, 4);
    int num11 = index11 + 4;
  }

  private void ParseEnCcHoliday(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enCcHoliday (0104)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int index3 = index2 + 1;
    this.InsertLine("number", ": ", words2, index2);
    int num3 = int.Parse(words[index3]);
    int num4 = 1;
    int num5;
    if (num3 == 0)
    {
      num5 = index3 + 1;
      this.InsertLine("year: 0 - Delete All Holidays");
      num4 = 0;
    }
    else
    {
      string[] words3 = words;
      int index4 = index3;
      num5 = index4 + 1;
      this.InsertLine("year", ": ", words3, index4);
    }
    string[] words4 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    int minVal1 = num4;
    this.InsertLineValidateRange("month", ": ", words4, index5, minVal1, 12);
    string[] words5 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    int minVal2 = num4;
    this.InsertLineValidateRange("day", ": ", words5, index6, minVal2, 31 /*0x1F*/);
    string[] words6 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("extend", ": ", this.ParseHolidayExtendField(words6, index7));
    string[] words7 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLine("type_mask", ": ", this.ParseHolidayTypeMask(words7, index8));
  }

  private void ParseEnCcScpHoliday(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpHoliday (1108)");
    else
      this.InsertLine("enCcScpHoliday (1104)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int index4 = index3 + 1;
    this.InsertLine("number", ": ", words3, index3);
    int num4 = int.Parse(words[index4]);
    int num5 = 1;
    int num6;
    if (num4 == 0)
    {
      num6 = index4 + 1;
      this.InsertLine("year: 0 - Delete All Holidays");
      num5 = 0;
    }
    else
    {
      string[] words4 = words;
      int index5 = index4;
      num6 = index5 + 1;
      this.InsertLine("year", ": ", words4, index5);
    }
    string[] words5 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    int minVal1 = num5;
    this.InsertLineValidateRange("month", ": ", words5, index6, minVal1, 12);
    string[] words6 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    int minVal2 = num5;
    this.InsertLineValidateRange("day", ": ", words6, index7, minVal2, 31 /*0x1F*/);
    string[] words7 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLine("extend", ": ", this.ParseHolidayExtendField(words7, index8));
    string[] words8 = words;
    int index9 = num9;
    int num10 = index9 + 1;
    this.InsertLine("type_mask", ": ", this.ParseHolidayTypeMask(words8, index9));
  }

  private void ParseEnCcRledSpc(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRledSpc (1106)");
    else
      this.InsertLine("enCcRledSpc (0122)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("led_mode", ": ", words3, index3, 1, 3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("rled_id", ": ", this.ParseRledId(words4, index4));
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("on_color", ": ", this.ParseLEDColor(words5, index5));
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("off_color", ": ", this.ParseLEDColor(words6, index6));
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("on_time", ": ", words7, index7);
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLine("off_time", ": ", words8, index8);
    string[] words9 = words;
    int index9 = num9;
    int num10 = index9 + 1;
    this.InsertLineValidateRange("repeat_count", ": ", words9, index9, 0, (int) byte.MaxValue);
    string[] words10 = words;
    int index10 = num10;
    int num11 = index10 + 1;
    this.InsertLineValidateRange("beep_count", ": ", words10, index10, 0, 15);
    string[] words11 = words;
    int index11 = num11;
    int num12 = index11 + 1;
    this.InsertLine("rdiLine1", ": ", words11, index11);
    string[] words12 = words;
    int index12 = num12;
    int num13 = index12 + 1;
    this.InsertLine("rdiLine2", ": ", words12, index12);
  }

  private void ParseEnCcAcrLogDeny(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcAcrLogDeny (1221)");
    else
      this.InsertLine("enCcAcrLogDeny (0151)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("acr_number", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nLpdFlags", ": ", this.ParseLpdFlags(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLineValidateRange("nLpdLimit", ": ", words4, index4, 0, (int) byte.MaxValue);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("nLpdTime", ": ", words5, index5);
  }

  private void ParseEnCcScpAsAdd(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpAsAdd (NA)");
    else
      this.InsertLine("enCcScpAsAdd (1124)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int index4 = index3 + 1;
    this.InsertLine("cAssetId[MAX_ASSET_SIZE]", ": ", words3, index3);
    this.InsertLine("nOwners[MAX_OWNERS]", ": ", words, index4, 8);
    int index5 = index4 + 8;
    this.InsertLine("nAssetArg[MAX_ASSET_ARGS]", ": ", words, index5, 16 /*0x10*/);
    int num4 = index5 + 16 /*0x10*/;
  }

  private void ParseEnCcScpBioDbAdd1(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpBioDbAdd1 (NA)");
    else
      this.InsertLine("enCcScpBioDbAdd1 (1132)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int index4 = index3 + 1;
    this.InsertLine("card_number", ": ", words3, index3);
    int num4 = int.Parse(words[index4]);
    string[] words4 = words;
    int index5 = index4;
    int num5 = index5 + 1;
    this.InsertLine("nBioType", ": ", this.ParseBioType(words4, index5));
    string[] words5 = words;
    int index6 = num5;
    int num6 = index6 + 1;
    int bioType = num4;
    this.InsertLine("nFlags", ": ", this.ParseBioFlags(words5, index6, bioType));
    string[] words6 = words;
    int index7 = num6;
    int num7 = index7 + 1;
    this.InsertLine("nMinScore", ": ", words6, index7);
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("nTemplateSize", ": ", words7, index8, 0, 1024 /*0x0400*/);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("cBioTemplate[MAX_BIO_TEMPLATE*2]", ": ", words8, index9);
  }

  private void ParseEnScpDown(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNScpDown (1002)");
    else
      this.InsertLine("enScpDown (0203)");
    string[] words1 = words;
    int index = num1;
    int num2 = index + 1;
    this.InsertLine("dummy", ": ", words1, index);
  }

  private void ParseEnCcFirmwareDown(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcFirmwareDown (1025)");
    else
      this.InsertLine("enCcFirmwareDown (0206)");
    string[] words1 = words;
    int index1 = num1;
    int index2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    int num2 = this.InsertLineWithText("file_name[200]", ": ", words, index2);
    int num3 = index2 + num2;
  }

  private void ParseEnCcDetachScp(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcDetachScp (1024)");
    else
      this.InsertLine("enCcDetachScp (0208)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nSCPId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nChannelId", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nToAltPort", ": ", words3, index3);
  }

  private void ParseEnCcConfigSave(string[] words, int numWords, bool honeywell)
  {
    int index1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcConfigSave (NA)");
    else
      this.InsertLine("enCcConfigSave (0209)");
    int num1 = this.InsertLineWithText("file_name[100]", ": ", words, index1);
    int num2 = index1 + num1;
    string[] words1 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nMode", ": ", this.ParseSaveMode(words1, index2));
    string[] words2 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words2, index3, 0, 16383 /*0x3FFF*/);
  }

  private void ParseEnCcConfigDelta(string[] words, int numWords, bool honeywell)
  {
    int index = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcConfigDelta (NA)");
    else
      this.InsertLine("enCcConfigDelta (0210)");
    int num1 = this.InsertLineWithText("file_name[100]", ": ", words, index);
    int num2 = index + num1;
  }

  private void ParseEnCcPollMode(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcPollMode (1029)");
    else
      this.InsertLine("enCcPollMode (0214)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nPollMode", ": ", this.ParsePollMode(words2, index2));
  }

  private void ParseEnCcFileDownload(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcFileDownload (1030)");
    else
      this.InsertLine("enCcFileDownload (0215)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int index3 = index2 + 1;
    this.InsertLine("file_type", ": ", this.ParseFileType(words2, index2));
    int num3 = this.InsertLineWithText("src_file[MAX_PATH]", ": ", words, index3);
    int index4 = index3 + num3;
    int num4 = this.InsertLineWithText("dest_file[MAX_PATH]", ": ", words, index4);
    int num5 = index4 + num4;
  }

  private void ParseEnCcHexOutInternal(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcHexOutInternal (1032)");
    else
      this.InsertLine("enCcHexOutInternal (0217)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("data_len", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("data[2048]", ": ", words3, index3);
  }

  private void ParseEnCcDeleteFile(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcDeleteFile (1033)");
    else
      this.InsertLine("enCcDeleteFile (0218)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("file_type", ": ", this.ParseFileType(words2, index2));
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("file_name[100]", ": ", words3, index3);
  }

  private void ParseEnCcGetFileInfo(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcGetFileInfo (1034)");
    else
      this.InsertLine("enCcGetFileInfo (0219)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("file_type", ": ", this.ParseFileType(words2, index2));
  }

  private void ParseEnCcCpCtl(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcCpCtl (1307)");
    else
      this.InsertLine("enCcCpCtl (0307)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("cp_number", ": ", words2, index2, 0, 2047 /*0x07FF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("command", ": ", this.ParseCpCtlCommand(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("on_time", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("off_time", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("repeat", ": ", words6, index6);
  }

  private void ParseEnCcHeldOpenMask(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcHeldOpenMask (1310)");
    else
      this.InsertLine("enCcHeldOpenMask (0310)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("acr_number", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("set_clear", ": ", this.ParseSetClear(words3, index3));
  }

  private void ParseEnCcProcedure(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcProcedure (1312)");
    else
      this.InsertLine("enCcProcedure (0312)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("proc_number", ": ", words2, index2, 0, 8195);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("command", ": ", this.ParseProcedureCommand(words3, index3));
  }

  private void ParseEnCcTVCommand(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcTVCommand (1313)");
    else
      this.InsertLine("enCcTVCommand (0313)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("tv_number", ": ", words2, index2, 1, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("set_clear", ": ", this.ParseSetClear(words3, index3));
  }

  private void ParseEnCcOemCode(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcOemCode (1316)");
    else
      this.InsertLine("enCcOemCode (0316)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("oem_code", ": ", this.ParseOEMCodes(words2, index2));
  }

  private void ParseEnCcPassword(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcPassword (1317)");
    else
      this.InsertLine("enCcPassword (0317)");
    string[] words1 = words;
    int index1 = num1;
    int index2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    int num2 = this.InsertLineWithText("password[16]", ": ", words, index2);
    int num3 = index2 + num2;
  }

  private void ParseEnCcApbFreePass(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcApbFreePass (NA)");
    else
      this.InsertLine("enCcApbFreePass (0319)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("cardholder_id", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("apb_area", ": ", words3, index3, 0, (int) sbyte.MaxValue);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("flags", ": ", this.ParseApbFreePassFlags(words4, index4));
  }

  private void ParseEnCcApbFreePassDbl(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcApbFreePassDbl (NA)");
    else
      this.InsertLine("enCcApbFreePassDbl (2319)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("cardholder_id", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("apb_area", ": ", words3, index3, 0, (int) sbyte.MaxValue);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("flags", ": ", this.ParseApbFreePassFlags(words4, index4));
  }

  private void ParseEnCcApbFreePassI64(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcApbFreePassDbl (1319)");
    else
      this.InsertLine("enCcApbFreePassI64 (3319)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("cardholder_id", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("apb_area", ": ", words3, index3, 0, (int) sbyte.MaxValue);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("flags", ": ", this.ParseApbFreePassFlags(words4, index4));
  }

  private void ParseEnCcHexOut(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcHexOut (1320)");
    else
      this.InsertLine("enCcHexOut (0320)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("baud", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("port", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int index5 = index4 + 1;
    this.InsertLine("channel", ": ", words4, index4);
    int num5 = this.InsertLineWithText("data[128+1]", ": ", words, index5);
    int num6 = index5 + num5;
  }

  private void ParseEnCcAreaSet(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcAreaSet (1322)");
    else
      this.InsertLine("enCcAreaSet (0322)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("area_number", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("command", ": ", this.ParseAreaSetCommand(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("occ_set", ": ", words4, index4);
  }

  private void ParseEnCcUseLimit(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcUseLimit (NA)");
    else
      this.InsertLine("enCcUseLimit (0323)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("cardholder_id", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("new_limit", ": ", words3, index3, -1, (int) byte.MaxValue);
  }

  private void ParseEnCcUseLimitDbl(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcUseLimitDbl (NA)");
    else
      this.InsertLine("enCcUseLimitDbl (2323)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("cardholder_id", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("new_limit", ": ", words3, index3, -1, (int) byte.MaxValue);
  }

  private void ParseEnCcUseLimitI64(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcUseLimitDbl (1323)");
    else
      this.InsertLine("enCcUseLimitI64 (3323)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("cardholder_id", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("new_limit", ": ", words3, index3, -1, (int) byte.MaxValue);
  }

  private void ParseEnCcRLedTemp(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRLedTemp  (1325)");
    else
      this.InsertLine("enCcRLedTemp  (0325)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("acr_number", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("color_on", ": ", this.ParseLEDColor(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("color_off", ": ", this.ParseLEDColor(words4, index4));
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLineValidateRange("ticks_on", ": ", words5, index5, 0, (int) byte.MaxValue);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLineValidateRange("ticks_off", ": ", words6, index6, 0, (int) byte.MaxValue);
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLineValidateRange("repeat", ": ", words7, index7, 1, (int) byte.MaxValue);
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLineValidateRange("beeps", ": ", words8, index8, 0, 15);
    string[] words9 = words;
    int index9 = num9;
    int num10 = index9 + 1;
    this.InsertLine("LED_number", ": ", words9, index9);
    string[] words10 = words;
    int index10 = num10;
    int num11 = index10 + 1;
    this.InsertLine("color_on_RGB", ": ", this.ParseRGBLEDColor(words10, index10));
    string[] words11 = words;
    int index11 = num11;
    int num12 = index11 + 1;
    this.InsertLine("color_off_RGB", ": ", this.ParseRGBLEDColor(words11, index11));
    string[] words12 = words;
    int index12 = num12;
    int num13 = index12 + 1;
    this.InsertLine("flags", ": ", this.ParseTempLEDFlags(words12, index12));
  }

  private void ParseEnCcLcdText(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcLcdText  (1326)");
    else
      this.InsertLine("enCcLcdText  (0326)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("term_number", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("type", ": ", this.ParseLcdTextType(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLineValidateRange("temp_time", ": ", words4, index4, 0, 31 /*0x1F*/);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("tone", ": ", this.ParseLcdTextTones(words5, index5));
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLineValidateRange("tone_time", ": ", words6, index6, 0, 31 /*0x1F*/);
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLineValidateRange("row", ": ", words7, index7, 0, 1);
    string[] words8 = words;
    int index8 = num8;
    int index9 = index8 + 1;
    this.InsertLineValidateRange("column", ": ", words8, index8, 0, 15);
    int num9 = this.InsertLineWithText("text[64]", ": ", words, index9);
    int num10 = index9 + num9;
    string[] words9 = words;
    int index10 = num10;
    int num11 = index10 + 1;
    this.InsertLine("nLineIndex", ": ", words9, index10);
    string[] words10 = words;
    int index11 = num11;
    int num12 = index11 + 1;
    this.InsertLine("nLanguageIndex", ": ", words10, index11);
  }

  private void ParseEnCcSioDc(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcSioDc (1327)");
    else
      this.InsertLine("enCcSioDc (0327)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("sio_number", ": ", words2, index2, 0, 95);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("cmnd_tag", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("cmnd_id", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("data[MAX_SIODC_DATA*2+1]", ": ", words5, index5);
  }

  private void ParseEnCcHostResponse(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcHostResponse (1329)");
    else
      this.InsertLine("enCcHostResponse (0329)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("acr_number", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("command", ": ", this.ParseHostResponseCommand(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("cardholder_id", ": ", words4, index4);
  }

  private void ParseEnCcNvArgSet(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcNvArgSet (1330)");
    else
      this.InsertLine("enCcNvArgSet (0330)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nNvArgType", ": ", this.ParseNvArgType(words2, index2));
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("nSioNumber", ": ", words3, index3, -1, 95);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("nNvArgValue", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("nNvAuthCode", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("nNvDataLen", ": ", words6, index6);
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("data", ": ", words7, index7);
  }

  private void ParseEnCcCardSim(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcCardSim (1331)");
    else
      this.InsertLine("enCcCardSim (0331)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScp", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nCommand", ": ", this.ParseCardSimCommand(words2, index2));
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("nAcr", ": ", words3, index3, 0, (int) sbyte.MaxValue);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("e_time", ": ", this.ParseTime(words4, index4, 1970));
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLineValidateRange("nFmtNum", ": ", words5, index5, 0, 15);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("nFacilityCode", ": ", words6, index6);
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("nCardholderId", ": ", words7, index7);
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLine("nIssueCode", ": ", words8, index8);
  }

  private void ParseEnCcDiag(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcDiag (1333)");
    else
      this.InsertLine("enCcDiag (0333)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("diag_code", ": ", this.ParseDiagCode(words2, index2));
  }

  private void ParseEnCcTempAcrMode(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcTempAcrMode (1334)");
    else
      this.InsertLine("enCcTempAcrMode (0334)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("acr_number", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("acr_mode", ": ", this.ParseAcrMode(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("time", ": ", this.ParseTempAcrModeTime(words4, index4));
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("nAuthModFlags", ": ", words5, index5);
  }

  private void ParseEnCcOperatingMode(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcOperatingMode (1335)");
    else
      this.InsertLine("enCcOperatingMode (0335)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("oper_mode", ": ", words2, index2, 0, 7);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("enforce_existing", ": ", words3, index3, 0, 1);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLineValidateRange("existing_mode", ": ", words4, index4, 0, 7);
  }

  private void ParseEnCcAcrOsdpPassthrough(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcAcrOsdpPassthrough (1337)");
    else
      this.InsertLine("enCcAcrOsdpPassthrough (0337)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("acr_number", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("sequence_num", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("reader_role", ": ", this.ParseOsdpPassThruReaderRole(words4, index4));
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("msg_type", ": ", this.ParseOsdpPassThruMsgType(words5, index5));
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("data_len", ": ", words6, index6);
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("data[MAX_OSDP_PASSTHROUGH]", ": ", words7, index7);
  }

  private void ParseEnCcKeySim(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcKeySim (1339)");
    else
      this.InsertLine("enCcKeySim (0339)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nAcr", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("e_time", ": ", this.ParseTime(words3, index3, 1970));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("keys[MAX_SIM_KEY_SIZE]", ": ", words4, index4);
  }

  private void ParseEnCcOsdpReaderTransfer(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcOsdpReaderTransfer (1340)");
    else
      this.InsertLine("enCcOsdpReaderTransfer (0340)");
    string[] words1 = words;
    int index1 = num1;
    int index2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    this.InsertLine("acr_pri[16]", ": ", words, index2, 16 /*0x10*/);
    int index3 = index2 + 16 /*0x10*/;
    this.InsertLine("acr_alt[16]", ": ", words, index3, 16 /*0x10*/);
    int num2 = index3 + 16 /*0x10*/;
    string[] words2 = words;
    int index4 = num2;
    int num3 = index4 + 1;
    this.InsertLine("src_file[100]", ": ", words2, index4);
  }

  private void ParseEnCcControlReboot(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcControlReboot (1341)");
    else
      this.InsertLine("enCcControlReboot (0341)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("sio_number", ": ", words2, index2, 0, 32 /*0x20*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("target_type", ": ", words3, index3, 0, 2);
    string[] words4 = words;
    int index4 = num4;
    int index5 = index4 + 1;
    this.InsertLineValidateRange("reset_type", ": ", words4, index4, 0, 1);
    this.InsertLine("ctl_list[16]", ": ", words, index5, 16 /*0x10*/);
    string[] words5 = words;
    int index6 = index5;
    int num5 = index6 + 1;
    this.InsertLine("Ctls", ": ", this.ParseCtlBitmap(words5, index6));
    int num6 = num5 + 16 /*0x10*/;
  }

  private void ParseEnCcAcrOfflineAccessList(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcAcrOfflineAccessList (1338)");
    else
      this.InsertLine("enCcAcrOfflineAccessList (0338)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int index3 = index2 + 1;
    this.InsertLineValidateRange("acr_number", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    int num3 = int.Parse(words[index3]);
    string[] words3 = words;
    int index4 = index3;
    int num4 = index4 + 1;
    this.InsertLine("protocol", ": ", this.ParseOALProtocol(words3, index4));
    string[] words4 = words;
    int index5 = num4;
    int num5 = index5 + 1;
    int protocol = num3;
    this.InsertLine("action", ": ", this.ParseOALAction(words4, index5, protocol));
    string[] words5 = words;
    int index6 = num5;
    int num6 = index6 + 1;
    this.InsertLine("params_len", ": ", words5, index6);
    string[] words6 = words;
    int index7 = num6;
    int num7 = index7 + 1;
    this.InsertLine("params_data[MAX_OAL_PARAMS_SIZE]", ": ", words6, index7);
  }

  private void ParseEnCcSioRdrHexLoad(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcSioRdrHexLoad (1336)");
    else
      this.InsertLine("enCcSioRdrHexLoad (0336)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("sio_number", ": ", words2, index2, 0, 95);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("rdr_list", ": ", this.ParseSioRdrHexRdrListFlags(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("target_type", ": ", this.ParseSioRdrHexTargetType(words4, index4));
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("file_name[200]", ": ", words5, index5);
  }

  private void ParseEnCcMpSrq(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcMpSrq (1405)");
    else
      this.InsertLine("enCcMpSrq (0405)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("first", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("count", ": ", words3, index3);
  }

  private void ParseEnCcCpSrq(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcCpSrq (1406)");
    else
      this.InsertLine("enCcCpSrq (0406)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("first", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("count", ": ", words3, index3);
  }

  private void ParseEnCcTzSrq(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcTzSrq (1408)");
    else
      this.InsertLine("enCcTzSrq (0408)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("first", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("count", ": ", words3, index3);
  }

  private void ParseEnCcTvSrq(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcTvSrq (1409)");
    else
      this.InsertLine("enCcTvSrq (0409)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("first", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("count", ": ", words3, index3);
  }

  private void ParseEnCcMpgSrq(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcMpgSrq (1411)");
    else
      this.InsertLine("enCcMpgSrq (0411)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("first", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("count", ": ", words3, index3);
  }

  private void ParseEnCcAreaSrq(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcAreaSrq (1412)");
    else
      this.InsertLine("enCcAreaSrq (0412)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("first", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("count", ": ", words3, index3);
  }

  private void ParseEnCcIpsSrq(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcIpsSrq (1413)");
    else
      this.InsertLine("enCcIpsSrq (0413)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("first", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("count", ": ", words3, index3);
  }

  private void ParseEnCcIpsSrqPts(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcIpsSrqPts (1414)");
    else
      this.InsertLine("enCcIpsSrqPts (0414)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nIps", ": ", words2, index2, 0, (int) byte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nFirst", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("nMax", ": ", words4, index4);
  }

  private void ParseEnCcSioRelayCtSrq(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcSioRelayCtSrq (1415)");
    else
      this.InsertLine("enCcSioRelayCtSrq (0415)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("sio_number", ": ", words2, index2, 0, 95);
  }

  private void ParseEnCcElAlvl(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcElAlvl (1502)");
    else
      this.InsertLine("enCcElAlvl (0502)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int index4 = index3 + 1;
    this.InsertLineValidateRange("el_number", ": ", words3, index3, 0, (int) byte.MaxValue);
    this.InsertLineValidateRange("tz[MAX_FLOORS_PER_ACR]", ": ", words, index4, 128 /*0x80*/, -1, (int) byte.MaxValue);
    int num4 = index4 + 128 /*0x80*/;
  }

  private void ParseEnCcElFloorConfig(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcElFloorConfig (1503)");
    else
      this.InsertLine("enCcElFloorConfig (0503)");
    string[] words1 = words;
    int index1 = num1;
    int index2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    int elev_config = int.Parse(words[index2]);
    string[] words2 = words;
    int index3 = index2;
    int index4 = index3 + 1;
    this.InsertLine("elev_config", ": ", this.ParseAccessCfg(words2, index3));
    this.InsertLineValidateRange("elev_data[MAX_FLOORS_PER_ACR]", ": ", words, index4, 128 /*0x80*/, -1, 32768 /*0x8000*/);
    for (int index5 = 0; index5 < 128 /*0x80*/ && index4 + index5 < ((IEnumerable<string>) words).Count<string>(); ++index5)
      this.InsertLine("Floor " + (object) (index5 + 1), ": ", this.Parse503ElevData(words, index4 + index5, elev_config));
    int num2 = index4 + 128 /*0x80*/;
  }

  private void ParseEnCcElCabAccess(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcElCabAccess (1504)");
    else
      this.InsertLine("enCcElCabAccess (0504)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int index3 = index2 + 1;
    this.InsertLineValidateRange("el_cbc_num", ": ", words2, index2, 0, (int) byte.MaxValue);
    this.InsertLineValidateRange("el_cab_access[MAX_CABINS]", ": ", words, index3, (int) byte.MaxValue, -1, (int) byte.MaxValue);
    int num3 = index3 + 128 /*0x80*/;
  }

  private void ParseEnCcRdScp(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdScp (1701)");
    else
      this.InsertLine("enCcRdScp (1701)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdDaylight(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdDaylight (1702)");
    else
      this.InsertLine("enCcRdDaylight (1702)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdAdbSpec(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdAdbSpec (1703)");
    else
      this.InsertLine("enCcRdAdbSpec (1703)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdIcvt(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdIcvt (1704)");
    else
      this.InsertLine("enCcRdIcvt (1704)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdCfmt(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdCfmt (1705)");
    else
      this.InsertLine("enCcRdCfmt (1705)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdRledSpc(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdRledSpc (1706)");
    else
      this.InsertLine("enCcRdRledSpc (1706)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdTimezone(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdTimezone (1707)");
    else
      this.InsertLine("enCcRdTimezone (1707)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdHoliday(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdHoliday (1708)");
    else
      this.InsertLine("enCcRdHoliday (1708)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdAdbCard(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdAdbCard (1709)");
    else
      this.InsertLine("enCcRdAdbCard (1709)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdMsp1(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdMsp1 (1801)");
    else
      this.InsertLine("enCcRdMsp1 (1801)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdSio(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdSio (1802)");
    else
      this.InsertLine("enCcRdSio (1802)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdInput(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdInput (1803)");
    else
      this.InsertLine("enCcRdInput (1803)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdOutput(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdOutput (1804)");
    else
      this.InsertLine("enCcRdOutput (1804)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdReader(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdReader (1805)");
    else
      this.InsertLine("enCcRdReader (1805)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdMP(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdMP (1811)");
    else
      this.InsertLine("enCcRdMP (1811)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdCP(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdCP (1812)");
    else
      this.InsertLine("enCcRdCP (1812)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdACR(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdACR (1813)");
    else
      this.InsertLine("enCcRdACR (1813)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdAlvl(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdAlvl (1814)");
    else
      this.InsertLine("enCcRdAlvl (1814)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdTrgr(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdTrgr (1815)");
    else
      this.InsertLine("enCcRdTrgr (1815)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdProc(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdProc (1816)");
    else
      this.InsertLine("enCcRdProc (1816)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdspare(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdspare (1817)");
    else
      this.InsertLine("enCcRdspare (1817)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdMpg(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdMpg (1818)");
    else
      this.InsertLine("enCcRdMpg (1818)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRdArea(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdArea (1819)");
    else
      this.InsertLine("enCcRdArea (1819)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcRMS(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRMS (1852)");
    else
      this.InsertLine("enCcRMS (1852)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nRmsSize", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nOffset", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("nDataLength", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("nData", ": ", words5, index5);
  }

  private void ParseEnCcIpsPointsSia(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcIpsPointsSia (3223)");
    else
      this.InsertLine("enCcIpsPointsSia (2223)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nIpsNumber", ": ", words2, index2, 0, (int) byte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int index4 = index3 + 1;
    this.InsertLine("nIpsPtAddCommand", ": ", this.ParseIpsPtAddCommand(words3, index3));
    int num4 = int.Parse(words[index4]);
    string[] words4 = words;
    int index5 = index4;
    int num5 = index5 + 1;
    this.InsertLineValidateRange("nIpsPtCount", ": ", words4, index5, 0, 128 /*0x80*/);
    if (num4 > 128 /*0x80*/)
      num4 = 128 /*0x80*/;
    for (int index6 = 0; index6 < num4; ++index6)
    {
      this.InsertLine(" ");
      string[] words5 = words;
      int index7 = num5;
      int num6 = index7 + 1;
      this.InsertLine("nPointType", ": ", this.ParseIpsPointType(words5, index7));
      string[] words6 = words;
      int index8 = num6;
      int num7 = index8 + 1;
      this.InsertLine("nPointNumber", ": ", words6, index8);
      string[] words7 = words;
      int index9 = num7;
      int num8 = index9 + 1;
      this.InsertLine("nPointFlags", ": ", this.ParseIpsPointFlags(words7, index9));
      string[] words8 = words;
      int index10 = num8;
      int num9 = index10 + 1;
      this.InsertLine("nSiaZoneNumber", ": ", words8, index10);
      string[] words9 = words;
      int index11 = num9;
      int num10 = index11 + 1;
      this.InsertLine("cSiaPointType", ": ", words9, index11);
      string[] words10 = words;
      int index12 = num10;
      num5 = index12 + 1;
      this.InsertLine("cSiaStateDesignates[16]", ": ", words10, index12);
    }
  }

  private void ParseEnCcActnRem(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcActnRem (1217)");
    else
      this.InsertLine("enCcActnRem (0119)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("cProcFirst", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("cProcLast", ": ", words4, index4);
    int index5 = num5;
    string[] words5 = words;
    int index6 = num5;
    int num6 = index6 + 1;
    this.InsertLine("cActionType", ": ", this.ParseActionType(words5, index6));
    string[] words6 = words;
    int index7 = num6;
    int num7 = index7 + 1;
    this.InsertLine("arg", ": ", words6, index7);
    this.InsertLine("    ", " ", this.ParseArgActionType(words, index5));
  }

  private void ParseEnCcIcvt(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enCcIcvt (0101)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("number", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("pri_5pr", ": ", words3, index3, 0, 2);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLineValidateRange("sts_5pr", ": ", words4, index4, 0, 6);
    for (int index5 = 0; index5 < 8; ++index5)
    {
      this.InsertLine("");
      string[] words5 = words;
      int index6 = num5;
      int num6 = index6 + 1;
      this.InsertLineValidateRange("priority", ": ", words5, index6, 0, 2);
      string[] words6 = words;
      int index7 = num6;
      int num7 = index7 + 1;
      this.InsertLine("status_code", ": ", this.ParseIcvtStatusCode(words6, index7));
      string[] words7 = words;
      int index8 = num7;
      int num8 = index8 + 1;
      this.InsertLine("res_code_1", ": ", this.ParseIcvtResCode(words7, index8));
      string[] words8 = words;
      int index9 = num8;
      num5 = index9 + 1;
      this.InsertLine("res_code_2", ": ", this.ParseIcvtResCode(words8, index9));
    }
  }

  private void ParseEnCcScpIcvt(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpIcvt (1104)");
    else
      this.InsertLine("enCcScpIcvt (1101)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("number", ": ", words3, index3, 128 /*0x80*/, 131);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLineValidateRange("pri_5pr", ": ", words4, index4, 0, 2);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLineValidateRange("sts_5pr", ": ", words5, index5, 0, 6);
    for (int index6 = 0; index6 < 8; ++index6)
    {
      this.InsertLine("");
      string[] words6 = words;
      int index7 = num6;
      int num7 = index7 + 1;
      this.InsertLineValidateRange("priority", ": ", words6, index7, 0, 2);
      string[] words7 = words;
      int index8 = num7;
      int num8 = index8 + 1;
      this.InsertLine("status_code", ": ", this.ParseIcvtStatusCode(words7, index8));
      string[] words8 = words;
      int index9 = num8;
      int num9 = index9 + 1;
      this.InsertLine("res_code_1", ": ", this.ParseIcvtResCode(words8, index9));
      string[] words9 = words;
      int index10 = num9;
      num6 = index10 + 1;
      this.InsertLine("res_code_2", ": ", this.ParseIcvtResCode(words9, index10));
    }
  }

  private void ParseEnCcDeleteChannel(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcDeleteChannel (1012)");
    else
      this.InsertLine("enCcDeleteChannel (0014)");
    string[] words1 = words;
    int index = num1;
    int num2 = index + 1;
    this.InsertLine("nChannelId", ": ", words1, index);
  }

  private void ParseEnCcDeleteScp(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcDeleteScp (1022)");
    else
      this.InsertLine("enCcDeleteScp (0015)");
    string[] words1 = words;
    int index = num1;
    int num2 = index + 1;
    this.InsertLineValidateRange("nSCPid", ": ", words1, index, 0, 16383 /*0x3FFF*/);
  }

  private void ParseEnCcRTxtSpc(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRTxtSpc (1229)");
    else
      this.InsertLine("enCcRTxtSpc (0123)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nMaxLines", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nArg", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int index5 = index4 + 1;
    this.InsertLine("nLineIndex", ": ", this.ParseRTxtLineIndex(words4, index4));
    int num5 = this.InsertLineWithText("cTextLine[16+1]", ": ", words, index5);
    int num6 = index5 + num5;
    string[] words5 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("nLanguageIndex", ": ", words5, index6);
  }

  private void ParseEnCcAdbCardFile(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcAdbCardFile (1109)");
    else
      this.InsertLine("enCcAdbCardFile (0106)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int index3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    int num3 = this.InsertLineWithText("file_name[100]", ": ", words, index3);
    int num4 = index3 + num3;
  }

  private void ParseEnCcDualPortControl(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcDualPortControl (1026)");
    else
      this.InsertLine("enCcDualPortControl (0211)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nHcpDriver", ": ", this.ParseHcpDriver(words2, index2));
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("command", ": ", this.ParseDualPortControlCommand(words3, index3));
  }

  private void ParseEnCcScpBioDbAdd1I64(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpBioDbAdd1I64 (1232)");
    else
      this.InsertLine("enCcScpBioDbAdd1I64 (3132)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int index4 = index3 + 1;
    this.InsertLine("card_number", ": ", words3, index3);
    int num4 = int.Parse(words[index4]);
    string[] words4 = words;
    int index5 = index4;
    int num5 = index5 + 1;
    this.InsertLine("nBioType", ": ", this.ParseBioType(words4, index5));
    string[] words5 = words;
    int index6 = num5;
    int num6 = index6 + 1;
    int bioType = num4;
    this.InsertLine("nFlags", ": ", this.ParseBioFlags(words5, index6, bioType));
    string[] words6 = words;
    int index7 = num6;
    int num7 = index7 + 1;
    this.InsertLine("nMinScore", ": ", words6, index7);
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("nTemplateSize", ": ", words7, index8, 0, 1024 /*0x0400*/);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("cBioTemplate[MAX_BIO_TEMPLATE*2]", ": ", words8, index9);
  }

  private void ParseEnCcScpBioDbAddBin(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpBioDbAddBin (1235)");
    else
      this.InsertLine("enCcScpBioDbAddBin (3133)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int index4 = index3 + 1;
    this.InsertLine("card_number", ": ", words3, index3);
    int num4 = int.Parse(words[index4]);
    string[] words4 = words;
    int index5 = index4;
    int num5 = index5 + 1;
    this.InsertLine("nBioType", ": ", this.ParseBioType(words4, index5));
    string[] words5 = words;
    int index6 = num5;
    int num6 = index6 + 1;
    int bioType = num4;
    this.InsertLine("nFlags", ": ", this.ParseBioFlags(words5, index6, bioType));
    string[] words6 = words;
    int index7 = num6;
    int num7 = index7 + 1;
    this.InsertLine("nMinScore", ": ", words6, index7);
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("nTemplateSize", ": ", words7, index8, 0, 1024 /*0x0400*/);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("cBioTemplate[MAX_BIO_TEMPLATE_EXT]", ": ", words8, index9);
  }

  private void ParseEnCcSioAesControl(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcSioAesControl (1227)");
    else
      this.InsertLine("enCcSioAesControl (0127)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("sio_number", ": ", words2, index2, 0, 95);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nAesCommand", ": ", this.ParseSioAesCommand(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("cAesKey[128/8]", ": ", words4, index4);
  }

  private void ParseEnCcScpOfflineTime(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpOffLineTime (1324)");
    else
      this.InsertLine("enCcScpOffLineTime (0324)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("offline_time", ": ", words2, index2, 2000, (int) short.MaxValue);
  }

  private void ParseEnCcForcedOpenMask(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcForcedOpenMask (1309)");
    else
      this.InsertLine("enCcForcedOpenMask (0309)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("acr_number", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("set_clear", ": ", this.ParseSetClear(words3, index3));
  }

  private void ParseEnCcMpMask(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcMpMask (1306)");
    else
      this.InsertLine("enCcMpMask (0306)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("mp_number", ": ", words2, index2, 0, 2047 /*0x07FF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("set_clear", ": ", this.ParseSetClear(words3, index3));
  }

  private void ParseEnCcMemRead(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcMemRead (1851)");
    else
      this.InsertLine("enCcMemRead (1851)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nFirst", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCount", ": ", words3, index3);
  }

  private void ParseEnCcUnlock(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcUnlock (1311)");
    else
      this.InsertLine("enCcUnlock (0311)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("acr_number", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("floor_number", ": ", words3, index3, 0, (int) sbyte.MaxValue);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLineValidateRange("strk_tm", ": ", words4, index4, 0, (int) byte.MaxValue);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLineValidateRange("t_held", ": ", words5, index5, 0, (int) short.MaxValue);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLineValidateRange("t_held_pre", ": ", words6, index6, 0, (int) short.MaxValue);
  }

  private void ParseEnCcMpgSet(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcMpgSet (1321)");
    else
      this.InsertLine("enCcMpgSet (0321)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("mpg_number", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("command", ": ", this.ParseMpgSetCommand(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("arg1", ": ", words4, index4);
  }

  private void ParseEnCcMpg(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcMpg (1218)");
    else
      this.InsertLine("enCcMpg (0120)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int index4 = index3 + 1;
    this.InsertLineValidateRange("mpg_number", ": ", words3, index3, 0, (int) sbyte.MaxValue);
    int num4 = int.Parse(words[index4]);
    string[] words4 = words;
    int index5 = index4;
    int num5 = index5 + 1;
    this.InsertLineValidateRange("nMpCount", ": ", words4, index5, 0, 256 /*0x0100*/);
    if (num4 > 256 /*0x0100*/)
      num4 = 256 /*0x0100*/;
    this.InsertLine("");
    this.InsertLine("nMpList[MAX_MPPERMPG*2]:");
    for (int index6 = 0; index6 < num4; ++index6)
    {
      this.InsertLine("PointType", ": ", this.ParseIpsPointType(words, num5 + index6 * 2));
      this.InsertLine("PointNumber", ": ", words, num5 + index6 * 2 + 1);
      this.InsertLine("");
    }
  }

  private void ParseEnCcAccException(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    int num2;
    if (honeywell)
    {
      this.InsertLine("enNCcAccException (1220)");
      string[] words1 = words;
      int index1 = num1;
      int num3 = index1 + 1;
      this.InsertLine("lastModified", ": ", words1, index1);
      string[] words2 = words;
      int index2 = num3;
      num2 = index2 + 1;
      this.InsertLineValidateRange("mcb_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    }
    else
    {
      this.InsertLine("enCcAccException (1220)");
      string[] words3 = words;
      int index3 = num1;
      int num4 = index3 + 1;
      this.InsertLine("lastModified", ": ", words3, index3);
      string[] words4 = words;
      int index4 = num4;
      num2 = index4 + 1;
      this.InsertLineValidateRange("scp_number", ": ", words4, index4, 0, 16383 /*0x3FFF*/);
    }
    string[] words5 = words;
    int index5 = num2;
    int index6 = index5 + 1;
    this.InsertLine("nCardholderId", ": ", words5, index5);
    int num5 = int.Parse(words[index6]);
    string[] words6 = words;
    int index7 = index6;
    int num6 = index7 + 1;
    this.InsertLineValidateRange("nEntries", ": ", words6, index7, 0, 128 /*0x80*/);
    for (int index8 = 0; index8 < num5; ++index8)
    {
      this.InsertLine("");
      string[] words7 = words;
      int index9 = num6;
      int num7 = index9 + 1;
      this.InsertLineValidateRange("nAcrNumber", ": ", words7, index9, 0, (int) sbyte.MaxValue);
      string[] words8 = words;
      int index10 = num7;
      num6 = index10 + 1;
      this.InsertLineValidateRange("nTimezone", ": ", words8, index10, 0, (int) byte.MaxValue);
    }
  }

  private void ParseEnCcTzCommand(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcTzCommand (1314)");
    else
      this.InsertLine("enCcTzCommand (0314)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("tz_number", ": ", words2, index2, 0, (int) byte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("command", ": ", this.ParseTzCommand(words3, index3));
  }

  private void ParseEnCcArea(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcArea (1219)");
    else
      this.InsertLine("enCcArea (0121)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("area_number", ": ", words3, index3, 0, (int) sbyte.MaxValue);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("multi_occupancy", ": ", this.ParseMultipleOccupancyMode(words4, index4));
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("access_control", ": ", this.ParseAreaAccessControl(words5, index5));
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("occ_control", ": ", this.ParseOccControl(words6, index6));
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("occ_set", ": ", words7, index7);
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLine("occ_max", ": ", words8, index8);
    string[] words9 = words;
    int index9 = num9;
    int num10 = index9 + 1;
    this.InsertLine("occ_up", ": ", words9, index9);
    string[] words10 = words;
    int index10 = num10;
    int num11 = index10 + 1;
    this.InsertLine("occ_down", ": ", words10, index10);
  }

  private void ParseEnCcAreaSpc(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcAreaSpc (2219)");
    else
      this.InsertLine("enCcAreaSpc (1121)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("area_number", ": ", words3, index3, 0, (int) sbyte.MaxValue);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("multi_occupancy", ": ", this.ParseMultipleOccupancyMode(words4, index4));
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("access_control", ": ", this.ParseAreaAccessControl(words5, index5));
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("occ_control", ": ", this.ParseOccControl(words6, index6));
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("occ_set", ": ", words7, index7);
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLine("occ_max", ": ", words8, index8);
    string[] words9 = words;
    int index9 = num9;
    int num10 = index9 + 1;
    this.InsertLine("occ_up", ": ", words9, index9);
    string[] words10 = words;
    int index10 = num10;
    int num11 = index10 + 1;
    this.InsertLine("occ_down", ": ", words10, index10);
    string[] words11 = words;
    int index11 = num11;
    int num12 = index11 + 1;
    this.InsertLine("occ_set_spc", ": ", words11, index11);
    string[] words12 = words;
    int index12 = num12;
    int num13 = index12 + 1;
    this.InsertLine("custodian", ": ", words12, index12);
    string[] words13 = words;
    int index13 = num13;
    int num14 = index13 + 1;
    this.InsertLine("nAppRqRlySio", ": ", words13, index13);
    string[] words14 = words;
    int index14 = num14;
    int num15 = index14 + 1;
    this.InsertLine("nAppRqRlyNum", ": ", words14, index14);
    string[] words15 = words;
    int index15 = num15;
    int num16 = index15 + 1;
    this.InsertLine("nAppRqRlyDly", ": ", words15, index15);
    string[] words16 = words;
    int index16 = num16;
    int num17 = index16 + 1;
    this.InsertLine("area_flags", ": ", this.ParseAreaFlags(words16, index16));
  }

  private void ParseEnCcAcrLedMode(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcAcrLedMode (1315)");
    else
      this.InsertLine("enCcAcrLedMode (0315)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("acr_number", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("led_mode", ": ", words3, index3, 1, 3);
  }

  private void ParseEnCcSioNetwork(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcSioNetwork (1228)");
    else
      this.InsertLine("enCcSioNetwork (0128)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("sio_number", ": ", words2, index2, 0, 95);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("cIpAddr[22]", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("cMacAddr[18]", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("nMode", ": ", this.ParseSioNetworkMode(words5, index5));
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("cHostname[64]", ": ", words6, index6);
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("iPort", ": ", words7, index7);
  }

  private void ParseEnCcLanguageCodePages(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcLanguageCodePages (1233)");
    else
      this.InsertLine("enCcLanguageCodePages (0129)");
    string[] words1 = words;
    int index1 = num1;
    int index2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    this.InsertLine("nCodePageTokens[MAX_LANGUAGES]", ": ", words, index2, 16 /*0x10*/);
    int num2 = index2 + 16 /*0x10*/;
  }

  private void ParseEnCcAcrExtendedLedDefault(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcAcrExtendedLedDefault (1234)");
    else
      this.InsertLine("enCcAcrExtendedLedDefault (0130)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("acr_number", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("LED_number", ": ", words3, index3, 0, 1);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLineValidateRange("ticks_on", ": ", words4, index4, 0, (int) byte.MaxValue);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLineValidateRange("ticks_off", ": ", words5, index5, 0, (int) byte.MaxValue);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("color_on_RGB", ": ", this.ParseRGBLEDColor(words6, index6));
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("color_off_RGB", ": ", this.ParseRGBLEDColor(words7, index7));
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLine("flags", ": ", this.ParseTempLEDFlags(words8, index8));
  }

  private void ParseEnCcCardDelete(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcCardDelete (NA)");
    else
      this.InsertLine("enCcCardDelete (0305)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("cardholder_id", ": ", words2, index2);
  }

  private void ParseEnCcCardDeleteDbl(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcCardDeleteDbl (NA)");
    else
      this.InsertLine("enCcCardDeleteDbl (2305)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("cardholder_id", ": ", words2, index2);
  }

  private void ParseEnCcCardDeleteI64(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcCardDeleteDbl (1305)");
    else
      this.InsertLine("enCcCardDeleteI64 (3305)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("cardholder_id", ": ", words2, index2);
  }

  private void ParseEnCcScpAsDel(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpAsDel (1604)");
    else
      this.InsertLine("enCcScpAsDel (1125)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int index3 = index2 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    this.InsertLine("cAssetId[MAX_ASSET_SIZE]", ": ", words, index3, 16 /*0x10*/);
    int num3 = index3 + 16 /*0x10*/;
  }

  private void ParseEnCcScpAsAddI64(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpAsAddI64 (1603)");
    else
      this.InsertLine("enCcScpAsAddI64 (3124)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int index3 = index2 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    this.InsertLine("cAssetId[MAX_ASSET_SIZE]", ": ", words, index3, 16 /*0x10*/);
    int index4 = index3 + 16 /*0x10*/;
    this.InsertLine("nOwners[MAX_OWNERS]", ": ", words, index4, 8);
    int index5 = index4 + 8;
    this.InsertLine("nAssetArg[MAX_ASSET_ARGS]", ": ", words, index5, 16 /*0x10*/);
    int num3 = index5 + 16 /*0x10*/;
  }

  private void ParseEnCcScpAsGroup(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpAsGroup (1602)");
    else
      this.InsertLine("enCcScpAsGroup (1126)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int index4 = index3 + 1;
    this.InsertLineValidateRange("nAssetGroup", ": ", words3, index3, 0, 65534);
    this.InsertLine("nClassList[MAX_ASSET_CLASS_PER_GROUP]", ": ", words, index4, 64 /*0x40*/);
    int num4 = index4 + 64 /*0x40*/;
  }

  private void ParseEnCcMsp1Srq(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcMsp1Srq (1403)");
    else
      this.InsertLine("enCcMsp1Srq (0403)");
    string[] words1 = words;
    int index = num1;
    int num2 = index + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index, 0, 16383 /*0x3FFF*/);
  }

  private void ParseEnCcScpAsdbSpec(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpAsdbSpec (1601)");
    else
      this.InsertLine("enCcScpAsdbSpec (1123)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nAssets", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLineValidateRange("nAssetIdSize", ": ", words4, index4, 0, 16 /*0x10*/);
    string[] words5 = words;
    int index5 = num5;
    int index6 = index5 + 1;
    this.InsertLineValidateRange("nOwners", ": ", words5, index5, 0, 8);
    this.InsertLine("nAssetArgId[MAX_ASSET_ARGS]", ": ", words, index6, 16 /*0x10*/);
    int num6 = index6 + 16 /*0x10*/;
    string[] words6 = words;
    int index7 = num6;
    int num7 = index7 + 1;
    this.InsertLineValidateRange("nAssetGroups", ": ", words6, index7, 0, (int) ushort.MaxValue);
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("nEntriesPerGroup", ": ", words7, index8, 0, 64 /*0x40*/);
  }

  private void ParseEnCcElAlvlSpc(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcElAlvlSpc (1501)");
    else
      this.InsertLine("enCcElAlvlSpc (0501)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("max_elalvl", ": ", words3, index3, 0, 256 /*0x0100*/);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLineValidateRange("max_floors", ": ", words4, index4, 0, 128 /*0x80*/);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("floor_offset", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("floor_flags", ": ", this.ParseFloorFlags(words6, index6));
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("max_cab_access", ": ", words7, index7);
  }

  private void ParseEnCcSioHexLoad(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcSioHexLoad (1328)");
    else
      this.InsertLine("enCcSioHexLoad (328)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int index3 = index2 + 1;
    this.InsertLineValidateRange("sio_number", ": ", words2, index2, 0, 95);
    int num3 = this.InsertLineWithText("file_name[200]", ": ", words, index3);
    int num4 = index3 + num3;
  }

  private void ParseEnCcSioSrq(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcSioSrq (1404)");
    else
      this.InsertLine("enCcSioSrq (0404)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("first", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("count", ": ", words3, index3);
  }

  private void ParseEnCcTranIndex(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcTranIndex (1303)");
    else
      this.InsertLine("enCcTranIndex (0303)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("tran_index", ": ", this.ParseTranIndex(words2, index2));
  }

  private void ParseEnCcScpUCmndBkgd(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpUCmndBkgd (1225)");
    else
      this.InsertLine("enCcScpUCmndBkgd (1144)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nAcr", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nBkgdSpec", ": ", this.ParseBkgdSpec(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int index5 = index4 + 1;
    this.InsertLineValidateRange("nBkgdIndex", ": ", words4, index4, 0, 7);
    int num5 = this.InsertLineWithText("cBkgdText[16]", ": ", words, index5);
    int num6 = index5 + num5;
  }

  private void ParseEnCcScpUCmndBurgX(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpUCmndBurgX (1224)");
    else
      this.InsertLine("enCcScpUCmndBurgX (1143)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nAcr", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nUCmndId", ": ", this.ParseUCmnd(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("nDfltMpg", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("nCMap0", ": ", this.ParseCMap(words5, index5));
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("nAccDelay", ": ", words6, index6);
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("nCMap1", ": ", this.ParseCMap(words7, index7));
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLine("nUsrLevel1", ": ", this.ParseBurgUsrLevel(words8, index8));
    string[] words9 = words;
    int index9 = num9;
    int num10 = index9 + 1;
    this.InsertLineValidateRange("nAccLevel1", ": ", words9, index9, -1, (int) sbyte.MaxValue);
    string[] words10 = words;
    int index10 = num10;
    int num11 = index10 + 1;
    this.InsertLine("nCMap2", ": ", this.ParseCMap(words10, index10));
    string[] words11 = words;
    int index11 = num11;
    int num12 = index11 + 1;
    this.InsertLine("nUsrLevel2", ": ", this.ParseBurgUsrLevel(words11, index11));
    string[] words12 = words;
    int index12 = num12;
    int num13 = index12 + 1;
    this.InsertLineValidateRange("nAccLevel2", ": ", words12, index12, -1, (int) sbyte.MaxValue);
    string[] words13 = words;
    int index13 = num13;
    int num14 = index13 + 1;
    this.InsertLine("nDisplaySpec", ": ", this.ParseDisplaySpec(words13, index13));
  }

  private void ParseEnCcIpsSet(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcIpsSet (1332)");
    else
      this.InsertLine("enCcIpsSet (0332)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int index3 = index2 + 1;
    this.InsertLineValidateRange("nIpsNumber", ": ", words2, index2, 0, (int) byte.MaxValue);
    int num3 = int.Parse(words[index3]);
    string[] words3 = words;
    int index4 = index3;
    int num4 = index4 + 1;
    this.InsertLine("nCommand", ": ", this.ParseIpsSetCommand(words3, index4));
    int nCommand1 = num3;
    string[] words4 = words;
    int index5 = num4;
    int num5 = index5 + 1;
    this.InsertLine("nArg1", ": ", this.ParseIpsSetCommandArgs(1, nCommand1, words4, index5));
    int nCommand2 = num3;
    string[] words5 = words;
    int index6 = num5;
    int num6 = index6 + 1;
    this.InsertLine("nArg2", ": ", this.ParseIpsSetCommandArgs(2, nCommand2, words5, index6));
    int nCommand3 = num3;
    string[] words6 = words;
    int index7 = num6;
    int num7 = index7 + 1;
    this.InsertLine("nArg3", ": ", this.ParseIpsSetCommandArgs(3, nCommand3, words6, index7));
  }

  private void ParseEnCcPointName(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcPointName (1126)");
    else
      this.InsertLine("enCcPointName (0125)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nUseNames", ": ", this.ParseUseNames(words2, index2));
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nPointType", ": ", this.ParsePointType(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int index5 = index4 + 1;
    this.InsertLine("nPointNumber", ": ", words4, index4);
    int num5 = this.InsertLineWithText("cPointName[32]", ": ", words, index5);
    int num6 = index5 + num5;
  }

  private void ParseEnCcIpsPoints(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcIpsPoints (3222)");
    else
      this.InsertLine("enCcIpsPoints (2222)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nIpsNumber", ": ", words2, index2, 0, (int) byte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int index4 = index3 + 1;
    this.InsertLine("nIpsPtAddCommand", ": ", this.ParseIpsPtAddCommand(words3, index3));
    int num4 = int.Parse(words[index4]);
    string[] words4 = words;
    int index5 = index4;
    int num5 = index5 + 1;
    this.InsertLineValidateRange("nIpsPtCount", ": ", words4, index5, 0, 128 /*0x80*/);
    for (int index6 = 0; index6 < num4; ++index6)
    {
      this.InsertLine("");
      string[] words5 = words;
      int index7 = num5;
      int num6 = index7 + 1;
      this.InsertLine("nPointType", ": ", this.ParseIpsPointType(words5, index7));
      string[] words6 = words;
      int index8 = num6;
      int num7 = index8 + 1;
      this.InsertLine("nPointNumber", ": ", words6, index8);
      string[] words7 = words;
      int index9 = num7;
      num5 = index9 + 1;
      this.InsertLine("nPointFlags", ": ", this.ParseIpsPointFlags(words7, index9));
    }
  }

  private void ParseEnCcIpsAlloc(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcIpsAlloc (3220)");
    else
      this.InsertLine("enCcIpsAlloc (2220)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nIpsCount", ": ", words2, index2, 0, 256 /*0x0100*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("nPtCount", ": ", words3, index3, 0, 128 /*0x80*/);
  }

  private void ParseEnCcUCmndMacro(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpUCmndMacro (1223)");
    else
      this.InsertLine("enCcScpUCmndMacro (1142)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nAcr", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nType", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("nKeySpec", ": ", this.ParseKeySpec(words4, index4));
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("cMacro[16]", ": ", words5, index5);
  }

  private void ParseEnCcIpsConfig(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcIpsConfig (3221)");
    else
      this.InsertLine("enCcIpsConfig (2221)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nIpsNumber", ": ", words2, index2, 0, (int) byte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nIpsDfltState", ": ", this.ParseIpsDfltState(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("nEntryDelay", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int index6 = index5 + 1;
    this.InsertLine("nExitDelay", ": ", words5, index5);
    this.InsertLine("nIpsProc[16]", ": ", words, index6, 16 /*0x10*/);
    int num6 = index6 + 16 /*0x10*/;
    string[] words6 = words;
    int index7 = num6;
    int num7 = index7 + 1;
    this.InsertLine("nArgList[0]", ": ", this.Parse2221ArgList0Flags(words6, index7));
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLine("nArgList[1]", ": ", words7, index8);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("nArgList[2]", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num9;
    int num10 = index10 + 1;
    this.InsertLine("nArgList[3]", ": ", words9, index10);
    string[] words10 = words;
    int index11 = num10;
    int num11 = index11 + 1;
    this.InsertLine("nArgList[4]", ": ", words10, index11);
    string[] words11 = words;
    int index12 = num11;
    int num12 = index12 + 1;
    this.InsertLine("nArgList[5]", ": ", words11, index12);
    string[] words12 = words;
    int index13 = num12;
    int num13 = index13 + 1;
    this.InsertLine("nArgList[6]", ": ", words12, index13);
    string[] words13 = words;
    int index14 = num13;
    int num14 = index14 + 1;
    this.InsertLine("nArgList[7]", ": ", words13, index14);
    string[] words14 = words;
    int index15 = num14;
    int num15 = index15 + 1;
    this.InsertLine("nSiaReporting", ": ", words14, index15);
    string[] words15 = words;
    int index16 = num15;
    int num16 = index16 + 1;
    this.InsertLine("nSiaAccountId", ": ", words15, index16);
    string[] words16 = words;
    int index17 = num16;
    int num17 = index17 + 1;
    this.InsertLine("nSiaAccountIdAlt", ": ", words16, index17);
    string[] words17 = words;
    int index18 = num17;
    int num18 = index18 + 1;
    this.InsertLine("nSiaGroupNum", ": ", words17, index18);
    string[] words18 = words;
    int index19 = num18;
    int num19 = index19 + 1;
    this.InsertLine("nSiaTimeout", ": ", words18, index19);
  }

  private void ParseEnCcReader(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcReader (1205)");
    else
      this.InsertLine("enCcReader (0112)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("sio_number", ": ", words3, index3, 0, 95);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLineValidateRange("reader", ": ", words4, index4, 0, 15);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("dt_fmt", ": ", this.ParseDtFmt(words5, index5));
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("keypad_mode", ": ", this.ParseKeypadMode(words6, index6));
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("led_drive_mode", ": ", this.ParseLedDriveMode(words7, index7));
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLine("osdp_flags", ": ", this.ParseOsdpFlags(words8, index8));
  }

  private void ParseEnCcCreateScp(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcCreateScpLn (2021)");
    else
      this.InsertLine("enCcCreateScpLn (1013)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nSCPId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("address", ": ", this.ParseScpAddress(words2, index2));
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCommAccess", ": ", this.ParseCType(words3, index3, false));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("e_max", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLineValidateRange("poll_delay", ": ", words5, index5, 0, 5000);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("cCommString[32]", ": ", words6, index6);
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("cPswdString[16]", ": ", words7, index7);
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLine("offline_time", ": ", words8, index8);
    string[] words9 = words;
    int index9 = num9;
    int num10 = index9 + 1;
    this.InsertLine("nAltPortEnable", ": ", words9, index9);
    string[] words10 = words;
    int index10 = num10;
    int num11 = index10 + 1;
    this.InsertLine("nAltPortCommAccess", ": ", this.ParseCType(words10, index10, false));
    string[] words11 = words;
    int index11 = num11;
    int num12 = index11 + 1;
    this.InsertLine("nAltPortE_max", ": ", words11, index11);
    string[] words12 = words;
    int index12 = num12;
    int num13 = index12 + 1;
    this.InsertLine("nAltPortPoll_delay", ": ", words12, index12);
    string[] words13 = words;
    int index13 = num13;
    int num14 = index13 + 1;
    this.InsertLine("cAltPortCommString[32]", ": ", words13, index13);
  }

  private void ParseEnCcCreateScpLn(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcCreateScpLn (2021)");
    else
      this.InsertLine("enCcCreateScpLn (1013)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nSCPId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("address", ": ", this.ParseScpAddress(words2, index2));
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nCommAccess", ": ", this.ParseCType(words3, index3, false));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("e_max", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLineValidateRange("poll_delay", ": ", words5, index5, 0, 5000);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("cCommString[128]", ": ", words6, index6);
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("cPswdString[16]", ": ", words7, index7);
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLine("offline_time", ": ", words8, index8);
    string[] words9 = words;
    int index9 = num9;
    int num10 = index9 + 1;
    this.InsertLine("nAltPortEnable", ": ", this.ParseZeroExpected(words9, index9));
    string[] words10 = words;
    int index10 = num10;
    int num11 = index10 + 1;
    this.InsertLine("nAltPortCommAccess", ": ", this.ParseZeroExpected(words10, index10));
    string[] words11 = words;
    int index11 = num11;
    int num12 = index11 + 1;
    this.InsertLine("nAltPortE_max", ": ", this.ParseZeroExpected(words11, index11));
    string[] words12 = words;
    int index12 = num12;
    int num13 = index12 + 1;
    this.InsertLine("nAltPortPoll_delay", ": ", this.ParseZeroExpected(words12, index12));
    string[] words13 = words;
    int index13 = num13;
    int num14 = index13 + 1;
    this.InsertLine("cAltPortCommString[128]", ": ", this.ParseNullExpected(words13, index13));
  }

  private void ParseEnCcAlvl(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    int num2;
    if (honeywell)
    {
      this.InsertLine("enNCcAlvl (1214)");
      string[] words1 = words;
      int index1 = num1;
      int num3 = index1 + 1;
      this.InsertLine("lastModified", ": ", words1, index1);
      string[] words2 = words;
      int index2 = num3;
      int num4 = index2 + 1;
      this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
      string[] words3 = words;
      int index3 = num4;
      int index4 = index3 + 1;
      this.InsertLineValidateRange("alvl_number", ": ", words3, index3, 0, 31999);
      this.InsertLineValidateRange("tz[]", ": ", words, index4, 128 /*0x80*/, -1, (int) byte.MaxValue);
      num2 = index4 + 128 /*0x80*/;
    }
    else
    {
      this.InsertLine("enCcAlvl (0116)");
      string[] words4 = words;
      int index5 = num1;
      int num5 = index5 + 1;
      this.InsertLine("lastModified", ": ", words4, index5);
      string[] words5 = words;
      int index6 = num5;
      int num6 = index6 + 1;
      this.InsertLineValidateRange("scp_number", ": ", words5, index6, 0, 16383 /*0x3FFF*/);
      string[] words6 = words;
      int index7 = num6;
      int index8 = index7 + 1;
      this.InsertLineValidateRange("alvl_number", ": ", words6, index7, 0, 31999);
      this.InsertLineValidateRange("tz[]", ": ", words, index8, 128 /*0x80*/, -1, (int) byte.MaxValue);
      num2 = index8 + 128 /*0x80*/;
    }
  }

  private void ParseEnCcAlvlEx(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    int num2;
    if (honeywell)
    {
      this.InsertLine("enNCcAlvlEx (2214)");
      string[] words1 = words;
      int index1 = num1;
      int num3 = index1 + 1;
      this.InsertLine("lastModified", ": ", words1, index1);
      string[] words2 = words;
      int index2 = num3;
      int num4 = index2 + 1;
      this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
      string[] words3 = words;
      int index3 = num4;
      int num5 = index3 + 1;
      this.InsertLineValidateRange("alvl_number", ": ", words3, index3, 0, 31999);
      string[] words4 = words;
      int index4 = num5;
      int index5 = index4 + 1;
      this.InsertLineValidateRange("oper_mode", ": ", words4, index4, 0, 7);
      this.InsertLineValidateRange("tz", ": ", words, index5, 128 /*0x80*/, -1, (int) byte.MaxValue);
      num2 = index5 + 128 /*0x80*/;
    }
    else
    {
      this.InsertLine("enCcAlvlEx (2116)");
      string[] words5 = words;
      int index6 = num1;
      int num6 = index6 + 1;
      this.InsertLine("lastModified", ": ", words5, index6);
      string[] words6 = words;
      int index7 = num6;
      int num7 = index7 + 1;
      this.InsertLineValidateRange("scp_number", ": ", words6, index7, 0, 16383 /*0x3FFF*/);
      string[] words7 = words;
      int index8 = num7;
      int num8 = index8 + 1;
      this.InsertLineValidateRange("alvl_number", ": ", words7, index8, 0, 31999);
      string[] words8 = words;
      int index9 = num8;
      int index10 = index9 + 1;
      this.InsertLineValidateRange("oper_mode", ": ", words8, index9, 0, 7);
      this.InsertLineValidateRange("tz", ": ", words, index10, 128 /*0x80*/, -1, (int) byte.MaxValue);
      num2 = index10 + 128 /*0x80*/;
    }
  }

  private void ParseEnCcCreateChannel(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcCreateChannel (1001)");
    else
      this.InsertLine("enCcCreateChannel (0012)");
    string[] words1 = words;
    int index1 = num1;
    int index2 = index1 + 1;
    this.InsertLine("nChannelId", ": ", words1, index1);
    int num2 = int.Parse(words[index2]);
    string[] words2 = words;
    int index3 = index2;
    int num3 = index3 + 1;
    this.InsertLine("cType", ": ", this.ParseCType(words2, index3, true));
    string[] words3 = words;
    int index4 = num3;
    int num4 = index4 + 1;
    int cType = num2;
    this.InsertLine("cPort", ": ", this.ParseCPort(words3, index4, cType));
    string[] words4 = words;
    int index5 = num4;
    int num5 = index5 + 1;
    this.InsertLine("baud_rate", ": ", words4, index5);
    string[] words5 = words;
    int index6 = num5;
    int num6 = index6 + 1;
    this.InsertLine("timer1 (SCP reply timeout)", ": ", words5, index6);
    string[] words6 = words;
    int index7 = num6;
    int num7 = index7 + 1;
    this.InsertLine("timer2 (TCP/IP retry connect interval)", ": ", words6, index7);
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLine("cModemId[64]", ": ", words7, index8);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("cRTSMode", ": ", this.ParseRtsMode(words8, index9));
  }

  private void ParseEnCcMsp1(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcMsp1 (1201)");
    else
      this.InsertLine("enCcMsp1 (0108)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("msp1_number", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("port_number", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("baud_rate", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("reply_time", ": ", words6, index6);
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("nProtocol", ": ", this.ParseNProtocol(words7, index7));
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLine("nDialect", ": ", this.ParseDialect(words8, index8));
  }

  private void ParseEnCcSystem(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcSystem (1001)");
    else
      this.InsertLine("enCcSystem (011)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("nPorts", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nScps", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("nTimezones", ": ", words3, index3, 0, (int) byte.MaxValue);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("nHolidays", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("bDirectMode", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int index7 = index6 + 1;
    this.InsertLine("debug_rq", ": ", this.ParseDebugRq(words6, index6));
    this.InsertLine("nDebugArg[4]", ": ", words, index7, 4);
    int num7 = index7 + 4;
  }

  private void ParseEnCcAdbSpec(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcAdbSpec (NA)");
    else
      this.InsertLine("enCcAdbSpec (0105)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nCards", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("nAlvl", ": ", words3, index3, 0, 128 /*0x80*/);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("nPinDigits", ": ", this.ParsePinDigits(words4, index4));
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLineValidateRange("bIssueCode", ": ", words5, index5, 0, 2);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLineValidateRange("bApbLocation", ": ", words6, index6, 0, 1);
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLineValidateRange("bActDate", ": ", words7, index7, 0, 2);
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLineValidateRange("bDeactDate", ": ", words8, index8, 0, 2);
    string[] words9 = words;
    int index9 = num9;
    int num10 = index9 + 1;
    this.InsertLineValidateRange("bVacationDate", ": ", words9, index9, 0, 1);
    string[] words10 = words;
    int index10 = num10;
    int num11 = index10 + 1;
    this.InsertLineValidateRange("bUpgradeDate", ": ", words10, index10, 0, 1);
    string[] words11 = words;
    int index11 = num11;
    int num12 = index11 + 1;
    this.InsertLineValidateRange("bUserLevel", ": ", words11, index11, 0, 7);
    string[] words12 = words;
    int index12 = num12;
    int num13 = index12 + 1;
    this.InsertLineValidateRange("bUseLimit", ": ", words12, index12, 0, 1);
    string[] words13 = words;
    int index13 = num13;
    int num14 = index13 + 1;
    this.InsertLineValidateRange("bSupportTimedApb", ": ", words13, index13, 0, 1);
    string[] words14 = words;
    int index14 = num14;
    int num15 = index14 + 1;
    this.InsertLineValidateRange("nTz", ": ", words14, index14, 0, 64 /*0x40*/);
    string[] words15 = words;
    int index15 = num15;
    int num16 = index15 + 1;
    this.InsertLineValidateRange("bAssetGroup", ": ", words15, index15, 0, 1);
    string[] words16 = words;
    int index16 = num16;
    int num17 = index16 + 1;
    this.InsertLine("nHostResponseTimeout", ": ", words16, index16);
    string[] words17 = words;
    int index17 = num17;
    int num18 = index17 + 1;
    this.InsertLineValidateRange("nMxmTypeIndex", ": ", words17, index17, 0, 0);
    string[] words18 = words;
    int index18 = num18;
    int num19 = index18 + 1;
    this.InsertLine("nAlvlUse4Arq", ": ", words18, index18);
  }

  private void ParseEnCcScpAdbSpec(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    int num2;
    if (honeywell)
    {
      this.InsertLine("enNCcScpAdbSpec (1103)");
      string[] words1 = words;
      int index1 = num1;
      int num3 = index1 + 1;
      this.InsertLine("lastModified", ": ", words1, index1);
      string[] words2 = words;
      int index2 = num3;
      int num4 = index2 + 1;
      this.InsertLineValidateRange("nMcbID", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
      string[] words3 = words;
      int index3 = num4;
      int num5 = index3 + 1;
      this.InsertLine("nCards", ": ", words3, index3);
      string[] words4 = words;
      int index4 = num5;
      int num6 = index4 + 1;
      this.InsertLineValidateRange("nAlvl", ": ", words4, index4, 0, 128 /*0x80*/);
      string[] words5 = words;
      int index5 = num6;
      int num7 = index5 + 1;
      this.InsertLine("nPinDigits", ": ", this.ParsePinDigits(words5, index5));
      string[] words6 = words;
      int index6 = num7;
      int num8 = index6 + 1;
      this.InsertLineValidateRange("bIssueCode", ": ", words6, index6, 0, 2);
      string[] words7 = words;
      int index7 = num8;
      int num9 = index7 + 1;
      this.InsertLineValidateRange("bApbLocation", ": ", words7, index7, 0, 1);
      string[] words8 = words;
      int index8 = num9;
      int num10 = index8 + 1;
      this.InsertLineValidateRange("bActDate", ": ", words8, index8, 0, 2);
      string[] words9 = words;
      int index9 = num10;
      int num11 = index9 + 1;
      this.InsertLineValidateRange("bDeactDate", ": ", words9, index9, 0, 2);
      string[] words10 = words;
      int index10 = num11;
      int num12 = index10 + 1;
      this.InsertLineValidateRange("bVacationDate", ": ", words10, index10, 0, 1);
      string[] words11 = words;
      int index11 = num12;
      int num13 = index11 + 1;
      this.InsertLineValidateRange("bUpgradeDate", ": ", words11, index11, 0, 1);
      string[] words12 = words;
      int index12 = num13;
      int num14 = index12 + 1;
      this.InsertLineValidateRange("bUserLevel", ": ", words12, index12, 0, 7);
      string[] words13 = words;
      int index13 = num14;
      int num15 = index13 + 1;
      this.InsertLineValidateRange("bUseLimit", ": ", words13, index13, 0, 1);
      string[] words14 = words;
      int index14 = num15;
      int num16 = index14 + 1;
      this.InsertLineValidateRange("bSupportTimedApb", ": ", words14, index14, 0, 1);
      string[] words15 = words;
      int index15 = num16;
      int num17 = index15 + 1;
      this.InsertLineValidateRange("nTz", ": ", words15, index15, 0, 64 /*0x40*/);
      string[] words16 = words;
      int index16 = num17;
      int num18 = index16 + 1;
      this.InsertLineValidateRange("bAssetGroup", ": ", words16, index16, 0, 1);
      string[] words17 = words;
      int index17 = num18;
      int num19 = index17 + 1;
      this.InsertLineValidateRange("bAccExceptionList", ": ", words17, index17, 0, 1);
      string[] words18 = words;
      int index18 = num19;
      int num20 = index18 + 1;
      this.InsertLine("nHostResponseTimeout", ": ", words18, index18);
      string[] words19 = words;
      int index19 = num20;
      int num21 = index19 + 1;
      this.InsertLine("nMxmTypeIndex", ": ", words19, index19);
      string[] words20 = words;
      int index20 = num21;
      int num22 = index20 + 1;
      this.InsertLine("nAlvlUse4Arq", ": ", words20, index20);
      string[] words21 = words;
      int index21 = num22;
      int num23 = index21 + 1;
      this.InsertLineValidateRange("nFreeformBlockSize", ": ", words21, index21, 0, 128 /*0x80*/);
      for (int index22 = 0; index22 < 8; ++index22)
      {
        if (num23 < ((IEnumerable<string>) words).Count<string>())
        {
          string[] words22 = words;
          int index23 = num23;
          int num24 = index23 + 1;
          this.InsertLine("nFieldType", ": ", this.ParseFreeFormFieldType(words22, index23));
          string[] words23 = words;
          int index24 = num24;
          num23 = index24 + 1;
          this.InsertLineValidateRange("nFieldSize", ": ", words23, index24, 0, 128 /*0x80*/);
        }
      }
      string[] words24 = words;
      int index25 = num23;
      int num25 = index25 + 1;
      this.InsertLineValidateRange("nEscortTimeout", ": ", words24, index25, 0, 60);
      string[] words25 = words;
      int index26 = num25;
      int num26 = index26 + 1;
      this.InsertLineValidateRange("nMultiCardTimeout", ": ", words25, index26, 0, 60);
      string[] words26 = words;
      int index27 = num26;
      num2 = index27 + 1;
      this.InsertLineValidateRange("nAssetTimeout", ": ", words26, index27, 0, 60);
    }
    else
    {
      this.InsertLine("enCcScpAdbSpec (1105)");
      string[] words27 = words;
      int index28 = num1;
      int num27 = index28 + 1;
      this.InsertLine("lastModified", ": ", words27, index28);
      string[] words28 = words;
      int index29 = num27;
      int num28 = index29 + 1;
      this.InsertLineValidateRange("nScpID", ": ", words28, index29, 0, 16383 /*0x3FFF*/);
      string[] words29 = words;
      int index30 = num28;
      int num29 = index30 + 1;
      this.InsertLine("nCards", ": ", words29, index30);
      string[] words30 = words;
      int index31 = num29;
      int num30 = index31 + 1;
      this.InsertLineValidateRange("nAlvl", ": ", words30, index31, 0, 128 /*0x80*/);
      string[] words31 = words;
      int index32 = num30;
      int num31 = index32 + 1;
      this.InsertLine("nPinDigits", ": ", this.ParsePinDigits(words31, index32));
      string[] words32 = words;
      int index33 = num31;
      int num32 = index33 + 1;
      this.InsertLineValidateRange("bIssueCode", ": ", words32, index33, 0, 2);
      string[] words33 = words;
      int index34 = num32;
      int num33 = index34 + 1;
      this.InsertLineValidateRange("bApbLocation", ": ", words33, index34, 0, 1);
      string[] words34 = words;
      int index35 = num33;
      int num34 = index35 + 1;
      this.InsertLineValidateRange("bActDate", ": ", words34, index35, 0, 2);
      string[] words35 = words;
      int index36 = num34;
      int num35 = index36 + 1;
      this.InsertLineValidateRange("bDeactDate", ": ", words35, index36, 0, 2);
      string[] words36 = words;
      int index37 = num35;
      int num36 = index37 + 1;
      this.InsertLineValidateRange("bVacationDate", ": ", words36, index37, 0, 1);
      string[] words37 = words;
      int index38 = num36;
      int num37 = index38 + 1;
      this.InsertLineValidateRange("bUpgradeDate", ": ", words37, index38, 0, 1);
      string[] words38 = words;
      int index39 = num37;
      int num38 = index39 + 1;
      this.InsertLineValidateRange("bUserLevel", ": ", words38, index39, 0, 7);
      string[] words39 = words;
      int index40 = num38;
      int num39 = index40 + 1;
      this.InsertLineValidateRange("bUseLimit", ": ", words39, index40, 0, 1);
      string[] words40 = words;
      int index41 = num39;
      int num40 = index41 + 1;
      this.InsertLineValidateRange("bSupportTimedApb", ": ", words40, index41, 0, 1);
      string[] words41 = words;
      int index42 = num40;
      int num41 = index42 + 1;
      this.InsertLineValidateRange("nTz", ": ", words41, index42, 0, 64 /*0x40*/);
      string[] words42 = words;
      int index43 = num41;
      int num42 = index43 + 1;
      this.InsertLineValidateRange("bAssetGroup", ": ", words42, index43, 0, 1);
      string[] words43 = words;
      int index44 = num42;
      int num43 = index44 + 1;
      this.InsertLine("nHostResponseTimeout", ": ", words43, index44);
      string[] words44 = words;
      int index45 = num43;
      int num44 = index45 + 1;
      this.InsertLine("nMxmTypeIndex", ": ", words44, index45);
      string[] words45 = words;
      int index46 = num44;
      int num45 = index46 + 1;
      this.InsertLine("nAlvlUse4Arq", ": ", words45, index46);
      string[] words46 = words;
      int index47 = num45;
      int num46 = index47 + 1;
      this.InsertLineValidateRange("nFreeformBlockSize", ": ", words46, index47, 0, 128 /*0x80*/);
      for (int index48 = 0; index48 < 8; ++index48)
      {
        if (num46 < ((IEnumerable<string>) words).Count<string>())
        {
          string[] words47 = words;
          int index49 = num46;
          int num47 = index49 + 1;
          this.InsertLine("nFieldType", ": ", this.ParseFreeFormFieldType(words47, index49));
          string[] words48 = words;
          int index50 = num47;
          num46 = index50 + 1;
          this.InsertLineValidateRange("nFieldSize", ": ", words48, index50, 0, 128 /*0x80*/);
        }
      }
      string[] words49 = words;
      int index51 = num46;
      int num48 = index51 + 1;
      this.InsertLineValidateRange("nEscortTimeout", ": ", words49, index51, 0, 60);
      string[] words50 = words;
      int index52 = num48;
      int num49 = index52 + 1;
      this.InsertLineValidateRange("nMultiCardTimeout", ": ", words50, index52, 0, 60);
      string[] words51 = words;
      int index53 = num49;
      int num50 = index53 + 1;
      this.InsertLineValidateRange("nAssetTimeout", ": ", words51, index53, 0, 60);
      string[] words52 = words;
      int index54 = num50;
      int num51 = index54 + 1;
      this.InsertLineValidateRange("bAccExceptionList", ": ", words52, index54, 0, 1);
      string[] words53 = words;
      int index55 = num51;
      num2 = index55 + 1;
      this.InsertLine("adbFlags", ": ", this.ParseAdbFlags(words53, index55));
    }
  }

  private void ParseEnCcAdbCard304(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enCcAdbCard (304)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("flags", ": ", this.ParseAdbcFlags(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("card_number", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("issue_code", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int index7 = index6 + 1;
    this.InsertLine("pin[MAX_PIN_STD+1]", ": ", words6, index6);
    this.InsertLineValidateRange("alvl[MAX_ALVL_STD]", ": ", words, index7, 6, -1, 31999);
    int num7 = index7 + 6;
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("apb_loc", ": ", words7, index8, -1, (int) sbyte.MaxValue);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("use_count", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num9;
    int num10 = index10 + 1;
    this.InsertLine("act_date", ": ", this.ParseTimeDays(words9, index10, 1900));
    string[] words10 = words;
    int index11 = num10;
    int num11 = index11 + 1;
    this.InsertLine("dact_date", ": ", this.ParseTimeDays(words10, index11, 1900));
    string[] words11 = words;
    int index12 = num11;
    int num12 = index12 + 1;
    this.InsertLine("vac_date", ": ", this.ParseTimeDays(words11, index12, 1900));
    string[] words12 = words;
    int index13 = num12;
    int num13 = index13 + 1;
    this.InsertLineValidateRange("vac_days", ": ", words12, index13, 0, (int) byte.MaxValue);
    string[] words13 = words;
    int index14 = num13;
    int num14 = index14 + 1;
    this.InsertLine("tmp_date", ": ", this.ParseTimeDays(words13, index14, 1900));
    string[] words14 = words;
    int index15 = num14;
    int num15 = index15 + 1;
    this.InsertLineValidateRange("tmp_days", ": ", words14, index15, 0, (int) byte.MaxValue);
    string[] words15 = words;
    int index16 = num15;
    int index17 = index16 + 1;
    this.InsertLine("user_level", ": ", words15, index16);
    this.InsertLineValidateRange("alvl_prec[MAX_ACR_PER_SCP]", ": ", words, index17, 64 /*0x40*/, -1, 254);
    int num16 = index17 + 64 /*0x40*/;
    string[] words16 = words;
    int index18 = num16;
    int num17 = index18 + 1;
    this.InsertLine("asset_group", ": ", words16, index18);
  }

  private void ParseEnCcAdbCard1304(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enCcAdbCard (1304)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("flags", ": ", this.ParseAdbcFlags(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("card_number", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("issue_code", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int index7 = index6 + 1;
    this.InsertLine("pin[MAX_PIN_STD+1]", ": ", words6, index6);
    this.InsertLineValidateRange("alvl[MAX_ALVL_EXTD]", ": ", words, index7, 32 /*0x20*/, -1, 31999);
    int num7 = index7 + 32 /*0x20*/;
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("apb_loc", ": ", words7, index8, -1, (int) sbyte.MaxValue);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("use_count", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num9;
    int num10 = index10 + 1;
    this.InsertLine("act_date", ": ", this.ParseTimeDays(words9, index10, 1900));
    string[] words10 = words;
    int index11 = num10;
    int num11 = index11 + 1;
    this.InsertLine("dact_date", ": ", this.ParseTimeDays(words10, index11, 1900));
    string[] words11 = words;
    int index12 = num11;
    int num12 = index12 + 1;
    this.InsertLine("vac_date", ": ", this.ParseTimeDays(words11, index12, 1900));
    string[] words12 = words;
    int index13 = num12;
    int num13 = index13 + 1;
    this.InsertLineValidateRange("vac_days", ": ", words12, index13, 0, (int) byte.MaxValue);
    string[] words13 = words;
    int index14 = num13;
    int num14 = index14 + 1;
    this.InsertLine("tmp_date", ": ", this.ParseTimeDays(words13, index14, 1900));
    string[] words14 = words;
    int index15 = num14;
    int num15 = index15 + 1;
    this.InsertLineValidateRange("tmp_days", ": ", words14, index15, 0, (int) byte.MaxValue);
    string[] words15 = words;
    int index16 = num15;
    int index17 = index16 + 1;
    this.InsertLine("user_level", ": ", words15, index16);
    this.InsertLineValidateRange("alvl_prec[MAX_ACR_PER_SCP]", ": ", words, index17, 64 /*0x40*/, -1, 254);
    int num16 = index17 + 64 /*0x40*/;
    string[] words16 = words;
    int index18 = num16;
    int num17 = index18 + 1;
    this.InsertLine("asset_group", ": ", words16, index18);
  }

  private void ParseEnNCcAdbCard1304(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enNCcAdbCard (1304)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("mcb_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("flags", ": ", this.ParseAdbcFlags(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("card_number", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("issue_code", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int index7 = index6 + 1;
    this.InsertLine("pin[8+1]", ": ", words6, index6);
    this.InsertLineValidateRange("alvl[MAX_ALVL_EXTD]", ": ", words, index7, 32 /*0x20*/, -1, 31999);
    int num7 = index7 + 32 /*0x20*/;
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("apb_loc", ": ", words7, index8, -1, (int) sbyte.MaxValue);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("use_count", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num9;
    int num10 = index10 + 1;
    this.InsertLine("act_date", ": ", this.ParseTimeDays(words9, index10, 1900));
    string[] words10 = words;
    int index11 = num10;
    int num11 = index11 + 1;
    this.InsertLine("dact_date", ": ", this.ParseTimeDays(words10, index11, 1900));
    string[] words11 = words;
    int index12 = num11;
    int num12 = index12 + 1;
    this.InsertLine("vac_date", ": ", this.ParseTimeDays(words11, index12, 1900));
    string[] words12 = words;
    int index13 = num12;
    int num13 = index13 + 1;
    this.InsertLineValidateRange("vac_days", ": ", words12, index13, 0, (int) byte.MaxValue);
    string[] words13 = words;
    int index14 = num13;
    int num14 = index14 + 1;
    this.InsertLine("tmp_date", ": ", this.ParseTimeDays(words13, index14, 1900));
    string[] words14 = words;
    int index15 = num14;
    int num15 = index15 + 1;
    this.InsertLineValidateRange("tmp_days", ": ", words14, index15, 0, (int) byte.MaxValue);
    string[] words15 = words;
    int index16 = num15;
    int index17 = index16 + 1;
    this.InsertLine("user_level", ": ", words15, index16);
    this.InsertLineValidateRange("alvl_prec[MAX_ACR_PER_CARD]", ": ", words, index17, 64 /*0x40*/, -1, 254);
    int num16 = index17 + 64 /*0x40*/;
    string[] words16 = words;
    int index18 = num16;
    int num17 = index18 + 1;
    this.InsertLine("asset_group", ": ", words16, index18);
  }

  private void ParseEnCcAdbCard2304(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enCcAdbCard (2304)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("flags", ": ", this.ParseAdbcFlags(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("card_number", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("issue_code", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int index7 = index6 + 1;
    this.InsertLine("pin[MAX_PIN_STD+1]", ": ", words6, index6);
    this.InsertLineValidateRange("alvl[MAX_ALVL_EXTD]", ": ", words, index7, 32 /*0x20*/, -1, 31999);
    int num7 = index7 + 32 /*0x20*/;
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("apb_loc", ": ", words7, index8, -1, (int) sbyte.MaxValue);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("use_count", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num9;
    int num10 = index10 + 1;
    this.InsertLine("act_date", ": ", this.ParseTimeDays(words9, index10, 1900));
    string[] words10 = words;
    int index11 = num10;
    int num11 = index11 + 1;
    this.InsertLine("dact_date", ": ", this.ParseTimeDays(words10, index11, 1900));
    string[] words11 = words;
    int index12 = num11;
    int num12 = index12 + 1;
    this.InsertLine("vac_date", ": ", this.ParseTimeDays(words11, index12, 1900));
    string[] words12 = words;
    int index13 = num12;
    int num13 = index13 + 1;
    this.InsertLineValidateRange("vac_days", ": ", words12, index13, 0, (int) byte.MaxValue);
    string[] words13 = words;
    int index14 = num13;
    int num14 = index14 + 1;
    this.InsertLine("tmp_date", ": ", this.ParseTimeDays(words13, index14, 1900));
    string[] words14 = words;
    int index15 = num14;
    int num15 = index15 + 1;
    this.InsertLineValidateRange("tmp_days", ": ", words14, index15, 0, (int) byte.MaxValue);
    string[] words15 = words;
    int index16 = num15;
    int index17 = index16 + 1;
    this.InsertLine("user_level", ": ", words15, index16);
    this.InsertLineValidateRange("alvl_prec[MAX_ACR_PER_SCP]", ": ", words, index17, 64 /*0x40*/, -1, 254);
    int num16 = index17 + 64 /*0x40*/;
    string[] words16 = words;
    int index18 = num16;
    int num17 = index18 + 1;
    this.InsertLine("asset_group", ": ", words16, index18);
  }

  private void ParseEnNCcAdbCard2304(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enNCcAdbCard (2304)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("mcb_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("flags", ": ", this.ParseAdbcFlags(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("card_number", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("issue_code", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int index7 = index6 + 1;
    this.InsertLine("pin[MAX_PIN_STD+1]", ": ", words6, index6);
    this.InsertLineValidateRange("alvl[MAX_ALVL_EXTD]", ": ", words, index7, 32 /*0x20*/, -1, 31999);
    int num7 = index7 + 32 /*0x20*/;
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("apb_loc", ": ", words7, index8, -1, (int) sbyte.MaxValue);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("use_count", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num9;
    int num10 = index10 + 1;
    this.InsertLine("act_date", ": ", this.ParseTimeDays(words9, index10, 1900));
    string[] words10 = words;
    int index11 = num10;
    int num11 = index11 + 1;
    this.InsertLine("dact_date", ": ", this.ParseTimeDays(words10, index11, 1900));
    string[] words11 = words;
    int index12 = num11;
    int num12 = index12 + 1;
    this.InsertLine("vac_date", ": ", this.ParseTimeDays(words11, index12, 1900));
    string[] words12 = words;
    int index13 = num12;
    int num13 = index13 + 1;
    this.InsertLineValidateRange("vac_days", ": ", words12, index13, 0, (int) byte.MaxValue);
    string[] words13 = words;
    int index14 = num13;
    int num14 = index14 + 1;
    this.InsertLine("tmp_date", ": ", this.ParseTimeDays(words13, index14, 1900));
    string[] words14 = words;
    int index15 = num14;
    int index16 = index15 + 1;
    this.InsertLineValidateRange("tmp_days", ": ", words14, index15, 0, (int) byte.MaxValue);
    this.InsertLine("user_level[MAX_ULVL]", ": ", words, index16, 8);
    int index17 = index16 + 8;
    this.InsertLineValidateRange("alvl_prec[MAX_ACR_PER_CARD]", ": ", words, index17, 64 /*0x40*/, -1, 254);
    int num15 = index17 + 64 /*0x40*/;
    string[] words15 = words;
    int index18 = num15;
    int num16 = index18 + 1;
    this.InsertLine("asset_group", ": ", words15, index18);
  }

  private void ParseEnCcAdbCard3304(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enCcAdbCard (3304)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("flags", ": ", this.ParseAdbcFlags(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("card_number", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("issue_code", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int index7 = index6 + 1;
    this.InsertLine("pin[MAX_PIN_EXTD+1]", ": ", words6, index6);
    this.InsertLineValidateRange("alvl[MAX_ALVL_EXTD]", ": ", words, index7, 32 /*0x20*/, -1, 31999);
    int num7 = index7 + 32 /*0x20*/;
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("apb_loc", ": ", words7, index8, -1, (int) sbyte.MaxValue);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("use_count", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num9;
    int num10 = index10 + 1;
    this.InsertLine("act_date", ": ", this.ParseTimeDays(words9, index10, 1900));
    string[] words10 = words;
    int index11 = num10;
    int num11 = index11 + 1;
    this.InsertLine("dact_date", ": ", this.ParseTimeDays(words10, index11, 1900));
    string[] words11 = words;
    int index12 = num11;
    int num12 = index12 + 1;
    this.InsertLine("vac_date", ": ", this.ParseTimeDays(words11, index12, 1900));
    string[] words12 = words;
    int index13 = num12;
    int num13 = index13 + 1;
    this.InsertLineValidateRange("vac_days", ": ", words12, index13, 0, (int) byte.MaxValue);
    string[] words13 = words;
    int index14 = num13;
    int num14 = index14 + 1;
    this.InsertLine("tmp_date", ": ", this.ParseTimeDays(words13, index14, 1900));
    string[] words14 = words;
    int index15 = num14;
    int num15 = index15 + 1;
    this.InsertLineValidateRange("tmp_days", ": ", words14, index15, 0, (int) byte.MaxValue);
    string[] words15 = words;
    int index16 = num15;
    int index17 = index16 + 1;
    this.InsertLine("user_level", ": ", words15, index16);
    this.InsertLineValidateRange("alvl_prec[MAX_ACR_PER_SCP]", ": ", words, index17, 64 /*0x40*/, -1, 254);
    int num16 = index17 + 64 /*0x40*/;
    string[] words16 = words;
    int index18 = num16;
    int num17 = index18 + 1;
    this.InsertLine("asset_group", ": ", words16, index18);
  }

  private void ParseEnCcAdbCard4304(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enCcAdbCard (4304)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("flags", ": ", this.ParseAdbcFlags(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("card_number", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("issue_code", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int index7 = index6 + 1;
    this.InsertLine("pin[MAX_PIN_EXTD+1]", ": ", words6, index6);
    this.InsertLineValidateRange("alvl[MAX_ALVL_EXTD]", ": ", words, index7, 32 /*0x20*/, -1, 31999);
    int num7 = index7 + 32 /*0x20*/;
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("apb_loc", ": ", words7, index8, -1, (int) sbyte.MaxValue);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("use_count", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num9;
    int num10 = index10 + 1;
    this.InsertLine("act_time", ": ", this.ParseTime(words9, index10, 1970));
    string[] words10 = words;
    int index11 = num10;
    int num11 = index11 + 1;
    this.InsertLine("dact_time", ": ", this.ParseTime(words10, index11, 1970));
    string[] words11 = words;
    int index12 = num11;
    int num12 = index12 + 1;
    this.InsertLine("vac_date", ": ", this.ParseTimeDays(words11, index12, 1900));
    string[] words12 = words;
    int index13 = num12;
    int num13 = index13 + 1;
    this.InsertLineValidateRange("vac_days", ": ", words12, index13, 0, (int) byte.MaxValue);
    string[] words13 = words;
    int index14 = num13;
    int num14 = index14 + 1;
    this.InsertLine("tmp_date", ": ", this.ParseTimeDays(words13, index14, 1900));
    string[] words14 = words;
    int index15 = num14;
    int index16 = index15 + 1;
    this.InsertLineValidateRange("tmp_days", ": ", words14, index15, 0, (int) byte.MaxValue);
    this.InsertLine("user_level[MAX_ULVL]", ": ", words, index16, 8);
    int index17 = index16 + 8;
    this.InsertLineValidateRange("alvl_prec[MAX_ACR_PER_SCP]", ": ", words, index17, 64 /*0x40*/, -1, 254);
    int num15 = index17 + 64 /*0x40*/;
    string[] words15 = words;
    int index18 = num15;
    int num16 = index18 + 1;
    this.InsertLine("asset_group", ": ", words15, index18);
  }

  private void ParseEnNCcAdbCard4304(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enNCcAdbCard (4304)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("mcb_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("flags", ": ", this.ParseAdbcFlags(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("card_number", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("issue_code", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int index7 = index6 + 1;
    this.InsertLine("pin[MAX_PIN_EXTD+1]", ": ", words6, index6);
    this.InsertLineValidateRange("alvl[MAX_ALVL_128]", ": ", words, index7, 128 /*0x80*/, -1, 31999);
    int num7 = index7 + 128 /*0x80*/;
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("apb_loc", ": ", words7, index8, -1, (int) sbyte.MaxValue);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("use_count", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num9;
    int num10 = index10 + 1;
    this.InsertLine("act_time", ": ", this.ParseTime(words9, index10, 1970));
    string[] words10 = words;
    int index11 = num10;
    int num11 = index11 + 1;
    this.InsertLine("dact_time", ": ", this.ParseTime(words10, index11, 1970));
    string[] words11 = words;
    int index12 = num11;
    int num12 = index12 + 1;
    this.InsertLine("vac_date", ": ", this.ParseTimeDays(words11, index12, 1900));
    string[] words12 = words;
    int index13 = num12;
    int num13 = index13 + 1;
    this.InsertLineValidateRange("vac_days", ": ", words12, index13, 0, (int) byte.MaxValue);
    string[] words13 = words;
    int index14 = num13;
    int num14 = index14 + 1;
    this.InsertLine("tmp_date", ": ", this.ParseTimeDays(words13, index14, 1900));
    string[] words14 = words;
    int index15 = num14;
    int index16 = index15 + 1;
    this.InsertLineValidateRange("tmp_days", ": ", words14, index15, 0, (int) byte.MaxValue);
    this.InsertLine("user_level[MAX_ULVL]", ": ", words, index16, 8);
    int index17 = index16 + 8;
    this.InsertLineValidateRange("alvl_prec[MAX_ACR_PER_CARD]", ": ", words, index17, 64 /*0x40*/, -1, 254);
    int num15 = index17 + 64 /*0x40*/;
    string[] words15 = words;
    int index18 = num15;
    int num16 = index18 + 1;
    this.InsertLine("asset_group", ": ", words15, index18);
  }

  private void ParseEnCcAdbCard5304(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enCcAdbCard (5304)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("flags", ": ", this.ParseAdbcFlags(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("card_number", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("issue_code", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int index7 = index6 + 1;
    this.InsertLine("pin[MAX_PIN_EXTD+1]", ": ", words6, index6);
    this.InsertLineValidateRange("alvl[MAX_ALVL_EXTD]", ": ", words, index7, 32 /*0x20*/, -1, 31999);
    int num7 = index7 + 32 /*0x20*/;
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("apb_loc", ": ", words7, index8, -1, (int) sbyte.MaxValue);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("use_count", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num9;
    int num10 = index10 + 1;
    this.InsertLine("act_time", ": ", this.ParseTime(words9, index10, 1970));
    string[] words10 = words;
    int index11 = num10;
    int num11 = index11 + 1;
    this.InsertLine("dact_time", ": ", this.ParseTime(words10, index11, 1970));
    string[] words11 = words;
    int index12 = num11;
    int num12 = index12 + 1;
    this.InsertLine("vac_date", ": ", this.ParseTimeDays(words11, index12, 1900));
    string[] words12 = words;
    int index13 = num12;
    int num13 = index13 + 1;
    this.InsertLineValidateRange("vac_days", ": ", words12, index13, 0, (int) byte.MaxValue);
    string[] words13 = words;
    int index14 = num13;
    int num14 = index14 + 1;
    this.InsertLine("tmp_date", ": ", this.ParseTimeDays(words13, index14, 1900));
    string[] words14 = words;
    int index15 = num14;
    int index16 = index15 + 1;
    this.InsertLineValidateRange("tmp_days", ": ", words14, index15, 0, (int) byte.MaxValue);
    this.InsertLine("user_level[MAX_ULVL]", ": ", words, index16, 8);
    int index17 = index16 + 8;
    this.InsertLineValidateRange("alvl_prec[MAX_ACR_PER_SCP]", ": ", words, index17, 64 /*0x40*/, -1, 254);
    int num15 = index17 + 64 /*0x40*/;
    string[] words15 = words;
    int index18 = num15;
    int num16 = index18 + 1;
    this.InsertLine("asset_group", ": ", words15, index18);
  }

  private void ParseEnNCcAdbCard5304(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enNCcAdbCard (5304)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("mcb_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("flags", ": ", this.ParseAdbcFlags(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("card_number", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("issue_code", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int index7 = index6 + 1;
    this.InsertLine("pin[MAX_PIN_EXTD+1]", ": ", words6, index6);
    this.InsertLineValidateRange("alvl[MAX_ALVL_128]", ": ", words, index7, 128 /*0x80*/, -1, 31999);
    int num7 = index7 + 128 /*0x80*/;
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("apb_loc", ": ", words7, index8, -1, (int) sbyte.MaxValue);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("use_count", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num9;
    int num10 = index10 + 1;
    this.InsertLine("act_time", ": ", this.ParseTime(words9, index10, 1970));
    string[] words10 = words;
    int index11 = num10;
    int num11 = index11 + 1;
    this.InsertLine("dact_time", ": ", this.ParseTime(words10, index11, 1970));
    string[] words11 = words;
    int index12 = num11;
    int num12 = index12 + 1;
    this.InsertLine("vac_date", ": ", this.ParseTimeDays(words11, index12, 1900));
    string[] words12 = words;
    int index13 = num12;
    int num13 = index13 + 1;
    this.InsertLineValidateRange("vac_days", ": ", words12, index13, 0, (int) byte.MaxValue);
    string[] words13 = words;
    int index14 = num13;
    int num14 = index14 + 1;
    this.InsertLine("tmp_date", ": ", this.ParseTimeDays(words13, index14, 1900));
    string[] words14 = words;
    int index15 = num14;
    int index16 = index15 + 1;
    this.InsertLineValidateRange("tmp_days", ": ", words14, index15, 0, (int) byte.MaxValue);
    this.InsertLine("user_level[MAX_ULVL]", ": ", words, index16, 8);
    int index17 = index16 + 8;
    this.InsertLineValidateRange("alvl_prec[MAX_ACR_PER_CARD]", ": ", words, index17, 64 /*0x40*/, -1, 254);
    int num15 = index17 + 64 /*0x40*/;
    string[] words15 = words;
    int index18 = num15;
    int index19 = index18 + 1;
    this.InsertLine("asset_group", ": ", words15, index18);
    this.InsertLine("freeform", ": ", words, index19, 1024 /*0x0400*/);
    int num16 = index19 + 1024 /*0x0400*/;
  }

  private void ParseEnNCcAdbCard6304(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enNCcAdbCard (6304)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("mcb_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("flags", ": ", this.ParseAdbcFlags(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("card_number", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("issue_code", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int index7 = index6 + 1;
    this.InsertLine("pin[MAX_PIN_EXTD+1]", ": ", words6, index6);
    this.InsertLineValidateRange("alvl[MAX_ALVL_255]", ": ", words, index7, (int) byte.MaxValue, -1, 31999);
    int num7 = index7 + (int) byte.MaxValue;
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("apb_loc", ": ", words7, index8, -1, (int) sbyte.MaxValue);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("use_count", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num9;
    int num10 = index10 + 1;
    this.InsertLine("act_time", ": ", this.ParseTime(words9, index10, 1970));
    string[] words10 = words;
    int index11 = num10;
    int num11 = index11 + 1;
    this.InsertLine("dact_time", ": ", this.ParseTime(words10, index11, 1970));
    string[] words11 = words;
    int index12 = num11;
    int num12 = index12 + 1;
    this.InsertLine("vac_date", ": ", this.ParseTimeDays(words11, index12, 1900));
    string[] words12 = words;
    int index13 = num12;
    int num13 = index13 + 1;
    this.InsertLineValidateRange("vac_days", ": ", words12, index13, 0, (int) byte.MaxValue);
    string[] words13 = words;
    int index14 = num13;
    int num14 = index14 + 1;
    this.InsertLine("tmp_date", ": ", this.ParseTimeDays(words13, index14, 1900));
    string[] words14 = words;
    int index15 = num14;
    int index16 = index15 + 1;
    this.InsertLineValidateRange("tmp_days", ": ", words14, index15, 0, (int) byte.MaxValue);
    this.InsertLine("user_level[MAX_ULVL]", ": ", words, index16, 8);
    int index17 = index16 + 8;
    this.InsertLineValidateRange("alvl_prec[MAX_ACR_PER_CARD]", ": ", words, index17, 64 /*0x40*/, -1, 254);
    int num15 = index17 + 64 /*0x40*/;
    string[] words15 = words;
    int index18 = num15;
    int index19 = index18 + 1;
    this.InsertLine("asset_group", ": ", words15, index18);
    this.InsertLine("freeform", ": ", words, index19, 1024 /*0x0400*/);
    int num16 = index19 + 1024 /*0x0400*/;
  }

  private void ParseEnCcAdbCard6304(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enCcAdbCard (6304)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("flags", ": ", this.ParseAdbcFlags(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("card_number", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("issue_code", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int index7 = index6 + 1;
    this.InsertLine("pin[MAX_PIN_EXTD+1]", ": ", words6, index6);
    this.InsertLineValidateRange("alvl[MAX_ALVL_128]", ": ", words, index7, 128 /*0x80*/, -1, 31999);
    int num7 = index7 + 128 /*0x80*/;
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("apb_loc", ": ", words7, index8, -1, (int) sbyte.MaxValue);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("use_count", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num9;
    int num10 = index10 + 1;
    this.InsertLine("act_time", ": ", this.ParseTime(words9, index10, 1970));
    string[] words10 = words;
    int index11 = num10;
    int num11 = index11 + 1;
    this.InsertLine("dact_time", ": ", this.ParseTime(words10, index11, 1970));
    string[] words11 = words;
    int index12 = num11;
    int num12 = index12 + 1;
    this.InsertLine("vac_date", ": ", this.ParseTimeDays(words11, index12, 1900));
    string[] words12 = words;
    int index13 = num12;
    int num13 = index13 + 1;
    this.InsertLineValidateRange("vac_days", ": ", words12, index13, 0, (int) byte.MaxValue);
    string[] words13 = words;
    int index14 = num13;
    int num14 = index14 + 1;
    this.InsertLine("tmp_date", ": ", this.ParseTimeDays(words13, index14, 1900));
    string[] words14 = words;
    int index15 = num14;
    int index16 = index15 + 1;
    this.InsertLineValidateRange("tmp_days", ": ", words14, index15, 0, (int) byte.MaxValue);
    this.InsertLine("user_level[MAX_ULVL]", ": ", words, index16, 8);
    int index17 = index16 + 8;
    this.InsertLineValidateRange("alvl_prec[MAX_ACR_PER_SCP]", ": ", words, index17, 64 /*0x40*/, -1, 254);
    int num15 = index17 + 64 /*0x40*/;
    string[] words15 = words;
    int index18 = num15;
    int num16 = index18 + 1;
    this.InsertLine("asset_group", ": ", words15, index18);
  }

  private void ParseEnCcAdbCard7304(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enCcAdbCard (7304)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("flags", ": ", this.ParseAdbcFlags(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("card_number", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("issue_code", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int index7 = index6 + 1;
    this.InsertLine("pin[MAX_PIN_EXTD+1]", ": ", words6, index6);
    this.InsertLineValidateRange("alvl[MAX_ALVL_128]", ": ", words, index7, 128 /*0x80*/, -1, 31999);
    int num7 = index7 + 128 /*0x80*/;
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("apb_loc", ": ", words7, index8, -1, (int) sbyte.MaxValue);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("use_count", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num9;
    int num10 = index10 + 1;
    this.InsertLine("act_time", ": ", this.ParseTime(words9, index10, 1970));
    string[] words10 = words;
    int index11 = num10;
    int num11 = index11 + 1;
    this.InsertLine("dact_time", ": ", this.ParseTime(words10, index11, 1970));
    string[] words11 = words;
    int index12 = num11;
    int num12 = index12 + 1;
    this.InsertLine("vac_date", ": ", this.ParseTimeDays(words11, index12, 1900));
    string[] words12 = words;
    int index13 = num12;
    int num13 = index13 + 1;
    this.InsertLineValidateRange("vac_days", ": ", words12, index13, 0, (int) byte.MaxValue);
    string[] words13 = words;
    int index14 = num13;
    int num14 = index14 + 1;
    this.InsertLine("tmp_date", ": ", this.ParseTimeDays(words13, index14, 1900));
    string[] words14 = words;
    int index15 = num14;
    int index16 = index15 + 1;
    this.InsertLineValidateRange("tmp_days", ": ", words14, index15, 0, (int) byte.MaxValue);
    this.InsertLine("user_level[MAX_ULVL]", ": ", words, index16, 8);
    int index17 = index16 + 8;
    this.InsertLineValidateRange("alvl_prec[MAX_ACR_PER_SCP]", ": ", words, index17, 64 /*0x40*/, -1, 254);
    int num15 = index17 + 64 /*0x40*/;
    string[] words15 = words;
    int index18 = num15;
    int index19 = index18 + 1;
    this.InsertLine("asset_group", ": ", words15, index18);
    this.InsertLine("freeform", ": ", words, index19, 1024 /*0x0400*/);
    int num16 = index19 + 1024 /*0x0400*/;
  }

  private void ParseEnCcAdbCard8304(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enCcAdbCard (8304)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("flags", ": ", this.ParseAdbcFlags(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("card_number", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("issue_code", ": ", words5, index5);
    string[] words6 = words;
    int index6 = num6;
    int index7 = index6 + 1;
    this.InsertLine("pin[MAX_PIN_EXTD+1]", ": ", words6, index6);
    this.InsertLineValidateRange("alvl[MAX_ALVL_255]", ": ", words, index7, (int) byte.MaxValue, -1, 31999);
    int num7 = index7 + (int) byte.MaxValue;
    string[] words7 = words;
    int index8 = num7;
    int num8 = index8 + 1;
    this.InsertLineValidateRange("apb_loc", ": ", words7, index8, -1, (int) sbyte.MaxValue);
    string[] words8 = words;
    int index9 = num8;
    int num9 = index9 + 1;
    this.InsertLine("use_count", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num9;
    int num10 = index10 + 1;
    this.InsertLine("act_time", ": ", this.ParseTime(words9, index10, 1970));
    string[] words10 = words;
    int index11 = num10;
    int num11 = index11 + 1;
    this.InsertLine("dact_time", ": ", this.ParseTime(words10, index11, 1970));
    string[] words11 = words;
    int index12 = num11;
    int num12 = index12 + 1;
    this.InsertLine("vac_date", ": ", this.ParseTimeDays(words11, index12, 1900));
    string[] words12 = words;
    int index13 = num12;
    int num13 = index13 + 1;
    this.InsertLineValidateRange("vac_days", ": ", words12, index13, 0, (int) byte.MaxValue);
    string[] words13 = words;
    int index14 = num13;
    int num14 = index14 + 1;
    this.InsertLine("tmp_date", ": ", this.ParseTimeDays(words13, index14, 1900));
    string[] words14 = words;
    int index15 = num14;
    int index16 = index15 + 1;
    this.InsertLineValidateRange("tmp_days", ": ", words14, index15, 0, (int) byte.MaxValue);
    this.InsertLine("user_level[MAX_ULVL]", ": ", words, index16, 8);
    int index17 = index16 + 8;
    this.InsertLineValidateRange("alvl_prec[MAX_ACR_PER_SCP]", ": ", words, index17, 64 /*0x40*/, -1, 254);
    int num15 = index17 + 64 /*0x40*/;
    string[] words15 = words;
    int index18 = num15;
    int index19 = index18 + 1;
    this.InsertLine("asset_group", ": ", words15, index18);
    this.InsertLine("freeform", ": ", words, index19, 1024 /*0x0400*/);
    int num16 = index19 + 1024 /*0x0400*/;
  }

  private void ParseEnCcTrgr(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcTrgr (1215)");
    else
      this.InsertLine("enCcTrgr (0117)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("trgr_number", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("command", ": ", this.ParseTriggerCommand(words4, index4));
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLineValidateRange("proc_num", ": ", words5, index5, 0, 8195);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("src_type", ": ", this.ParseTransactionSourceType(words6, index6, false));
    string[] words7 = words;
    int index7 = num7;
    int index8 = index7 + 1;
    this.InsertLine("src_number", ": ", words7, index7);
    int num8 = int.Parse(words[index8]);
    string[] words8 = words;
    int index9 = index8;
    int num9 = index9 + 1;
    this.InsertLine("tran_type", ": ", this.ParseTranType(words8, index9));
    string[] words9 = words;
    int index10 = num9;
    int num10 = index10 + 1;
    this.InsertLine("code_map", ": ", this.ParseCodeMap1(words9, index10));
    string[] words10 = words;
    int index11 = num10;
    int index12 = index11 + 1;
    this.InsertLineValidateRange("timezone", ": ", words10, index11, 0, (int) byte.MaxValue);
    this.InsertLineValidateRange("trig_var[4]", ": ", words, index12, 4, -1, (int) sbyte.MaxValue);
    int num11 = index12 + 4;
    string[] words11 = words;
    int index13 = num11;
    int num12 = index13 + 1;
    int transType1 = num8;
    this.InsertLine("arg[0]", ": ", this.ParseTriggerArgs(words11, index13, 0, transType1));
    string[] words12 = words;
    int index14 = num12;
    int num13 = index14 + 1;
    int transType2 = num8;
    this.InsertLine("arg[1]", ": ", this.ParseTriggerArgs(words12, index14, 1, transType2));
    string[] words13 = words;
    int index15 = num13;
    int num14 = index15 + 1;
    int transType3 = num8;
    this.InsertLine("arg[2]", ": ", this.ParseTriggerArgs(words13, index15, 2, transType3));
    string[] words14 = words;
    int index16 = num14;
    int num15 = index16 + 1;
    int transType4 = num8;
    this.InsertLine("arg[3]", ": ", this.ParseTriggerArgs(words14, index16, 3, transType4));
  }

  private void ParseEnCcTrgr128(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcTrgr128 (2215)");
    else
      this.InsertLine("enCcTrgr128 (1117)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("trgr_number", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("command", ": ", this.ParseTriggerCommand(words4, index4));
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLineValidateRange("proc_num", ": ", words5, index5, 0, 8195);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("src_type", ": ", this.ParseTransactionSourceType(words6, index6, false));
    string[] words7 = words;
    int index7 = num7;
    int index8 = index7 + 1;
    this.InsertLine("src_number", ": ", words7, index7);
    int num8 = int.Parse(words[index8]);
    string[] words8 = words;
    int index9 = index8;
    int index10 = index9 + 1;
    this.InsertLine("tran_type", ": ", this.ParseTranType(words8, index9));
    this.InsertLine("code_map[2]", ": ", this.ParseCodeMap2(words, index10));
    int num9 = index10 + 2;
    string[] words9 = words;
    int index11 = num9;
    int index12 = index11 + 1;
    this.InsertLineValidateRange("timezone", ": ", words9, index11, 0, (int) byte.MaxValue);
    this.InsertLineValidateRange("trig_var[4]", ": ", words, index12, 4, -1, (int) sbyte.MaxValue);
    int num10 = index12 + 4;
    string[] words10 = words;
    int index13 = num10;
    int num11 = index13 + 1;
    int transType1 = num8;
    this.InsertLine("arg[0]", ": ", this.ParseTriggerArgs(words10, index13, 0, transType1));
    string[] words11 = words;
    int index14 = num11;
    int num12 = index14 + 1;
    int transType2 = num8;
    this.InsertLine("arg[1]", ": ", this.ParseTriggerArgs(words11, index14, 1, transType2));
    string[] words12 = words;
    int index15 = num12;
    int num13 = index15 + 1;
    int transType3 = num8;
    this.InsertLine("arg[2]", ": ", this.ParseTriggerArgs(words12, index15, 2, transType3));
    string[] words13 = words;
    int index16 = num13;
    int num14 = index16 + 1;
    int transType4 = num8;
    this.InsertLine("arg[3]", ": ", this.ParseTriggerArgs(words13, index16, 3, transType4));
  }

  private void ParseEnCcProc(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcProc (1216)");
    else
      this.InsertLine("enCcProc (0118)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int index4 = index3 + 1;
    this.InsertLineValidateRange("proc_number", ": ", words3, index3, 0, 8195);
    int num4 = int.Parse(words[index4]);
    string[] words4 = words;
    int index5 = index4;
    int num5 = index5 + 1;
    this.InsertLine("action_type", ": ", this.ParseActionType(words4, index5));
    this.InsertLine("");
    this.InsertLine("Action Details:");
    int num6;
    switch (num4 & (int) byte.MaxValue)
    {
      case 1:
        string[] words5 = words;
        int index6 = num5;
        int num7 = index6 + 1;
        this.InsertLine("    scp_number", ": ", words5, index6);
        string[] words6 = words;
        int index7 = num7;
        int num8 = index7 + 1;
        this.InsertLine("    mp_number", ": ", words6, index7);
        string[] words7 = words;
        int index8 = num8;
        num6 = index8 + 1;
        this.InsertLine("    set_clear", ": ", this.ParseSetClear(words7, index8));
        break;
      case 2:
        string[] words8 = words;
        int index9 = num5;
        int num9 = index9 + 1;
        this.InsertLine("    scp_number", ": ", words8, index9);
        string[] words9 = words;
        int index10 = num9;
        int num10 = index10 + 1;
        this.InsertLine("    cp_number", ": ", words9, index10);
        string[] words10 = words;
        int index11 = num10;
        int num11 = index11 + 1;
        this.InsertLine("    command", ": ", this.ParseCpCtlCommand(words10, index11));
        string[] words11 = words;
        int index12 = num11;
        int num12 = index12 + 1;
        this.InsertLine("    on_time", ": ", words11, index12);
        string[] words12 = words;
        int index13 = num12;
        int num13 = index13 + 1;
        this.InsertLine("    off_time", ": ", words12, index13);
        string[] words13 = words;
        int index14 = num13;
        num6 = index14 + 1;
        this.InsertLine("    repeat", ": ", words13, index14);
        break;
      case 3:
        string[] words14 = words;
        int index15 = num5;
        int num14 = index15 + 1;
        this.InsertLine("    scp_number", ": ", words14, index15);
        string[] words15 = words;
        int index16 = num14;
        int num15 = index16 + 1;
        this.InsertLineValidateRange("    acr_number", ": ", words15, index16, 0, (int) sbyte.MaxValue);
        string[] words16 = words;
        int index17 = num15;
        int num16 = index17 + 1;
        this.InsertLine("    acr_mode", ": ", this.ParseAcrMode(words16, index17));
        string[] words17 = words;
        int index18 = num16;
        int index19 = index18 + 1;
        this.InsertLine("    nAuthModFlags", ": ", words17, index18);
        int num17 = 0;
        if (index19 < ((IEnumerable<string>) words).Count<string>())
          num17 = (int) short.Parse(words[index19]);
        string[] words18 = words;
        int index20 = index19;
        int num18 = index20 + 1;
        this.InsertLine("    nExtFeatureType", ": ", this.ParseExtFeatureType(words18, index20));
        switch (num17)
        {
          case 1:
          case 2:
          case 3:
          case 4:
            string[] words19 = words;
            int index21 = num18;
            int num19 = index21 + 1;
            this.InsertLine("    iIPB_sio", ": ", words19, index21);
            string[] words20 = words;
            int index22 = num19;
            int num20 = index22 + 1;
            this.InsertLine("    iIPB_number", ": ", words20, index22);
            string[] words21 = words;
            int index23 = num20;
            int num21 = index23 + 1;
            this.InsertLine("    iIPB_long_press", ": ", words21, index23);
            string[] words22 = words;
            int index24 = num21;
            int num22 = index24 + 1;
            this.InsertLine("    iIPB_out_sio", ": ", words22, index24);
            string[] words23 = words;
            int index25 = num22;
            num6 = index25 + 1;
            this.InsertLine("    iIPB_out_num", ": ", words23, index25);
            return;
          case 5:
          case 6:
          case 7:
          case 8:
            string[] words24 = words;
            int index26 = num18;
            int num23 = index26 + 1;
            this.InsertLine("    ip_octet1", ": ", words24, index26);
            string[] words25 = words;
            int index27 = num23;
            int num24 = index27 + 1;
            this.InsertLine("    ip_octet2", ": ", words25, index27);
            string[] words26 = words;
            int index28 = num24;
            int num25 = index28 + 1;
            this.InsertLine("    ip_octet3", ": ", words26, index28);
            string[] words27 = words;
            int index29 = num25;
            num6 = index29 + 1;
            this.InsertLine("    ip_octet4", ": ", words27, index29);
            return;
          case 9:
            string[] words28 = words;
            int index30 = num18;
            int num26 = index30 + 1;
            this.InsertLine("    op_type", ": ", this.ParseKoneOpType(words28, index30));
            string[] words29 = words;
            int index31 = num26;
            int num27 = index31 + 1;
            this.InsertLine("    op_id", ": ", words29, index31);
            string[] words30 = words;
            int index32 = num27;
            int num28 = index32 + 1;
            this.InsertLine("    op_num", ": ", words30, index32);
            string[] words31 = words;
            int index33 = num28;
            int num29 = index33 + 1;
            this.InsertLine("    op_calltypes", ": ", words31, index33);
            string[] words32 = words;
            int index34 = num29;
            num6 = index34 + 1;
            this.InsertLine("    op_timeout", ": ", words32, index34);
            return;
          case 10:
            string[] words33 = words;
            int index35 = num18;
            int num30 = index35 + 1;
            this.InsertLine("    es_group_id", ": ", words33, index35);
            string[] words34 = words;
            int index36 = num30;
            int num31 = index36 + 1;
            this.InsertLine("    floor_id", ": ", words34, index36);
            string[] words35 = words;
            int index37 = num31;
            int num32 = index37 + 1;
            this.InsertLine("    ob_id", ": ", words35, index37);
            string[] words36 = words;
            int index38 = num32;
            int num33 = index38 + 1;
            this.InsertLine("    flags", ": ", this.ParseTKEFlags(words36, index38));
            string[] words37 = words;
            int index39 = num33;
            num6 = index39 + 1;
            this.InsertLine("    dedMode", ": ", this.ParseTKEDedModeFlags(words37, index39));
            return;
          case 11:
            string[] words38 = words;
            int index40 = num18;
            int num34 = index40 + 1;
            this.InsertLine("    bSeacDevNum", ": ", words38, index40);
            string[] words39 = words;
            int index41 = num34;
            int num35 = index41 + 1;
            this.InsertLine("    bOpdcDevNum", ": ", words39, index41);
            string[] words40 = words;
            int index42 = num35;
            int num36 = index42 + 1;
            this.InsertLine("    bFloorOffset", ": ", words40, index42);
            string[] words41 = words;
            int index43 = num36;
            int num37 = index43 + 1;
            this.InsertLine("    bVerificationLoc", ": ", this.ParseMitsuVeriLoc(words41, index43));
            string[] words42 = words;
            int index44 = num37;
            int num38 = index44 + 1;
            this.InsertLine("    bBoardingFrontRear", ": ", this.ParseMitsuBoardingEntrance(words42, index44));
            string[] words43 = words;
            int index45 = num38;
            int num39 = index45 + 1;
            this.InsertLine("    wBoardingFloor", ": ", words43, index45);
            string[] words44 = words;
            int index46 = num39;
            num6 = index46 + 1;
            this.InsertLine("    wCardReaderNum", ": ", words44, index46);
            return;
          default:
            return;
        }
      case 4:
        string[] words45 = words;
        int index47 = num5;
        int num40 = index47 + 1;
        this.InsertLine("    scp_number", ": ", words45, index47);
        string[] words46 = words;
        int index48 = num40;
        int num41 = index48 + 1;
        this.InsertLineValidateRange("    acr_number", ": ", words46, index48, 0, (int) sbyte.MaxValue);
        string[] words47 = words;
        int index49 = num41;
        num6 = index49 + 1;
        this.InsertLine("    set_clear", ": ", this.ParseSetClear(words47, index49));
        break;
      case 5:
        string[] words48 = words;
        int index50 = num5;
        int num42 = index50 + 1;
        this.InsertLine("    scp_number", ": ", words48, index50);
        string[] words49 = words;
        int index51 = num42;
        int num43 = index51 + 1;
        this.InsertLineValidateRange("    acr_number", ": ", words49, index51, 0, (int) sbyte.MaxValue);
        string[] words50 = words;
        int index52 = num43;
        num6 = index52 + 1;
        this.InsertLine("    set_clear", ": ", this.ParseSetClear(words50, index52));
        break;
      case 6:
        string[] words51 = words;
        int index53 = num5;
        int num44 = index53 + 1;
        this.InsertLine("    scp_number", ": ", words51, index53);
        string[] words52 = words;
        int index54 = num44;
        int num45 = index54 + 1;
        this.InsertLineValidateRange("    acr_number", ": ", words52, index54, 0, (int) sbyte.MaxValue);
        string[] words53 = words;
        int index55 = num45;
        int num46 = index55 + 1;
        this.InsertLineValidateRange("    floor_number", ": ", words53, index55, 0, (int) sbyte.MaxValue);
        string[] words54 = words;
        int index56 = num46;
        int num47 = index56 + 1;
        this.InsertLineValidateRange("    strk_tm", ": ", words54, index56, 0, (int) byte.MaxValue);
        string[] words55 = words;
        int index57 = num47;
        int num48 = index57 + 1;
        this.InsertLineValidateRange("    t_held", ": ", words55, index57, 0, (int) short.MaxValue);
        string[] words56 = words;
        int index58 = num48;
        num6 = index58 + 1;
        this.InsertLineValidateRange("    t_held_pre", ": ", words56, index58, 0, (int) short.MaxValue);
        break;
      case 7:
        string[] words57 = words;
        int index59 = num5;
        int num49 = index59 + 1;
        this.InsertLine("    scp_number", ": ", words57, index59);
        string[] words58 = words;
        int index60 = num49;
        int num50 = index60 + 1;
        this.InsertLineValidateRange("    proc_number", ": ", words58, index60, 0, 8195);
        string[] words59 = words;
        int index61 = num50;
        num6 = index61 + 1;
        this.InsertLine("    command", ": ", this.ParseProcedureCommand(words59, index61));
        break;
      case 8:
        string[] words60 = words;
        int index62 = num5;
        int num51 = index62 + 1;
        this.InsertLine("    scp_number", ": ", words60, index62);
        string[] words61 = words;
        int index63 = num51;
        int num52 = index63 + 1;
        this.InsertLineValidateRange("    tv_number", ": ", words61, index63, 1, (int) sbyte.MaxValue);
        string[] words62 = words;
        int index64 = num52;
        num6 = index64 + 1;
        this.InsertLine("    set_clear", ": ", this.ParseSetClear(words62, index64));
        break;
      case 9:
        string[] words63 = words;
        int index65 = num5;
        int num53 = index65 + 1;
        this.InsertLine("    scp_number", ": ", words63, index65);
        string[] words64 = words;
        int index66 = num53;
        int num54 = index66 + 1;
        this.InsertLineValidateRange("    tz_number", ": ", words64, index66, 0, (int) byte.MaxValue);
        string[] words65 = words;
        int index67 = num54;
        num6 = index67 + 1;
        this.InsertLine("    command", ": ", this.ParseTzCommand(words65, index67));
        break;
      case 10:
        string[] words66 = words;
        int index68 = num5;
        int num55 = index68 + 1;
        this.InsertLine("    scp_number", ": ", words66, index68);
        string[] words67 = words;
        int index69 = num55;
        int num56 = index69 + 1;
        this.InsertLineValidateRange("    acr_number", ": ", words67, index69, 0, (int) sbyte.MaxValue);
        string[] words68 = words;
        int index70 = num56;
        num6 = index70 + 1;
        this.InsertLineValidateRange("    led_mode", ": ", words68, index70, 1, 3);
        break;
      case 11:
        string[] words69 = words;
        int index71 = num5;
        int num57 = index71 + 1;
        this.InsertLine("    scp_number", ": ", words69, index71);
        string[] words70 = words;
        int index72 = num57;
        int num58 = index72 + 1;
        this.InsertLine("    cardholder_id", ": ", words70, index72);
        string[] words71 = words;
        int index73 = num58;
        int num59 = index73 + 1;
        this.InsertLineValidateRange("    apb_area", ": ", words71, index73, 0, (int) sbyte.MaxValue);
        string[] words72 = words;
        int index74 = num59;
        num6 = index74 + 1;
        this.InsertLine("    flags", ": ", this.ParseApbFreePassFlags(words72, index74));
        break;
      case 12:
        string[] words73 = words;
        int index75 = num5;
        num6 = index75 + 1 + this.InsertLineWithText("    dial_str[24]", ": ", words73, index75);
        break;
      case 13:
        string[] words74 = words;
        int index76 = num5;
        int num60 = index76 + 1;
        this.InsertLine("    scp_number", ": ", words74, index76);
        string[] words75 = words;
        int index77 = num60;
        int num61 = index77 + 1;
        this.InsertLine("    baud", ": ", words75, index77);
        string[] words76 = words;
        int index78 = num61;
        int num62 = index78 + 1;
        this.InsertLine("    port", ": ", words76, index78);
        string[] words77 = words;
        int index79 = num62;
        int num63 = index79 + 1;
        this.InsertLine("    channel", ": ", words77, index79);
        string[] words78 = words;
        int index80 = num63;
        num6 = index80 + 1;
        this.InsertLine("    data[128+1]", ": ", words78, index80);
        break;
      case 14:
        string[] words79 = words;
        int index81 = num5;
        int num64 = index81 + 1;
        this.InsertLine("    scp_number", ": ", words79, index81);
        string[] words80 = words;
        int index82 = num64;
        int num65 = index82 + 1;
        this.InsertLineValidateRange("    mpg_number", ": ", words80, index82, 0, (int) sbyte.MaxValue);
        string[] words81 = words;
        int index83 = num65;
        int num66 = index83 + 1;
        this.InsertLine("    command", ": ", this.ParseMpgSetCommand(words81, index83));
        string[] words82 = words;
        int index84 = num66;
        num6 = index84 + 1;
        this.InsertLine("    arg1", ": ", words82, index84);
        break;
      case 15:
        string[] words83 = words;
        int index85 = num5;
        int num67 = index85 + 1;
        this.InsertLine("    scp_number", ": ", words83, index85);
        string[] words84 = words;
        int index86 = num67;
        int num68 = index86 + 1;
        this.InsertLineValidateRange("    mpg_number", ": ", words84, index86, 0, (int) sbyte.MaxValue);
        string[] words85 = words;
        int index87 = num68;
        int num69 = index87 + 1;
        this.InsertLine("    action_prefix_ifz", ": ", words85, index87);
        string[] words86 = words;
        int index88 = num69;
        num6 = index88 + 1;
        this.InsertLine("    action_prefix_ifnz", ": ", words86, index88);
        break;
      case 16 /*0x10*/:
        string[] words87 = words;
        int index89 = num5;
        int num70 = index89 + 1;
        this.InsertLine("    scp_number", ": ", words87, index89);
        string[] words88 = words;
        int index90 = num70;
        int num71 = index90 + 1;
        this.InsertLineValidateRange("    mpg_number", ": ", words88, index90, 0, (int) sbyte.MaxValue);
        string[] words89 = words;
        int index91 = num71;
        int num72 = index91 + 1;
        this.InsertLine("    action_prefix_ifnoactive", ": ", words89, index91);
        string[] words90 = words;
        int index92 = num72;
        num6 = index92 + 1;
        this.InsertLine("    action_prefix_ifactive", ": ", words90, index92);
        break;
      case 17:
        string[] words91 = words;
        int index93 = num5;
        int num73 = index93 + 1;
        this.InsertLine("    scp_number", ": ", words91, index93);
        string[] words92 = words;
        int index94 = num73;
        int num74 = index94 + 1;
        this.InsertLineValidateRange("    area_number", ": ", words92, index94, 0, (int) sbyte.MaxValue);
        string[] words93 = words;
        int index95 = num74;
        int num75 = index95 + 1;
        this.InsertLine("    command", ": ", this.ParseAreaSetCommand(words93, index95));
        string[] words94 = words;
        int index96 = num75;
        num6 = index96 + 1;
        this.InsertLine("    occ_set", ": ", words94, index96);
        break;
      case 18:
        string[] words95 = words;
        int index97 = num5;
        int num76 = index97 + 1;
        this.InsertLine("    scp_number", ": ", words95, index97);
        string[] words96 = words;
        int index98 = num76;
        int num77 = index98 + 1;
        this.InsertLineValidateRange("    acr_number", ": ", words96, index98, 0, (int) sbyte.MaxValue);
        string[] words97 = words;
        int index99 = num77;
        int num78 = index99 + 1;
        this.InsertLineValidateRange("    floor_number", ": ", words97, index99, 0, (int) sbyte.MaxValue);
        string[] words98 = words;
        int index100 = num78;
        int num79 = index100 + 1;
        this.InsertLineValidateRange("    strk_tm", ": ", words98, index100, 0, (int) byte.MaxValue);
        string[] words99 = words;
        int index101 = num79;
        int num80 = index101 + 1;
        this.InsertLineValidateRange("    t_held", ": ", words99, index101, 0, (int) short.MaxValue);
        string[] words100 = words;
        int index102 = num80;
        num6 = index102 + 1;
        this.InsertLineValidateRange("    t_held_pre", ": ", words100, index102, 0, (int) short.MaxValue);
        break;
      case 19:
        string[] words101 = words;
        int index103 = num5;
        int num81 = index103 + 1;
        this.InsertLine("    scp_number", ": ", words101, index103);
        string[] words102 = words;
        int index104 = num81;
        int num82 = index104 + 1;
        this.InsertLineValidateRange("    acr_number", ": ", words102, index104, 0, (int) sbyte.MaxValue);
        string[] words103 = words;
        int index105 = num82;
        int num83 = index105 + 1;
        this.InsertLine("    color_on", ": ", this.ParseLEDColor(words103, index105));
        string[] words104 = words;
        int index106 = num83;
        int num84 = index106 + 1;
        this.InsertLine("    color_off", ": ", this.ParseLEDColor(words104, index106));
        string[] words105 = words;
        int index107 = num84;
        int num85 = index107 + 1;
        this.InsertLineValidateRange("    ticks_on", ": ", words105, index107, 0, (int) byte.MaxValue);
        string[] words106 = words;
        int index108 = num85;
        int num86 = index108 + 1;
        this.InsertLineValidateRange("    ticks_off", ": ", words106, index108, 0, (int) byte.MaxValue);
        string[] words107 = words;
        int index109 = num86;
        int num87 = index109 + 1;
        this.InsertLineValidateRange("    repeat", ": ", words107, index109, 1, (int) byte.MaxValue);
        string[] words108 = words;
        int index110 = num87;
        int num88 = index110 + 1;
        this.InsertLineValidateRange("    beeps", ": ", words108, index110, 0, 15);
        string[] words109 = words;
        int index111 = num88;
        int num89 = index111 + 1;
        this.InsertLine("LED_number", ": ", words109, index111);
        string[] words110 = words;
        int index112 = num89;
        int num90 = index112 + 1;
        this.InsertLine("color_on_RGB", ": ", this.ParseRGBLEDColor(words110, index112));
        string[] words111 = words;
        int index113 = num90;
        int num91 = index113 + 1;
        this.InsertLine("color_off_RGB", ": ", this.ParseRGBLEDColor(words111, index113));
        string[] words112 = words;
        int index114 = num91;
        num6 = index114 + 1;
        this.InsertLine("flags", ": ", this.ParseTempLEDFlags(words112, index114));
        break;
      case 20:
        string[] words113 = words;
        int index115 = num5;
        int num92 = index115 + 1;
        this.InsertLine("    scp_number", ": ", words113, index115);
        string[] words114 = words;
        int index116 = num92;
        int num93 = index116 + 1;
        this.InsertLineValidateRange("    term_number", ": ", words114, index116, 0, (int) sbyte.MaxValue);
        string[] words115 = words;
        int index117 = num93;
        int num94 = index117 + 1;
        this.InsertLine("    type", ": ", this.ParseLcdTextType(words115, index117));
        string[] words116 = words;
        int index118 = num94;
        int num95 = index118 + 1;
        this.InsertLineValidateRange("    temp_time", ": ", words116, index118, 0, 31 /*0x1F*/);
        string[] words117 = words;
        int index119 = num95;
        int num96 = index119 + 1;
        this.InsertLine("    tone", ": ", this.ParseLcdTextTones(words117, index119));
        string[] words118 = words;
        int index120 = num96;
        int num97 = index120 + 1;
        this.InsertLineValidateRange("    tone_time", ": ", words118, index120, 0, 31 /*0x1F*/);
        string[] words119 = words;
        int index121 = num97;
        int num98 = index121 + 1;
        this.InsertLineValidateRange("    row", ": ", words119, index121, 0, 1);
        string[] words120 = words;
        int index122 = num98;
        int index123 = index122 + 1;
        this.InsertLineValidateRange("    column", ": ", words120, index122, 0, 15);
        int num99 = this.InsertLineWithText("    text[64]", ": ", words, index123);
        int num100 = index123 + num99;
        string[] words121 = words;
        int index124 = num100;
        int num101 = index124 + 1;
        this.InsertLine("nLineIndex", ": ", words121, index124);
        string[] words122 = words;
        int index125 = num101;
        num6 = index125 + 1;
        this.InsertLine("nLanguageIndex", ": ", words122, index125);
        break;
      case 21:
        string[] words123 = words;
        int index126 = num5;
        num6 = index126 + 1 + this.InsertLineWithText("    dial_str_alt[24]", ": ", words123, index126);
        break;
      case 23:
        string[] words124 = words;
        int index127 = num5;
        int num102 = index127 + 1;
        this.InsertLine("    nScpId", ": ", words124, index127);
        string[] words125 = words;
        int index128 = num102;
        int index129 = index128 + 1;
        this.InsertLineValidateRange("    nIpsNumber", ": ", words125, index128, 0, (int) byte.MaxValue);
        int num103 = int.Parse(words[index129]);
        string[] words126 = words;
        int index130 = index129;
        int num104 = index130 + 1;
        this.InsertLine("    nCommand", ": ", this.ParseIpsSetCommand(words126, index130));
        int nCommand1 = num103;
        string[] words127 = words;
        int index131 = num104;
        int num105 = index131 + 1;
        this.InsertLine("    nArg1", ": ", this.ParseIpsSetCommandArgs(1, nCommand1, words127, index131));
        int nCommand2 = num103;
        string[] words128 = words;
        int index132 = num105;
        int num106 = index132 + 1;
        this.InsertLine("    nArg2", ": ", this.ParseIpsSetCommandArgs(2, nCommand2, words128, index132));
        int nCommand3 = num103;
        string[] words129 = words;
        int index133 = num106;
        num6 = index133 + 1;
        this.InsertLine("    nArg3", ": ", this.ParseIpsSetCommandArgs(3, nCommand3, words129, index133));
        break;
      case 24:
        string[] words130 = words;
        int index134 = num5;
        int num107 = index134 + 1;
        this.InsertLine("    scp_number", ": ", words130, index134);
        string[] words131 = words;
        int index135 = num107;
        int num108 = index135 + 1;
        this.InsertLineValidateRange("    acr_number", ": ", words131, index135, 0, (int) sbyte.MaxValue);
        string[] words132 = words;
        int index136 = num108;
        int num109 = index136 + 1;
        this.InsertLine("    acr_mode", ": ", this.ParseAcrMode(words132, index136));
        string[] words133 = words;
        int index137 = num109;
        int num110 = index137 + 1;
        this.InsertLine("    time", ": ", this.ParseTempAcrModeTime(words133, index137));
        string[] words134 = words;
        int index138 = num110;
        num6 = index138 + 1;
        this.InsertLine("    nAuthModFlags", ": ", words134, index138);
        break;
      case 25:
        string[] words135 = words;
        int index139 = num5;
        int num111 = index139 + 1;
        this.InsertLine("    nScp", ": ", words135, index139);
        string[] words136 = words;
        int index140 = num111;
        int num112 = index140 + 1;
        this.InsertLine("    nCommand", ": ", this.ParseCardSimCommand(words136, index140));
        string[] words137 = words;
        int index141 = num112;
        int num113 = index141 + 1;
        this.InsertLineValidateRange("    nAcr", ": ", words137, index141, 0, (int) sbyte.MaxValue);
        string[] words138 = words;
        int index142 = num113;
        int num114 = index142 + 1;
        this.InsertLine("    e_time", ": ", this.ParseTime(words138, index142, 1970));
        string[] words139 = words;
        int index143 = num114;
        int num115 = index143 + 1;
        this.InsertLine("    nFmtNum", ": ", words139, index143);
        string[] words140 = words;
        int index144 = num115;
        int num116 = index144 + 1;
        this.InsertLine("    nFacilityCode", ": ", words140, index144);
        string[] words141 = words;
        int index145 = num116;
        int num117 = index145 + 1;
        this.InsertLine("    nCardholderId", ": ", words141, index145);
        string[] words142 = words;
        int index146 = num117;
        num6 = index146 + 1;
        this.InsertLine("    nIssueCode", ": ", words142, index146);
        break;
      case 26:
        string[] words143 = words;
        int index147 = num5;
        int num118 = index147 + 1;
        this.InsertLine("    scp_number", ": ", words143, index147);
        string[] words144 = words;
        int index148 = num118;
        int num119 = index148 + 1;
        this.InsertLine("    cardholder_id", ": ", words144, index148);
        string[] words145 = words;
        int index149 = num119;
        num6 = index149 + 1;
        this.InsertLineValidateRange("    new_limit", ": ", words145, index149, -1, (int) byte.MaxValue);
        break;
      case 27:
        string[] words146 = words;
        int index150 = num5;
        int num120 = index150 + 1;
        this.InsertLine("    scp_number", ": ", words146, index150);
        string[] words147 = words;
        int index151 = num120;
        int num121 = index151 + 1;
        this.InsertLineValidateRange("    oper_mode", ": ", words147, index151, 0, 7);
        string[] words148 = words;
        int index152 = num121;
        int num122 = index152 + 1;
        this.InsertLineValidateRange("    enforce_existing", ": ", words148, index152, 0, 1);
        string[] words149 = words;
        int index153 = num122;
        num6 = index153 + 1;
        this.InsertLineValidateRange("    existing_mode", ": ", words149, index153, 0, 7);
        break;
      case 126:
        string[] words150 = words;
        int index154 = num5;
        num6 = index154 + 1;
        this.InsertLine("    delay (.1 sec)", ": ", words150, index154);
        break;
      case (int) sbyte.MaxValue:
        string[] words151 = words;
        int index155 = num5;
        num6 = index155 + 1;
        this.InsertLine("    delay (1 sec)", ": ", words151, index155);
        break;
    }
  }

  private void ParseEnCcAlvlSpc(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcAlvlSpc (1230)");
    else
      this.InsertLine("enCcAlvlSpc (0124)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("nScpNumber", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int index3 = index2 + 1;
    this.InsertLineValidateRange("nAlvlNumber", ": ", words2, index2, 0, 31999);
    int num3 = int.Parse(words[index3]);
    int num4 = 1;
    int num5;
    if (num3 == 0)
    {
      num5 = index3 + 1;
      num4 = 0;
      this.InsertLine("nActYear: 0 (use minimum activation date)");
    }
    else
    {
      string[] words3 = words;
      int index4 = index3;
      num5 = index4 + 1;
      this.InsertLine("nActYear", ": ", words3, index4);
    }
    string[] words4 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    int minVal1 = num4;
    this.InsertLineValidateRange("nActMonth", ": ", words4, index5, minVal1, 12);
    string[] words5 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    int minVal2 = num4;
    this.InsertLineValidateRange("nActDay", ": ", words5, index6, minVal2, 31 /*0x1F*/);
    string[] words6 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLineValidateRange("nActHh", ": ", words6, index7, 0, 23);
    string[] words7 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLineValidateRange("nActMm", ": ", words7, index8, 0, 59);
    string[] words8 = words;
    int index9 = num9;
    int index10 = index9 + 1;
    this.InsertLineValidateRange("nActSs", ": ", words8, index9, 0, 59);
    int num10 = int.Parse(words[index10]);
    int num11 = 1;
    int num12;
    if (num10 == 0)
    {
      num12 = index10 + 1;
      num11 = 0;
      this.InsertLine("nDactYear: 0 (use maximum deactivation date)");
    }
    else
    {
      string[] words9 = words;
      int index11 = index10;
      num12 = index11 + 1;
      this.InsertLine("nDactYear", ": ", words9, index11);
    }
    string[] words10 = words;
    int index12 = num12;
    int num13 = index12 + 1;
    int minVal3 = num11;
    this.InsertLineValidateRange("nDactMonth", ": ", words10, index12, minVal3, 12);
    string[] words11 = words;
    int index13 = num13;
    int num14 = index13 + 1;
    int minVal4 = num11;
    this.InsertLineValidateRange("nDactDay", ": ", words11, index13, minVal4, 31 /*0x1F*/);
    string[] words12 = words;
    int index14 = num14;
    int num15 = index14 + 1;
    this.InsertLineValidateRange("nDactHh", ": ", words12, index14, 0, 23);
    string[] words13 = words;
    int index15 = num15;
    int num16 = index15 + 1;
    this.InsertLineValidateRange("nDactMm", ": ", words13, index15, 0, 59);
    string[] words14 = words;
    int index16 = num16;
    int num17 = index16 + 1;
    this.InsertLineValidateRange("nDactSs", ": ", words14, index16, 0, 59);
    string[] words15 = words;
    int index17 = num17;
    int num18 = index17 + 1;
    this.InsertLine("nEscortCode", ": ", this.ParseEscortCode(words15, index17));
    string[] words16 = words;
    int index18 = num18;
    int num19 = index18 + 1;
    this.InsertLineValidateRange("oper_mode", ": ", words16, index18, 0, 7);
  }

  private void ParseEnCcCfmt(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcCfmt (NA)");
    else
      this.InsertLine("enCcCfmt (0102)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("number", ": ", words2, index2, 0, 15);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("facility", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int index5 = index4 + 1;
    this.InsertLine("offset", ": ", words4, index4);
    int num5 = int.Parse(words[index5]);
    string[] words5 = words;
    int index6 = index5;
    int num6 = index6 + 1;
    this.InsertLine("function_id", ": ", this.ParseCardFormatterFunction(words5, index6));
    int num7;
    switch (num5)
    {
      case 1:
      case 3:
        string[] words6 = words;
        int index7 = num6;
        int num8 = index7 + 1;
        this.InsertLine("flags", ": ", this.ParseCardFormatterFlags(words6, index7));
        string[] words7 = words;
        int index8 = num8;
        int num9 = index8 + 1;
        this.InsertLine("bits", ": ", words7, index8);
        string[] words8 = words;
        int index9 = num9;
        int num10 = index9 + 1;
        this.InsertLine("pe_ln", ": ", words8, index9);
        string[] words9 = words;
        int index10 = num10;
        int num11 = index10 + 1;
        this.InsertLine("pe_loc", ": ", words9, index10);
        string[] words10 = words;
        int index11 = num11;
        int num12 = index11 + 1;
        this.InsertLine("po_ln", ": ", words10, index11);
        string[] words11 = words;
        int index12 = num12;
        int num13 = index12 + 1;
        this.InsertLine("po_loc", ": ", words11, index12);
        string[] words12 = words;
        int index13 = num13;
        int num14 = index13 + 1;
        this.InsertLine("fc_ln", ": ", words12, index13);
        string[] words13 = words;
        int index14 = num14;
        int num15 = index14 + 1;
        this.InsertLine("fc_loc", ": ", words13, index14);
        string[] words14 = words;
        int index15 = num15;
        int num16 = index15 + 1;
        this.InsertLine("ch_ln", ": ", words14, index15);
        string[] words15 = words;
        int index16 = num16;
        int num17 = index16 + 1;
        this.InsertLine("ch_loc", ": ", words15, index16);
        string[] words16 = words;
        int index17 = num17;
        int num18 = index17 + 1;
        this.InsertLine("ic_ln", ": ", words16, index17);
        string[] words17 = words;
        int index18 = num18;
        num7 = index18 + 1;
        this.InsertLine("ic_loc", ": ", words17, index18);
        break;
      case 2:
        string[] words18 = words;
        int index19 = num6;
        int num19 = index19 + 1;
        this.InsertLine("flags", ": ", this.ParseCardFormatterMTAFlags(words18, index19));
        string[] words19 = words;
        int index20 = num19;
        int num20 = index20 + 1;
        this.InsertLine("min_digits", ": ", words19, index20);
        string[] words20 = words;
        int index21 = num20;
        int num21 = index21 + 1;
        this.InsertLine("max_digits", ": ", words20, index21);
        string[] words21 = words;
        int index22 = num21;
        int num22 = index22 + 1;
        this.InsertLine("fc_ln", ": ", words21, index22);
        string[] words22 = words;
        int index23 = num22;
        int num23 = index23 + 1;
        this.InsertLine("fc_loc", ": ", words22, index23);
        string[] words23 = words;
        int index24 = num23;
        int num24 = index24 + 1;
        this.InsertLine("ch_ln", ": ", words23, index24);
        string[] words24 = words;
        int index25 = num24;
        int num25 = index25 + 1;
        this.InsertLine("ch_loc", ": ", words24, index25);
        string[] words25 = words;
        int index26 = num25;
        int num26 = index26 + 1;
        this.InsertLine("ic_ln", ": ", words25, index26);
        string[] words26 = words;
        int index27 = num26;
        num7 = index27 + 1;
        this.InsertLine("ic_loc", ": ", words26, index27);
        break;
    }
  }

  private void ParseEnCcScpCfmt(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpCfmt (1105)");
    else
      this.InsertLine("enCcScpCfmt (1102)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("number", ": ", words3, index3, 0, 15);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("facility", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int index6 = index5 + 1;
    this.InsertLine("offset", ": ", words5, index5);
    int num6 = int.Parse(words[index6]);
    string[] words6 = words;
    int index7 = index6;
    int num7 = index7 + 1;
    this.InsertLine("function_id", ": ", this.ParseCardFormatterFunction(words6, index7));
    int num8;
    switch (num6)
    {
      case 1:
      case 3:
        string[] words7 = words;
        int index8 = num7;
        int num9 = index8 + 1;
        this.InsertLine("flags", ": ", this.ParseCardFormatterFlags(words7, index8));
        string[] words8 = words;
        int index9 = num9;
        int num10 = index9 + 1;
        this.InsertLine("bits", ": ", words8, index9);
        string[] words9 = words;
        int index10 = num10;
        int num11 = index10 + 1;
        this.InsertLine("pe_ln", ": ", words9, index10);
        string[] words10 = words;
        int index11 = num11;
        int num12 = index11 + 1;
        this.InsertLine("pe_loc", ": ", words10, index11);
        string[] words11 = words;
        int index12 = num12;
        int num13 = index12 + 1;
        this.InsertLine("po_ln", ": ", words11, index12);
        string[] words12 = words;
        int index13 = num13;
        int num14 = index13 + 1;
        this.InsertLine("po_loc", ": ", words12, index13);
        string[] words13 = words;
        int index14 = num14;
        int num15 = index14 + 1;
        this.InsertLine("fc_ln", ": ", words13, index14);
        string[] words14 = words;
        int index15 = num15;
        int num16 = index15 + 1;
        this.InsertLine("fc_loc", ": ", words14, index15);
        string[] words15 = words;
        int index16 = num16;
        int num17 = index16 + 1;
        this.InsertLine("ch_ln", ": ", words15, index16);
        string[] words16 = words;
        int index17 = num17;
        int num18 = index17 + 1;
        this.InsertLine("ch_loc", ": ", words16, index17);
        string[] words17 = words;
        int index18 = num18;
        int num19 = index18 + 1;
        this.InsertLine("ic_ln", ": ", words17, index18);
        string[] words18 = words;
        int index19 = num19;
        num8 = index19 + 1;
        this.InsertLine("ic_loc", ": ", words18, index19);
        break;
      case 2:
        string[] words19 = words;
        int index20 = num7;
        int num20 = index20 + 1;
        this.InsertLine("flags", ": ", this.ParseCardFormatterMTAFlags(words19, index20));
        string[] words20 = words;
        int index21 = num20;
        int num21 = index21 + 1;
        this.InsertLine("min_digits", ": ", words20, index21);
        string[] words21 = words;
        int index22 = num21;
        int num22 = index22 + 1;
        this.InsertLine("max_digits", ": ", words21, index22);
        string[] words22 = words;
        int index23 = num22;
        int num23 = index23 + 1;
        this.InsertLine("fc_ln", ": ", words22, index23);
        string[] words23 = words;
        int index24 = num23;
        int num24 = index24 + 1;
        this.InsertLine("fc_loc", ": ", words23, index24);
        string[] words24 = words;
        int index25 = num24;
        int num25 = index25 + 1;
        this.InsertLine("ch_ln", ": ", words24, index25);
        string[] words25 = words;
        int index26 = num25;
        int num26 = index26 + 1;
        this.InsertLine("ch_loc", ": ", words25, index26);
        string[] words26 = words;
        int index27 = num26;
        int num27 = index27 + 1;
        this.InsertLine("ic_ln", ": ", words26, index27);
        string[] words27 = words;
        int index28 = num27;
        num8 = index28 + 1;
        this.InsertLine("ic_loc", ": ", words27, index28);
        break;
    }
  }

  private void ParseSimpleCommand(string[] words, int numWords, bool honeywell)
  {
    int cmdIndex = this.GetCmdIndex();
    string[] strArray = words;
    int index1 = cmdIndex;
    int num1 = index1 + 1;
    int num2 = int.Parse(strArray[index1]);
    if (honeywell)
    {
      switch (num2)
      {
        case 1301:
          this.InsertLine("enNCcReset (1301)");
          break;
        case 1401:
          this.InsertLine("enNCcIDRequest (1401)");
          break;
        case 1402:
          this.InsertLine("enNCcTranSrq (1402)");
          break;
        case 1410:
          this.InsertLine("enNCcUtagRequest (1410)");
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          this.InsertLine("UNKNOWN");
          break;
      }
    }
    else
    {
      switch (num2)
      {
        case 301:
          this.InsertLine("enCcReset (0301)");
          break;
        case 401:
          this.InsertLine("enCcIDRequest (0401)");
          break;
        case 402:
          this.InsertLine("enCcTranSrq (0402)");
          break;
        case 410:
          this.InsertLine("enCcUtagRequest (0410)");
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          this.InsertLine("UNKNOWN");
          break;
      }
    }
    string[] words1 = words;
    int index2 = num1;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index2, 0, 16383 /*0x3FFF*/);
  }

  private void ParseEnCcAcrMode(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcAcrMode (1308)");
    else
      this.InsertLine("enCcAcrMode (0308)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("acr_number", ": ", words2, index2, 0, (int) sbyte.MaxValue);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("acr_mode", ": ", this.ParseAcrMode(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int index5 = index4 + 1;
    this.InsertLine("nAuthModFlags", ": ", words4, index4);
    int num5 = 0;
    if (index5 < ((IEnumerable<string>) words).Count<string>())
      num5 = (int) short.Parse(words[index5]);
    string[] words5 = words;
    int index6 = index5;
    int num6 = index6 + 1;
    this.InsertLine("nExtFeatureType", ": ", this.ParseExtFeatureType(words5, index6));
    int num7;
    switch (num5)
    {
      case 1:
      case 2:
      case 3:
      case 4:
        string[] words6 = words;
        int index7 = num6;
        int num8 = index7 + 1;
        this.InsertLine("iIPB_sio", ": ", words6, index7);
        string[] words7 = words;
        int index8 = num8;
        int num9 = index8 + 1;
        this.InsertLine("iIPB_number", ": ", words7, index8);
        string[] words8 = words;
        int index9 = num9;
        int num10 = index9 + 1;
        this.InsertLine("iIPB_long_press", ": ", words8, index9);
        string[] words9 = words;
        int index10 = num10;
        int num11 = index10 + 1;
        this.InsertLine("iIPB_out_sio", ": ", words9, index10);
        string[] words10 = words;
        int index11 = num11;
        num7 = index11 + 1;
        this.InsertLine("iIPB_out_num", ": ", words10, index11);
        break;
      case 5:
      case 6:
      case 7:
      case 8:
        string[] words11 = words;
        int index12 = num6;
        int num12 = index12 + 1;
        this.InsertLine("ip_octet1", ": ", words11, index12);
        string[] words12 = words;
        int index13 = num12;
        int num13 = index13 + 1;
        this.InsertLine("ip_octet2", ": ", words12, index13);
        string[] words13 = words;
        int index14 = num13;
        int num14 = index14 + 1;
        this.InsertLine("ip_octet3", ": ", words13, index14);
        string[] words14 = words;
        int index15 = num14;
        num7 = index15 + 1;
        this.InsertLine("ip_octet4", ": ", words14, index15);
        break;
      case 9:
        string[] words15 = words;
        int index16 = num6;
        int num15 = index16 + 1;
        this.InsertLine("op_type", ": ", this.ParseKoneOpType(words15, index16));
        string[] words16 = words;
        int index17 = num15;
        int num16 = index17 + 1;
        this.InsertLine("op_id", ": ", words16, index17);
        string[] words17 = words;
        int index18 = num16;
        int num17 = index18 + 1;
        this.InsertLine("op_num", ": ", words17, index18);
        string[] words18 = words;
        int index19 = num17;
        int num18 = index19 + 1;
        this.InsertLine("op_calltypes", ": ", words18, index19);
        string[] words19 = words;
        int index20 = num18;
        num7 = index20 + 1;
        this.InsertLine("op_timeout", ": ", words19, index20);
        break;
      case 10:
        string[] words20 = words;
        int index21 = num6;
        int num19 = index21 + 1;
        this.InsertLine("es_group_id", ": ", words20, index21);
        string[] words21 = words;
        int index22 = num19;
        int num20 = index22 + 1;
        this.InsertLine("floor_id", ": ", words21, index22);
        string[] words22 = words;
        int index23 = num20;
        int num21 = index23 + 1;
        this.InsertLine("ob_id", ": ", words22, index23);
        string[] words23 = words;
        int index24 = num21;
        int num22 = index24 + 1;
        this.InsertLine("flags", ": ", this.ParseTKEFlags(words23, index24));
        string[] words24 = words;
        int index25 = num22;
        num7 = index25 + 1;
        this.InsertLine("dedMode", ": ", this.ParseTKEDedModeFlags(words24, index25));
        break;
      case 11:
        string[] words25 = words;
        int index26 = num6;
        int num23 = index26 + 1;
        this.InsertLine("bSeacDevNum", ": ", words25, index26);
        string[] words26 = words;
        int index27 = num23;
        int num24 = index27 + 1;
        this.InsertLine("bOpdcDevNum", ": ", words26, index27);
        string[] words27 = words;
        int index28 = num24;
        int num25 = index28 + 1;
        this.InsertLine("bFloorOffset", ": ", words27, index28);
        string[] words28 = words;
        int index29 = num25;
        int num26 = index29 + 1;
        this.InsertLine("bVerificationLoc", ": ", this.ParseMitsuVeriLoc(words28, index29));
        string[] words29 = words;
        int index30 = num26;
        int num27 = index30 + 1;
        this.InsertLine("bBoardingFrontRear", ": ", this.ParseMitsuBoardingEntrance(words29, index30));
        string[] words30 = words;
        int index31 = num27;
        int num28 = index31 + 1;
        this.InsertLine("wBoardingFloor", ": ", words30, index31);
        string[] words31 = words;
        int index32 = num28;
        num7 = index32 + 1;
        this.InsertLine("wCardReaderNum", ": ", words31, index32);
        break;
    }
  }

  private void ParseEnCcTime(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcTime (1302)");
    else
      this.InsertLine("enCcTime (302)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    if (numWords != 3)
      return;
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("custom_time", ": ", words2, index2, 0, int.MaxValue);
  }

  private void ParseEnCcScpID(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpID (1318)");
    else
      this.InsertLine("enCcScpID (0318)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("scp_id", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
  }

  private void ParseEnCcDaylight(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("enCcDaylight (0016)");
    string[] words1 = words;
    int index1 = num1;
    int index2 = index1 + 1;
    this.InsertLine("nDstID", ": ", words1, index1);
    int num2 = int.Parse(words[index2]);
    int num3 = 1;
    int num4;
    if (num2 == 0)
    {
      num4 = index2 + 1;
      this.InsertLine("nSYear: 0 - Clear All");
      num3 = 0;
    }
    else
    {
      string[] words2 = words;
      int index3 = index2;
      num4 = index3 + 1;
      this.InsertLine("nSYear", ": ", words2, index3);
    }
    string[] words3 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    int minVal1 = num3;
    this.InsertLineValidateRange("nSMonth", ": ", words3, index4, minVal1, 12);
    string[] words4 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    int minVal2 = num3;
    this.InsertLineValidateRange("nSDay", ": ", words4, index5, minVal2, 32 /*0x20*/);
    string[] words5 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLineValidateRange("nSHh", ": ", words5, index6, 0, 23);
    string[] words6 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLineValidateRange("nSMm", ": ", words6, index7, 0, 59);
    string[] words7 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLineValidateRange("nSSs", ": ", words7, index8, 0, 59);
    string[] words8 = words;
    int index9 = num9;
    int num10 = index9 + 1;
    this.InsertLine("nEYear", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num10;
    int num11 = index10 + 1;
    int minVal3 = num3;
    this.InsertLineValidateRange("nEMonth", ": ", words9, index10, minVal3, 12);
    string[] words10 = words;
    int index11 = num11;
    int num12 = index11 + 1;
    int minVal4 = num3;
    this.InsertLineValidateRange("nEDay", ": ", words10, index11, minVal4, 32 /*0x20*/);
    string[] words11 = words;
    int index12 = num12;
    int num13 = index12 + 1;
    this.InsertLineValidateRange("nEHh", ": ", words11, index12, 0, 23);
    string[] words12 = words;
    int index13 = num13;
    int num14 = index13 + 1;
    this.InsertLineValidateRange("nEMm", ": ", words12, index13, 0, 59);
    string[] words13 = words;
    int index14 = num14;
    int num15 = index14 + 1;
    this.InsertLineValidateRange("nESs", ": ", words13, index14, 0, 59);
  }

  private void ParseEnCcPeerCertificate(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enCcPeerCertificate (0017)");
    else
      this.InsertLine("enCcPeerCertificate (0017)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("nEnable", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("cIssuer", ": ", words2, index2);
  }

  private void ParseEnCcIpClientDefaults(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcIpClientDefaults (0018)");
    else
      this.InsertLine("enCcIpClientDefaults (0018)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("e_max", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("poll_delay", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("offline_time", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("provisioning_time", ": ", words4, index4);
  }

  private void ParseEnCcIpClientAssignment(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcIpClientIDAassignment (0019)");
    else
      this.InsertLine("enCcIpClientIDAassignment (0019)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("assignmentFlags", ": ", this.ParseIpClientAssignFlags(words1, index1));
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("lowScpID", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("highScpID", ": ", words3, index3);
  }

  private void ParseEnCcScpDaylight(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpDaylight (1102)");
    else
      this.InsertLine("enCcScpDaylight (1116)");
    string[] words1 = words;
    int index1 = num1;
    int index2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    int num2 = int.Parse(words[index2]);
    int num3 = 1;
    int num4;
    if (num2 == 0)
    {
      num4 = index2 + 1;
      this.InsertLine("nSYear: 0 - Clear All");
      num3 = 0;
    }
    else
    {
      string[] words2 = words;
      int index3 = index2;
      num4 = index3 + 1;
      this.InsertLine("nSYear", ": ", words2, index3);
    }
    string[] words3 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    int minVal1 = num3;
    this.InsertLineValidateRange("nSMonth", ": ", words3, index4, minVal1, 12);
    string[] words4 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    int minVal2 = num3;
    this.InsertLineValidateRange("nSDay", ": ", words4, index5, minVal2, 32 /*0x20*/);
    string[] words5 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLineValidateRange("nSHh", ": ", words5, index6, 0, 23);
    string[] words6 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLineValidateRange("nSMm", ": ", words6, index7, 0, 59);
    string[] words7 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLineValidateRange("nSSs", ": ", words7, index8, 0, 59);
    string[] words8 = words;
    int index9 = num9;
    int num10 = index9 + 1;
    this.InsertLine("nEYear", ": ", words8, index9);
    string[] words9 = words;
    int index10 = num10;
    int num11 = index10 + 1;
    int minVal3 = num3;
    this.InsertLineValidateRange("nEMonth", ": ", words9, index10, minVal3, 12);
    string[] words10 = words;
    int index11 = num11;
    int num12 = index11 + 1;
    int minVal4 = num3;
    this.InsertLineValidateRange("nEDay", ": ", words10, index11, minVal4, 32 /*0x20*/);
    string[] words11 = words;
    int index12 = num12;
    int num13 = index12 + 1;
    this.InsertLineValidateRange("nEHh", ": ", words11, index12, 0, 23);
    string[] words12 = words;
    int index13 = num13;
    int num14 = index13 + 1;
    this.InsertLineValidateRange("nEMm", ": ", words12, index13, 0, 59);
    string[] words13 = words;
    int index14 = num14;
    int num15 = index14 + 1;
    this.InsertLineValidateRange("nESs", ": ", words13, index14, 0, 59);
  }

  private void ParseEnCcAesControl(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcAesControl (1027)");
    else
      this.InsertLine("enCcAesControl (0212)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nAesCommand", ": ", this.ParseAESCmnd(words2, index2));
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("cAesKey[128/8]", ": ", words3, index3);
  }

  private void ParseEnCcAesTest(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcAesTest (1028)");
    else
      this.InsertLine("enCcAesTest (0213)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int index3 = index2 + 1;
    this.InsertLine("nAesTestCode", ": ", this.ParseAESTestCode(words2, index2));
    int num3 = this.InsertLineWithText("cAesTestFile[200]", ": ", words, index3);
    int index4 = index3 + num3;
    int num4 = this.InsertLineWithText("cAesResultFile[200]", ": ", words, index4);
    int num5 = index4 + num4;
  }

  private void ParseEnCcAttachScp(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcAttachScp (1023)");
    else
      this.InsertLine("enCcAttachScp (0207)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nChannelId", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nToAltPort", ": ", words3, index3);
  }

  private void ParseEnCcScpBioDbSpec1(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpBioDbSpec1 (1231)");
    else
      this.InsertLine("enCcScpBioDbSpec1 (1131)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("lastModified", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("nScpId", ": ", words2, index2, 0, 16383 /*0x3FFF*/);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nBioRecords", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("nBioType", ": ", this.ParseBioType(words4, index4));
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("nFlags", ": ", this.ParseBioDbFlags(words5, index5));
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("nMinScoreDflt", ": ", words6, index6);
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLine("nMinScore", ": ", words7, index7);
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLineValidateRange("nTemplateSize", ": ", words8, index8, 0, 1024 /*0x0400*/);
  }

  private void ParseEnCcStrSRq(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcStrSRq (1853)");
    else
      this.InsertLine("enCcStrSRq (1853)");
    string[] words1 = words;
    int index1 = num1;
    int index2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    int num2 = int.Parse(words[index2]);
    string[] words2 = words;
    int index3 = index2;
    int index4 = index3 + 1;
    this.InsertLineValidateRange("nListLength", ": ", words2, index3, 0, 32 /*0x20*/);
    int num3 = index4;
    this.InsertLine("nStructId[32]", ": ", words, index4, 32 /*0x20*/);
    int num4 = index4 + 32 /*0x20*/;
    if (num2 > 32 /*0x20*/)
      num2 = 32 /*0x20*/;
    this.InsertLine("");
    this.InsertLine("");
    for (int index5 = 0; index5 < num2; ++index5)
      this.InsertLine($"  nStructId[{(object) index5}]", ": ", this.ParseStructId(words, num3 + index5));
  }

  private void ParseEnCcPkgInfo(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcPkgInfo (1854)");
    else
      this.InsertLine("enCcPkgInfo (1854)");
    string[] words1 = words;
    int index1 = num1;
    int index2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    int num2 = this.InsertLineWithText("pkgName[32]", ": ", words, index2);
    int num3 = index2 + num2;
  }

  private void ParseEnCcCertInfo(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcCertInfo (1856)");
    else
      this.InsertLine("enCcCertInfo (1856)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("certType", ": ", this.ParseCertType(words2, index2));
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("certType", ": ", words3, index3, 0, 16383 /*0x3FFF*/);
  }

  private void ParseEnCcElevRelayStatus(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcElevRelayInfo (1416)");
    else
      this.InsertLine("enCcElevRelayInfo (416)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("first", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("count", ": ", words3, index3);
  }

  private void ParseEnCcRdBioTemplate(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcRdBioTemplate (1855)");
    else
      this.InsertLine("enCcRdBioTemplate (1855)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("card_number", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nBioType", ": ", this.ParseBioType(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("nTemplateSize", ": ", words4, index4);
  }

  private void ParseEnCcWebConfigNotes(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcWebConfigNotes (901)");
    else
      this.InsertLine("enCcWebConfigNotes (901)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("notes[251]", ": ", words2, index2);
  }

  private void ParseEnCcWebConfigNetwork(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcWebConfigNetwork (902)");
    else
      this.InsertLine("enCcWebConfigNetwork (902)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("method", ": ", words2, index2, 1, 2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("cIpAddr", ": ", this.ParseIPAddress(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("cSubnetMask", ": ", this.ParseIPAddress(words4, index4));
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLine("cDfltGateway", ": ", this.ParseIPAddress(words5, index5));
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("cHostName[64]", ": ", words6, index6);
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLineValidateRange("dnsType", ": ", words7, index7, 1, 2);
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLine("cDns", ": ", this.ParseIPAddress(words8, index8));
    string[] words9 = words;
    int index9 = num9;
    int num10 = index9 + 1;
    this.InsertLine("cDnsSuffix[256]", ": ", words9, index9);
    string[] words10 = words;
    int index10 = num10;
    int num11 = index10 + 1;
    this.InsertLineValidateRange("method2", ": ", words10, index10, 0, 2);
    string[] words11 = words;
    int index11 = num11;
    int num12 = index11 + 1;
    this.InsertLine("cIpAddr2", ": ", this.ParseIPAddress(words11, index11));
    string[] words12 = words;
    int index12 = num12;
    int num13 = index12 + 1;
    this.InsertLine("cSubnetMask2", ": ", this.ParseIPAddress(words12, index12));
    string[] words13 = words;
    int index13 = num13;
    int num14 = index13 + 1;
    this.InsertLine("cDfltGateway2", ": ", this.ParseIPAddress(words13, index13));
    string[] words14 = words;
    int index14 = num14;
    int num15 = index14 + 1;
    this.InsertLine("cDns2", ": ", this.ParseIPAddress(words14, index14));
    string[] words15 = words;
    int index15 = num15;
    int num16 = index15 + 1;
    this.InsertLine("TnlEnable", ": ", words15, index15);
    string[] words16 = words;
    int index16 = num16;
    int num17 = index16 + 1;
    this.InsertLine("cIpTnl", ": ", words16, index16);
    string[] words17 = words;
    int index17 = num17;
    int num18 = index17 + 1;
    this.InsertLine("cPortTnl", ": ", words17, index17);
  }

  private void ParseEnCcWebConfigHostCommPrim(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcWebConfigHostCommPrim (903)");
    else
      this.InsertLine("enCcWebConfigHostCommPrim (903)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("address", ": ", words2, index2, 0, 7);
    string[] words3 = words;
    int index3 = num3;
    int index4 = index3 + 1;
    this.InsertLine("data_security", ": ", this.ParseDataSecurity(words3, index3));
    int num4 = 0;
    if (index4 < ((IEnumerable<string>) words).Count<string>())
      num4 = (int) short.Parse(words[index4]);
    string[] words4 = words;
    int index5 = index4;
    int num5 = index5 + 1;
    this.InsertLine("cType", ": ", this.ParseHostCommCType(words4, index5, false));
    int num6;
    switch (num4)
    {
      case 1:
        string[] words5 = words;
        int index6 = num5;
        int num7 = index6 + 1;
        this.InsertLine("cAuthIP1", ": ", this.ParseIPAddress(words5, index6));
        string[] words6 = words;
        int index7 = num7;
        int num8 = index7 + 1;
        this.InsertLine("cAuthIP2", ": ", this.ParseIPAddress(words6, index7));
        string[] words7 = words;
        int index8 = num8;
        int num9 = index8 + 1;
        this.InsertLine("nPort", ": ", words7, index8);
        string[] words8 = words;
        int index9 = num9;
        int num10 = index9 + 1;
        this.InsertLineValidateRange("enableAuthIP", ": ", words8, index9, 0, 1);
        string[] words9 = words;
        int index10 = num10;
        num6 = index10 + 1;
        this.InsertLine("nNicSel", ": ", words9, index10);
        break;
      case 2:
        string[] words10 = words;
        int index11 = num5;
        int num11 = index11 + 1;
        this.InsertLine("cHostIP", ": ", this.ParseIPAddress(words10, index11));
        string[] words11 = words;
        int index12 = num11;
        int num12 = index12 + 1;
        this.InsertLine("nPort", ": ", words11, index12);
        string[] words12 = words;
        int index13 = num12;
        int num13 = index13 + 1;
        this.InsertLine("rqIntvl", ": ", words12, index13);
        string[] words13 = words;
        int index14 = num13;
        int num14 = index14 + 1;
        this.InsertLine("connMode", ": ", this.ParseConnMode(words13, index14));
        string[] words14 = words;
        int index15 = num14;
        int num15 = index15 + 1;
        this.InsertLine("cHostName[256]", ": ", words14, index15);
        string[] words15 = words;
        int index16 = num15;
        num6 = index16 + 1;
        this.InsertLineValidateRange("nNicSel", ": ", words15, index16, 0, 2);
        break;
      case 3:
        string[] words16 = words;
        int index17 = num5;
        int num16 = index17 + 1;
        this.InsertLine("baud_rate", ": ", words16, index17);
        string[] words17 = words;
        int index18 = num16;
        num6 = index18 + 1;
        this.InsertLineValidateRange("cRTSMode", ": ", words17, index18, 0, 1);
        break;
      case 4:
        string[] words18 = words;
        int index19 = num5;
        num6 = index19 + 1;
        this.InsertLine("baud_rate", ": ", words18, index19);
        break;
      case 5:
        string[] words19 = words;
        int index20 = num5;
        num6 = index20 + 1;
        this.InsertLine("baud_rate", ": ", words19, index20);
        break;
      case 6:
        string[] words20 = words;
        int index21 = num5;
        num6 = index21 + 1;
        this.InsertLine("baud_rate", ": ", words20, index21);
        break;
    }
  }

  private void ParseEnCcWebConfigHostCommAlt(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcWebConfigHostCommAlt (904)");
    else
      this.InsertLine("enCcWebConfigHostCommAlt (904)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("address", ": ", words2, index2, 0, 0);
    string[] words3 = words;
    int index3 = num3;
    int index4 = index3 + 1;
    this.InsertLine("data_security", ": ", this.ParseDataSecurity(words3, index3));
    int num4 = 0;
    if (index4 < ((IEnumerable<string>) words).Count<string>())
      num4 = (int) short.Parse(words[index4]);
    string[] words4 = words;
    int index5 = index4;
    int num5 = index5 + 1;
    this.InsertLine("cType", ": ", this.ParseHostCommCType(words4, index5, false));
    int num6;
    switch (num4)
    {
      case 1:
        string[] words5 = words;
        int index6 = num5;
        int num7 = index6 + 1;
        this.InsertLine("cAuthIP1", ": ", this.ParseIPAddress(words5, index6));
        string[] words6 = words;
        int index7 = num7;
        int num8 = index7 + 1;
        this.InsertLine("cAuthIP2", ": ", this.ParseIPAddress(words6, index7));
        string[] words7 = words;
        int index8 = num8;
        int num9 = index8 + 1;
        this.InsertLine("nPort", ": ", words7, index8);
        string[] words8 = words;
        int index9 = num9;
        int num10 = index9 + 1;
        this.InsertLineValidateRange("enableAuthIP", ": ", words8, index9, 0, 1);
        string[] words9 = words;
        int index10 = num10;
        num6 = index10 + 1;
        this.InsertLineValidateRange("nNicSel", ": ", words9, index10, 0, 2);
        break;
      case 2:
        string[] words10 = words;
        int index11 = num5;
        int num11 = index11 + 1;
        this.InsertLine("cHostIP", ": ", this.ParseIPAddress(words10, index11));
        string[] words11 = words;
        int index12 = num11;
        int num12 = index12 + 1;
        this.InsertLine("nPort", ": ", words11, index12);
        string[] words12 = words;
        int index13 = num12;
        int num13 = index13 + 1;
        this.InsertLine("rqIntvl", ": ", words12, index13);
        string[] words13 = words;
        int index14 = num13;
        int num14 = index14 + 1;
        this.InsertLine("connMode", ": ", this.ParseConnMode(words13, index14));
        string[] words14 = words;
        int index15 = num14;
        int num15 = index15 + 1;
        this.InsertLine("cHostName[256]", ": ", words14, index15);
        string[] words15 = words;
        int index16 = num15;
        num6 = index16 + 1;
        this.InsertLine("nNicSel", ": ", words15, index16);
        break;
      case 3:
        string[] words16 = words;
        int index17 = num5;
        int num16 = index17 + 1;
        this.InsertLine("baud_rate", ": ", words16, index17);
        string[] words17 = words;
        int index18 = num16;
        num6 = index18 + 1;
        this.InsertLineValidateRange("cRTSMode", ": ", words17, index18, 0, 1);
        break;
      case 4:
        string[] words18 = words;
        int index19 = num5;
        num6 = index19 + 1;
        this.InsertLine("baud_rate", ": ", words18, index19);
        break;
      case 5:
        string[] words19 = words;
        int index20 = num5;
        num6 = index20 + 1;
        this.InsertLine("baud_rate", ": ", words19, index20);
        break;
      case 6:
        string[] words20 = words;
        int index21 = num5;
        num6 = index21 + 1;
        this.InsertLine("baud_rate", ": ", words20, index21);
        break;
    }
  }

  private void ParseEnCcWebConfigSessionTmr(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcWebConfigSessionTmr (905)");
    else
      this.InsertLine("enCcWebConfigSessionTmr (905)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("minutes", ": ", words2, index2, 0, 60);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("pswd_strength", ": ", words3, index3, 1, 3);
  }

  private void ParseEnCcWebConfigWebConn(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcWebConfigWebConn (906)");
    else
      this.InsertLine("enCcWebConfigWebConn (906)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("web_server", ": ", words2, index2, 0, 1);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLineValidateRange("zeroconf", ": ", words3, index3, 0, 1);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLineValidateRange("dfo_filter", ": ", words4, index4, 0, (int) ushort.MaxValue);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLineValidateRange("diag_log", ": ", words5, index5, 0, 1);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("snmp", ": ", this.ParseSnmpFlags(words6, index6));
    string[] words7 = words;
    int index7 = num7;
    int num8 = index7 + 1;
    this.InsertLineValidateRange("disable_default_user", ": ", words7, index7, 0, 1);
    string[] words8 = words;
    int index8 = num8;
    int num9 = index8 + 1;
    this.InsertLineValidateRange("disable_sd_int", ": ", words8, index8, 0, 1);
    string[] words9 = words;
    int index9 = num9;
    int num10 = index9 + 1;
    this.InsertLineValidateRange("disable_usb_int", ": ", words9, index9, 0, 1);
    string[] words10 = words;
    int index10 = num10;
    int num11 = index10 + 1;
    this.InsertLineValidateRange("enGratArp", ": ", words10, index10, 0, 1);
  }

  private void ParseEnCcWebConfigAutoSave(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcWebConfigAutoSave (907)");
    else
      this.InsertLine("enCcWebConfigAutoSave (907)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("auto_save", ": ", words2, index2, 0, 1);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("seconds", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLineValidateRange("restore", ": ", words4, index4, 0, 1);
  }

  private void ParseEnCcWebConfigNetworkDiag(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcWebConfigNetworkDiag (908)");
    else
      this.InsertLine("enCcWebConfigNetworkDiag (908)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("flag", ": ", words2, index2, 0, 1);
  }

  private void ParseEnCcWebConfigTimeServer(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcWebConfigTimeServer (909)");
    else
      this.InsertLine("enCcWebConfigTimeServer (909)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("ts_disabled", ": ", words2, index2, 0, 1);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("index", ": ", this.ParseNTPServerTypeCommand(words3, index3));
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("cTmServ[65]", ": ", words4, index4);
    string[] words5 = words;
    int index5 = num5;
    int num6 = index5 + 1;
    this.InsertLineValidateRange("port", ": ", words5, index5, 0, (int) ushort.MaxValue);
    string[] words6 = words;
    int index6 = num6;
    int num7 = index6 + 1;
    this.InsertLine("interval", ": ", words6, index6);
  }

  private void ParseEnCcWebConfigCentralStation(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcWebConfigCentralStation (910)");
    else
      this.InsertLine("enCcWebConfigCentralStation (910)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int index3 = index2 + 1;
    this.InsertLine("data_security", ": ", words2, index2);
    int num3 = 0;
    if (index3 < ((IEnumerable<string>) words).Count<string>())
      num3 = (int) short.Parse(words[index3]);
    string[] words3 = words;
    int index4 = index3;
    int num4 = index4 + 1;
    this.InsertLineValidateRange("cType", ": ", words3, index4, 0, 1);
    if (num3 != 1)
      return;
    string[] words4 = words;
    int index5 = num4;
    int num5 = index5 + 1;
    this.InsertLine("baud_rate", ": ", words4, index5);
    string[] words5 = words;
    int index6 = num5;
    int num6 = index6 + 1;
    this.InsertLineValidateRange("cRTSMode", ": ", words5, index6, 0, 1);
  }

  private void ParseEnCcWebConfigCardDBSize(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcWebConfigCardDBSize (911)");
    else
      this.InsertLine("enCcWebConfigCardDBSize (911)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("db_size", ": ", words2, index2);
  }

  private void ParseEnCcWebConfigDiagnostics(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcWebConfigDiagnostics (912)");
    else
      this.InsertLine("enCcWebConfigDiagnostics (912)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLineValidateRange("dump_files_enabled", ": ", words2, index2, 0, 1);
  }

  private void ParseEnCcWebConfigApplyReboot(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcWebConfigApplyReboot (913)");
    else
      this.InsertLine("enCcWebConfigApplyReboot (913)");
    string[] words1 = words;
    int index = num1;
    int num2 = index + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index, 0, 16383 /*0x3FFF*/);
  }

  private void ParseEnCcWebConfigRead(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcWebConfigRead (900)");
    else
      this.InsertLine("enCcWebConfigRead (900)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("read_type", ": ", this.ParseWebConfigReadType(words2, index2));
  }

  private void ParseNvToolsGetAuthorizationCode(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("NvTools GetAuthorizationCode (9331)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("nNvArgType", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nProductId", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nSerialNumber", ": ", words3, index3);
    string[] words4 = words;
    int index4 = num4;
    int num5 = index4 + 1;
    this.InsertLine("nArgValue", ": ", words4, index4);
  }

  private void ParseNvToolsGetAuthorizationNumber(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    this.InsertLine("NvTools GetAuthorizationNumber (9332)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLine("nNvArgType", ": ", words1, index1);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nProductId", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("nArgValue", ": ", words3, index3);
  }

  private void ParseEnCcScpLogin(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpLogin (3250)");
    else
      this.InsertLine("enCcScpLogin (2250)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int index3 = index2 + 1;
    this.InsertLine("nLoginNumber", ": ", words2, index2);
    int num3 = int.Parse(words[index3]);
    string[] words3 = words;
    int index4 = index3;
    int num4 = index4 + 1;
    this.InsertLine("nLoginType", ": ", this.ParseLoginType(words3, index4));
    int num5;
    switch (num3)
    {
      case 0:
        string[] words4 = words;
        int index5 = num4;
        int num6 = index5 + 1;
        this.InsertLine("cName", ": ", words4, index5);
        string[] words5 = words;
        int index6 = num6;
        int num7 = index6 + 1;
        this.InsertLine("cPassword", ": ", words5, index6);
        string[] words6 = words;
        int index7 = num7;
        int num8 = index7 + 1;
        this.InsertLine("nAcctType", ": ", words6, index7);
        string[] words7 = words;
        int index8 = num8;
        num5 = index8 + 1;
        this.InsertLine("cNotes", ": ", words7, index8);
        break;
      case 1:
      case 2:
        string[] words8 = words;
        int index9 = num4;
        int num9 = index9 + 1;
        this.InsertLine("cName", ": ", words8, index9);
        string[] words9 = words;
        int index10 = num9;
        num5 = index10 + 1;
        this.InsertLine("cPassword", ": ", words9, index10);
        break;
      case 3:
        string[] words10 = words;
        int index11 = num4;
        int num10 = index11 + 1;
        this.InsertLine("iDevInstance", ": ", words10, index11);
        string[] words11 = words;
        int index12 = num10;
        int num11 = index12 + 1;
        this.InsertLine("wBACnetPort", ": ", words11, index12);
        string[] words12 = words;
        int index13 = num11;
        int num12 = index13 + 1;
        this.InsertLine("cFDBBMDAddress", ": ", words12, index13);
        string[] words13 = words;
        int index14 = num12;
        num5 = index14 + 1;
        this.InsertLine("wFDLifetime", ": ", words13, index14);
        break;
      case 4:
        string[] words14 = words;
        int index15 = num4;
        int num13 = index15 + 1;
        this.InsertLine("cName", ": ", words14, index15);
        string[] words15 = words;
        int index16 = num13;
        int num14 = index16 + 1;
        this.InsertLine("cAuthKey", ": ", words15, index16);
        string[] words16 = words;
        int index17 = num14;
        int num15 = index17 + 1;
        this.InsertLine("cPrivKey", ": ", words16, index17);
        string[] words17 = words;
        int index18 = num15;
        int num16 = index18 + 1;
        this.InsertLine("wSecurityInfo", ": ", this.ParseSecurityInfoFlags(words17, index18));
        string[] words18 = words;
        int index19 = num16;
        num5 = index19 + 1;
        this.InsertLine("cContextName", ": ", words18, index19);
        break;
      case 5:
        string[] words19 = words;
        int index20 = num4;
        int num17 = index20 + 1;
        this.InsertLine("cName", ": ", words19, index20);
        string[] words20 = words;
        int index21 = num17;
        int num18 = index21 + 1;
        this.InsertLine("cPassword", ": ", words20, index21);
        string[] words21 = words;
        int index22 = num18;
        int num19 = index22 + 1;
        this.InsertLine("cBrokerAddress", ": ", words21, index22);
        string[] words22 = words;
        int index23 = num19;
        int num20 = index23 + 1;
        this.InsertLine("cPort", ": ", words22, index23);
        string[] words23 = words;
        int index24 = num20;
        num5 = index24 + 1;
        this.InsertLine("cFlags", ": ", words23, index24);
        break;
    }
  }

  private void ParseEnCcScpLoginResources(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpLoginResources (3251)");
    else
      this.InsertLine("enCcScpLoginResources (2251)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("nScpID", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("nLoginNumber", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int index4 = index3 + 1;
    this.InsertLine("nCommand", ": ", this.ParseLoginResourcesCommand(words3, index3));
    int num4 = int.Parse(words[index4]);
    string[] words4 = words;
    int index5 = index4;
    int num5 = index5 + 1;
    this.InsertLineValidateRange("nResourceCount", ": ", words4, index5, 1, 64 /*0x40*/);
    if (num4 > 64 /*0x40*/)
      num4 = 64 /*0x40*/;
    this.InsertLine("");
    for (int index6 = 0; index6 < num4; ++index6)
    {
      string[] words5 = words;
      int index7 = num5;
      int num6 = index7 + 1;
      this.InsertLine("nResourceType", ": ", this.ParseTransactionSourceType(words5, index7, false));
      string[] words6 = words;
      int index8 = num6;
      int num7 = index8 + 1;
      this.InsertLine("nResourceFlags", ": ", this.ParseLoginResourcesFlags(words6, index8));
      string[] words7 = words;
      int index9 = num7;
      int num8 = index9 + 1;
      this.InsertLine("nResourceNumber", ": ", words7, index9);
      string[] words8 = words;
      int index10 = num8;
      num5 = index10 + 1;
      this.InsertLine("nResourceCount", ": ", words8, index10);
      this.InsertLine("");
    }
  }

  private void ParseEnCcScpLoginUsers(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcScpLoginUsers (3252)");
    else
      this.InsertLine("enCcScpLoginUsers (2252)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("userType", ": ", this.ParseLoginType(words2, index2));
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("userIndex", ": ", words3, index3);
  }

  private void ParseEnCcBatch(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcBatch (0011)");
    else
      this.InsertLine("enCcBatch (0001)");
    string[] words1 = words;
    int index = num1;
    int num2 = index + 1;
    this.InsertLine("cfilename[128]", ": ", words1, index);
  }

  private void ParseEnCcAcrSrq(string[] words, int numWords, bool honeywell)
  {
    int num1 = this.GetCmdIndex() + 1;
    if (honeywell)
      this.InsertLine("enNCcAcrSrq (1407)");
    else
      this.InsertLine("enCcAcrSrq (0407)");
    string[] words1 = words;
    int index1 = num1;
    int num2 = index1 + 1;
    this.InsertLineValidateRange("scp_number", ": ", words1, index1, 0, 16383 /*0x3FFF*/);
    string[] words2 = words;
    int index2 = num2;
    int num3 = index2 + 1;
    this.InsertLine("first", ": ", words2, index2);
    string[] words3 = words;
    int index3 = num3;
    int num4 = index3 + 1;
    this.InsertLine("count", ": ", words3, index3);
  }

  private string ParseSetClear(string[] words, int index)
  {
    string setClear = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          setClear = "0 - Clear Mask (generate alarm)";
          break;
        case 1:
          setClear = "1 - Set Mask (suppress alarm generation)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          setClear = words[index] + " - UNKNOWN";
          break;
      }
    }
    return setClear;
  }

  private string ParseExtFeatureType(string[] words, int index)
  {
    string extFeatureType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          extFeatureType = "0 - None";
          break;
        case 1:
          extFeatureType = "1 - Classroom";
          break;
        case 2:
          extFeatureType = "2 - Office";
          break;
        case 3:
          extFeatureType = "3 - Privacy";
          break;
        case 4:
          extFeatureType = "4 - Apartment";
          break;
        case 5:
          extFeatureType = "5 - Otis Elev Mode 1";
          break;
        case 6:
          extFeatureType = "6 - Otis Elev Mode 2";
          break;
        case 7:
          extFeatureType = "7 - Otis Elev Mode 3";
          break;
        case 8:
          extFeatureType = "8 - Otis Elev Mode 4";
          break;
        case 9:
          extFeatureType = "9 - Kone";
          break;
        case 10:
          extFeatureType = "10 - TKE";
          break;
        case 11:
          extFeatureType = "11 - Mitsubishi";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          extFeatureType = words[index] + " - UNKNOWN";
          break;
      }
    }
    return extFeatureType;
  }

  private string ParseKoneOpType(string[] words, int index)
  {
    string koneOpType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          koneOpType = "0 - Destination Operation Panel (DOP)";
          break;
        case 1:
          koneOpType = "1 - Car Operation Panel (COP)";
          break;
        case 2:
          koneOpType = "2 - Remote Call Giving Interface (RCGIF)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          koneOpType = words[index] + " - UNKNOWN";
          break;
      }
    }
    return koneOpType;
  }

  private string ParseIPAddress(string[] words, int index)
  {
    string ipAddress = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      ipAddress = $"{(object) num} ({(object) (num >> 24 & (int) byte.MaxValue)}.{(object) (num >> 16 /*0x10*/ & (int) byte.MaxValue)}.{(object) (num >> 8 & (int) byte.MaxValue)}.{(object) (num & (int) byte.MaxValue)})";
    }
    return ipAddress;
  }

  private string ParseOsdpPassThruReaderRole(string[] words, int index)
  {
    string passThruReaderRole = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          passThruReaderRole = "0 - primary";
          break;
        case 1:
          passThruReaderRole = "1 - alternate";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          passThruReaderRole = words[index] + " - UNKNOWN";
          break;
      }
    }
    return passThruReaderRole;
  }

  private string ParseOsdpPassThruMsgType(string[] words, int index)
  {
    string osdpPassThruMsgType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          osdpPassThruMsgType = "0 - osdp_MFG";
          break;
        case 1:
          osdpPassThruMsgType = "1 - osdp_XWR";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          osdpPassThruMsgType = words[index] + " - UNKNOWN";
          break;
      }
    }
    return osdpPassThruMsgType;
  }

  private string ParseOALProtocol(string[] words, int index)
  {
    string oalProtocol = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      if (int.Parse(words[index]) == 9)
      {
        oalProtocol = "9 - SimonsVoss";
      }
      else
      {
        this.textColor = this.errorFoundTextColor;
        oalProtocol = words[index] + " - UNKNOWN";
      }
    }
    return oalProtocol;
  }

  private string ParseOALAction(string[] words, int index, int protocol)
  {
    string oalAction = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      if (protocol == 9)
      {
        switch (num)
        {
          case 1:
            oalAction = "1 - Activate/Deactivate OAL";
            break;
          case 2:
            oalAction = "2 - Add Credential";
            break;
          case 3:
            oalAction = "3 - Remove Credential";
            break;
          default:
            this.textColor = this.errorFoundTextColor;
            oalAction = words[index] + " - UNKNOWN";
            break;
        }
      }
    }
    return oalAction;
  }

  private string ParseAcrMode(string[] words, int index)
  {
    string acrMode = "";
    bool flag = false;
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
      {
        flag = true;
        num &= (int) sbyte.MaxValue;
      }
      switch (num)
      {
        case 0:
          acrMode = "0 - no change";
          break;
        case 1:
          acrMode = "1 - disable";
          break;
        case 2:
          acrMode = "2 - unlock";
          break;
        case 3:
          acrMode = "3 - locked";
          break;
        case 4:
          acrMode = "4 - facility code only";
          break;
        case 5:
          acrMode = "5 - card only";
          break;
        case 6:
          acrMode = "6 - pin only";
          break;
        case 7:
          acrMode = "7 - card and PIN";
          break;
        case 8:
          acrMode = "8 - card or PIN";
          break;
        case 16 /*0x10*/:
          acrMode = "16 - disable 2-card mode";
          break;
        case 17:
          acrMode = "17 - enable 2-card mode";
          break;
        case 18:
          acrMode = "18 - disable BIO_VERFY mode";
          break;
        case 19:
          acrMode = "19 - enable BIO_VERIFY mode";
          break;
        case 20:
          acrMode = "20 - disable BIO_ENROLL mode";
          break;
        case 21:
          acrMode = "21 - enable BIO_ENROLL mode";
          break;
        case 22:
          acrMode = "22 - disable CIPHER mode";
          break;
        case 23:
          acrMode = "23 - enable CIPHER mode";
          break;
        case 24:
          acrMode = "24 - clear ACR_F_MODE_TRIG flag";
          break;
        case 25:
          acrMode = "25 - set ACR_F_MODE_TRIG flag";
          break;
        case 26:
          acrMode = "26 - clear ACR_FE_NO_ARQ flag";
          break;
        case 27:
          acrMode = "27 - set ACR_FE_NO_ARQ flag";
          break;
        case 28:
          acrMode = "28 - change auth mod flags";
          break;
        case 29:
          acrMode = "29 - set extended actl_flags::ACR_FE_LINK_MODE";
          break;
        case 30:
          acrMode = "30 - clear extended actl_flags::ACR_FE_LINK_MODE";
          break;
        case 31 /*0x1F*/:
          acrMode = "31 - Extended Feature Change";
          break;
        case 32 /*0x20*/:
          acrMode = "32 - set extended actl_flags::ACR_FE_LINK_MODE_ALT";
          break;
        case 33:
          acrMode = "33 - clear extended actl_flags::ACR_FE_LINK_MODE_ALT";
          break;
        case 35:
          acrMode = "35 - clear ACR_F_HOST_CBG flag";
          break;
        case 36:
          acrMode = "36 - set ACR_F_HOST_CBG flag";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          acrMode = words[index] + " - UNKNOWN";
          break;
      }
    }
    if (flag)
      acrMode += " (Set Current Mode Flag Set)";
    return acrMode;
  }

  private string ParseWebConfigReadType(string[] words, int index)
  {
    string webConfigReadType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          webConfigReadType = "1 - Notes";
          break;
        case 2:
          webConfigReadType = "2 - Network";
          break;
        case 3:
          webConfigReadType = "3 - Host Comm Primary";
          break;
        case 4:
          webConfigReadType = "4 - Host Comm Alternate";
          break;
        case 5:
          webConfigReadType = "5 - Session Timer";
          break;
        case 6:
          webConfigReadType = "6 - Web Conn";
          break;
        case 7:
          webConfigReadType = "7 - Auto Save";
          break;
        case 8:
          webConfigReadType = "8 - Network Diag";
          break;
        case 9:
          webConfigReadType = "9 - Time Server";
          break;
        case 10:
          webConfigReadType = "10 - Central Station";
          break;
        case 11:
          webConfigReadType = "11 - Card DB Size";
          break;
        case 12:
          webConfigReadType = "12 - Diagnostics";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          webConfigReadType = words[index] + " - UNKNOWN";
          break;
      }
    }
    return webConfigReadType;
  }

  private string ParseTempAcrModeTime(string[] words, int index)
  {
    string tempAcrModeTime = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num1 = int.Parse(words[index]);
      int num2 = (num1 & 49152 /*0xC000*/) >> 14;
      int num3 = num1 & 16383 /*0x3FFF*/;
      switch (num2)
      {
        case 0:
          tempAcrModeTime = num3.ToString() + " minutes (standard)";
          break;
        case 1:
          string str = $", Time: {num3 / 60:d2}:{num3 % 60:d2}";
          tempAcrModeTime = $"{(object) num3} (Minutes since Midnight){str}";
          break;
        case 2:
          tempAcrModeTime = num3.ToString() + " (Indefinite)";
          break;
        case 3:
          tempAcrModeTime = num3.ToString() + " seconds (standard)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          tempAcrModeTime = $"{(object) num3} UNKNOWN Mode {(object) num2}";
          break;
      }
    }
    return tempAcrModeTime;
  }

  private string ParseAESTestCode(string[] words, int index)
  {
    string aesTestCode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          aesTestCode = "1 - AES_T_ECB_E";
          break;
        case 2:
          aesTestCode = "2 - AES_T_ECB_D";
          break;
        case 3:
          aesTestCode = "3 - AES_T_CBC_E";
          break;
        case 4:
          aesTestCode = "4 - AES_T_CBC_D";
          break;
        case 5:
          aesTestCode = "5 - AES_T_MCT_ECB_E";
          break;
        case 6:
          aesTestCode = "6 - AES_T_MCT_ECB_D";
          break;
        case 7:
          aesTestCode = "7 - AES_T_MCT_CBC_E";
          break;
        case 8:
          aesTestCode = "8 - AES_T_MCT_CBC_D";
          break;
        case 9:
          aesTestCode = "9 - AES_T_RND_VST";
          break;
        case 10:
          aesTestCode = "10 - AES_T_RND_MCT";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          aesTestCode = words[index] + " - UNKNOWN";
          break;
      }
    }
    return aesTestCode;
  }

  private string ParseIpClientAssignFlags(string[] words, int index)
  {
    string clientAssignFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      clientAssignFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        clientAssignFlags += "DECREMENT ";
      if ((num & 2) == 2)
        clientAssignFlags += "NO_REUSE ";
      if ((num & 4) == 4)
        clientAssignFlags += "NO_HWID ";
    }
    return clientAssignFlags;
  }

  private string ParseAESCmnd(string[] words, int index)
  {
    string aesCmnd = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          aesCmnd = "0 - AESCMND_OFF (disable encryption)";
          break;
        case 1:
          aesCmnd = "1 - AESCMND_SET_MK1 (Load Master Key 1)";
          break;
        case 2:
          aesCmnd = "2 - AESCMND_SET_MK2 (Load Master Key 2)";
          break;
        case 3:
          aesCmnd = "3 - AESCMND_ENA_MK1 (enable encryption to this SCP, using Master Key 1)";
          break;
        case 4:
          aesCmnd = "4 - AESCMND_ENA_MK2 (enable encryption to this SCP, using Master Key 2)";
          break;
        case 5:
          aesCmnd = "5 - AESCMND_ENA_TST (enable TEST MODE: std encryption protocol, plain text, use Mk-1)";
          break;
        case 6:
          aesCmnd = "6 - AESCMND_NEW_SK (set new session key)";
          break;
        case 7:
          aesCmnd = "7 - AESCMND_MK12SCP (transfer Master Key 1 to the Scp)";
          break;
        case 8:
          aesCmnd = "8 - AESCMND_MK22SCP (transfer Master Key 2 to the Scp)";
          break;
        case 9:
          aesCmnd = "9 - AESCMND_ENA_MK256 (enable encryption to this SCP, using both master keys)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          aesCmnd = words[index] + " - UNKNOWN";
          break;
      }
    }
    return aesCmnd;
  }

  private string ParseActionType(string[] words, int index)
  {
    string str1 = "";
    string str2 = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      str2 = num >= 256 /*0x0100*/ ? (num >= 512 /*0x0200*/ ? (num >= 1024 /*0x0400*/ ? (num >= 1280 /*0x0500*/ ? " (invalid prefix)" : " (prefix 1024)") : " (prefix 512)") : " (prefix 256)") : " (prefix 0)";
      switch (num & (int) byte.MaxValue)
      {
        case 0:
          str1 = num.ToString() + " - Delete All Actions";
          break;
        case 1:
          str1 = num.ToString() + " - Monitor Point Mask Control";
          break;
        case 2:
          str1 = num.ToString() + " - Control Point Command";
          break;
        case 3:
          str1 = num.ToString() + " - ACR Mode Control";
          break;
        case 4:
          str1 = num.ToString() + " - Forced Open Mask Control";
          break;
        case 5:
          str1 = num.ToString() + " - Held Open Mask Control";
          break;
        case 6:
          str1 = num.ToString() + " - Momentary Unlock";
          break;
        case 7:
          str1 = num.ToString() + " - Procedure Control Command";
          break;
        case 8:
          str1 = num.ToString() + " - Trigger Variable Command";
          break;
        case 9:
          str1 = num.ToString() + " - Timezone Control Command";
          break;
        case 10:
          str1 = num.ToString() + " - Reader LED mode set command";
          break;
        case 11:
          str1 = num.ToString() + " - APB Free Pass to all or individual";
          break;
        case 12:
          str1 = num.ToString() + " - Use Primary Port Dial String";
          break;
        case 13:
          str1 = num.ToString() + " - Raw HEX Character Output";
          break;
        case 14:
          str1 = num.ToString() + " - Arm/Disarm a monitor point group";
          break;
        case 15:
          str1 = num.ToString() + " - Set Action_type prefix based on mask count";
          break;
        case 16 /*0x10*/:
          str1 = num.ToString() + " - Set Action_type prefix based on active points";
          break;
        case 17:
          str1 = num.ToString() + " - Access Area Control Command";
          break;
        case 18:
          str1 = num.ToString() + " - Momentary unlock, abor the wait for door open state";
          break;
        case 19:
          str1 = num.ToString() + " - Temp reader LED command";
          break;
        case 20:
          str1 = num.ToString() + " - Text output to an LCD terminal";
          break;
        case 21:
          str1 = num.ToString() + " - Use Alternate Port Dial String";
          break;
        case 23:
          str1 = num.ToString() + " - IPS Control Command";
          break;
        case 24:
          str1 = num.ToString() + " - Temp ACR Mode";
          break;
        case 25:
          str1 = num.ToString() + " - Card Sim";
          break;
        case 26:
          str1 = num.ToString() + " - Use Limit";
          break;
        case 27:
          str1 = num.ToString() + " - Operating Mode";
          break;
        case 126:
          str1 = num.ToString() + " - Delay in .1 seconds";
          break;
        case (int) sbyte.MaxValue:
          str1 = num.ToString() + " - Delay in seconds";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          str1 = num.ToString() + " - UNKNOWN";
          break;
      }
    }
    return str1 + str2;
  }

  private string ParseStartEndTime(string[] words, int index)
  {
    string startEndTime = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      string str = words[index] + " ";
      int num1 = int.Parse(words[index]);
      int num2 = num1 / 60;
      int num3 = num1 % 60;
      if (num2 > 23)
        this.textColor = this.errorFoundTextColor;
      startEndTime = $"({num2:00}:{num3:00})";
    }
    return startEndTime;
  }

  private string Parse2221ArgList0Flags(string[] words, int index)
  {
    string str = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      str = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        str += "IPS_ARG_AUTODISARM ";
      if ((num & 2) == 2)
        str += "IPS_ARG_SKIPCANCEL ";
      if ((num & 4) == 4)
        str += "IPS_ARG_RPRT_NOT_READY_TO_ARM ";
      if ((num & 8) == 8)
      {
        this.textColor = this.errorFoundTextColor;
        str += "(UNKNOWN 0x0008) ";
      }
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
      {
        this.textColor = this.errorFoundTextColor;
        str += "(UNKNOWN 0x0010) ";
      }
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
      {
        this.textColor = this.errorFoundTextColor;
        str += "(UNKNOWN 0x0020)  ";
      }
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
      {
        this.textColor = this.errorFoundTextColor;
        str += "(UNKNOWN 0x0040)  ";
      }
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
      {
        this.textColor = this.errorFoundTextColor;
        str += "(UNKNOWN 0x0080) ";
      }
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
      {
        this.textColor = this.errorFoundTextColor;
        str += "(UNKNOWN 0x0100) ";
      }
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
      {
        this.textColor = this.errorFoundTextColor;
        str += "(UNKNOWN 0x0200) ";
      }
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
      {
        this.textColor = this.errorFoundTextColor;
        str += "(UNKNOWN 0x0400) ";
      }
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
      {
        this.textColor = this.errorFoundTextColor;
        str += "(UNKNOWN 0x0800) ";
      }
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
      {
        this.textColor = this.errorFoundTextColor;
        str += "(UNKNOWN 0x1000) ";
      }
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
      {
        this.textColor = this.errorFoundTextColor;
        str += "(UNKNOWN 0x2000) ";
      }
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
      {
        this.textColor = this.errorFoundTextColor;
        str += "(UNKNOWN 0x4000) ";
      }
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        this.textColor = this.errorFoundTextColor;
        str += "(UNKNOWN 0x8000) ";
      }
    }
    return str;
  }

  private string ParseSioRdrHexRdrListFlags(string[] words, int index)
  {
    string rdrHexRdrListFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      rdrHexRdrListFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        rdrHexRdrListFlags += "RDR-1 ";
      if ((num & 2) == 2)
        rdrHexRdrListFlags += "RDR-2 ";
      if ((num & 4) == 4)
        rdrHexRdrListFlags += "RDR-3 ";
      if ((num & 8) == 8)
        rdrHexRdrListFlags += "RDR-4 ";
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
        rdrHexRdrListFlags += "RDR-5 ";
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
        rdrHexRdrListFlags += "RDR-6 ";
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
        rdrHexRdrListFlags += "RDR-7 ";
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
        rdrHexRdrListFlags += "RDR-8 ";
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
        rdrHexRdrListFlags += "RDR-9 ";
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
        rdrHexRdrListFlags += "RDR-10 ";
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
        rdrHexRdrListFlags += "RDR-11 ";
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
        rdrHexRdrListFlags += "RDR-12 ";
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
        rdrHexRdrListFlags += "RDR-13 ";
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
        rdrHexRdrListFlags += "RDR-14 ";
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
        rdrHexRdrListFlags += "RDR-15 ";
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
        rdrHexRdrListFlags += "RDR-16 ";
    }
    return rdrHexRdrListFlags;
  }

  private string ParseServiceTypeFlags(string[] words, int index)
  {
    return this.ParseZeroExpected(words, index);
  }

  private string ParseSnmpFlags(string[] words, int index)
  {
    string snmpFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num1 = int.Parse(words[index]);
      snmpFlags = string.Format("{0} (0x{0:X4}) ", (object) num1);
      if ((num1 & 1) == 1)
        snmpFlags += "SNMPv2c ";
      if ((num1 & 2) == 2)
        snmpFlags += "SNMPv3 ";
      int num2 = num1 & 4;
      int num3 = num1 & 8;
    }
    return snmpFlags;
  }

  private string ParseHolidayExtendField(string[] words, int index)
  {
    string holidayExtendField = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      holidayExtendField = (num & 128 /*0x80*/) != 128 /*0x80*/ ? words[index] : (num & (int) sbyte.MaxValue).ToString() + " (Special Holiday)";
    }
    return holidayExtendField;
  }

  private string ParseSecurityInfoFlags(string[] words, int index)
  {
    string securityInfoFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      securityInfoFlags = words[index] + " ";
      if ((num & 1) == 1)
        securityInfoFlags += "SNMP_V2C ";
      if ((num & 2) == 2)
        securityInfoFlags += "SNMP_V3_USM ";
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
        securityInfoFlags += "SNMPV3_AUTH_ENABLE ";
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
        securityInfoFlags += "SNMPV3_PRIV_ENABLE ";
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
        securityInfoFlags += "SNMPV3_AUTHTYPE ";
      if ((num & 32768 /*0x8000*/) == 128 /*0x80*/)
        securityInfoFlags += "SNMPV3_PRIVTYPE ";
    }
    return securityInfoFlags;
  }

  private string ParseHolidayTypeMask(string[] words, int index)
  {
    string holidayTypeMask = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      holidayTypeMask = words[index] + " ";
      if (num == 0)
        holidayTypeMask += "Delete Holiday";
      if ((num & 1) == 1)
        holidayTypeMask += "Holiday0 ";
      if ((num & 2) == 2)
        holidayTypeMask += "Holiday1 ";
      if ((num & 4) == 4)
        holidayTypeMask += "Holiday2 ";
      if ((num & 8) == 8)
        holidayTypeMask += "Holiday3 ";
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
        holidayTypeMask += "Holiday4 ";
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
        holidayTypeMask += "Holiday5 ";
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
        holidayTypeMask += "Holiday6 ";
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
        holidayTypeMask += "Holiday7 ";
    }
    return holidayTypeMask;
  }

  private string ParseDayMask(string[] words, int index)
  {
    string dayMask = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      dayMask = words[index] + " ";
      if ((num & 1) == 1)
        dayMask += "Sunday ";
      if ((num & 2) == 2)
        dayMask += "Monday ";
      if ((num & 4) == 4)
        dayMask += "Tuesday ";
      if ((num & 8) == 8)
        dayMask += "Wednesday ";
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
        dayMask += "Thursday ";
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
        dayMask += "Friday ";
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
        dayMask += "Saturday ";
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
        dayMask += "Holiday0 ";
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
        dayMask += "Holiday1 ";
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
        dayMask += "Holiday2 ";
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
        dayMask += "Holiday3 ";
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
        dayMask += "Holiday4 ";
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
        dayMask += "Holiday5 ";
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
        dayMask += "Holiday6 ";
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
        dayMask += "Holiday7 ";
    }
    return dayMask;
  }

  private string Parse503ElevData(string[] words, int index, int elev_config)
  {
    string str = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      str = words[index] + " ";
      if (elev_config == 7)
      {
        if ((num & 1) == 1)
          str += "(DOP Source Front Allowed) ";
        if ((num & 2) == 2)
          str += "(DOP Source Rear Allowed) ";
        if ((num & 4) == 4)
          str += "(DOP Allowed Destination Front) ";
        if ((num & 8) == 8)
          str += "(DOP Allowed Destination Rear) ";
        if ((num & 16 /*0x10*/) == 16 /*0x10*/)
          str += "(COP Allowed Destination Front) ";
        if ((num & 32 /*0x20*/) == 32 /*0x20*/)
          str += "(COP Allowed Destination Rear) ";
        if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
          str += "(DOP Source Front Allowed - Disconnected State) ";
        if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
          str += "(DOP Source Rear Allowed - Disconnected State) ";
        if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
          str += "(DOP Allowed Destination Front - Disconnected State) ";
        if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
          str += "(DOP Allowed Destination Rear - Disconnected State) ";
        if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
          str += "(COP Allowed Destination Front - Disconnected State) ";
        if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
          str += "(DOP Allowed Destination Rear - Disconnected State) ";
      }
    }
    return str;
  }

  private string ParseSioEnable(string[] words, int index)
  {
    string sioEnable = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      switch (num)
      {
        case 0:
          sioEnable = "0 - Disable";
          break;
        case 1:
          sioEnable = "1 - Enable";
          break;
        case 2:
          sioEnable = "2 - Enable only after AES-Encryption preference configured";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          sioEnable = num.ToString() + " (Unknown mode)";
          break;
      }
    }
    return sioEnable;
  }

  private string ParseTimezoneMode(string[] words, int index)
  {
    string timezoneMode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      switch (num)
      {
        case 0:
          timezoneMode = "0 - OFF";
          break;
        case 1:
          timezoneMode = "1 - ON";
          break;
        case 2:
          timezoneMode = "2 - SCAN";
          break;
        case 3:
          timezoneMode = "3 - One Time Event";
          break;
        case 4:
          timezoneMode = "4 - Scan-Always Honor DayOfWeek";
          break;
        case 5:
          timezoneMode = "5 - Scan-Day of Week and Holiday";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          timezoneMode = num.ToString() + " (Unknown mode)";
          break;
      }
    }
    return timezoneMode;
  }

  private string ParseAuthMode(string[] words, int index) => this.ParseZeroExpected(words, index);

  private string ParseNullExpected(string[] words, int index)
  {
    string nullExpected = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      if (string.Equals("null", words[index], StringComparison.OrdinalIgnoreCase))
      {
        nullExpected = words[index];
      }
      else
      {
        this.textColor = this.errorFoundTextColor;
        nullExpected = words[index] + "  - NULL Expected";
      }
    }
    return nullExpected;
  }

  private string ParseZeroExpected(string[] words, int index)
  {
    string zeroExpected = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      if (int.Parse(words[index]) != 0)
      {
        this.textColor = this.errorFoundTextColor;
        zeroExpected = words[index] + "  - 0 Expected";
      }
      else
        zeroExpected = words[index];
    }
    return zeroExpected;
  }

  private string ParseOperType(string[] words, int index)
  {
    string operType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      switch (num)
      {
        case 0:
          operType = "0 - access level mappings";
          break;
        case 1:
          operType = "1 - access level assignments";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          operType = num.ToString() + " (Unknown operating mode type)";
          break;
      }
    }
    return operType;
  }

  private string ParseGmtOffset(string[] words, int index)
  {
    string gmtOffset = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num1 = int.Parse(words[index]);
      string str1 = gmtOffset + words[index];
      if (num1 != 0)
      {
        int num2 = num1 / 3600;
        int num3 = Math.Abs(num1 % 3600 / 60);
        int num4 = num1 % 3600;
        string str2 = num2 <= 0 ? $" (UTC+{Math.Abs(num2):d2}:{num3:d2}) " : $" (UTC-{Math.Abs(num2):d2}:{num3:d2}) ";
        gmtOffset = str1 + str2;
      }
      else
        gmtOffset = str1 + " (UTC) ";
      switch (num1)
      {
        case -46800:
          gmtOffset += "Ex. Samoa";
          break;
        case -43200:
          gmtOffset += "Ex. Fiji";
          break;
        case -39600:
          gmtOffset += "Ex. Solomon Is., New Caledonia";
          break;
        case -36000:
          gmtOffset += "Ex. Brisbane";
          break;
        case -34200:
          gmtOffset += "Ex. Adelaide";
          break;
        case -32400:
          gmtOffset += "Ex. Osaka, Sapporo, Toyko";
          break;
        case -28800:
          gmtOffset += "Ex. Beijing, Chongqing, Hong Kong, Urumqi";
          break;
        case -25200:
          gmtOffset += "Ex. Bangkok, Hanoi, Jakarta";
          break;
        case -23400:
          gmtOffset += "Ex. Yangon (Rangoon)";
          break;
        case -21600:
          gmtOffset += "Ex. Dhaka";
          break;
        case -20700:
          gmtOffset += "Ex. Kathmandu";
          break;
        case -19800:
          gmtOffset += "Ex. Chennai, Kolkata, Mumbai, New Delhi";
          break;
        case -18000:
          gmtOffset += "Ex. Islamabad, Karachi";
          break;
        case -16200:
          gmtOffset += "Ex. Kabul";
          break;
        case -14400:
          gmtOffset += "Ex. Moscow, St. Petersburg, Volgograd";
          break;
        case -12600:
          gmtOffset += "Ex. Tehran";
          break;
        case -10800:
          gmtOffset += "Ex. Kuwait, Riyadh";
          break;
        case -7200:
          gmtOffset += "Ex. Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius";
          break;
        case -3600:
          gmtOffset += "Ex. Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna";
          break;
        case 0:
          gmtOffset += "Ex. Dublin, Edinburgh, Lisbon, London";
          break;
        case 3600:
          gmtOffset += "Ex. Cape Verde Is.";
          break;
        case 7200:
          gmtOffset += "Ex. Mid-Atlantic";
          break;
        case 10800:
          gmtOffset += "Ex. Buenos Aires";
          break;
        case 12600:
          gmtOffset += "Ex. Newfoundland";
          break;
        case 14400:
          gmtOffset += "Ex. Atlantic Time (Canada)";
          break;
        case 16200:
          gmtOffset += "Ex. Caracas";
          break;
        case 18000:
          gmtOffset += "Ex. Eastern Time (US & Canada)";
          break;
        case 21600:
          gmtOffset += "Ex. Central Time (US & Canada)";
          break;
        case 25200:
          gmtOffset += "Ex. Mountain Time (US & Canada)";
          break;
        case 28800:
          gmtOffset += "Ex. Pacific Time (US & Canada)";
          break;
        case 32400:
          gmtOffset += "Ex. Alaska";
          break;
        case 36000:
          gmtOffset += "Ex. Hawaii";
          break;
        case 39600:
          gmtOffset += "Ex. Coordinated Universal Time-11";
          break;
        case 43200:
          gmtOffset += "Ex. Interantional Date Line West";
          break;
      }
    }
    return gmtOffset;
  }

  private string ParseOEMCodes(string[] words, int index)
  {
    string oemCodes = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
      oemCodes = this.ParseOEMCodes(int.Parse(words[index]));
    return oemCodes;
  }

  private string ParseDipSwitches(int value)
  {
    string str1 = value.ToString() + " (";
    for (int y = 0; y < 8; ++y)
    {
      string str2 = $"{str1}{(object) (y + 1)}=";
      str1 = (double) ((int) Math.Pow(2.0, (double) y) & value) != Math.Pow(2.0, (double) y) ? str2 + "OFF" : str2 + "ON";
      if (y < 7)
        str1 += ", ";
    }
    return str1 + ")";
  }

  private string ParseTlsStatus(int value)
  {
    string str = value.ToString() + " ( ";
    if ((value & 1) == 1)
      str += "TLS_Available ";
    if ((value & 2) == 2)
      str += "TLS_Enabled ";
    return str + ")";
  }

  private string ParseOEMCodes(int value)
  {
    string oemCodes;
    switch (value)
    {
      case 0:
        oemCodes = value.ToString() + " (default)";
        break;
      case 1:
        oemCodes = value.ToString() + " - Mercury Security";
        break;
      case 338:
      case 339:
        oemCodes = value.ToString() + " - Mercury Security";
        break;
      case 347:
        oemCodes = value.ToString() + " - DHS AP Series";
        break;
      case 467:
        oemCodes = value.ToString() + " - Imron AP Series";
        break;
      case 484:
      case 772:
        oemCodes = value.ToString() + " - RS2 AP Series";
        break;
      case 496:
        oemCodes = value.ToString() + " - Covi AP Series";
        break;
      case 1024 /*0x0400*/:
        oemCodes = value.ToString() + " - Lenel Systems International, Inc.";
        break;
      case 1028:
        oemCodes = value.ToString() + " - Stanley BAS";
        break;
      case 1033:
        oemCodes = value.ToString() + " - Bosch RKP";
        break;
      case 1280 /*0x0500*/:
        oemCodes = value.ToString() + " - AMT";
        break;
      case 1536 /*0x0600*/:
        oemCodes = value.ToString() + " - ARINC";
        break;
      case 1792 /*0x0700*/:
        oemCodes = value.ToString() + " - CTR";
        break;
      case 2048 /*0x0800*/:
      case 2049:
      case 2052:
      case 2053:
        oemCodes = value.ToString() + " - Keri Systems";
        break;
      case 2304 /*0x0900*/:
        oemCodes = value.ToString() + " - DVTel";
        break;
      case 2560 /*0x0A00*/:
        oemCodes = value.ToString() + " - IDenticard";
        break;
      case 2816 /*0x0B00*/:
        oemCodes = value.ToString() + " - Imron";
        break;
      case 3072 /*0x0C00*/:
        oemCodes = value.ToString() + " - MDI";
        break;
      case 3328 /*0x0D00*/:
        oemCodes = value.ToString() + " - Nexwatch";
        break;
      case 3584 /*0x0E00*/:
      case 3585:
      case 3586:
      case 3587:
      case 3588:
      case 3589:
      case 3590:
        oemCodes = value.ToString() + " - Open Options";
        break;
      case 3840 /*0x0F00*/:
      case 3841:
        oemCodes = value.ToString() + " - AccessNsite";
        break;
      case 4096 /*0x1000*/:
        oemCodes = value.ToString() + " - Ronake";
        break;
      case 4352:
      case 4353:
      case 4360:
      case 4361:
      case 4362:
      case 4363:
      case 4364:
      case 4365:
      case 4366:
      case 4367:
      case 4368:
      case 4369 /*0x1111*/:
        oemCodes = value.ToString() + " - RS2";
        break;
      case 4864:
        oemCodes = value.ToString() + " - SIASA";
        break;
      case 5120:
        oemCodes = value.ToString() + " - Synergistics";
        break;
      case 5376:
        oemCodes = value.ToString() + " - Vindicator";
        break;
      case 5632:
        oemCodes = value.ToString() + " - G4S (Touchcom)";
        break;
      case 5888:
        oemCodes = value.ToString() + " - DAQ";
        break;
      case 6144:
        oemCodes = value.ToString() + " - Avigilon";
        break;
      case 6400:
        oemCodes = value.ToString() + " - Acumen";
        break;
      case 6656:
        oemCodes = value.ToString() + " - BadgePass";
        break;
      case 6912:
        oemCodes = value.ToString() + " - Maxxess";
        break;
      case 7168:
        oemCodes = value.ToString() + " - Midpoint";
        break;
      case 7424:
        oemCodes = value.ToString() + " - Genetec";
        break;
      case 7680:
        oemCodes = value.ToString() + " - NLSS";
        break;
      case 7936:
        oemCodes = value.ToString() + " - S2";
        break;
      case 8192 /*0x2000*/:
        oemCodes = value.ToString() + " - Honeywell";
        break;
      case 8448:
        oemCodes = value.ToString() + " - RF Logics";
        break;
      case 8704:
        oemCodes = value.ToString() + " - IR";
        break;
      case 8960:
        oemCodes = value.ToString() + " - Security Management Systems";
        break;
      case 9216:
        oemCodes = value.ToString() + " - Feenics Inc.";
        break;
      case 9472:
        oemCodes = value.ToString() + " - Matrix Systems";
        break;
      case 9728:
        oemCodes = value.ToString() + " - Titan Security Group";
        break;
      case 9984:
        oemCodes = value.ToString() + " - Kastle Systems";
        break;
      case 10240:
        oemCodes = value.ToString() + " - Johnson Controls Inc.";
        break;
      case 10496:
        oemCodes = value.ToString() + " - LockState";
        break;
      case 10752:
        oemCodes = value.ToString() + " - BluB0X";
        break;
      case 11008:
        oemCodes = value.ToString() + " - Video Insight";
        break;
      case 11264:
        oemCodes = value.ToString() + " - Vanderbilt";
        break;
      case 11520:
        oemCodes = value.ToString() + " - Vanderbilt International";
        break;
      case 11776:
        oemCodes = value.ToString() + " - Schneider Electric";
        break;
      case 12032:
        oemCodes = value.ToString() + " - ReconaSense";
        break;
      case 12288 /*0x3000*/:
        oemCodes = value.ToString() + " - AMAG";
        break;
      case 12544:
        oemCodes = value.ToString() + " - I&SI SPA";
        break;
      case 12800:
        oemCodes = value.ToString() + " - Alarm.com";
        break;
      case 13056:
        oemCodes = value.ToString() + " - Mavin";
        break;
      case 13312:
        oemCodes = value.ToString() + " - Averics";
        break;
      case 13568:
        oemCodes = value.ToString() + " - Tecca";
        break;
      case 13824:
        oemCodes = value.ToString() + " - Brivo";
        break;
      case 44608:
        oemCodes = value.ToString() + " - Seibold";
        break;
      default:
        this.textColor = this.errorFoundTextColor;
        oemCodes = value.ToString() + " - Unknown OEM code";
        break;
    }
    return oemCodes;
  }

  private string ParseScpAddress(string[] words, int index)
  {
    string scpAddress = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      bool flag = false;
      int num1 = int.Parse(words[index]);
      if (num1 >= 256 /*0x0100*/)
        flag = true;
      int num2 = num1 & (int) byte.MaxValue;
      scpAddress = num2.ToString() + " ";
      if (num2 < 0 || num2 > 7)
      {
        this.textColor = this.errorFoundTextColor;
        scpAddress += " (InvalidAddress)";
      }
      if (flag)
        scpAddress += " (Bilingual)";
    }
    return scpAddress;
  }

  private string ParseDataSecurity(string[] words, int index)
  {
    string dataSecurity = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      switch (num & (int) byte.MaxValue)
      {
        case 0:
          dataSecurity = num.ToString() + " - None";
          break;
        case 2:
          dataSecurity = num.ToString() + " - AES w/ Password";
          break;
        case 3:
          dataSecurity = num.ToString() + " - TLS Required";
          break;
        case 4:
          dataSecurity = num.ToString() + " - TLS if Available";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          dataSecurity = num.ToString() + " (Unknown type)";
          break;
      }
    }
    return dataSecurity;
  }

  private string ParseConnMode(string[] words, int index)
  {
    string connMode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      switch (num & (int) byte.MaxValue)
      {
        case 0:
          connMode = num.ToString() + " - Continuous";
          break;
        case 1:
          connMode = num.ToString() + " - On-Demand";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          connMode = num.ToString() + " (Unknown ConnMode)";
          break;
      }
    }
    return connMode;
  }

  private string ParseHostCommCType(string[] words, int index, bool channelConfig)
  {
    string hostCommCtype = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      switch (num & (int) byte.MaxValue)
      {
        case 0:
          hostCommCtype = num.ToString() + " - Disabled";
          break;
        case 1:
          hostCommCtype = num.ToString() + " - IP Server";
          break;
        case 2:
          hostCommCtype = num.ToString() + " - IP Client";
          break;
        case 3:
          hostCommCtype = num.ToString() + " - RS-232";
          break;
        case 4:
          hostCommCtype = num.ToString() + " - Modem";
          break;
        case 5:
          hostCommCtype = num.ToString() + " - RS-485";
          break;
        case 6:
          hostCommCtype = num.ToString() + " - Serial Adapter";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          hostCommCtype = num.ToString() + " (Unknown cType)";
          break;
      }
    }
    return hostCommCtype;
  }

  private string ParseCType(string[] words, int index, bool channelConfig)
  {
    string ctype = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      int num = int.Parse(words[index]);
      if (num >= 256 /*0x0100*/)
      {
        if (channelConfig)
        {
          if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
            flag2 = true;
          if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
            flag3 = true;
        }
        else
          flag1 = true;
      }
      switch (num & (int) byte.MaxValue)
      {
        case 0:
          ctype = num.ToString() + " - Serial";
          break;
        case 1:
          ctype = num.ToString() + " - Modem Dial Out";
          break;
        case 2:
          ctype = num.ToString() + " - Modem Dial In";
          break;
        case 3:
          ctype = num.ToString() + " - Modem Dial Out/In";
          break;
        case 4:
          ctype = num.ToString() + " - TCP/IP connect to remote";
          break;
        case 5:
          ctype = num.ToString() + " - accept single connection from remote";
          break;
        case 6:
          ctype = num.ToString() + " - Virtual IC internal connection";
          break;
        case 7:
          ctype = num.ToString() + " - Concurrent multiple inbound connections";
          break;
        case 9:
          ctype = num.ToString() + " - Direct SIO";
          break;
        case 10:
          ctype = num.ToString() + " - Direct SIO (Honeywell)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          ctype = num.ToString() + " (Unknown cType)";
          break;
      }
      if (flag1)
        ctype += " (Disable Transactions)";
      if (flag2)
        ctype += " (TLS Required)";
      if (flag3)
        ctype += " (TLS Cert Verification)";
    }
    return ctype;
  }

  private string ParseRtsMode(string[] words, int index)
  {
    string rtsMode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      switch (num)
      {
        case 0:
          rtsMode += "0 - On";
          break;
        case 1:
          rtsMode += "1 - Toggle";
          break;
        case 2:
          rtsMode += "2 - Off";
          break;
        case 3:
          rtsMode += "3 - CTS/RTS handshake";
          break;
        default:
          rtsMode = num.ToString() + " (Unknown mode)";
          this.textColor = this.errorFoundTextColor;
          break;
      }
    }
    return rtsMode;
  }

  private string ParseCPort(string[] words, int index, int cType)
  {
    string cport = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      if (cType >= 256 /*0x0100*/)
        cType &= (int) byte.MaxValue;
      cport = words[index] + " ";
      switch (cType)
      {
        case 0:
          cport += " (physical port number)";
          break;
        case 1:
        case 2:
        case 3:
        case 4:
          cport += " (not used)";
          break;
        case 5:
        case 7:
          cport += " (tcp/ip port number)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          break;
      }
    }
    return cport;
  }

  private string ParseBurgUsrLevel(string[] words, int index)
  {
    string burgUsrLevel = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num1 = int.Parse(words[index]);
      burgUsrLevel += words[index];
      if (num1 != -1)
      {
        int num2 = num1 & (int) byte.MaxValue;
        int num3 = (num1 & 65280) >> 8;
        burgUsrLevel = $"{burgUsrLevel} (Index: {(object) num3}, Level: {(object) num2})";
      }
    }
    return burgUsrLevel;
  }

  private string ParseCMap(string[] words, int index)
  {
    string str1 = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num1 = int.Parse(words[index]);
      if (num1 > (int) sbyte.MaxValue)
        str1 += " OutOfRange ";
      str1 = $"{str1}{words[index]} = (";
      bool flag = true;
      for (int y = 0; y < 7; ++y)
      {
        int num2 = (int) Math.Pow(2.0, (double) y);
        if ((num2 & num1) == num2)
        {
          string str2;
          if (flag)
          {
            str2 = "";
            flag = false;
          }
          else
            str2 = ", ";
          switch (y)
          {
            case 0:
              str2 += "DisplayStatus";
              break;
            case 1:
              str2 += "Disarm";
              break;
            case 2:
              str2 += "ArmAway";
              break;
            case 3:
              str2 += "ArmStay";
              break;
            case 4:
              str2 += "ArmInstant";
              break;
            case 5:
              str2 += "ToggleChime";
              break;
            case 6:
              str2 += "BypassControl";
              break;
          }
          str1 += str2;
        }
      }
    }
    return str1 + ")";
  }

  private string ParseUCmnd(string[] words, int index)
  {
    string ucmnd = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1001:
          ucmnd = "1001 - UCMND_XTEND";
          break;
        case 1002:
          ucmnd = "1002 - UCMND_DIRECT";
          break;
        case 1003:
          ucmnd = "1003 - UCMND_BURG";
          break;
        case 1004:
          ucmnd = "1004 - UCMND_BURGA";
          break;
        case 1005:
          ucmnd = "1005 - UCMND_BURGB";
          break;
        case 1006:
          ucmnd = "1006 - UCMND_IPSA";
          break;
        case 1007:
          ucmnd = "1007 - UCMND_IPSB";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          ucmnd = words[index] + " (Unknown nUCmndId)";
          break;
      }
    }
    return ucmnd;
  }

  private string ParseMpgSetCommand(string[] words, int index)
  {
    string mpgSetCommand = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          mpgSetCommand = "1 - access";
          break;
        case 2:
          mpgSetCommand = "2 - override";
          break;
        case 3:
          mpgSetCommand = "3 - force arm";
          break;
        case 4:
          mpgSetCommand = "4 - arm";
          break;
        case 5:
          mpgSetCommand = "5 - override arm";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          mpgSetCommand = words[index] + " (Unknown command)";
          break;
      }
    }
    return mpgSetCommand;
  }

  private string ParseTzCommand(string[] words, int index)
  {
    string tzCommand = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          tzCommand = "1 - TmpClr";
          break;
        case 2:
          tzCommand = "2 - TmpSet";
          break;
        case 3:
          tzCommand = "3 - OvrClr";
          break;
        case 4:
          tzCommand = "4 - OvrSet";
          break;
        case 5:
          tzCommand = "5 - Release";
          break;
        case 6:
          tzCommand = "6 - Refresh";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          tzCommand = words[index] + " (Unknown command)";
          break;
      }
    }
    return tzCommand;
  }

  private string ParseLoginResourcesCommand(string[] words, int index)
  {
    string resourcesCommand = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          resourcesCommand = "1 - New List";
          break;
        case 2:
          resourcesCommand = "2 - Append to List";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          resourcesCommand = words[index] + " (Unknown command)";
          break;
      }
    }
    return resourcesCommand;
  }

  private string ParseNTPServerTypeCommand(string[] words, int index)
  {
    string serverTypeCommand = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          serverTypeCommand = "0 - Hostname";
          break;
        case 1:
          serverTypeCommand = "1 - IP";
          break;
        case 2:
          serverTypeCommand = "2 -  pool.ntp.org";
          break;
        case 3:
          serverTypeCommand = "3 -  time.nist.gov";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          serverTypeCommand = words[index] + " (Unknown command)";
          break;
      }
    }
    return serverTypeCommand;
  }

  private string ParseLoginResourcesFlags(string[] words, int index)
  {
    string loginResourcesFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      loginResourcesFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        loginResourcesFlags += "LRF_WRITABLE ";
      if ((num & 2) == 2)
      {
        this.textColor = this.errorFoundTextColor;
        loginResourcesFlags += "(UNKNOWN 0x0002) ";
      }
      if ((num & 4) == 4)
      {
        this.textColor = this.errorFoundTextColor;
        loginResourcesFlags += "(UNKNOWN 0x0004) ";
      }
      if ((num & 8) == 8)
      {
        this.textColor = this.errorFoundTextColor;
        loginResourcesFlags += "(UNKNOWN 0x0008) ";
      }
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
      {
        this.textColor = this.errorFoundTextColor;
        loginResourcesFlags += "(UNKNOWN 0x0010) ";
      }
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
      {
        this.textColor = this.errorFoundTextColor;
        loginResourcesFlags += "(UNKNOWN 0x0020) ";
      }
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
      {
        this.textColor = this.errorFoundTextColor;
        loginResourcesFlags += "(UNKNOWN 0x0040) ";
      }
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
      {
        this.textColor = this.errorFoundTextColor;
        loginResourcesFlags += "(UNKNOWN 0x0080) ";
      }
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
      {
        this.textColor = this.errorFoundTextColor;
        loginResourcesFlags += "(UNKNOWN 0x0100) ";
      }
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
      {
        this.textColor = this.errorFoundTextColor;
        loginResourcesFlags += "(UNKNOWN 0x0200) ";
      }
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
      {
        this.textColor = this.errorFoundTextColor;
        loginResourcesFlags += "(UNKNOWN 0x0400) ";
      }
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
      {
        this.textColor = this.errorFoundTextColor;
        loginResourcesFlags += "(UNKNOWN 0x0800) ";
      }
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
      {
        this.textColor = this.errorFoundTextColor;
        loginResourcesFlags += "(UNKNOWN 0x1000) ";
      }
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
      {
        this.textColor = this.errorFoundTextColor;
        loginResourcesFlags += "(UNKNOWN 0x2000) ";
      }
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
      {
        this.textColor = this.errorFoundTextColor;
        loginResourcesFlags += "(UNKNOWN 0x4000) ";
      }
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        this.textColor = this.errorFoundTextColor;
        loginResourcesFlags += "(UNKNOWN 0x8000) ";
      }
    }
    return loginResourcesFlags;
  }

  private string ParseLoginType(string[] words, int index)
  {
    string loginType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case -1:
          loginType = "-1 - Inactive";
          break;
        case 0:
          loginType = "0 - Standard Login";
          break;
        case 1:
          loginType = "1 - PSIA/REST";
          break;
        case 2:
          loginType = "2 - PSIA Client";
          break;
        case 3:
          loginType = "3 - BACnet";
          break;
        case 4:
          loginType = "4 - SNMP";
          break;
        case 5:
          loginType = "5 - MQTT Client";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          loginType = words[index] + " (Unknown type)";
          break;
      }
    }
    return loginType;
  }

  private string ParseFreeFormFieldType(string[] words, int index)
  {
    string freeFormFieldType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          freeFormFieldType = "0";
          break;
        case 1:
          freeFormFieldType = "1 - FFRM_FLD_ENCID";
          break;
        case 2:
          freeFormFieldType = "2 - FFRM_FLD_CARDNAME";
          break;
        case 3:
          freeFormFieldType = "3 - FFRM_FLD_LANGUAGE";
          break;
        case 4:
          freeFormFieldType = "4 - FFRM_FLD_DFLT_FLOOR";
          break;
        case 5:
          freeFormFieldType = "5 - FFRM_FLD_OTIS_FLAGS";
          break;
        case 6:
          freeFormFieldType = "6 - FFRM_FLD_KONE_FLAGS";
          break;
        case 7:
          freeFormFieldType = "7 - FFRM_FLD_KONE_ECAS";
          break;
        case 8:
          freeFormFieldType = "8 - FFRM_FLD_ACCESSFLGS";
          break;
        case 9:
          freeFormFieldType = "9 - FFRM_FLD_TKE_CRD_HLDR_FLGS";
          break;
        case 10:
          freeFormFieldType = "10 - FFRM_FLD_TKE_CAR_PREF";
          break;
        case 11:
          freeFormFieldType = "11 - FFRM_FLD_MITSU_CRD_HLDR_FLGS";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          freeFormFieldType = words[index] + " (Unknown type)";
          break;
      }
    }
    return freeFormFieldType;
  }

  private string ParseMultipleOccupancyMode(string[] words, int index)
  {
    string multipleOccupancyMode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          multipleOccupancyMode = "0 - AREA_MO_NONE";
          break;
        case 1:
          multipleOccupancyMode = "1 - AREA_MO_STD";
          break;
        case 2:
          multipleOccupancyMode = "2 - AREA_MO_M1M";
          break;
        case 3:
          multipleOccupancyMode = "3 - AREA_MO_M2M";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          multipleOccupancyMode = words[index] + " (Unknown mode)";
          break;
      }
    }
    return multipleOccupancyMode;
  }

  private string ParseTransactionSourceType(string[] words, int index, bool hexValue)
  {
    string transactionSourceType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = !hexValue ? int.Parse(words[index]) : int.Parse(words[index], NumberStyles.HexNumber);
      switch (num)
      {
        case 0:
          transactionSourceType = num.ToString() + " - tranSrcScpDiag";
          break;
        case 1:
          transactionSourceType = num.ToString() + " - tranSrcScpCom";
          break;
        case 2:
          transactionSourceType = num.ToString() + " - tranSrcScpLcl";
          break;
        case 3:
          transactionSourceType = num.ToString() + " - tranSrcSioDiag";
          break;
        case 4:
          transactionSourceType = num.ToString() + " - tranSrcSioCom";
          break;
        case 5:
          transactionSourceType = num.ToString() + " - tranSrcSioTmpr";
          break;
        case 6:
          transactionSourceType = num.ToString() + " - tranSrcSioPwr";
          break;
        case 7:
          transactionSourceType = num.ToString() + " - tranSrcMP";
          break;
        case 8:
          transactionSourceType = num.ToString() + " - tranSrcCP";
          break;
        case 9:
          transactionSourceType = num.ToString() + " - tranSrcACR";
          break;
        case 10:
          transactionSourceType = num.ToString() + " - tranSrcAcrTmpr";
          break;
        case 11:
          transactionSourceType = num.ToString() + " - tranSrcAcrDoor";
          break;
        case 13:
          transactionSourceType = num.ToString() + " - tranSrcAcrRex0";
          break;
        case 14:
          transactionSourceType = num.ToString() + " - tranSrcAcrRex1";
          break;
        case 15:
          transactionSourceType = num.ToString() + " - tranSrcTimeZone";
          break;
        case 16 /*0x10*/:
          transactionSourceType = num.ToString() + " - tranSrcProcedure";
          break;
        case 17:
          transactionSourceType = num.ToString() + " - tranSrcTrigger";
          break;
        case 18:
          transactionSourceType = num.ToString() + " - tranSrcTrigVar";
          break;
        case 19:
          transactionSourceType = num.ToString() + " - tranSrcMPG";
          break;
        case 20:
          transactionSourceType = num.ToString() + " - tranSrcArea";
          break;
        case 21:
          transactionSourceType = num.ToString() + " - tranSrcAcrTmprAlt";
          break;
        case 22:
          transactionSourceType = num.ToString() + " - tranSrcIPS";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          transactionSourceType = words[index] + " (Unknown sourceType)";
          break;
      }
    }
    return transactionSourceType;
  }

  private string ParseTranType(string[] words, int index)
  {
    string tranType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      switch (num)
      {
        case 1:
          tranType = num.ToString() + " - tranTypeSys";
          break;
        case 2:
          tranType = num.ToString() + " - tranTypeSioComm";
          break;
        case 3:
          tranType = num.ToString() + " - tranTypeCardBin";
          break;
        case 4:
          tranType = num.ToString() + " - tranTypeCardBcd";
          break;
        case 5:
          tranType = num.ToString() + " - tranTypeCardFull";
          break;
        case 6:
          tranType = num.ToString() + " - tranTypeCardID";
          break;
        case 7:
          tranType = num.ToString() + " - tranTypeCoS";
          break;
        case 8:
          tranType = num.ToString() + " - tranTypeREX";
          break;
        case 9:
          tranType = num.ToString() + " - tranTypeCoSDoor";
          break;
        case 10:
          tranType = num.ToString() + " - tranTypeProcedure";
          break;
        case 11:
          tranType = num.ToString() + " - tranTypeUserCmnd";
          break;
        case 12:
          tranType = num.ToString() + " - tranTypeActivate";
          break;
        case 13:
          tranType = num.ToString() + " - tranTypeAcr";
          break;
        case 14:
          tranType = num.ToString() + " - tranTypeMpg";
          break;
        case 15:
          tranType = num.ToString() + " - tranTypeArea";
          break;
        case 16 /*0x10*/:
          tranType = num.ToString() + " - tranTypeAsset";
          break;
        case 17:
          tranType = num.ToString() + " - tranTypeBio1";
          break;
        case 18:
          tranType = num.ToString() + " - tranTypeUserCmndX";
          break;
        case 19:
          tranType = num.ToString() + " - tranTypeUseLimit";
          break;
        case 21:
          tranType = num.ToString() + " - tranTypeDblCardFull";
          break;
        case 22:
          tranType = num.ToString() + " - tranTypeDblCardID";
          break;
        case 23:
          tranType = num.ToString() + " - tranTypeMpgIps";
          break;
        case 24:
          tranType = num.ToString() + " - tranTypeOperatingMode";
          break;
        case 32 /*0x20*/:
          tranType = num.ToString() + " - tranTypeAssetI64";
          break;
        case 33:
          tranType = num.ToString() + " - tranTypeBio1I64";
          break;
        case 34:
          tranType = num.ToString() + " - tranTypeBioExt";
          break;
        case 37:
          tranType = num.ToString() + " - tranTypeI64CardFull";
          break;
        case 38:
          tranType = num.ToString() + " - tranTypeI64CardID";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          tranType = num.ToString() + " (Unknown tranType)";
          break;
      }
    }
    return tranType;
  }

  private string ParseTriggerArgs(string[] words, int index, int argNum, int transType)
  {
    string triggerArgs = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num1 = int.Parse(words[index]);
      switch (transType)
      {
        case 5:
          switch (argNum)
          {
            case 0:
              triggerArgs = $"{triggerArgs}{(object) num1} (User Level Index)";
              break;
            case 1:
              triggerArgs = $"{triggerArgs}{(object) num1} (User Level To Match)";
              break;
            case 2:
              triggerArgs = $"{triggerArgs}{(object) num1} (N/A)";
              break;
            case 3:
              triggerArgs = $"{triggerArgs}{(object) num1} (N/A)";
              break;
          }
          break;
        case 6:
          switch (argNum)
          {
            case 0:
              triggerArgs = $"{triggerArgs}{(object) num1} (User Level Index)";
              break;
            case 1:
              triggerArgs = $"{triggerArgs}{(object) num1} (User Level To Match)";
              break;
            case 2:
              triggerArgs = $"{triggerArgs}{(object) num1} (Floor number selected (-1 = any floor))";
              break;
            case 3:
              triggerArgs = $"{triggerArgs}{(object) num1} (Special C_ACR::actl_flags)";
              break;
          }
          break;
        case 7:
          switch (argNum)
          {
            case 0:
              switch (num1)
              {
                case 1:
                  triggerArgs = $"{triggerArgs}{(object) num1} (Trigger only if status is now inactive (was active))";
                  break;
                case 2:
                  triggerArgs = $"{triggerArgs}{(object) num1} (Trigger only if status is now active (was inactive))";
                  break;
                case 3:
                  triggerArgs = $"{triggerArgs}{(object) num1} (Trigger only if last state was ALARM)";
                  break;
                case 4:
                  triggerArgs = $"{triggerArgs}{(object) num1} (Trigger only if last state was FAULT)";
                  break;
                case 5:
                  triggerArgs = $"{triggerArgs}{(object) num1} (Trigger only if last state was ALARM or FAULT)";
                  break;
                default:
                  triggerArgs += (string) (object) num1;
                  break;
              }
              break;
            case 1:
              triggerArgs = $"{triggerArgs}{(object) num1} (N/A)";
              break;
            case 2:
              triggerArgs = $"{triggerArgs}{(object) num1} (N/A)";
              break;
            case 3:
              triggerArgs = $"{triggerArgs}{(object) num1} (N/A)";
              break;
          }
          break;
        case 9:
          switch (argNum)
          {
            case 0:
              switch (num1)
              {
                case 1:
                  triggerArgs = $"{triggerArgs}{(object) num1} (Trigger only if, TRAN_CODE=3: held open pre-alarm only, TRAN_CODE=4: Forced Open only)";
                  break;
                case 2:
                  triggerArgs = $"{triggerArgs}{(object) num1} (Trigger only if, TRAN_CODE=3: Forced Open is being cancelled, TRAN_CODE=4: Held Open only)";
                  break;
                case 3:
                  triggerArgs = $"{triggerArgs}{(object) num1} (Trigger only if, TRAN_CODE=3: Held Open is being cancelled, TRAN_CODE=4: Both Forced/Held)";
                  break;
                case 4:
                  triggerArgs = $"{triggerArgs}{(object) num1} (Trigger only if, TRAN_CODE=3: Held or Forced is being cancelled, TRAN_CODE=4: Held Open, regardless of forced open status)";
                  break;
                case 5:
                  triggerArgs = $"{triggerArgs}{(object) num1} (Trigger only if, TRAN_CODE=3: the door just closed, TRAN_CODE=4: Either Forced/Held)";
                  break;
                case 6:
                  triggerArgs = $"{triggerArgs}{(object) num1} (Trigger only if, TRAN_CODE=3: the door just opended, no alarm, TRAN_CODE=4: any reader alarm)";
                  break;
                default:
                  triggerArgs = $"{triggerArgs}{(object) num1} (Trigger on any other, TRAN_CODE=3: door safe message, TRAN_CODE=4: any reader alarm)";
                  break;
              }
              break;
            case 1:
              triggerArgs = $"{triggerArgs}{(object) num1} (N/A)";
              break;
            case 2:
              triggerArgs = $"{triggerArgs}{(object) num1} (N/A)";
              break;
            case 3:
              triggerArgs = $"{triggerArgs}{(object) num1} (N/A)";
              break;
          }
          break;
        case 11:
          switch (argNum)
          {
            case 0:
              int num2 = (num1 & 240 /*0xF0*/) >> 4;
              int num3 = num1 & 15;
              triggerArgs = $"{triggerArgs}{(object) num1} (1st:{(object) num2} + 2nd:{(object) num3})";
              break;
            case 1:
              int num4 = (num1 & 240 /*0xF0*/) >> 4;
              int num5 = num1 & 15;
              triggerArgs = $"{triggerArgs}{(object) num1} (3rd:{(object) num4} + 4th:{(object) num5})";
              break;
            case 2:
              int num6 = (num1 & 240 /*0xF0*/) >> 4;
              int num7 = num1 & 15;
              triggerArgs = $"{triggerArgs}{(object) num1} (5th:{(object) num6} + 6th:{(object) num7})";
              break;
            case 3:
              int num8 = (num1 & 240 /*0xF0*/) >> 4;
              int num9 = num1 & 15;
              triggerArgs = $"{triggerArgs}{(object) num1} (7th:{(object) num8} + 8th:{(object) num9})";
              break;
          }
          break;
        case 24:
          switch (argNum)
          {
            case 0:
              triggerArgs = $"{triggerArgs}{(object) num1} (Existing Operating Mode)";
              break;
            case 1:
              triggerArgs = $"{triggerArgs}{(object) num1} (N/A)";
              break;
            case 2:
              triggerArgs = $"{triggerArgs}{(object) num1} (N/A)";
              break;
            case 3:
              triggerArgs = $"{triggerArgs}{(object) num1} (N/A)";
              break;
          }
          break;
        case 65:
          switch (argNum)
          {
            case 0:
              triggerArgs = $"{triggerArgs}{(object) num1} (Pseudo Point Number)";
              break;
            case 1:
              triggerArgs = $"{triggerArgs}{(object) num1} (N/A)";
              break;
            case 2:
              triggerArgs = $"{triggerArgs}{(object) num1} (N/A)";
              break;
            case 3:
              triggerArgs = $"{triggerArgs}{(object) num1} (N/A)";
              break;
          }
          break;
        default:
          triggerArgs = words[index] + " (N/A)";
          break;
      }
    }
    return triggerArgs;
  }

  private string ParseTriggerCommand(string[] words, int index)
  {
    string triggerCommand = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          triggerCommand = "1 - (Abort a delayed procedure)";
          break;
        case 2:
          triggerCommand = "2 - (Execute actions with prefix 0)";
          break;
        case 3:
          triggerCommand = "3 - (Resume a delayed procedure)";
          break;
        case 4:
          triggerCommand = "4 - (Execute actions with prefix 256)";
          break;
        case 5:
          triggerCommand = "5 - (Execute actions with prefix 512)";
          break;
        case 6:
          triggerCommand = "6 - (Execute actions with prefix 1024)";
          break;
        case 7:
          triggerCommand = "7 - (Resume actions with prefix 256)";
          break;
        case 8:
          triggerCommand = "8 - (Resume actions with prefix 512)";
          break;
        case 9:
          triggerCommand = "9 - (Resume actions with prefix 1024)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          triggerCommand = words[index] + " (Unknown command)";
          break;
      }
    }
    return triggerCommand;
  }

  private string ParseSaveMode(string[] words, int index)
  {
    string saveMode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          saveMode = "0 - Save all settings (system and all Scp's)";
          break;
        case 1:
          saveMode = "1 - Save ONLY the system settings";
          break;
        case 2:
          saveMode = "2 - Save only the settings for the Scp specified";
          break;
        case 3:
          saveMode = "3 - Save the 'skeleton' configuration parameters only for the specified SCP";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          saveMode = words[index] + " (Unknown mode)";
          break;
      }
    }
    return saveMode;
  }

  private string ParsePollMode(string[] words, int index)
  {
    string pollMode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          pollMode = "0 - DPOLL_ENABLE";
          break;
        case 1:
          pollMode = "1 - DPOLL_DISABLE";
          break;
        case 2:
          pollMode = "2 - DPOLL_ALTERNATE";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          pollMode = words[index] + " (Unknown mode)";
          break;
      }
    }
    return pollMode;
  }

  private string ParseFileType(string[] words, int index)
  {
    string fileType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          fileType = "0 - Certificate";
          break;
        case 1:
          fileType = "1 - User Defined";
          break;
        case 2:
          fileType = "2 - License";
          break;
        case 3:
          fileType = "3 - Peer Cert";
          break;
        case 4:
          fileType = "4 - OSDP File Transfer";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          fileType = words[index] + " (Unknown FileType)";
          break;
      }
    }
    return fileType;
  }

  private string ParseCertType(string[] words, int index)
  {
    string certType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          certType = "0 - Certificate";
          break;
        case 1:
          certType = "1 - Peer Certificate";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          certType = words[index] + " (Unknown CertType)";
          break;
      }
    }
    return certType;
  }

  private string ParseCpCtlCommand(string[] words, int index)
  {
    string cpCtlCommand = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          cpCtlCommand = "1 - Off";
          break;
        case 2:
          cpCtlCommand = "2 - On";
          break;
        case 3:
          cpCtlCommand = "3 - Single Pulse";
          break;
        case 4:
          cpCtlCommand = "4 - Repeating Pulse";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          cpCtlCommand = words[index] + " (Unknown command)";
          break;
      }
    }
    return cpCtlCommand;
  }

  private string ParseProcedureCommand(string[] words, int index)
  {
    string procedureCommand = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          procedureCommand = "1 - Abort a delayed procedure";
          break;
        case 2:
          procedureCommand = "2 - Execute (prefix 0 actions)";
          break;
        case 3:
          procedureCommand = "3 - Resume a delayed procedure";
          break;
        case 4:
          procedureCommand = "4 - Execute (prefix 256 actions)";
          break;
        case 5:
          procedureCommand = "5 - Execute (prefix 512 actions)";
          break;
        case 6:
          procedureCommand = "6 - Execute (prefix 1024 actions)";
          break;
        case 7:
          procedureCommand = "7 - Resume  (prefix 256 actions)";
          break;
        case 8:
          procedureCommand = "8 - Resume  (prefix 512 actions)";
          break;
        case 9:
          procedureCommand = "9 - Resume  (prefix 1024 actions)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          procedureCommand = words[index] + " (Unknown ProcedureCommand)";
          break;
      }
    }
    return procedureCommand;
  }

  private string ParseLcdTextType(string[] words, int index)
  {
    string lcdTextType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          lcdTextType = "1 - Permanent";
          break;
        case 2:
          lcdTextType = "2 - Temp";
          break;
        case 3:
          lcdTextType = "3 - Offline (Not Used)";
          break;
        case 4:
          lcdTextType = "4 - Indexed, Permanent";
          break;
        case 5:
          lcdTextType = "5 - Indexed, Temp";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          lcdTextType = words[index] + " (Unknown type)";
          break;
      }
    }
    return lcdTextType;
  }

  private string ParseSioDcCmndId(string[] words, int index)
  {
    string sioDcCmndId = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          sioDcCmndId = "0 - Poll";
          break;
        case 1:
          sioDcCmndId = "1 - ID Request";
          break;
        case 2:
          sioDcCmndId = "2 - Local Status Request";
          break;
        case 3:
          sioDcCmndId = "3 - Enable Diagnostic Report";
          break;
        case 4:
          sioDcCmndId = "4 - Disable Diagnostic Report";
          break;
        case 6:
          sioDcCmndId = "6 - Reply: tx enable to tx start time";
          break;
        case 7:
          sioDcCmndId = "7 - Loads the ID";
          break;
        case 8:
          sioDcCmndId = "8 - ID Request - sets CRC mode for comm";
          break;
        case 11:
          sioDcCmndId = "11 - Set NV parameter";
          break;
        case 16 /*0x10*/:
          sioDcCmndId = "16 - Input Conversion Table Configuration";
          break;
        case 17:
          sioDcCmndId = "17 - Input Point Configuration";
          break;
        case 18:
          sioDcCmndId = "18 - Input Point Status Request";
          break;
        case 19:
          sioDcCmndId = "19 - Ouput Point Status Request";
          break;
        case 21:
          sioDcCmndId = "21 - Board timeout - watchdog fault test";
          break;
        case 25:
          sioDcCmndId = "25 - Output Configuration Command";
          break;
        case 26:
          sioDcCmndId = "26 - Output Control Command";
          break;
        case 27:
          sioDcCmndId = "27 - Reader Led Control Command";
          break;
        case 28:
          sioDcCmndId = "28 - Reader Buzzer Control Command";
          break;
        case 30:
          sioDcCmndId = "30 - Monitor Output";
          break;
        case 33:
          sioDcCmndId = "33 - Reader Configuration Command";
          break;
        case 34:
          sioDcCmndId = "34 - Reader Tamper Status Request";
          break;
        case 35:
          sioDcCmndId = "35 - Card Format Configuration Commnad";
          break;
        case 36:
          sioDcCmndId = "36 - Offline Reader Configuration";
          break;
        case 37:
          sioDcCmndId = "37 - Offline LED Configuration";
          break;
        case 40:
          sioDcCmndId = "40 - Veify IDX Bio Record";
          break;
        case 41:
          sioDcCmndId = "41 - PIB/Signature Info Request";
          break;
        case 42:
          sioDcCmndId = "42 - Relay Control Command";
          break;
        case 65:
          sioDcCmndId = "65 - Load Firmware Hex File";
          break;
        case 81:
          sioDcCmndId = "81 - Broadcast Time String";
          break;
        case 160 /*0xA0*/:
          sioDcCmndId = "160 - AES Encrypted Command";
          break;
        case 161:
          sioDcCmndId = "161 - AES Load Master Key";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          sioDcCmndId = words[index] + " (Unknown CmndId)";
          break;
      }
    }
    return sioDcCmndId;
  }

  private string ParseLcdTextTones(string[] words, int index)
  {
    string lcdTextTones = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          lcdTextTones = "0 - None";
          break;
        case 1:
          lcdTextTones = "1 - Steady";
          break;
        case 2:
          lcdTextTones = "2 - Tone2 (on = .1 sec., off = .1 sec., repeat)";
          break;
        case 3:
          lcdTextTones = "3 - Tone3 (on = .3 sec., off = .5 sec., repeat)";
          break;
        case 4:
          lcdTextTones = "4 - Tone4 (on = .6 sec., off = .2 sec., repeat)";
          break;
        case 5:
          lcdTextTones = "5 - Tone5 (on = .5 sec., off = .1 sec., on = .1 sec., off = .1 sec., on = .1 sec., off = .1 sec., on = 1.1 sec., off = .1 sec., on = .1 sec., off = .1 sec., on = .1 sec., off = .1 sec., on = .6 sec., repeat.)";
          break;
        case 7:
          lcdTextTones = "7 - Abort current tone sequence";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          lcdTextTones = words[index] + " (Unknown tone)";
          break;
      }
    }
    return lcdTextTones;
  }

  private string ParseHostResponseCommand(string[] words, int index)
  {
    string hostResponseCommand = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          hostResponseCommand = "0 - Deny Access";
          break;
        case 1:
          hostResponseCommand = "1 - Allow Access";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          hostResponseCommand = words[index] + " (Unknown command)";
          break;
      }
    }
    return hostResponseCommand;
  }

  private string ParseNvArgType(string[] words, int index)
  {
    string nvArgType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 11:
          nvArgType = "11 - Factory settings";
          break;
        case 12:
          nvArgType = "12 - Factory settings";
          break;
        case 13:
          nvArgType = "13 - SCP oem code";
          break;
        case 14:
          nvArgType = "14 - Factory settings";
          break;
        case 15:
          nvArgType = "15 - Factory settings";
          break;
        case 16 /*0x10*/:
          nvArgType = "16 - Factory settings";
          break;
        case 20:
          nvArgType = "20 - Factory settings";
          break;
        case 31 /*0x1F*/:
          nvArgType = "31 - Factory settings";
          break;
        case 32 /*0x20*/:
          nvArgType = "32 - Factory settings";
          break;
        case 33:
          nvArgType = "33 - SIO oem code";
          break;
        case 34:
          nvArgType = "34 - Factory settings";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          nvArgType = words[index] + " (Unknown type)";
          break;
      }
    }
    return nvArgType;
  }

  private string ParseDiagCode(string[] words, int index)
  {
    string diagCode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          diagCode = "0 - ForceException";
          break;
        case 1:
          diagCode = "1 - BulkErase";
          break;
        case 2:
          diagCode = "2 - Erase eMMC Flash";
          break;
        case 3:
          diagCode = "3 - Retest License";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          diagCode = words[index] + " (Unknown code)";
          break;
      }
    }
    return diagCode;
  }

  private string ParseAreaAccessControl(string[] words, int index)
  {
    string areaAccessControl = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          areaAccessControl = "0 - NOP";
          break;
        case 1:
          areaAccessControl = "1 - DisableAccess";
          break;
        case 2:
          areaAccessControl = "2 - EnableAccess";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          areaAccessControl = words[index] + " (Unknown access_control)";
          break;
      }
    }
    return areaAccessControl;
  }

  private string ParseSioNetworkMode(string[] words, int index)
  {
    string sioNetworkMode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          sioNetworkMode = "0 - Mercury DHCP";
          break;
        case 1:
          sioNetworkMode = "1 - DHCP";
          break;
        case 2:
          sioNetworkMode = "2 - Static";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          sioNetworkMode = words[index] + " (Unknown nMode)";
          break;
      }
    }
    return sioNetworkMode;
  }

  private string ParseOccControl(string[] words, int index)
  {
    string occControl = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          occControl = "0 - Do Not Change";
          break;
        case 1:
          occControl = "1 - AREA_OC_STD";
          break;
        case 2:
          occControl = "2 - AREA_OC_SPC";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          occControl = words[index] + " (Unknown occ_control)";
          break;
      }
    }
    return occControl;
  }

  private string ParseStructId(string[] words, int index)
  {
    string structId = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          structId = "1 - SCPSID_TRAN";
          break;
        case 2:
          structId = "2 - SCPSID_TZ";
          break;
        case 3:
          structId = "3 - SCPSID_HOL";
          break;
        case 4:
          structId = "4 - SCPSID_MSP1";
          break;
        case 5:
          structId = "5 - SCPSID_SIO";
          break;
        case 6:
          structId = "6 - SCPSID_MP";
          break;
        case 7:
          structId = "7 - SCPSID_CP";
          break;
        case 8:
          structId = "8 - SCPSID_ACR";
          break;
        case 9:
          structId = "9 - SCPSID_ALVL";
          break;
        case 10:
          structId = "10 - SCPSID_TRIG";
          break;
        case 11:
          structId = "11 - SCPSID_PROC";
          break;
        case 12:
          structId = "12 - SCPSID_MPG";
          break;
        case 13:
          structId = "13 - SCPSID_AREA";
          break;
        case 14:
          structId = "14 - SCPSID_EAL";
          break;
        case 15:
          structId = "15 - SCPSID_CRDB";
          break;
        case 16 /*0x10*/:
          structId = "16 - SCPSID_ASDB";
          break;
        case 17:
          structId = "17 - SCPSID_BIO1";
          break;
        case 18:
          structId = "18 - SCPSID_BIO2";
          break;
        case 19:
          structId = "19 - SCPSID_BIO3";
          break;
        case 20:
          structId = "20 - SCPSID_FLASH";
          break;
        case 21:
          structId = "21 - SCPSID_BSQN";
          break;
        case 22:
          structId = "22 - SCPSID_SAVE_STAT";
          break;
        case 23:
          structId = "23 - SCPSID_MAB1_FREE";
          break;
        case 24:
          structId = "24 - SCPSID_MAB2_FREE";
          break;
        case 25:
          structId = "25 - SCPSID_IPS";
          break;
        case 26:
          structId = "26 - SCPSID_ARQ_BUFFER";
          break;
        case 27:
          structId = "27 - SCPSID_PART_FREE_CNT";
          break;
        case 28:
          structId = "28 - SCPSID_FEATURECODE";
          break;
        case 29:
          structId = "29 - SCPSID_BIO_OSDP_1";
          break;
        case 30:
          structId = "30 - SCPSID_BIO_OSDP_2";
          break;
        case 31 /*0x1F*/:
          structId = "31 - SCPSID_BIO_OSDP_3";
          break;
        case 32 /*0x20*/:
          structId = "32 - SCPSID_BIO_OSDP_4";
          break;
        case 33:
          structId = "33 - SCPSID_LOGIN_STANDARD";
          break;
        case 34:
          structId = "34 - SCPSID_LOGIN_PSIA";
          break;
        case 35:
          structId = "35 - SCPSID_FILE_SYSTEM";
          break;
        case 36:
          structId = "36 - SCPSID_CPU_BBSN";
          break;
        case 37:
          structId = "37 - SCPSID_CPU_CPUBSN";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          structId = words[index] + " (Unknown)";
          break;
      }
    }
    return structId;
  }

  private string ParseAreaSetCommand(string[] words, int index)
  {
    string areaSetCommand = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          areaSetCommand = "1 - Disable Access";
          break;
        case 2:
          areaSetCommand = "2 - Enable Access";
          break;
        case 3:
          areaSetCommand = "3 - Set occupancy count (standard)";
          break;
        case 4:
          areaSetCommand = "4 - Set occupancy count (special)";
          break;
        case 5:
          areaSetCommand = "5 - Clear occupancy counts (standard + special)";
          break;
        case 6:
          areaSetCommand = "6 - Disable multi-occupancy rules";
          break;
        case 7:
          areaSetCommand = "7 - Enable standard multi-occupancy";
          break;
        case 8:
          areaSetCommand = "8 - Enable modified 1-man rule";
          break;
        case 9:
          areaSetCommand = "9 - Enable modified 2-man rule";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          areaSetCommand = words[index] + " (Unknown command)";
          break;
      }
    }
    return areaSetCommand;
  }

  private string ParseArgActionType(string[] words, int index)
  {
    string argActionType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          argActionType = "0 - this action type is ignored";
          break;
        case 1:
          argActionType = "1 - arg == MP number";
          break;
        case 2:
          argActionType = "2 - arg == CP number";
          break;
        case 3:
          argActionType = "3 - arg == ACR number";
          break;
        case 4:
          argActionType = "4 - arg == ACR number";
          break;
        case 5:
          argActionType = "5 - arg == ACR number";
          break;
        case 6:
          argActionType = "6 - arg == ACR number";
          break;
        case 7:
          argActionType = "7 - arg == proc number";
          break;
        case 8:
          argActionType = "8 - TV number";
          break;
        case 9:
          argActionType = "9 - TZ number";
          break;
        case 10:
          argActionType = "10 - arg == ACR number";
          break;
        case 11:
          argActionType = "11 -  arg == CardholderID/-1";
          break;
        case 12:
          argActionType = "12 - arg not used";
          break;
        case 13:
          argActionType = "13 - arg == (MSP1 port * 16) + channel";
          break;
        case 14:
          argActionType = "14 - arg == Monitor Point Group number";
          break;
        case 15:
          argActionType = "15 - arg == Monitor Point Group number";
          break;
        case 16 /*0x10*/:
          argActionType = "16 - arg == Monitor Point Group number";
          break;
        case 17:
          argActionType = "17 - arg == Area number";
          break;
        case 18:
          argActionType = "18 - arg == ACR number";
          break;
        case 19:
          argActionType = "19 - arg == ACR number";
          break;
        case 20:
          argActionType = "20 - arg == Terminal (ACR) number";
          break;
        case 21:
          argActionType = "21 - arg not used";
          break;
        case 23:
          argActionType = "23 - arg == IPS Group number";
          break;
        case 24:
          argActionType = "24 - arg == ACR number";
          break;
        case 25:
          argActionType = "25 - arg == ACR number";
          break;
        case (int) sbyte.MaxValue:
          argActionType = "127 - this action type is ignored";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          argActionType = words[index] + " (Unknown actiontype)";
          break;
      }
    }
    return argActionType;
  }

  private string ParseAccessCfg(string[] words, int index)
  {
    string accessCfg = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          accessCfg = "0 - ACR_A_SINGLE";
          break;
        case 1:
          accessCfg = "1 - ACR_A_MASTER";
          break;
        case 2:
          accessCfg = "2 - ACR_A_SLAVE";
          break;
        case 3:
          accessCfg = "3 - ACR_A_TURNSTILE";
          break;
        case 4:
          accessCfg = "4 - ACR_A_EL1";
          break;
        case 5:
          accessCfg = "5 - ACR_A_EL2";
          break;
        case 6:
          accessCfg = "6 - ACR_A_OT1";
          break;
        case 7:
          accessCfg = "7 - ACR_A_KONE";
          break;
        case 8:
          accessCfg = "8 - ACR_A_TKE";
          break;
        case 9:
          accessCfg = "9 - ACR_A_MITSU";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          accessCfg = words[index] + " (Unknown mode)";
          break;
      }
    }
    return accessCfg;
  }

  private string ParseAltRdrSpec(string[] words, int index)
  {
    string altRdrSpec = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          altRdrSpec = "0 - ACR_AR_NONE";
          break;
        case 1:
          altRdrSpec = "1 - ACR_AR_NRML";
          break;
        case 2:
          altRdrSpec = "2 - ACR_AR_BIO1 (RSI)";
          break;
        case 3:
          altRdrSpec = "3 - ACR_AR_BIO2 (IDX)";
          break;
        case 4:
          altRdrSpec = "4 - ACR_AR_BIO3 (BIOSCRYPT)";
          break;
        case 5:
          altRdrSpec = "5 - ACR_AR_BIO4 (Iridian)";
          break;
        case 6:
          altRdrSpec = "6 - ACR_AR_BIO_OSDP_1 (OSDP-1)";
          break;
        case 7:
          altRdrSpec = "7 - ACR_AR_BIO_OSDP_2 (OSDP-2)";
          break;
        case 8:
          altRdrSpec = "8 - ACR_AR_BIO_OSDP_3 (OSDP-3)";
          break;
        case 9:
          altRdrSpec = "9 - ACR_AR_BIO_OSDP_4 (OSDP-4)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          altRdrSpec = words[index] + " (Unknown mode)";
          break;
      }
    }
    return altRdrSpec;
  }

  private string ParseTime(string[] words, int index, int baseYear)
  {
    string time = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      if (words[index] != "-1")
      {
        uint num = uint.Parse(words[index]);
        time = num.ToString() + " ";
        if (num > 0U)
        {
          DateTime dateTime = new DateTime(baseYear, 1, 1).AddSeconds((double) num);
          time += dateTime.ToString();
        }
      }
      else
        time = words[index];
    }
    return time;
  }

  private string ParseTimeDays(string[] words, int index, int baseYear)
  {
    string timeDays = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      if (words[index] != "-1")
      {
        uint num = uint.Parse(words[index]);
        timeDays = num.ToString() + " ";
        if (num > 0U)
        {
          DateTime dateTime = new DateTime(baseYear, 1, 1).AddDays((double) num);
          timeDays += dateTime.ToString();
        }
      }
      else
        timeDays = words[index];
    }
    return timeDays;
  }

  private string ParseCardFormatBits(string[] words, int index)
  {
    string cardFormatBits = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      cardFormatBits += words[index];
      if (num > 0)
      {
        cardFormatBits += "  (Format IDs: ";
        if ((num & 1) == 1)
          cardFormatBits += "0, ";
        if ((num & 2) == 2)
          cardFormatBits += "1, ";
        if ((num & 4) == 4)
          cardFormatBits += "2, ";
        if ((num & 8) == 8)
          cardFormatBits += "3, ";
        if ((num & 16 /*0x10*/) == 16 /*0x10*/)
          cardFormatBits += "4, ";
        if ((num & 32 /*0x20*/) == 32 /*0x20*/)
          cardFormatBits += "5, ";
        if ((num & 64 /*0x40*/) == 64 /*0x40*/)
          cardFormatBits += "6, ";
        if ((num & 128 /*0x80*/) == 128 /*0x80*/)
          cardFormatBits += "7, ";
        if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
          cardFormatBits += "8, ";
        if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
          cardFormatBits += "9, ";
        if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
          cardFormatBits += "10, ";
        if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
          cardFormatBits += "11, ";
        if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
          cardFormatBits += "12, ";
        if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
          cardFormatBits += "13, ";
        if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
          cardFormatBits += "14, ";
        if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
          cardFormatBits += "15, ";
        string str = cardFormatBits;
        if (str[str.Length - 2] == ',')
          cardFormatBits = new StringBuilder(cardFormatBits)
          {
            [cardFormatBits.Length - 2] = ')'
          }.ToString();
      }
    }
    return cardFormatBits;
  }

  private string ParseApbMode(string[] words, int index)
  {
    string apbMode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          apbMode = "0 - ACR_APB_NONE";
          break;
        case 1:
          apbMode = "1 - ACR_APB_ANY";
          break;
        case 2:
          apbMode = "2 - ACR_APB_CHK";
          break;
        case 3:
          apbMode = "3 - ACR_APB_DLY_L";
          break;
        case 4:
          apbMode = "4 - ACR_APB_DLY_R";
          break;
        case 5:
          apbMode = "5 - ACR_APB_DLY_A";
          break;
        case 6:
          apbMode = "6 - ACR_APB_DLY_M_L (apb_delay is in minutes)";
          break;
        case 7:
          apbMode = "7 - ACR_APB_DLY_M_R (apb_delay is in minutes)";
          break;
        case 8:
          apbMode = "8 - ACR_APB_DLY_M_A (apb_delay is in minutes)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          apbMode = words[index] + " (Unknown mode)";
          break;
      }
    }
    return apbMode;
  }

  private string ParseStrikeMode(string[] words, int index)
  {
    string strikeMode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      switch (num & 15)
      {
        case 0:
          strikeMode = "0 - ACR_S_NONE ";
          break;
        case 1:
          strikeMode = "1 - ACR_S_OPEN ";
          break;
        case 2:
          strikeMode = "2 - ACS_S_CLOSE ";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          strikeMode = words[index] + " (Unknown mode) ";
          break;
      }
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
        strikeMode += "ACR_S_TAILGATE ";
    }
    return strikeMode;
  }

  private string ParseExtActlFlags(string[] words, int index)
  {
    string extActlFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      extActlFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        extActlFlags += "ACR_FE_NOEXTEND ";
      if ((num & 2) == 2)
        extActlFlags += "ACR_FE_NOPINCARD ";
      if ((num & 4) == 4)
        extActlFlags += "ACR_FE_ASSETREQD ";
      if ((num & 8) == 8)
        extActlFlags += "ACR_FE_DFO_FLTR ";
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
        extActlFlags += "ACR_FE_NO_ARQ ";
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
        extActlFlags += "ACR_FE_SHNTRLY ";
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
        extActlFlags += "ACR_FE_FLOOR_PIN ";
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
        extActlFlags += "ACR_FE_LINK_MODE ";
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
        extActlFlags += "ACR_FE_DCARD ";
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
        extActlFlags += "ACR_FE_OVERRIDE ";
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
        extActlFlags += "ACR_FE_CRD_OVR_EN ";
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
        extActlFlags += "ACR_FE_ELV_DISABLE ";
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
        extActlFlags += "ACR_FE_LINK_MODE_ALT ";
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
        extActlFlags += "ACR_FE_REX_HOLD ";
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
      {
        this.textColor = this.errorFoundTextColor;
        extActlFlags += "(UNKNOWN 0x4000) ";
      }
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        this.textColor = this.errorFoundTextColor;
        extActlFlags += "(UNKNOWN 0x8000) ";
      }
    }
    return extActlFlags;
  }

  private string ParseDtFmt(string[] words, int index)
  {
    string dtFmt = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      dtFmt = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        dtFmt += "IDRDR_D1D0 ";
      if ((num & 2) == 2)
        dtFmt += "IDRDR_ZTRIM ";
      if ((num & 4) == 4)
        dtFmt += "IDRDR_T2FMT ";
      if ((num & 8) == 8)
        dtFmt += "IDRDR_BIDIR ";
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
      {
        this.textColor = this.errorFoundTextColor;
        dtFmt += "(UNKNOWN 0x0100)  ";
      }
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
      {
        this.textColor = this.errorFoundTextColor;
        dtFmt += "(UNKNOWN 0x0020)  ";
      }
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
      {
        this.textColor = this.errorFoundTextColor;
        dtFmt += "(UNKNOWN 0x0040)  ";
      }
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
      {
        this.textColor = this.errorFoundTextColor;
        dtFmt += "(UNKNOWN 0x0080)  ";
      }
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
      {
        this.textColor = this.errorFoundTextColor;
        dtFmt += "(UNKNOWN 0x0100)  ";
      }
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
      {
        this.textColor = this.errorFoundTextColor;
        dtFmt += "(UNKNOWN 0x0200)  ";
      }
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
      {
        this.textColor = this.errorFoundTextColor;
        dtFmt += "(UNKNOWN 0x0400)  ";
      }
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
      {
        this.textColor = this.errorFoundTextColor;
        dtFmt += "(UNKNOWN 0x0800)  ";
      }
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
      {
        this.textColor = this.errorFoundTextColor;
        dtFmt += "(UNKNOWN 0x1000)  ";
      }
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
      {
        this.textColor = this.errorFoundTextColor;
        dtFmt += "(UNKNOWN 0x2000)  ";
      }
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
      {
        this.textColor = this.errorFoundTextColor;
        dtFmt += "(UNKNOWN 0x4000)  ";
      }
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        this.textColor = this.errorFoundTextColor;
        dtFmt += "(UNKNOWN 0x8000)  ";
      }
    }
    return dtFmt;
  }

  private string ParseOsdpFileTransferFtAction(string[] words, int index)
  {
    string transferFtAction = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      transferFtAction = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        transferFtAction += "(Ok To Interleave)  ";
      if ((num & 2) == 2)
        transferFtAction += "(Leave Secure Channel)  ";
      if ((num & 4) == 4)
        transferFtAction += "(Poll Response Avail)  ";
      if ((num & 8) == 8)
      {
        this.textColor = this.errorFoundTextColor;
        transferFtAction += "(UNKNOWN 0x0008)  ";
      }
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
      {
        this.textColor = this.errorFoundTextColor;
        transferFtAction += "(UNKNOWN 0x0010)  ";
      }
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
      {
        this.textColor = this.errorFoundTextColor;
        transferFtAction += "(UNKNOWN 0x0020)  ";
      }
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
      {
        this.textColor = this.errorFoundTextColor;
        transferFtAction += "(UNKNOWN 0x0040)  ";
      }
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
      {
        this.textColor = this.errorFoundTextColor;
        transferFtAction += "(UNKNOWN 0x0080)  ";
      }
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
      {
        this.textColor = this.errorFoundTextColor;
        transferFtAction += "(UNKNOWN 0x0100)  ";
      }
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
      {
        this.textColor = this.errorFoundTextColor;
        transferFtAction += "(UNKNOWN 0x0200)  ";
      }
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
      {
        this.textColor = this.errorFoundTextColor;
        transferFtAction += "(UNKNOWN 0x0400)  ";
      }
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
      {
        this.textColor = this.errorFoundTextColor;
        transferFtAction += "(UNKNOWN 0x0800)  ";
      }
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
      {
        this.textColor = this.errorFoundTextColor;
        transferFtAction += "(UNKNOWN 0x1000)  ";
      }
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
      {
        this.textColor = this.errorFoundTextColor;
        transferFtAction += "(UNKNOWN 0x2000)  ";
      }
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
      {
        this.textColor = this.errorFoundTextColor;
        transferFtAction += "(UNKNOWN 0x4000)  ";
      }
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        this.textColor = this.errorFoundTextColor;
        transferFtAction += "(UNKNOWN 0x8000)  ";
      }
    }
    return transferFtAction;
  }

  private string ParseOsdpFileTransferFtStatusDetail(int value)
  {
    string transferFtStatusDetail;
    switch ((short) value)
    {
      case -3:
        transferFtStatusDetail = "-3 = Unspecified diagnostic data";
        break;
      case -2:
        transferFtStatusDetail = "-2 = Unrecognized file contents";
        break;
      case -1:
        transferFtStatusDetail = "-1 = Abort transfer";
        break;
      case 0:
        transferFtStatusDetail = "0 = OK to proceed";
        break;
      case 1:
        transferFtStatusDetail = "1 = File contents processed";
        break;
      case 2:
        transferFtStatusDetail = "2 = Rebooting now, expect full communications reset";
        break;
      case 3:
        transferFtStatusDetail = "3 = PD needs more processing time";
        break;
      default:
        this.textColor = this.errorFoundTextColor;
        transferFtStatusDetail = "";
        break;
    }
    return transferFtStatusDetail;
  }

  private string ParseOsdpFlags(string[] words, int index)
  {
    string osdpFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      osdpFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if (num == 0)
      {
        osdpFlags += "Defaults for Reader Port";
      }
      else
      {
        if ((num & 7) == 1)
          osdpFlags += "BAUD_9600 ";
        if ((num & 7) == 2)
          osdpFlags += "BAUD_19200 ";
        if ((num & 7) == 3)
          osdpFlags += "BAUD_38400 ";
        if ((num & 7) == 4)
          osdpFlags += "BAUD_115200 ";
        if ((num & 7) == 5)
          osdpFlags += "BAUD_57600 ";
        if ((num & 7) == 6)
          osdpFlags += "BAUD_230400 ";
        if ((num & 7) == 7)
        {
          this.textColor = this.errorFoundTextColor;
          osdpFlags += "Unknown OSDP Flag ";
        }
        if ((num & 8) == 8)
          osdpFlags += "NO_DISCOVER ";
        if ((num & 16 /*0x10*/) == 16 /*0x10*/)
          osdpFlags += "DEBUG_TRACE ";
        if ((num & 96 /*0x60*/) == 0)
          osdpFlags += "ADDR_0 ";
        if ((num & 96 /*0x60*/) == 32 /*0x20*/)
          osdpFlags += "ADDR_1 ";
        if ((num & 96 /*0x60*/) == 64 /*0x40*/)
          osdpFlags += "ADDR_2 ";
        if ((num & 96 /*0x60*/) == 96 /*0x60*/)
          osdpFlags += "ADDR_3 ";
        if ((num & 128 /*0x80*/) == 128 /*0x80*/)
          osdpFlags += "OSDP_SC ";
      }
    }
    return osdpFlags;
  }

  private string ParseNProtocol(string[] words, int index)
  {
    string nprotocol = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          nprotocol = "0 - X100, X200, and X300";
          break;
        case 15:
          nprotocol = "15 - VertX V100, V200, and V300";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          nprotocol = words[index] + " (Unknown protocol)";
          break;
      }
    }
    return nprotocol;
  }

  private string ParseDialect(string[] words, int index)
  {
    string dialect = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      if (int.Parse(words[index]) == 0)
      {
        dialect = "0 - Default";
      }
      else
      {
        this.textColor = this.errorFoundTextColor;
        dialect = words[index] + " (Unknown Dialect)";
      }
    }
    return dialect;
  }

  private string ParseCodeMap1(string[] words, int index)
  {
    string str1 = "";
    bool flag = true;
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num1 = int.Parse(words[index]);
      str1 = words[index] + " = TranCodes (";
      for (int index1 = 1; index1 < 32 /*0x20*/; ++index1)
      {
        int num2 = 2 << index1 - 1;
        if ((num2 & num1) == num2)
        {
          string str2;
          if (flag)
          {
            str2 = string.Concat((object) index1);
            flag = false;
          }
          else
            str2 = ", " + (object) index1;
          str1 += str2;
        }
      }
    }
    return str1 + ")";
  }

  private string ParseCodeMap2(string[] words, int index)
  {
    string str1 = "";
    bool flag1 = true;
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      long num1 = long.Parse(words[index]);
      long num2 = long.Parse(words[index + 1]);
      string str2 = words[index] + " = TranCodes1 (";
      for (int y = 1; y < 64 /*0x40*/; ++y)
      {
        long num3 = (long) Math.Pow(2.0, (double) y);
        if ((num3 & num1) == num3)
        {
          string str3;
          if (flag1)
          {
            str3 = string.Concat((object) y);
            flag1 = false;
          }
          else
            str3 = ", " + (object) y;
          str2 += str3;
        }
      }
      str1 = $"{str2}), {words[index + 1]} = TranCodes2 (";
      bool flag2 = true;
      for (int y = 1; y < 64 /*0x40*/; ++y)
      {
        long num4 = (long) Math.Pow(2.0, (double) y);
        if ((num4 & num2) == num4)
        {
          string str4;
          if (flag2)
          {
            str4 = string.Concat((object) y);
            flag2 = false;
          }
          else
            str4 = ", " + (object) y;
          str1 += str4;
        }
      }
    }
    return str1 + ")";
  }

  private string ParseKeypadMode(string[] words, int index)
  {
    string keypadMode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          keypadMode = "0 - IDRDR_K_NONE";
          break;
        case 2:
          keypadMode = "2 - IDRDR_K_HID (4-bit)";
          break;
        case 3:
          keypadMode = "3 - IDRDR_K_INDALA";
          break;
        case 6:
          keypadMode = "6 - IDRDR_K_4BIT_ALIVE_60";
          break;
        case 7:
          keypadMode = "7 - IDRDR_K_8BIT_ALIVE_60";
          break;
        case 8:
          keypadMode = "8 - IDRDR_K_4BIT_ALIVE_10";
          break;
        case 9:
          keypadMode = "9 - IDRDR_K_8BIT_ALIVE_10";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          keypadMode = words[index] + " (Unknown mode)";
          break;
      }
    }
    return keypadMode;
  }

  private string ParseLedDriveMode(string[] words, int index)
  {
    string ledDriveMode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          ledDriveMode = "0 - NONE";
          break;
        case 1:
          ledDriveMode = "1 - IDRDR_L_BICOLOR";
          break;
        case 7:
          ledDriveMode = "7 - IDRDR_L_OSDP";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          ledDriveMode = words[index] + " (Unknown mode)";
          break;
      }
    }
    return ledDriveMode;
  }

  private string ParseLogFunction(string[] words, int index)
  {
    string logFunction = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          logFunction = "0 - MPLG_0 (logs all changes)";
          break;
        case 1:
          logFunction = "1 - MPLG_1 (do not log contact c-o-s if masked)";
          break;
        case 2:
          logFunction = "2 - MPLG_2 (1 plus no fault-to-fault changes)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          logFunction = words[index] + " (Unknown mode)";
          break;
      }
    }
    return logFunction;
  }

  private string ParseIcvtNum(string[] words, int index)
  {
    string icvtNum = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          icvtNum = "0 - Normally closed, No EOL";
          break;
        case 1:
          icvtNum = "1 - Normally open, No EOL";
          break;
        case 2:
          icvtNum = "2 - Standard EOL (1K Normal, 2K Active)";
          break;
        case 3:
          icvtNum = "3 - Standard EOL (2K Normal, 1K Active)";
          break;
        case 128 /*0x80*/:
          icvtNum = "128 - Custom EOL";
          break;
        case 129:
          icvtNum = "129 - Custom EOL";
          break;
        case 130:
          icvtNum = "130 - Custom EOL";
          break;
        case 131:
          icvtNum = "131 - Custom EOL";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          icvtNum = words[index] + " (Unknown value)";
          break;
      }
    }
    return icvtNum;
  }

  private string ParseMpMode(string[] words, int index)
  {
    string mpMode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          mpMode = "0 - Normal";
          break;
        case 1:
          mpMode = "1 - Non-Latching";
          break;
        case 2:
          mpMode = "2 - Latching";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          mpMode = words[index] + " (Unknown mode)";
          break;
      }
    }
    return mpMode;
  }

  private string ParseSioModel(string[] words, int index, bool hexValue)
  {
    string sioModel = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (!hexValue ? int.Parse(words[index]) : int.Parse(words[index], NumberStyles.HexNumber))
      {
        case -1:
          sioModel = "-1 - (Unspecified model)";
          break;
        case 33:
          sioModel = "33 - PW5K/PW6K1R1";
          break;
        case 34:
          sioModel = "34 - PW5K/PW6K1R2";
          break;
        case 35:
          sioModel = "35 - PW5K/PW6K1IN";
          break;
        case 36:
          sioModel = "36 - PW5K/PW6K1OUT";
          break;
        case 38:
          sioModel = "38 - SNET Reader";
          break;
        case 50:
          sioModel = "50 - SALTO";
          break;
        case 51:
          sioModel = "51 - SimonsVoss SIO";
          break;
        case 52:
          sioModel = "52 - VRINX";
          break;
        case 53:
          sioModel = "53 - VIONX-8";
          break;
        case 54:
          sioModel = "54 - GIONX-24";
          break;
        case 55:
          sioModel = "55 - Aperio 8:1";
          break;
        case 61:
          sioModel = "61 - AP3108";
          break;
        case 62:
          sioModel = "62 - AP3208";
          break;
        case 64 /*0x40*/:
          sioModel = "64 - AP3402";
          break;
        case 70:
          sioModel = "70 - FOD24in";
          break;
        case 71:
          sioModel = "71 - M5-2RP";
          break;
        case 72:
          sioModel = "72 - M5-2SRP";
          break;
        case 73:
          sioModel = "73 - M5-20DI";
          break;
        case 74:
          sioModel = "74 - M5-16DO";
          break;
        case 75:
          sioModel = "75 - M5-16DOR";
          break;
        case 76:
          sioModel = "76 - M5-8RP";
          break;
        case 77:
          sioModel = "77 - M5-2K";
          break;
        case 80 /*0x50*/:
          sioModel = "80 - MR50";
          break;
        case 81:
          sioModel = "81 - MR16IN";
          break;
        case 82:
          sioModel = "82 - MR16OUT";
          break;
        case 83:
          sioModel = "83 - MR-DT";
          break;
        case 84:
          sioModel = "84 - MR52";
          break;
        case 85:
          sioModel = "85 - EP1502-SIO";
          break;
        case 86:
          sioModel = "86 - EP4502-SIO";
          break;
        case 87:
          sioModel = "87 - MR51e";
          break;
        case 88:
          sioModel = "88 - EP1501-SIO";
          break;
        case 89:
          sioModel = "89 - EP1501 as MR51e";
          break;
        case 90:
          sioModel = "90 - MR51eH";
          break;
        case 91:
          sioModel = "91 - MI-XL16-SIO";
          break;
        case 92:
          sioModel = "92 - Virtual SIO";
          break;
        case 93:
          sioModel = "93 - IDenticard RIO";
          break;
        case 94:
          sioModel = "94 - IDenticard 9000";
          break;
        case 95:
          sioModel = "95 - MI-RS4-SIO";
          break;
        case 96 /*0x60*/:
          sioModel = "96 - MAXxess BLP-205";
          break;
        case 97:
          sioModel = "97 - Gateway HID (obsolete)";
          break;
        case 98:
          sioModel = "98 - Gateway RSI";
          break;
        case 99:
          sioModel = "99 - Gateway IDX (obsolete)";
          break;
        case 100:
          sioModel = "100 - Gateway Bioscrypt";
          break;
        case 101:
          sioModel = "101 - PIM Module";
          break;
        case 102:
          sioModel = "102 - Engage Gateway";
          break;
        case 104:
          sioModel = "104 - PSIA - Inovonics ACG";
          break;
        case 105:
          sioModel = "105 - RK-ARM SIO";
          break;
        case 106:
          sioModel = "106 - RK-ARM as SIO";
          break;
        case 107:
          sioModel = "107 - AP-SIO";
          break;
        case 108:
          sioModel = "108 - PSIA - LifeSafety Power";
          break;
        case 120:
          sioModel = "120 - NXT Reader";
          break;
        case 121:
          sioModel = "121 - LIOX Module";
          break;
        case 122:
          sioModel = "122 - Weigand Reader";
          break;
        case 123:
          sioModel = "123 - Keri Reader";
          break;
        case 124:
          sioModel = "124 - ABA (Not Implemented)";
          break;
        case 125:
          sioModel = "125 - Weigand Combo";
          break;
        case 126:
          sioModel = "126 - GIOX";
          break;
        case (int) sbyte.MaxValue:
          sioModel = "127 - NXT Exit Reader (addr 8)";
          break;
        case 130:
          sioModel = "130 - RRE1";
          break;
        case 131:
          sioModel = "131 - RRE2";
          break;
        case 132:
          sioModel = "132 - RRE4";
          break;
        case 133:
          sioModel = "133 - RRE1SS";
          break;
        case 134:
          sioModel = "134 - RRM";
          break;
        case 135:
          sioModel = "135 - RIM";
          break;
        case 136:
          sioModel = "136 - IKE";
          break;
        case 140:
          sioModel = "140 - MS-ACS";
          break;
        case 141:
          sioModel = "141 - MS-I8S";
          break;
        case 142:
          sioModel = "142 - MS-R8S";
          break;
        case 143:
          sioModel = "143 - MR62e";
          break;
        case 144 /*0x90*/:
          sioModel = "144 - AC-1 / AC-1A";
          break;
        case 145:
          sioModel = "145 - AC-1Plus";
          break;
        case 146:
          sioModel = "146 - ACX-SIO";
          break;
        case 170:
          sioModel = "170 - KONE";
          break;
        case 180:
          sioModel = "180 - CIA (Dormakaba)";
          break;
        case 190:
          sioModel = "190 - V100";
          break;
        case 191:
          sioModel = "191 - V200";
          break;
        case 192 /*0xC0*/:
          sioModel = "192 - V300";
          break;
        case 196:
          sioModel = "196 - X1100-SIO";
          break;
        case 221:
          sioModel = "221 - PRO22/PRO32R2";
          break;
        case 222:
          sioModel = "222 - PRO22/PRO32IN";
          break;
        case 223:
          sioModel = "223 - PRO22/PRO32OUT";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          sioModel = words[index] + " (Unknown model)";
          break;
      }
    }
    return sioModel;
  }

  private string ParseSioFlags(string[] words, int index)
  {
    string sioFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      sioFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 8) == 8)
        sioFlags += "(SIO Trace) ";
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
        sioFlags += "(Special Diag Mode) ";
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
        sioFlags += "(Reverse the processing order of this Sio's inputs) ";
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
        sioFlags += "(Display Cipher Text on Encrypted SIO Comm) ";
    }
    return sioFlags;
  }

  private string ParseSioConnectTest(string[] words, int index)
  {
    string sioConnectTest = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          sioConnectTest = "0 - (Test both SN AND OEM code) ";
          break;
        case 1:
          sioConnectTest = "1 - (test either SN OR OEM code)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          sioConnectTest = words[index] + " (Unknown mode)";
          break;
      }
    }
    return sioConnectTest;
  }

  private string ParsePointType(string[] words, int index)
  {
    string pointType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          pointType = "1 - (MP)";
          break;
        case 2:
          pointType = "2 - (CP)";
          break;
        case 3:
          pointType = "3 - (ACR)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          pointType = words[index] + " (Unknown type)";
          break;
      }
    }
    return pointType;
  }

  private string ParseIpsPtAddCommand(string[] words, int index)
  {
    string ipsPtAddCommand = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          ipsPtAddCommand = "0 - (Delete List)";
          break;
        case 1:
          ipsPtAddCommand = "1 - (New List)";
          break;
        case 2:
          ipsPtAddCommand = "2 - (Append to Point List)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          ipsPtAddCommand = words[index] + " (Unknown command)";
          break;
      }
    }
    return ipsPtAddCommand;
  }

  private string ParseIpsPointType(string[] words, int index)
  {
    string ipsPointType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          ipsPointType = "1 - (MP)";
          break;
        case 2:
          ipsPointType = "2 - (Forced Open)";
          break;
        case 3:
          ipsPointType = "3 - (Held Open)";
          break;
        case 4:
          ipsPointType = "4 - (ACR Door)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          ipsPointType = words[index] + " (Unknown Type)";
          break;
      }
    }
    return ipsPointType;
  }

  private string ParseLEDColor(string[] words, int index)
  {
    string ledColor = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          ledColor = "0 - (off)";
          break;
        case 1:
          ledColor = "1 - (red)";
          break;
        case 2:
          ledColor = "2 - (green)";
          break;
        case 3:
          ledColor = "3 - (amber)";
          break;
        case 4:
          ledColor = "4 - (blue)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          ledColor = words[index] + " (Unknown color)";
          break;
      }
    }
    return ledColor;
  }

  private string ParseOsdpLEDTempControlCode(string[] words, int index)
  {
    string ledTempControlCode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          ledTempControlCode = "0 - (NOP)";
          break;
        case 1:
          ledTempControlCode = "1 - (CANCEL Temp)";
          break;
        case 2:
          ledTempControlCode = "2 - (Set Temp)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          ledTempControlCode = words[index] + " (Unknown control code)";
          break;
      }
    }
    return ledTempControlCode;
  }

  private string ParseOsdpLEDPermControlCode(string[] words, int index)
  {
    string ledPermControlCode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          ledPermControlCode = "0 - (NOP)";
          break;
        case 1:
          ledPermControlCode = "1 - (Set Perm)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          ledPermControlCode = words[index] + " (Unknown control code)";
          break;
      }
    }
    return ledPermControlCode;
  }

  private string ParseOsdpVerrCode(string[] words, int index)
  {
    string osdpVerrCode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          osdpVerrCode = "1 - (SC_ERROR)";
          break;
        case 2:
          osdpVerrCode = "2 - (SC_TIMEOUT)";
          break;
        case 3:
          osdpVerrCode = "2 - (SC_NOSUCHFILE)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          osdpVerrCode = words[index] + " (Unknown code)";
          break;
      }
    }
    return osdpVerrCode;
  }

  private string ParseOsdpVstatCode(string[] words, int index)
  {
    string osdpVstatCode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          osdpVstatCode = "1 - (STAT_BUSY)";
          break;
        case 2:
          osdpVstatCode = "2 - (STAT_CONT)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          osdpVstatCode = words[index] + " (Unknown code)";
          break;
      }
    }
    return osdpVstatCode;
  }

  private string ParseRGBLEDColor(string[] words, int index)
  {
    string rgbledColor = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
      rgbledColor = string.Format("{0} (0x{0:X4}) ", (object) int.Parse(words[index]));
    return rgbledColor;
  }

  private string ParseOfflineLEDActionColor(string[] words, int index)
  {
    string offlineLedActionColor = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          offlineLedActionColor = "0 - (deny)";
          break;
        case 1:
          offlineLedActionColor = "1 - (grant)";
          break;
        case 2:
          offlineLedActionColor = "2 - (offline mode)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          offlineLedActionColor = words[index] + " (Unknown action)";
          break;
      }
    }
    return offlineLedActionColor;
  }

  private string ParseRledId(string[] words, int index)
  {
    string rledId = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          rledId = "1 - (disabled)";
          break;
        case 2:
          rledId = "2 - (unlocked)";
          break;
        case 3:
          rledId = "3 - (locked)";
          break;
        case 4:
          rledId = "4 - (facility)";
          break;
        case 5:
          rledId = "5 - (card only)";
          break;
        case 6:
          rledId = "6 - (pin only)";
          break;
        case 7:
          rledId = "7 - (card and pin)";
          break;
        case 8:
          rledId = "8 - (pin or card)";
          break;
        case 11:
          rledId = "11 - (rled_deny)";
          break;
        case 12:
          rledId = "12 - (rled_admit)";
          break;
        case 13:
          rledId = "13 - (rled_ucmd)";
          break;
        case 14:
          rledId = "14 - (rled_2ndcard)";
          break;
        case 15:
          rledId = "15 - (rled_2ndpin)";
          break;
        case 16 /*0x10*/:
          rledId = "16 - (rled_wait)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          rledId = words[index] + " (Unknown rled_id)";
          break;
      }
    }
    return rledId;
  }

  private string ParseCardSimCommand(string[] words, int index)
  {
    string cardSimCommand = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      if (int.Parse(words[index]) == 1)
      {
        cardSimCommand = "1 - (Process this as card data input)";
      }
      else
      {
        this.textColor = this.errorFoundTextColor;
        cardSimCommand = words[index] + " (Unknown nCommand)";
      }
    }
    return cardSimCommand;
  }

  private string ParseHcpDriver(string[] words, int index)
  {
    string hcpDriver = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          hcpDriver = "0 - (Primary) ";
          break;
        case 1:
          hcpDriver = "1 - (Alternate)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          hcpDriver = words[index] + " (Unknown HcpDriver)";
          break;
      }
    }
    return hcpDriver;
  }

  private string ParseIcvtResCode(string[] words, int index)
  {
    string icvtResCode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      switch (num)
      {
        case -4:
          icvtResCode = "-4 - (Ground Line B)";
          break;
        case -3:
          icvtResCode = "-3 - (Ground Line A)";
          break;
        case -2:
          icvtResCode = "-2 - (Shorted Line)";
          break;
        case -1:
          icvtResCode = "-1 - (Infinity) ";
          break;
        default:
          if (num < 0)
            this.textColor = this.errorFoundTextColor;
          icvtResCode = words[index];
          break;
      }
    }
    return icvtResCode;
  }

  private string ParseIcvtStatusCode(string[] words, int index)
  {
    string icvtStatusCode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          icvtStatusCode = "0 - (Inactive - Normal state of circuit) ";
          break;
        case 1:
          icvtStatusCode = "1 - (Active - Alarm state)";
          break;
        case 2:
          icvtStatusCode = "2 - (Supervisory Fault - Ground Fault)";
          break;
        case 3:
          icvtStatusCode = "3 - (Supervisory Fault - Shorted Circuit)";
          break;
        case 4:
          icvtStatusCode = "4 - (Supervisory Fault - Open Circuit)";
          break;
        case 5:
          icvtStatusCode = "5 - (Supervisory Fault - Foreign Voltage)";
          break;
        case 6:
          icvtStatusCode = "6 - (Supervisory Fault - Non-Settling Error)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          icvtStatusCode = words[index] + " (Unknown status code)";
          break;
      }
    }
    return icvtStatusCode;
  }

  private string ParseRTxtLineIndex(string[] words, int index)
  {
    string rtxtLineIndex = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          rtxtLineIndex = "1 - (rdiCLOCK)";
          break;
        case 2:
          rtxtLineIndex = "2 - (rdiLOCKED)";
          break;
        case 3:
          rtxtLineIndex = "3 - (rdiUNLOCKED)";
          break;
        case 4:
          rtxtLineIndex = "4 - (rdiREADY)";
          break;
        case 5:
          rtxtLineIndex = "5 - (rdiPINREQ)";
          break;
        case 6:
          rtxtLineIndex = "6 - (rdiDENY)";
          break;
        case 7:
          rtxtLineIndex = "7 - (rdiADMIT)";
          break;
        case 8:
          rtxtLineIndex = "8 - (rdiUCMD)";
          break;
        case 9:
          rtxtLineIndex = "9 - (rdi2NDCRD)";
          break;
        case 10:
          rtxtLineIndex = "10 - (rdi2NDPIN)";
          break;
        case 11:
          rtxtLineIndex = "11 - (rdiPROMPT)";
          break;
        case 12:
          rtxtLineIndex = "12 - (rdiCMD_OK)";
          break;
        case 13:
          rtxtLineIndex = "13 - (rdiCMD_EA)";
          break;
        case 14:
          rtxtLineIndex = "14 - (rdiCMD_ED)";
          break;
        case 15:
          rtxtLineIndex = "15 - (rdiTMD_OPEN1)";
          break;
        case 16 /*0x10*/:
          rtxtLineIndex = "16 - (rdiTMD_OPEN2)";
          break;
        case 17:
          rtxtLineIndex = "17 - (rdiWAIT)";
          break;
        case 18:
          rtxtLineIndex = "18 - (rdiBIORQ)";
          break;
        case 19:
          rtxtLineIndex = "19 - (rdiCARDRQ)";
          break;
        case 30:
          rtxtLineIndex = "30 - (rdiBrgAllSecure)";
          break;
        case 31 /*0x1F*/:
          rtxtLineIndex = "31 - (rdiBrgZonesFaulted)";
          break;
        case 32 /*0x20*/:
          rtxtLineIndex = "32 - (rdiBrgArming)";
          break;
        case 33:
          rtxtLineIndex = "33 - (rdiBrgToDisarm)";
          break;
        case 34:
          rtxtLineIndex = "34 - (rdiBrgArm_1)";
          break;
        case 35:
          rtxtLineIndex = "35 - (rdiBrgArm_2)";
          break;
        case 36:
          rtxtLineIndex = "36 - (rdiBrgToView)";
          break;
        case 37:
          rtxtLineIndex = "37 - (rdiBrgArmed)";
          break;
        case 38:
          rtxtLineIndex = "38 - (rdiBrgDisarmed)";
          break;
        case 39:
          rtxtLineIndex = "39 - (rdiPtIdErr)";
          break;
        case 40:
          rtxtLineIndex = "40 - (rdiPtMp)";
          break;
        case 41:
          rtxtLineIndex = "41 - (rdiPtCp)";
          break;
        case 42:
          rtxtLineIndex = "42 - (rdiPtACR)";
          break;
        case 43:
          rtxtLineIndex = "43 - (rdiNotAssigned_)";
          break;
        case 44:
          rtxtLineIndex = "44 - (rdiNotAssigned_)";
          break;
        case 45:
          rtxtLineIndex = "45 - (rdiNotAssigned_)";
          break;
        case 46:
          rtxtLineIndex = "46 - (rdiNotAssigned_)";
          break;
        case 47:
          rtxtLineIndex = "47 - (rdiNotAssigned_)";
          break;
        case 48 /*0x30*/:
          rtxtLineIndex = "48 - (rdiNotAssigned_)";
          break;
        case 49:
          rtxtLineIndex = "49 - (rdiNotAssigned_)";
          break;
        case 50:
          rtxtLineIndex = "50 - (rdiIpsDisarmed)";
          break;
        case 51:
          rtxtLineIndex = "51 - (rdiIpsDisarmedBypass)";
          break;
        case 52:
          rtxtLineIndex = "52 - (rdiIpsFault)";
          break;
        case 53:
          rtxtLineIndex = "53 - (rdiIpsArmedAway)";
          break;
        case 54:
          rtxtLineIndex = "54 - (rdiIpsArmedStay)";
          break;
        case 55:
          rtxtLineIndex = "55 - (rdiIpsArmedInstant)";
          break;
        case 56:
          rtxtLineIndex = "56 - (rdiIpsAlarmNew)";
          break;
        case 57:
          rtxtLineIndex = "57 - (rdiIpsAlarmAckd)";
          break;
        case 58:
          rtxtLineIndex = "58 - (rdiIpsExitDelay)";
          break;
        case 59:
          rtxtLineIndex = "59 - (rdiIpsEntryDelay)";
          break;
        case 60:
          rtxtLineIndex = "60 - (rdiIpsReadyToArm)";
          break;
        case 61:
          rtxtLineIndex = "61 - (rdiIpsNnFaulted)";
          break;
        case 62:
          rtxtLineIndex = "62 - (rdiIpsNnBypassed)";
          break;
        case 63 /*0x3F*/:
          rtxtLineIndex = "63 - (rdiIpsnNDisabled)";
          break;
        case 64 /*0x40*/:
          rtxtLineIndex = "64 - (rdiIpsShowingHistory)";
          break;
        case 65:
          rtxtLineIndex = "65 - (rdiIpsNoHistory)";
          break;
        case 66:
          rtxtLineIndex = "66 - (rdiIpsRepeatHistory)";
          break;
        case 67:
          rtxtLineIndex = "67 - (rdiIpsPressEnter)";
          break;
        case 68:
          rtxtLineIndex = "68 - (rdiKeySixBypasses)";
          break;
        case 69:
          rtxtLineIndex = "69 - (rdiListActivePoints)";
          break;
        case 70:
          rtxtLineIndex = "70 - (rdiListAllPoints)";
          break;
        case 71:
          rtxtLineIndex = "71 - (rdiEndPointsList)";
          break;
        case 72:
          rtxtLineIndex = "72 - (rdiEnterManagePoints1)";
          break;
        case 73:
          rtxtLineIndex = "73 - (rdiEnterManagePoints2)";
          break;
        case 74:
          rtxtLineIndex = "74 - (rdiPointActive)";
          break;
        case 75:
          rtxtLineIndex = "75 - (rdiPointBypassed)";
          break;
        case 76:
          rtxtLineIndex = "76 - (rdiPointFaulted)";
          break;
        case 77:
          rtxtLineIndex = "77 - (rdiEnterConfirm)";
          break;
        case 78:
          rtxtLineIndex = "78 - (rdiChimeToggleOff)";
          break;
        case 79:
          rtxtLineIndex = "79 - (rdiChimeToggleOn)";
          break;
        case 80 /*0x50*/:
          rtxtLineIndex = "80 - (rdiChimeDisabled)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          rtxtLineIndex = words[index] + " (Unknown LineIndex)";
          break;
      }
    }
    return rtxtLineIndex;
  }

  private string ParseSioAesCommand(string[] words, int index)
  {
    string sioAesCommand = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          sioAesCommand = "0 - AESCMND_SIO_OFF";
          break;
        case 1:
          sioAesCommand = "1 - AESCMND_SIO_SET_MK";
          break;
        case 2:
          sioAesCommand = "2 - AESCMND_SIO_ENA_MK";
          break;
        case 3:
          sioAesCommand = "3 - AESCMND_SIO_ENA_PKI";
          break;
        case 4:
          sioAesCommand = "4 - AESCMND_SIO_ENA_ANY";
          break;
        case 5:
          sioAesCommand = "5 - AESCMND_SIO_ENA_ANYORNONE";
          break;
        case 6:
          sioAesCommand = "6 - AESCMND_SIO_ENA_TST";
          break;
        case 7:
          sioAesCommand = "7 - AESCMND_SIO_TST_KEYS";
          break;
        case 8:
          sioAesCommand = "8 - AESCMND_SIO_NEW_SK";
          break;
        case 9:
          sioAesCommand = "9 - AESCMND_SIO_MK2SIO";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          sioAesCommand = words[index] + " (Unknown mode)";
          break;
      }
    }
    return sioAesCommand;
  }

  private string ParseOutputDriveMode(string[] words, int index)
  {
    string outputDriveMode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          outputDriveMode = "0 - (ONLINE: Normal, OFFLINE: No Change) ";
          break;
        case 1:
          outputDriveMode = "1 - (ONLINE: Inverted, OFFLINE: No Change)";
          break;
        case 16 /*0x10*/:
          outputDriveMode = "16 - (ONLINE: Normal, OFFLINE: Inactive)";
          break;
        case 17:
          outputDriveMode = "17 - (ONLINE: Inverted, OFFLINE: Inactive)";
          break;
        case 32 /*0x20*/:
          outputDriveMode = "32 - (ONLINE: Normal, OFFLINE: Active)";
          break;
        case 33:
          outputDriveMode = "33 - (ONLINE: Inverted, OFFLINE: Active)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          outputDriveMode = words[index] + " (Unknown mode)";
          break;
      }
    }
    return outputDriveMode;
  }

  private string ParseSioCommand(string[] words, int index)
  {
    string sioCommand = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 101:
          sioCommand = "101 - Id Request (Who-Are-You)";
          break;
        case 102:
          sioCommand = "102 - Reply turnaround delay spec";
          break;
        case 103:
          sioCommand = "103 - Input conversion table transfer";
          break;
        case 104:
          sioCommand = "104 - Input point configuration";
          break;
        case 105:
          sioCommand = "105 - output configuration";
          break;
        case 106:
          sioCommand = "106 - reader configuration";
          break;
        case 107:
          sioCommand = "107 - card format configuration";
          break;
        case 108:
          sioCommand = "108 - off-line reader configuration";
          break;
        case 109:
          sioCommand = "109 - off-line LED configuration";
          break;
        case 110:
          sioCommand = "110 - hex download";
          break;
        case 111:
          sioCommand = "111 - local status report request";
          break;
        case 112 /*0x70*/:
          sioCommand = "112 - input point status report request";
          break;
        case 113:
          sioCommand = "113 - reader tamper status report request";
          break;
        case 114:
          sioCommand = "114 - direct command";
          break;
        case 115:
          sioCommand = "115 - output control command";
          break;
        case 116:
          sioCommand = "116 - reader LED control command";
          break;
        case 117:
          sioCommand = "117 - reader BUZZER control command";
          break;
        case 118:
          sioCommand = "118 - terminal text output command";
          break;
        case 119:
          sioCommand = "119 - output control";
          break;
        case 120:
          sioCommand = "120 - extended ID report";
          break;
        case 121:
          sioCommand = "121 - schedule doverride";
          break;
        case 122:
          sioCommand = "122 - Load AES key into SIO";
          break;
        case 123:
          sioCommand = "123 - output point status report request";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          sioCommand = words[index] + " (Unknown mode)";
          break;
      }
    }
    return sioCommand;
  }

  private string ParseTranIndex(string[] words, int index)
  {
    string tranIndex = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case -2:
          tranIndex = "-2 (enable reporting from last_reported)";
          break;
        case -1:
          tranIndex = "-1 (disable reporting)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          tranIndex = words[index];
          break;
      }
    }
    return tranIndex;
  }

  private string ParseDualPortControlCommand(string[] words, int index)
  {
    string portControlCommand = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 4:
          portControlCommand = "4 (save the controller configuration - no cardholders)";
          break;
        case 5:
          portControlCommand = "5 (save the controllers database)";
          break;
        case (int) sbyte.MaxValue:
          portControlCommand = "127 (clear card database)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          portControlCommand = words[index] + " (Unknown command)";
          break;
      }
    }
    return portControlCommand;
  }

  private string ParseIpsDfltState(string[] words, int index)
  {
    string ipsDfltState = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          ipsDfltState = "1 - Disarmed";
          break;
        case 2:
          ipsDfltState = "2 - Fault";
          break;
        case 3:
          ipsDfltState = "3 - Armed-Away";
          break;
        case 4:
          ipsDfltState = "4 - Armed-Stay";
          break;
        case 5:
          ipsDfltState = "5 - Armed-Instant";
          break;
        case 6:
          ipsDfltState = "6 - Entry Delay in Progress";
          break;
        case 7:
          ipsDfltState = "7 - Exit Delay in Progress";
          break;
        case 8:
          ipsDfltState = "8 - Alarm-New";
          break;
        case 9:
          ipsDfltState = "9 - Cancelled Alarm";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          ipsDfltState = words[index] + " (Unknown IpsDfltState)";
          break;
      }
    }
    return ipsDfltState;
  }

  private string ParseKeySpec(string[] words, int index)
  {
    string keySpec = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          keySpec = "1 (F1)";
          break;
        case 2:
          keySpec = "2 (F2)";
          break;
        case 3:
          keySpec = "3 (F3)";
          break;
        case 4:
          keySpec = "4 (F4)";
          break;
        case 5:
          keySpec = "5 (F1&F2)";
          break;
        case 6:
          keySpec = "6 (F2&F3)";
          break;
        case 7:
          keySpec = "7 (F3&F4)";
          break;
        case 8:
          keySpec = "8 (F1&F4)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          keySpec = words[index] + " (Unknown keyspec)";
          break;
      }
    }
    return keySpec;
  }

  private string ParseCardFormatterFunction(string[] words, int index)
  {
    string formatterFunction = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          formatterFunction = "1 (Wiegand)";
          break;
        case 2:
          formatterFunction = "2 (Mag)";
          break;
        case 3:
          formatterFunction = "3 (MTA)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          formatterFunction = words[index] + " (Unknown Card Formatter Function)";
          break;
      }
    }
    return formatterFunction;
  }

  private string ParseEscortCode(string[] words, int index)
  {
    string escortCode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          escortCode = "0 (Not an Escort)";
          break;
        case 1:
          escortCode = "1 (Is an Escort)";
          break;
        case 2:
          escortCode = "2 (Escort is Required)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          escortCode = words[index] + " (Unknown Escort Code)";
          break;
      }
    }
    return escortCode;
  }

  private string ParseBioType(string[] words, int index)
  {
    string bioType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 101:
          bioType = "101 (RSI)";
          break;
        case 102:
          bioType = "102 (Identix)";
          break;
        case 103:
          bioType = "103 (Bioscrypt)";
          break;
        case 104:
          bioType = "104 (Iridian)";
          break;
        case 105:
          bioType = "105 (OSDP-1)";
          break;
        case 106:
          bioType = "106 (OSDP-2)";
          break;
        case 107:
          bioType = "107 (OSDP-3)";
          break;
        case 108:
          bioType = "108 (OSDP-4)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          bioType = words[index] + " (Unknown Bio Type)";
          break;
      }
    }
    return bioType;
  }

  private string ParseBioFlags(string[] words, int index, int bioType)
  {
    string bioFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num1 = int.Parse(words[index]);
      switch (bioType)
      {
        case 105:
        case 106:
        case 107:
        case 108:
          int num2 = num1 & (int) byte.MaxValue;
          int num3 = (num1 & 65280) >> 8;
          string str1;
          switch (num2)
          {
            case 0:
              str1 = "0=Unspecified";
              break;
            case 1:
              str1 = "1=Right Thumb";
              break;
            case 2:
              str1 = "2=Right Index";
              break;
            case 3:
              str1 = "3=Right Middle";
              break;
            case 4:
              str1 = "4=Right Ring";
              break;
            case 5:
              str1 = "5=Right Little";
              break;
            case 6:
              str1 = "6=Left Thumb";
              break;
            case 7:
              str1 = "7=Left Index";
              break;
            case 8:
              str1 = "8=Left Middle";
              break;
            case 9:
              str1 = "9=Left Ring";
              break;
            case 10:
              str1 = "10=Left Little";
              break;
            case 11:
              str1 = "11=Right Iris";
              break;
            case 12:
              str1 = "12=Right Retina";
              break;
            case 13:
              str1 = "13=Left Iris";
              break;
            case 14:
              str1 = "14=Left Retina";
              break;
            case 15:
              str1 = "15=Full Face";
              break;
            case 16 /*0x10*/:
              str1 = "16=Right Hand";
              break;
            case 17:
              str1 = "17=Left Hand";
              break;
            default:
              this.textColor = this.errorFoundTextColor;
              str1 = "Unknown=" + (object) num2;
              break;
          }
          string str2;
          switch (num3)
          {
            case 0:
              str2 = "0=Default";
              break;
            case 1:
              str2 = "1=PGM 48";
              break;
            case 2:
              str2 = "2=ANSI/INCITS 378";
              break;
            default:
              this.textColor = this.errorFoundTextColor;
              str2 = "Unknown=" + (object) num3;
              break;
          }
          bioFlags = $"{(object) num1} (BodyPart {str1}, Format {str2})";
          break;
        default:
          bioFlags = string.Concat((object) num1);
          break;
      }
    }
    return bioFlags;
  }

  private string ParseIpsSetCommand(string[] words, int index)
  {
    string ipsSetCommand = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 1:
          ipsSetCommand = "1 (Disarm)";
          break;
        case 2:
          ipsSetCommand = "2 (Arm)";
          break;
        case 3:
          ipsSetCommand = "3 (Point Mode Control)";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          ipsSetCommand = words[index] + " (Unknown IpsSetCommand)";
          break;
      }
    }
    return ipsSetCommand;
  }

  private string ParseIpsSetCommandArgs(int argNum, int nCommand, string[] words, int index)
  {
    string ipsSetCommandArgs = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      switch (nCommand)
      {
        case 1:
          ipsSetCommandArgs = words[index] + " (not used)";
          break;
        case 2:
          switch (argNum)
          {
            case 1:
              switch (num)
              {
                case 1:
                  ipsSetCommandArgs = "1 - Arm Away";
                  break;
                case 2:
                  ipsSetCommandArgs = "2 - Arm Stay";
                  break;
                case 3:
                  ipsSetCommandArgs = "3 - Arm Instant";
                  break;
                default:
                  this.textColor = this.errorFoundTextColor;
                  ipsSetCommandArgs = words[index] + " (unknown arming arg)";
                  break;
              }
              break;
            case 2:
              switch (num)
              {
                case 0:
                  ipsSetCommandArgs = "0 - Non-Force";
                  break;
                case 1:
                  ipsSetCommandArgs = "1 - Force";
                  break;
                default:
                  this.textColor = this.errorFoundTextColor;
                  ipsSetCommandArgs = words[index] + " (unknown arming arg)";
                  break;
              }
              break;
            default:
              ipsSetCommandArgs = words[index] + " (not used)";
              break;
          }
          break;
        case 3:
          switch (argNum)
          {
            case 1:
              switch (num)
              {
                case 1:
                  ipsSetCommandArgs = "1 - enable";
                  break;
                case 2:
                  ipsSetCommandArgs = "2 - bypass";
                  break;
                case 3:
                  ipsSetCommandArgs = "3 - disable";
                  break;
                default:
                  this.textColor = this.errorFoundTextColor;
                  ipsSetCommandArgs = words[index] + " (unknown point mode control command)";
                  break;
              }
              break;
            case 2:
              switch (num)
              {
                case -1:
                  ipsSetCommandArgs = "-1 - All";
                  break;
                case 1:
                  ipsSetCommandArgs = "1 - Monitor Point";
                  break;
                case 4:
                  ipsSetCommandArgs = "2 - ACR Door";
                  break;
                default:
                  this.textColor = this.errorFoundTextColor;
                  ipsSetCommandArgs = words[index] + " (unknown point type)";
                  break;
              }
              break;
            case 3:
              ipsSetCommandArgs = num.ToString() + " (point number)";
              break;
          }
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          ipsSetCommandArgs = words[index] + " (Unknown)";
          break;
      }
    }
    return ipsSetCommandArgs;
  }

  private string ParseSioRdrHexTargetType(string[] words, int index)
  {
    string rdrHexTargetType = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          rdrHexTargetType = "0x00 MG(K)/MS(K) Reader";
          break;
        case 1:
          rdrHexTargetType = "0x01 KP/PR(K)/SM(K)/MT(K)/MI(K)/FMK Reader";
          break;
        case 2:
          rdrHexTargetType = "0x02 AD-300 Main";
          break;
        case 3:
          rdrHexTargetType = "0x03 AD-400 Main";
          break;
        case 4:
          rdrHexTargetType = "0x04 PIM400-485";
          break;
        case 5:
          rdrHexTargetType = "0x05 MT(K)2/MI(K)2/FMK2 Reader";
          break;
        case 6:
          rdrHexTargetType = "0x06 NDE";
          break;
        case 7:
          rdrHexTargetType = "0x07 ADE-COM";
          break;
        case 8:
          rdrHexTargetType = "0x08 ADE-Main";
          break;
        case 9:
          rdrHexTargetType = "0x09 ENGAGE Gateway";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          rdrHexTargetType = words[index] + " (Unknown Target Type)";
          break;
      }
    }
    return rdrHexTargetType;
  }

  private string ParseIdMode(string[] words, int index)
  {
    string idMode = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      switch (int.Parse(words[index]))
      {
        case 0:
          idMode = "0";
          break;
        case 1:
          idMode = "1 (locked)";
          break;
        case 2:
          idMode = "2 (unlocked)";
          break;
        case 5:
        case 6:
        case 7:
        case 8:
          idMode = this.ParseAcrMode(words, index);
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          idMode = words[index] + " (Unknown IdMode)";
          break;
      }
    }
    return idMode;
  }

  private string ParsePinDigits(string[] words, int index)
  {
    string pinDigits = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num1 = int.Parse(words[index]);
      int num2 = (num1 & 61440 /*0xF000*/) / 4096 /*0x1000*/;
      int num3 = (num1 & 3840 /*0x0F00*/) / 256 /*0x0100*/;
      int num4 = (num1 & 240 /*0xF0*/) / 16 /*0x10*/;
      int num5 = num1 & 15;
      string str = "";
      switch (num2)
      {
        case 0:
          str = " (Disabled)";
          break;
        case 1:
          str = " (AddConstant)";
          break;
        case 2:
          str = " (AppendConstant)";
          break;
      }
      pinDigits = $"{(object) num1} [duressMode: {(object) num2}{str}, pinConstant: {(object) num3}, cardIDSize: {(object) num4}, nPinDigits: {(object) num5}]";
    }
    return pinDigits;
  }

  private string ParseDebugRq(string[] words, int index)
  {
    string debugRq = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      switch (num)
      {
        case 0:
          debugRq = "0 - Normal mode";
          break;
        case 1:
          debugRq = "1 - Dump Channel";
          break;
        case 2:
          debugRq = "2 - Dump SCP";
          break;
        case 3:
          debugRq = "3 - Enable/Disable Polls/Acks";
          break;
        default:
          debugRq = num.ToString() + " - Unknown value";
          break;
      }
    }
    return debugRq;
  }

  private string ParseActlFlags(string[] words, int index)
  {
    string actlFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      actlFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        actlFlags += "ACR_F_DCR ";
      if ((num & 2) == 2)
        actlFlags += "ACR_F_CUL ";
      if ((num & 4) == 4)
        actlFlags += "ACR_F_DRSS ";
      if ((num & 8) == 8)
        actlFlags += "ACR_F_ALLUSED ";
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
        actlFlags += "ACR_F_QEXIT ";
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
        actlFlags += "ACR_F_FILTER ";
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
        actlFlags += "ACR_F_2CARD ";
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
        actlFlags += "ACR_F_NOPIPE ";
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
        actlFlags += "ACR_F_BIOVERIFY ";
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
        actlFlags += "ACR_F_BIOENROLL ";
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
        actlFlags += "ACR_F_HOST_CBG ";
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
        actlFlags += "ACR_F_HOST_SFT ";
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
        actlFlags += "ACR_F_CIPHER ";
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
        actlFlags += "ACR_F_MODE_TRIG ";
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
        actlFlags += "ACR_F_LOG_EARLY ";
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
        actlFlags += "ACR_F_CNIF_WAIT ";
    }
    return actlFlags;
  }

  private string ParseTKEFlags(string[] words, int index)
  {
    string tkeFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      tkeFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        tkeFlags += "Front ";
      if ((num & 2) == 2)
        tkeFlags += "Rear ";
    }
    return tkeFlags;
  }

  private string ParseTKEDedModeFlags(string[] words, int index)
  {
    string tkeDedModeFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      tkeDedModeFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        tkeDedModeFlags += "PINEntryEnabled ";
      if ((num & 2) == 16 /*0x10*/)
        tkeDedModeFlags += "SleepModeEnabled ";
    }
    return tkeDedModeFlags;
  }

  private string ParseMitsuVeriLoc(string[] words, int index)
  {
    string mitsuVeriLoc = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      mitsuVeriLoc = string.Format("{0} (0x{0:X2}) ", (object) num);
      if ((num & 1) == 1)
        mitsuVeriLoc += "HOP ";
      if ((num & 4) == 4)
        mitsuVeriLoc += "Turnstile ";
    }
    return mitsuVeriLoc;
  }

  private byte reverseBitOrderOfByte(byte b)
  {
    b = (byte) (((int) b & 240 /*0xF0*/) >> 4 | ((int) b & 15) << 4);
    b = (byte) (((int) b & 204) >> 2 | ((int) b & 51) << 2);
    b = (byte) (((int) b & 170) >> 1 | ((int) b & 85) << 1);
    return b;
  }

  private string ParseCtlBitmap(string[] words, int index)
  {
    string ctlBitmap = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      byte num1 = 128 /*0x80*/;
      for (int index1 = 0; index1 < (words.Length - index) * 8; ++index1)
      {
        int num2 = (int) (byte) index1 % 8;
        int num3 = index1 / 8;
        int num4 = (int) num1 >> num2;
        ushort result = 0;
        byte num5 = 0;
        if (ushort.TryParse(words[index + num3], out result))
          num5 = this.reverseBitOrderOfByte((byte) ushort.Parse(words[index + num3]));
        if (((int) num5 & num4) > 0)
          ctlBitmap += $"{index1} ";
      }
    }
    return ctlBitmap;
  }

  private string ParseMitsuBoardingEntrance(string[] words, int index)
  {
    string boardingEntrance = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      boardingEntrance = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        boardingEntrance += "Front ";
      if ((num & 2) == 2)
        boardingEntrance += "Rear ";
    }
    return boardingEntrance;
  }

  private string ParseTempLEDFlags(string[] words, int index)
  {
    string tempLedFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      tempLedFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        tempLedFlags += "USE_RGB ";
      if ((num & 2) == 2)
        tempLedFlags += "FOLLOW_TRANSIENT ";
    }
    return tempLedFlags;
  }

  private string ParseAdbcFlags(string[] words, int index)
  {
    string adbcFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      adbcFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        adbcFlags += "ADBC_ACTIVE ";
      if ((num & 2) == 2)
        adbcFlags += "ADBC_1FREE ";
      if ((num & 4) == 4)
        adbcFlags += "ADBC_VIP ";
      if ((num & 8) == 8)
        adbcFlags += "ADBC_ADA ";
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
        adbcFlags += "ADBC_UNDEF ";
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
        adbcFlags += "ADBC_NOAPB ";
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
        adbcFlags += "ADBC_NOUSE ";
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
        adbcFlags += "ADBC_NOUSECRNT ";
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbcFlags += "(UNKNOWN 0x0100) ";
      }
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbcFlags += "(UNKNOWN 0x0200) ";
      }
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbcFlags += "(UNKNOWN 0x0400) ";
      }
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbcFlags += "(UNKNOWN 0x0800) ";
      }
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbcFlags += "(UNKNOWN 0x1000) ";
      }
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbcFlags += "(UNKNOWN 0x2000) ";
      }
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbcFlags += "(UNKNOWN 0x4000) ";
      }
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbcFlags += "(UNKNOWN 0x8000) ";
      }
    }
    return adbcFlags;
  }

  private string ParseAdbFlags(string[] words, int index)
  {
    string adbFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      adbFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        adbFlags += "(LowestEscortCode) ";
      if ((num & 2) == 2)
      {
        this.textColor = this.errorFoundTextColor;
        adbFlags += "(UNKNOWN 0x0002) ";
      }
      if ((num & 4) == 4)
      {
        this.textColor = this.errorFoundTextColor;
        adbFlags += "(UNKNOWN 0x0004) ";
      }
      if ((num & 8) == 8)
      {
        this.textColor = this.errorFoundTextColor;
        adbFlags += "(UNKNOWN 0x0008) ";
      }
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbFlags += "(UNKNOWN 0x0010) ";
      }
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbFlags += "(UNKNOWN 0x0020) ";
      }
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbFlags += "(UNKNOWN 0x0040) ";
      }
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbFlags += "(UNKNOWN 0x0080) ";
      }
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbFlags += "(UNKNOWN 0x0100) ";
      }
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbFlags += "(UNKNOWN 0x0200) ";
      }
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbFlags += "(UNKNOWN 0x0400) ";
      }
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbFlags += "(UNKNOWN 0x0800) ";
      }
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbFlags += "(UNKNOWN 0x1000) ";
      }
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbFlags += "(UNKNOWN 0x2000) ";
      }
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbFlags += "(UNKNOWN 0x4000) ";
      }
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        this.textColor = this.errorFoundTextColor;
        adbFlags += "(UNKNOWN 0x8000) ";
      }
    }
    return adbFlags;
  }

  private string ParseCardFormatterFlags(string[] words, int index)
  {
    string cardFormatterFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      cardFormatterFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        cardFormatterFlags += "(Step Parity by 2) ";
      if ((num & 2) == 2)
        cardFormatterFlags += "(Suppress FC Checking) ";
      if ((num & 4) == 4)
        cardFormatterFlags += "(Corporate Card) ";
      if ((num & 8) == 8)
        cardFormatterFlags += "(37-bit Parity Test w/ 4 parity bits) ";
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
        cardFormatterFlags += "(Motorola 64-bit BiStatic parity format) ";
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
        cardFormatterFlags += "(37-bit Parity Test w/ 2 parity bits in middle) ";
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
        cardFormatterFlags += "(48-bit PIV) ";
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
        cardFormatterFlags += "(Special Card Formats) ";
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
        cardFormatterFlags += "(Reverse Card Format) ";
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
        cardFormatterFlags += "(Large Encoded ID) ";
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
        cardFormatterFlags += "(Reverse Byte Order 0x0400) ";
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
        cardFormatterFlags += "(37-bit Parity Test - Special) ";
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
        cardFormatterFlags += "(Convert 200-bit to 128-bit) ";
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
        cardFormatterFlags += "(Require Matching Card Number) ";
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
      {
        this.textColor = this.errorFoundTextColor;
        cardFormatterFlags += "(UNKNOWN 0x4000) ";
      }
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        this.textColor = this.errorFoundTextColor;
        cardFormatterFlags += "(UNKNOWN 0x8000) ";
      }
    }
    return cardFormatterFlags;
  }

  private string ParseCardFormatterMTAFlags(string[] words, int index)
  {
    string formatterMtaFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      formatterMtaFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
      {
        this.textColor = this.errorFoundTextColor;
        formatterMtaFlags += "(UNKNOWN 0x0001) ";
      }
      if ((num & 2) == 2)
        formatterMtaFlags += "(Suppress FC Checking) ";
      if ((num & 4) == 4)
        formatterMtaFlags += "(Corporate Card) ";
      if ((num & 8) == 8)
      {
        this.textColor = this.errorFoundTextColor;
        formatterMtaFlags += "(UNKNOWN 0x0400) ";
      }
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
      {
        this.textColor = this.errorFoundTextColor;
        formatterMtaFlags += "(UNKNOWN 0x0010) ";
      }
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
      {
        this.textColor = this.errorFoundTextColor;
        formatterMtaFlags += "(UNKNOWN 0x0020) ";
      }
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
      {
        this.textColor = this.errorFoundTextColor;
        formatterMtaFlags += "(UNKNOWN 0x0040) ";
      }
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
      {
        this.textColor = this.errorFoundTextColor;
        formatterMtaFlags += "(UNKNOWN 0x0080) ";
      }
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
      {
        this.textColor = this.errorFoundTextColor;
        formatterMtaFlags += "(UNKNOWN 0x0100) ";
      }
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
      {
        this.textColor = this.errorFoundTextColor;
        formatterMtaFlags += "(UNKNOWN 0x0200) ";
      }
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
      {
        this.textColor = this.errorFoundTextColor;
        formatterMtaFlags += "(UNKNOWN 0x0400) ";
      }
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
      {
        this.textColor = this.errorFoundTextColor;
        formatterMtaFlags += "(UNKNOWN 0x0800) ";
      }
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
      {
        this.textColor = this.errorFoundTextColor;
        formatterMtaFlags += "(UNKNOWN 0x1000) ";
      }
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
      {
        this.textColor = this.errorFoundTextColor;
        formatterMtaFlags += "(UNKNOWN 0x2000) ";
      }
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
      {
        this.textColor = this.errorFoundTextColor;
        formatterMtaFlags += "(UNKNOWN 0x4000) ";
      }
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        this.textColor = this.errorFoundTextColor;
        formatterMtaFlags += "(UNKNOWN 0x8000) ";
      }
    }
    return formatterMtaFlags;
  }

  private string ParseUseNames(string[] words, int index)
  {
    string useNames = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      useNames = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        useNames += "(MPs) ";
      if ((num & 2) == 2)
        useNames += "(CPs) ";
      if ((num & 4) == 4)
        useNames += "(ACRs) ";
      if ((num & 8) == 8)
      {
        this.textColor = this.errorFoundTextColor;
        useNames += "(UNKNOWN 0x0008) ";
      }
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
      {
        this.textColor = this.errorFoundTextColor;
        useNames += "(UNKNOWN 0x0010) ";
      }
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
      {
        this.textColor = this.errorFoundTextColor;
        useNames += "(UNKNOWN 0x0020) ";
      }
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
      {
        this.textColor = this.errorFoundTextColor;
        useNames += "(UNKNOWN 0x0040) ";
      }
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
      {
        this.textColor = this.errorFoundTextColor;
        useNames += "(UNKNOWN 0x0080) ";
      }
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
      {
        this.textColor = this.errorFoundTextColor;
        useNames += "(UNKNOWN 0x0100) ";
      }
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
      {
        this.textColor = this.errorFoundTextColor;
        useNames += "(UNKNOWN 0x0200) ";
      }
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
      {
        this.textColor = this.errorFoundTextColor;
        useNames += "(UNKNOWN 0x0400) ";
      }
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
      {
        this.textColor = this.errorFoundTextColor;
        useNames += "(UNKNOWN 0x0800) ";
      }
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
      {
        this.textColor = this.errorFoundTextColor;
        useNames += "(UNKNOWN 0x1000) ";
      }
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
      {
        this.textColor = this.errorFoundTextColor;
        useNames += "(UNKNOWN 0x2000) ";
      }
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
      {
        this.textColor = this.errorFoundTextColor;
        useNames += "(UNKNOWN 0x4000) ";
      }
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        this.textColor = this.errorFoundTextColor;
        useNames += "(UNKNOWN 0x8000) ";
      }
    }
    return useNames;
  }

  private string ParseAreaFlags(string[] words, int index)
  {
    string areaFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      areaFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        areaFlags += "(AREA_F_AIRLOCK) ";
      if ((num & 2) == 2)
        areaFlags += "(AREA_F_AIRLOCK_ODO) ";
      if ((num & 4) == 4)
      {
        this.textColor = this.errorFoundTextColor;
        areaFlags += "(UNKNOWN 0x0004) ";
      }
      if ((num & 8) == 8)
      {
        this.textColor = this.errorFoundTextColor;
        areaFlags += "(UNKNOWN 0x0008) ";
      }
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
      {
        this.textColor = this.errorFoundTextColor;
        areaFlags += "(UNKNOWN 0x0010) ";
      }
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
      {
        this.textColor = this.errorFoundTextColor;
        areaFlags += "(UNKNOWN 0x0020) ";
      }
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
      {
        this.textColor = this.errorFoundTextColor;
        areaFlags += "(UNKNOWN 0x0040) ";
      }
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
      {
        this.textColor = this.errorFoundTextColor;
        areaFlags += "(UNKNOWN 0x0080) ";
      }
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
      {
        this.textColor = this.errorFoundTextColor;
        areaFlags += "(UNKNOWN 0x0100) ";
      }
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
      {
        this.textColor = this.errorFoundTextColor;
        areaFlags += "(UNKNOWN 0x0200) ";
      }
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
      {
        this.textColor = this.errorFoundTextColor;
        areaFlags += "(UNKNOWN 0x0400) ";
      }
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
      {
        this.textColor = this.errorFoundTextColor;
        areaFlags += "(UNKNOWN 0x0800) ";
      }
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
      {
        this.textColor = this.errorFoundTextColor;
        areaFlags += "(UNKNOWN 0x1000) ";
      }
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
      {
        this.textColor = this.errorFoundTextColor;
        areaFlags += "(UNKNOWN 0x2000) ";
      }
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
      {
        this.textColor = this.errorFoundTextColor;
        areaFlags += "(UNKNOWN 0x4000) ";
      }
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        this.textColor = this.errorFoundTextColor;
        areaFlags += "(UNKNOWN 0x8000) ";
      }
    }
    return areaFlags;
  }

  private string ParseDisplaySpec(string[] words, int index)
  {
    string displaySpec = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      displaySpec = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        displaySpec += "(UCMND_DF_IPSSTS) ";
      if ((num & 2) == 2)
        displaySpec += "(UCMND_DF_IPS_ACT) ";
      if ((num & 4) == 4)
        displaySpec += "(UCMND_DF_ENTRY) ";
      if ((num & 8) == 8)
        displaySpec += "(UCMND_DF_EXIT) ";
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
        displaySpec += "(UCMND_DF_CHIME_0) ";
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
        displaySpec += "(UCMND_DF_CHIME_TGLF) ";
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
      {
        this.textColor = this.errorFoundTextColor;
        displaySpec += "(UNKNOWN 0x0040) ";
      }
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
      {
        this.textColor = this.errorFoundTextColor;
        displaySpec += "(UNKNOWN 0x0080) ";
      }
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
      {
        this.textColor = this.errorFoundTextColor;
        displaySpec += "(UNKNOWN 0x0100) ";
      }
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
      {
        this.textColor = this.errorFoundTextColor;
        displaySpec += "(UNKNOWN 0x0200) ";
      }
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
      {
        this.textColor = this.errorFoundTextColor;
        displaySpec += "(UNKNOWN 0x0400) ";
      }
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
      {
        this.textColor = this.errorFoundTextColor;
        displaySpec += "(UNKNOWN 0x0800) ";
      }
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
      {
        this.textColor = this.errorFoundTextColor;
        displaySpec += "(UNKNOWN 0x1000) ";
      }
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
      {
        this.textColor = this.errorFoundTextColor;
        displaySpec += "(UNKNOWN 0x2000) ";
      }
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
      {
        this.textColor = this.errorFoundTextColor;
        displaySpec += "(UNKNOWN 0x4000) ";
      }
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        this.textColor = this.errorFoundTextColor;
        displaySpec += "(UNKNOWN 0x8000) ";
      }
    }
    return displaySpec;
  }

  private string ParseUcmdAuthorityFlags(string[] words, int index)
  {
    string ucmdAuthorityFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      ucmdAuthorityFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        ucmdAuthorityFlags += "(UCAC_ACRMODE) ";
      if ((num & 2) == 2)
        ucmdAuthorityFlags += "(UCAC_VALID) ";
      if ((num & 4) == 4)
        ucmdAuthorityFlags += "(UCAC_ADMIT) ";
      if ((num & 8) == 8)
      {
        this.textColor = this.errorFoundTextColor;
        ucmdAuthorityFlags += "(UNKNOWN 0x0008) ";
      }
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
      {
        this.textColor = this.errorFoundTextColor;
        ucmdAuthorityFlags += "(UNKNOWN 0x0010) ";
      }
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
      {
        this.textColor = this.errorFoundTextColor;
        ucmdAuthorityFlags += "(UNKNOWN 0x0020) ";
      }
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
      {
        this.textColor = this.errorFoundTextColor;
        ucmdAuthorityFlags += "(UNKNOWN 0x0040) ";
      }
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
      {
        this.textColor = this.errorFoundTextColor;
        ucmdAuthorityFlags += "(UNKNOWN 0x0080) ";
      }
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
      {
        this.textColor = this.errorFoundTextColor;
        ucmdAuthorityFlags += "(UNKNOWN 0x0100) ";
      }
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
      {
        this.textColor = this.errorFoundTextColor;
        ucmdAuthorityFlags += "(UNKNOWN 0x0200) ";
      }
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
      {
        this.textColor = this.errorFoundTextColor;
        ucmdAuthorityFlags += "(UNKNOWN 0x0400) ";
      }
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
      {
        this.textColor = this.errorFoundTextColor;
        ucmdAuthorityFlags += "(UNKNOWN 0x0800) ";
      }
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
      {
        this.textColor = this.errorFoundTextColor;
        ucmdAuthorityFlags += "(UNKNOWN 0x1000) ";
      }
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
      {
        this.textColor = this.errorFoundTextColor;
        ucmdAuthorityFlags += "(UNKNOWN 0x2000) ";
      }
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
      {
        this.textColor = this.errorFoundTextColor;
        ucmdAuthorityFlags += "(UNKNOWN 0x4000) ";
      }
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        this.textColor = this.errorFoundTextColor;
        ucmdAuthorityFlags += "(UNKNOWN 0x8000) ";
      }
    }
    return ucmdAuthorityFlags;
  }

  private string ParseBkgdSpec(string[] words, int index)
  {
    string bkgdSpec = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      bkgdSpec = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        bkgdSpec += "(change Settings) ";
      if ((num & 2) == 2)
        bkgdSpec += "(Clear all Bkgd buffers) ";
      if ((num & 4) == 4)
        bkgdSpec += "(Display Time) ";
      if ((num & 8) == 8)
        bkgdSpec += "(24 Hour Mode) ";
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
        bkgdSpec += "(Display User Specified Bkgd Text) ";
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
        bkgdSpec += "(Status of MPG) ";
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
        bkgdSpec += "(Name of Active Zones) ";
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
        bkgdSpec += "(Slow Update) ";
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
      {
        this.textColor = this.errorFoundTextColor;
        bkgdSpec += "(UNKNOWN 0x0100) ";
      }
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
      {
        this.textColor = this.errorFoundTextColor;
        bkgdSpec += "(UNKNOWN 0x0200) ";
      }
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
      {
        this.textColor = this.errorFoundTextColor;
        bkgdSpec += "(UNKNOWN 0x0400) ";
      }
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
      {
        this.textColor = this.errorFoundTextColor;
        bkgdSpec += "(UNKNOWN 0x0800) ";
      }
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
      {
        this.textColor = this.errorFoundTextColor;
        bkgdSpec += "(UNKNOWN 0x1000) ";
      }
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
      {
        this.textColor = this.errorFoundTextColor;
        bkgdSpec += "(UNKNOWN 0x2000) ";
      }
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
      {
        this.textColor = this.errorFoundTextColor;
        bkgdSpec += "(UNKNOWN 0x4000) ";
      }
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        this.textColor = this.errorFoundTextColor;
        bkgdSpec += "(UNKNOWN 0x8000) ";
      }
    }
    return bkgdSpec;
  }

  private string ParseBioDbFlags(string[] words, int index)
  {
    string bioDbFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int.Parse(words[index]);
      bioDbFlags = words[index] + " (low nibble is verify wait time in 2 second ticks)";
    }
    return bioDbFlags;
  }

  private string ParseIpsPointFlags(string[] words, int index)
  {
    string ipsPointFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      ipsPointFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 7) == 0)
        ipsPointFlags += "(MPF_IPG_24HOUR) ";
      if ((num & 7) == 1)
        ipsPointFlags += "(MPF_IPG_PERIMETER) ";
      if ((num & 7) == 2)
        ipsPointFlags += "(MPF_IPG_INTERIOR) ";
      if ((num & 4) == 4)
        ipsPointFlags += "(ACRs) ";
      if ((num & 8) == 8)
        ipsPointFlags += "(MPF_CHIME) ";
      if ((num & 48 /*0x30*/) == 0)
        ipsPointFlags += "(MPF_MM_NORMAL) ";
      if ((num & 48 /*0x30*/) == 16 /*0x10*/)
        ipsPointFlags += "(MPF_MM_BYPASSED) ";
      if ((num & 48 /*0x30*/) == 32 /*0x20*/)
        ipsPointFlags += "(MPF_MM_DISABLED) ";
      if ((num & 192 /*0xC0*/) == 0)
        ipsPointFlags += "(MPF_EDLY_INSTANT) ";
      if ((num & 192 /*0xC0*/) == 64 /*0x40*/)
        ipsPointFlags += "(MPF_EDLY_TRIGGER) ";
      if ((num & 192 /*0xC0*/) == 128 /*0x80*/)
        ipsPointFlags += "(MPF_EDLY_FOLLOW) ";
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
      {
        this.textColor = this.errorFoundTextColor;
        ipsPointFlags += "(UNKNOWN 0x0100) ";
      }
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
      {
        this.textColor = this.errorFoundTextColor;
        ipsPointFlags += "(UNKNOWN 0x0200) ";
      }
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
      {
        this.textColor = this.errorFoundTextColor;
        ipsPointFlags += "(UNKNOWN 0x0400) ";
      }
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
      {
        this.textColor = this.errorFoundTextColor;
        ipsPointFlags += "(UNKNOWN 0x0800) ";
      }
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
      {
        this.textColor = this.errorFoundTextColor;
        ipsPointFlags += "(UNKNOWN 0x1000) ";
      }
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
      {
        this.textColor = this.errorFoundTextColor;
        ipsPointFlags += "(UNKNOWN 0x2000) ";
      }
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
      {
        this.textColor = this.errorFoundTextColor;
        ipsPointFlags += "(UNKNOWN 0x4000) ";
      }
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        this.textColor = this.errorFoundTextColor;
        ipsPointFlags += "(UNKNOWN 0x8000) ";
      }
    }
    return ipsPointFlags;
  }

  private string ParseFloorFlags(string[] words, int index)
  {
    string floorFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      floorFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        floorFlags += "(rear floors) ";
      if ((num & 2) == 2)
      {
        this.textColor = this.errorFoundTextColor;
        floorFlags += "(UNKNOWN 0x0002) ";
      }
      if ((num & 4) == 4)
      {
        this.textColor = this.errorFoundTextColor;
        floorFlags += "(UNKNOWN 0x0004) ";
      }
      if ((num & 8) == 8)
      {
        this.textColor = this.errorFoundTextColor;
        floorFlags += "(UNKNOWN 0x0008) ";
      }
      if ((num & 16 /*0x10*/) == 16 /*0x10*/)
      {
        this.textColor = this.errorFoundTextColor;
        floorFlags += "(UNKNOWN 0x0010) ";
      }
      if ((num & 32 /*0x20*/) == 32 /*0x20*/)
      {
        this.textColor = this.errorFoundTextColor;
        floorFlags += "(UNKNOWN 0x0020) ";
      }
      if ((num & 64 /*0x40*/) == 64 /*0x40*/)
      {
        this.textColor = this.errorFoundTextColor;
        floorFlags += "(UNKNOWN 0x0040) ";
      }
      if ((num & 128 /*0x80*/) == 128 /*0x80*/)
      {
        this.textColor = this.errorFoundTextColor;
        floorFlags += "(UNKNOWN 0x0080) ";
      }
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
      {
        this.textColor = this.errorFoundTextColor;
        floorFlags += "(UNKNOWN 0x0100) ";
      }
      if ((num & 512 /*0x0200*/) == 512 /*0x0200*/)
      {
        this.textColor = this.errorFoundTextColor;
        floorFlags += "(UNKNOWN 0x0200) ";
      }
      if ((num & 1024 /*0x0400*/) == 1024 /*0x0400*/)
      {
        this.textColor = this.errorFoundTextColor;
        floorFlags += "(UNKNOWN 0x0400) ";
      }
      if ((num & 2048 /*0x0800*/) == 2048 /*0x0800*/)
      {
        this.textColor = this.errorFoundTextColor;
        floorFlags += "(UNKNOWN 0x0800) ";
      }
      if ((num & 4096 /*0x1000*/) == 4096 /*0x1000*/)
      {
        this.textColor = this.errorFoundTextColor;
        floorFlags += "(UNKNOWN 0x1000) ";
      }
      if ((num & 8192 /*0x2000*/) == 8192 /*0x2000*/)
      {
        this.textColor = this.errorFoundTextColor;
        floorFlags += "(UNKNOWN 0x2000) ";
      }
      if ((num & 16384 /*0x4000*/) == 16384 /*0x4000*/)
      {
        this.textColor = this.errorFoundTextColor;
        floorFlags += "(UNKNOWN 0x4000) ";
      }
      if ((num & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      {
        this.textColor = this.errorFoundTextColor;
        floorFlags += "(UNKNOWN 0x8000) ";
      }
    }
    return floorFlags;
  }

  private string ParseApbFreePassFlags(string[] words, int index)
  {
    string apbFreePassFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      apbFreePassFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        apbFreePassFlags += "(Do Not NAK) ";
    }
    return apbFreePassFlags;
  }

  private string ParseSioOutCommand(string[] words, int index)
  {
    string sioOutCommand = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      switch (num)
      {
        case 1:
          sioOutCommand = "1 - OFF";
          break;
        case 2:
          sioOutCommand = "2 - ON";
          break;
        case 3:
          sioOutCommand = "3 - Pulse On";
          break;
        case 4:
          sioOutCommand = "4 - Pulse Off";
          break;
        default:
          this.textColor = this.errorFoundTextColor;
          sioOutCommand = num.ToString() + " (Unknown command)";
          break;
      }
    }
    return sioOutCommand;
  }

  private string ParseLpdFlags(string[] words, int index)
  {
    string lpdFlags = "";
    if (index < ((IEnumerable<string>) words).Count<string>())
    {
      int num = int.Parse(words[index]);
      lpdFlags = string.Format("{0} (0x{0:X4}) ", (object) num);
      if ((num & 1) == 1)
        lpdFlags += "(LDF_PIN) ";
      if ((num & 2) == 2)
        lpdFlags += "(LDF_CIPHER) ";
      if ((num & 4) == 4)
        lpdFlags += "(LDF_CAP) ";
      if ((num & 8) == 8)
        lpdFlags += "(LDF_BIOX) ";
      if ((num & 256 /*0x0100*/) == 256 /*0x0100*/)
        lpdFlags += "(LDF_DACT) ";
    }
    return lpdFlags;
  }


  public string RemoveDuplicateWhiteSpace(string input)
  {
    StringBuilder stringBuilder = new StringBuilder();
    string[] strArray = input.Split(new char[6]
    {
      ' ',
      '\n',
      '\t',
      '\r',
      '\f',
      '\v'
    }, StringSplitOptions.RemoveEmptyEntries);
    int length = strArray.Length;
    for (int index = 0; index < length; ++index)
      stringBuilder.AppendFormat("{0} ", (object) strArray[index]);
    return stringBuilder.ToString();
  }


  private int GetCmdIndex()
  {
    int cmdIndex = 3;
    if (this.processCommandOnly)
      cmdIndex = 0;
    return cmdIndex;
  }

  private string ParseCtlSts(int ctlStsValue)
  {
    int num = ctlStsValue & 3;
    string ctlSts = $"{(object) ctlStsValue} SQN: {(object) num}";
    switch (ctlStsValue & 12)
    {
      case 4:
        ctlSts += " NAK-TOO-LONG";
        break;
      case 8:
        ctlSts += " NAK-CRC-ERROR";
        break;
      case 12:
        ctlSts += " NAK-SEQUENCE-ERROR";
        break;
    }
    if ((ctlStsValue & 128 /*0x80*/) == 128 /*0x80*/)
      ctlSts += " UTAG";
    switch (ctlStsValue & 14)
    {
      case 0:
        ctlSts += " PRIM-NOALT";
        break;
      case 16 /*0x10*/:
        ctlSts += " POLLME";
        break;
      case 32 /*0x20*/:
        ctlSts += " PRIM-ALTOFF";
        break;
      case 64 /*0x40*/:
        ctlSts += " PRIM-ALTSTBY";
        break;
      case 96 /*0x60*/:
        ctlSts += " PRIM-ALTPRIM";
        break;
      case 128 /*0x80*/:
        ctlSts += " ALT-PRIMOFF";
        break;
      case 160 /*0xA0*/:
        ctlSts += " ALT-PRIMSTBY";
        break;
      case 192 /*0xC0*/:
        ctlSts += " ALT-PRIMPRIM";
        break;
    }
    return ctlSts;
  }

  private string ParseNakReason(int reason)
  {
    string nakReason;
    switch (reason)
    {
      case 0:
        nakReason = "0 - invalid packet header";
        break;
      case 1:
      case 2:
      case 3:
        nakReason = reason.ToString() + " - invalid command type";
        break;
      case 4:
        nakReason = "4 - command content error";
        break;
      case 5:
        nakReason = "5 - requires password logon";
        break;
      case 6:
        nakReason = "6 - this port is in standby mode";
        break;
      case 7:
        nakReason = "7 - failed login: password and/or encryption key";
        break;
      default:
        nakReason = reason.ToString() + " - Unknown reason";
        break;
    }
    return nakReason;
  }

  private void Parse8102(string[] words, int wordCount, int index)
  {
    this.InsertLine("NAK");
    if (wordCount > index + 1)
      this.InsertLine("reason", ": ", this.ParseNakReason(this.ValueFromStrings(words[index + 0])));
    if (wordCount <= index + 5)
      return;
    this.InsertLine("data", ": ", this.ValueFromStrings(words[index + 1], words[index + 2], words[index + 3], words[index + 4]));
  }

  private void Parse0604(string[] words, int wordCount, int index)
  {
    this.InsertLine("Holiday Read");
  }

  private void Parse0502(string[] words, int wordCount, int index)
  {
    this.InsertLine("CP Configuration");
    for (int index1 = 0; index1 < this.num_records; ++index1)
    {
      this.InsertLine("");
      if (wordCount > index + (index1 * 6 + 1) + 1)
        this.InsertLine("cp_number", ": ", this.ValueFromStrings(words[index + (index1 * 6 + 0)], words[index + (index1 * 6 + 1)]));
      if (wordCount > index + (index1 * 6 + 2) + 1)
        this.InsertLine("sio_number", ": ", this.ValueFromStrings(words[index + (index1 * 6 + 2)]));
      if (wordCount > index + (index1 * 6 + 3) + 1)
        this.InsertLine("op_number", ": ", this.ValueFromStrings(words[index + (index1 * 6 + 3)]));
      if (wordCount > index + (index1 * 6 + 5) + 1)
        this.InsertLine("dflt_pulse", ": ", this.ValueFromStrings(words[index + (index1 * 6 + 4)], words[index + (index1 * 6 + 5)]));
    }
  }

  private void Parse8106(string[] words, int wordCount, int index, int rec_len)
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    this.InsertLine("Transactions");
    bool flag = true;
    while (flag)
    {
      num3 = 0;
      int num4 = 0;
      if (wordCount >= index + 4)
      {
        uint num5 = (uint) this.ValueFromStrings(words[index + 0], words[index + 1], words[index + 2], words[index + 3]);
        this.InsertLine("Time", ": ", $"{new DateTime(1900, 1, 1).AddSeconds((double) num5).ToString()} ({(object) num5})");
      }
      if (wordCount >= index + 5)
      {
        num1 = this.ValueFromStrings(words[index + 4]);
        this.InsertLine("Source Type", ": ", this.ParseTransactionSourceType(words, index + 4, true));
      }
      if (wordCount >= index + 6)
        this.InsertLine("Source Number", ": ", this.ValueFromStrings(words[index + 5]));
      if (wordCount >= index + 9)
        this.ParseTransactionType(words[index + 7], words[index + 8], true);
      if (wordCount >= index + 10)
      {
        int val = this.ValueFromStrings(words[index + 9]);
        num4 = val;
        num2 += num4;
        this.InsertLine("Length", ": ", val);
      }
      else
        flag = false;
      if (flag && num2 < rec_len)
      {
        flag = true;
        this.InsertLine(" ");
        index += num4;
      }
      else
        flag = false;
    }
  }

  private void Parse8103(string[] words, int wordCount, int index)
  {
    int num1 = 0;
    this.InsertLine("ID Report");
    if (wordCount >= index + 1)
    {
      num1 = this.ValueFromStrings(words[index + 0]);
      this.InsertLine("Device_ID", ": ", num1);
    }
    this.ParseHardwareID(words[index + 1], num1);
    if (wordCount >= index + 47)
      this.ParseFirmwareVersion(words[index + 2], words[index + 3], words[index + 46]);
    if (wordCount >= index + 36)
      this.InsertLine("ID", ": ", this.ValueFromStrings(words[index + 34], words[index + 35]));
    this.InsertLine("SerialNum", ": ", this.ValueFromStrings(words[14], words[15], words[16 /*0x10*/], words[17]));
    this.InsertLine("RAM Size", ": ", this.ValueFromStrings(words[18], words[19], words[index + 10], words[index + 11]));
    this.InsertLine("RAM Free", ": ", this.ValueFromStrings(words[index + 12], words[index + 13], words[index + 14], words[index + 15]));
    uint num2 = (uint) this.ValueFromStrings(words[index + 16 /*0x10*/], words[index + 17], words[index + 18], words[index + 19]);
    uint num3 = num2 - (2208988800U < num2 ? 2208988800U : 0U);
    this.InsertLine("UTC Time", ": ", $"{new DateTime(1970, 1, 1).AddSeconds((double) num3).ToString()} ({num3.ToString()})");
    this.InsertLine("Cards Max", ": ", this.ValueFromStrings(words[index + 20], words[index + 21], words[index + 22], words[index + 23]));
    this.InsertLine("Cards Used", ": ", this.ValueFromStrings(words[index + 24], words[index + 25], words[index + 26], words[index + 27]));
    if (wordCount >= index + 57)
      this.InsertLine("OEM", ": ", this.ParseOEMCodes(this.ValueFromStrings(words[index + 55], words[index + 56])));
    if (wordCount >= index + 32 /*0x20*/)
      this.InsertLine("Flash", ": ", this.ValueFromStrings(words[index + 28], words[index + 29], words[index + 30], words[index + 31 /*0x1F*/]));
    if (wordCount >= index + 33)
      this.InsertLine("DIP PowerUp", ": ", this.ParseDipSwitches(this.ValueFromStrings(words[index + 32 /*0x20*/])));
    if (wordCount >= index + 34)
      this.InsertLine("DIP Current", ": ", this.ParseDipSwitches(this.ValueFromStrings(words[index + 33])));
    if (wordCount >= index + 37)
      this.InsertLine("IN1", ": ", this.ValueFromStrings(words[index + 36]));
    if (wordCount >= index + 38)
      this.InsertLine("IN2", ": ", this.ValueFromStrings(words[index + 37]));
    if (wordCount >= index + 42)
      this.InsertLine("Asset DB Size", ": ", this.ValueFromStrings(words[index + 38], words[index + 39], words[index + 40], words[index + 41]));
    if (wordCount >= index + 46)
      this.InsertLine("Asset DB Used", ": ", this.ValueFromStrings(words[index + 42], words[index + 43], words[index + 44], words[index + 45]));
    if (wordCount >= index + 51)
      this.InsertLine("Bio DB Size", ": ", this.ValueFromStrings(words[index + 47], words[index + 48 /*0x30*/], words[index + 49], words[index + 50]));
    if (wordCount >= index + 55)
      this.InsertLine("Bio DB Used", ": ", this.ValueFromStrings(words[index + 51], words[index + 52], words[index + 53], words[index + 54]));
    if (wordCount >= index + 63 /*0x3F*/)
      this.InsertLine($"MAC Address: {words[index + 62]}:{words[index + 61]}:{words[index + 60]}:{words[index + 59]}:{words[index + 58]}:{words[index + 57]}");
    if (wordCount >= index + 64 /*0x40*/)
      this.InsertLine("TLS Status", ": ", this.ParseTlsStatus(this.ValueFromStrings(words[index + 63 /*0x3F*/])));
    if (wordCount >= index + 65)
      this.InsertLine("Oper Mode", ": ", this.ValueFromStrings(words[index + 64 /*0x40*/]));
    if (wordCount >= index + 66)
      this.InsertLine("IN3", ": ", this.ValueFromStrings(words[index + 65]));
    if (wordCount < index + 70)
      return;
    this.InsertLine("Cumulative Build Count", ": ", this.ValueFromStrings(words[index + 66], words[index + 67], words[index + 68], words[index + 69]));
  }

  private void Parse8107(string[] words, int wordCount, int index)
  {
    this.InsertLine("Dual Port Status");
    if (wordCount >= index + 1)
      this.InsertLine("HostPort", ": ", this.ValueFromStrings(words[index + 0]));
    if (wordCount >= index + 2)
      this.InsertLine("Status", ": ", this.ValueFromStrings(words[index + 1]));
    if (wordCount >= index + 3)
      this.InsertLine("PrimaryStatus", ": ", this.ValueFromStrings(words[index + 2]));
    if (wordCount < index + 4)
      return;
    this.InsertLine("AlternateStatus", ": ", this.ValueFromStrings(words[index + 3]));
  }

  private void Parse9102(string[] words, int wordCount, int index, int num_records)
  {
    int num1 = 0;
    this.InsertLine("SIO Status Report");
    this.InsertLine("");
    for (int index1 = 0; index1 < num_records; ++index1)
    {
      if (wordCount >= index + 1)
        this.InsertLine("com_status", ": ", this.ValueFromStrings(words[index]));
      if (wordCount >= index + 2)
        this.InsertLine("msp1_dnum", ": ", this.ValueFromStrings(words[index + 1]));
      if (wordCount >= index + 6)
        this.InsertLine("com_retries", ": ", this.ValueFromStrings(words[index + 2], words[index + 3], words[index + 4], words[index + 5]));
      if (wordCount >= index + 7)
        this.InsertLine("ct_stat", ": ", this.ValueFromStrings(words[index + 6]));
      if (wordCount >= index + 8)
        this.InsertLine("pw_stat", ": ", this.ValueFromStrings(words[index + 7]));
      if (wordCount >= index + 9)
      {
        num1 = this.ValueFromStrings(words[index + 8]);
        this.InsertLine("model", ": ", this.ParseSioModel(words, index + 8, true));
      }
      if (wordCount >= index + 10)
        this.InsertLine("revision", ": ", this.ValueFromStrings(words[index + 9]));
      if (wordCount >= index + 14)
        this.InsertLine("serial_num", ": ", this.ValueFromStrings(words[index + 10], words[index + 11], words[index + 12], words[index + 13]));
      if (wordCount >= index + 15)
        this.InsertLine("inputs", ": ", this.ValueFromStrings(words[index + 14]));
      if (wordCount >= index + 16 /*0x10*/)
        this.InsertLine("outputs", ": ", this.ValueFromStrings(words[index + 15]));
      if (wordCount >= index + 17)
        this.InsertLine("readers", ": ", this.ValueFromStrings(words[index + 16 /*0x10*/]));
      if (wordCount >= index + 50)
      {
        string str = "ip_stat: ";
        for (int index2 = 0; index2 < 32 /*0x20*/; ++index2)
          str = $"{str}{words[index + 17 + index2]} ";
        this.InsertLine(str);
      }
      if (wordCount >= index + 66)
      {
        string str = "op_stat: ";
        for (int index3 = 0; index3 < 16 /*0x10*/; ++index3)
          str = $"{str}{words[index + 49 + index3]} ";
        this.InsertLine(str);
      }
      if (wordCount >= index + 66)
      {
        string str = "rdr_stat: ";
        for (int index4 = 0; index4 < 8; ++index4)
          str = $"{str}{words[index + 65 + index4]} ";
        this.InsertLine(str);
      }
      if (wordCount >= index + 74)
        this.InsertLine("nHwType", ": ", this.ValueFromStrings(words[index + 73]));
      if (wordCount >= index + 75)
        this.InsertLine("nHwRev", ": ", this.ValueFromStrings(words[index + 74]));
      if (wordCount >= index + 76)
        this.InsertLine("nProdType", ": ", this.ValueFromStrings(words[index + 75]));
      if (wordCount >= index + 77)
        this.InsertLine("nProdVer", ": ", this.ValueFromStrings(words[index + 76]));
      if (wordCount >= index + 79)
      {
        int num2 = this.ValueFromStrings(words[index + 77], words[index + 78]);
        this.InsertLine($"nFirmwareBoot: {(object) ((num2 & 61440 /*0xF000*/) >> 12)}.{(object) ((num2 & 4080) >> 4)}.{(object) (num2 & 15)}");
      }
      if (wordCount >= index + 81)
      {
        int num3 = this.ValueFromStrings(words[index + 79], words[index + 80 /*0x50*/]);
        this.InsertLine($"nFirmwareLdr: {(object) ((num3 & 61440 /*0xF000*/) >> 12)}.{(object) ((num3 & 4080) >> 4)}.{(object) (num3 & 15)}");
      }
      if (wordCount >= index + 83)
      {
        int num4 = this.ValueFromStrings(words[index + 81], words[index + 82]);
        this.InsertLine($"nFirmwareApp: {(object) ((num4 & 61440 /*0xF000*/) >> 12)}.{(object) ((num4 & 4080) >> 4)}.{(object) (num4 & 15)}");
      }
      if (wordCount >= index + 85)
        this.InsertLine("nOemCode", ": ", this.ParseOEMCodes(this.ValueFromStrings(words[index + 83], words[index + 84])));
      if (wordCount >= index + 86)
        this.InsertLine("nEncConfig", ": ", this.ValueFromStrings(words[index + 85]));
      if (wordCount >= index + 87)
        this.InsertLine("nEncKeyStatus", ": ", this.ValueFromStrings(words[index + 86]));
      if (wordCount >= index + 93)
        this.InsertLine($"mac_addr: {words[index + 92]}:{words[index + 91]}:{words[index + 90]}:{words[index + 89]}:{words[index + 88]}:{words[index + 87]}");
      index += 93;
      this.InsertLine("");
      this.InsertLine("");
    }
  }

  private void InsertLine(string labelStr, string separatorStr, int val)
  {
    this.InsertLine(labelStr + separatorStr + (object) val);
  }

  private void InsertLine(string labelStr, string separatorStr, int val, int formatBase)
  {
    this.InsertLine(formatBase == 10 || formatBase != 16 /*0x10*/ ? labelStr + separatorStr + (object) val : $"{labelStr}{separatorStr}{val:X4}");
  }

  private int ValueFromStrings(string str1) => this.ValueFromStrings(str1, "0", "0", "0");

  private int ValueFromStrings(string str1, string str2)
  {
    return this.ValueFromStrings(str1, str2, "0", "0");
  }

  private int ValueFromStrings(string str1, string str2, string str3)
  {
    return this.ValueFromStrings(str1, str2, str3, "0");
  }

  private int ValueFromStrings(string str1, string str2, string str3, string str4)
  {
    return (int.Parse(str1, NumberStyles.HexNumber) & (int) byte.MaxValue) + ((int.Parse(str2, NumberStyles.HexNumber) & (int) byte.MaxValue) << 8) + ((int.Parse(str3, NumberStyles.HexNumber) & (int) byte.MaxValue) << 16 /*0x10*/) + ((int.Parse(str4, NumberStyles.HexNumber) & (int) byte.MaxValue) << 24);
  }

  private void ParseHardwareID(string str, int deviceID)
  {
    int num = int.Parse(str, NumberStyles.HexNumber);
    switch (num)
    {
      case 0:
        this.InsertLine("Controller: 0 - SCP");
        break;
      case 1:
        if (deviceID == 30)
        {
          this.InsertLine("Controller: 1 - PW3K");
          break;
        }
        if (deviceID == 25)
        {
          this.InsertLine("Controller: 1 - PRO2200");
          break;
        }
        this.InsertLine("Controller: 1 - SCPc");
        break;
      case 2:
        if (deviceID == 30)
        {
          this.InsertLine("Controller: 2 - PW3K AES");
          break;
        }
        if (deviceID == 25)
        {
          this.InsertLine("Controller: 2 - PRO2200 AES");
          break;
        }
        this.InsertLine("Controller: 2 - SCPe");
        break;
      case 3:
        this.InsertLine("Controller: 3 - SCP (AES)");
        break;
      case 4:
        this.InsertLine("Controller: 4 - SCPc (AES)");
        break;
      case 5:
        if (deviceID == 32 /*0x20*/)
        {
          this.InsertLine("Controller: 5 - PW5K");
          break;
        }
        this.InsertLine("Controller: 5 - SCPe (AES)");
        break;
      case 6:
        this.InsertLine("Controller: 6 - SCPap");
        break;
      case 7:
        if (deviceID == 32 /*0x20*/)
        {
          this.InsertLine("Controller: 7 - PW5K AES");
          break;
        }
        if (deviceID == 3)
        {
          this.InsertLine("Controller: 7 - LP2500");
          break;
        }
        this.InsertLine("Controller: 7 - EP2500");
        break;
      case 8:
        if (deviceID == 40)
        {
          this.InsertLine("Controller: 8 - PW6K");
          break;
        }
        if (deviceID == 3)
        {
          this.InsertLine("Controller: 8 - LP1502");
          break;
        }
        this.InsertLine("Controller: 8 - EP1502");
        break;
      case 9:
        if (deviceID == 3)
        {
          this.InsertLine("Controller: 9 - LP1501");
          break;
        }
        this.InsertLine("Controller: 9 - EP1501");
        break;
      case 10:
        this.InsertLine("Controller: 10 - MPL_ICD");
        break;
      case 11:
        this.InsertLine("Controller: 11 - EP1501-PIM");
        break;
      case 12:
        if (deviceID != 41)
          break;
        this.InsertLine("Controller: 12 - PRO32IC");
        break;
      case 13:
        this.InsertLine("Controller: 13 - M5-IC");
        break;
      case 15:
        this.InsertLine("Controller: 15 - Keri NXT - 2 Door");
        break;
      case 16 /*0x10*/:
        this.InsertLine("Controller: 16 - Keri NXT - 4 Door");
        break;
      case 17:
        this.InsertLine("Controller: 17 - MI-RS4");
        break;
      case 18:
        this.InsertLine("Controller: 18 - MI-XL16");
        break;
      case 19:
        if (deviceID == 3)
        {
          this.InsertLine("Controller: 19 - LP4502");
          break;
        }
        this.InsertLine("Controller: 19 - EP4502");
        break;
      case 20:
        this.InsertLine("Controller: 20 - Keri NXT - 1 Door");
        break;
      case 21:
        this.InsertLine("Controller: 21 - RK-ARM");
        break;
      case 22:
        this.InsertLine("Controller: 22 - MS-ICS");
        break;
      case 23:
        this.InsertLine("Controller: 23 - Aliro AP");
        break;
      case 24:
        this.InsertLine("Controller: 24 - Schneider Electric SSC");
        break;
      case 25:
        this.InsertLine("Controller: 25 - Schneider Electric ACX-4");
        break;
      case 26:
        this.InsertLine("Controller: 26 - Schneider Electric ACX-8");
        break;
      default:
        this.textColor = this.errorFoundTextColor;
        this.InsertLine($"Controller: {(object) num} - UNKNOWN Type");
        break;
    }
  }

  private void ParseFirmwareVersion(string majorStr, string minor1Str, string minor2Str)
  {
    this.InsertLine($"Firmware: {(object) int.Parse(majorStr, NumberStyles.HexNumber)}.{(object) int.Parse(minor1Str, NumberStyles.HexNumber)}.{(object) int.Parse(minor2Str, NumberStyles.HexNumber)}");
  }







  public enum ProductFamily
  {
    Mercury,
    Honeywell,
    Aero,
  }

  public class MercuryException : Exception
  {
    public MercuryException()
    {
    }

    public MercuryException(string message)
      : base(message)
    {
    }

    public MercuryException(string message, string HelpLink)
      : base(message)
    {
      this.HelpLink = HelpLink;
    }
  }
}
