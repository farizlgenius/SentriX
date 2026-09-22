using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Host.Helpers
{
    public class AeroCommandDecoder
    {
        #region Constants & Limits

        public const int maximumNumOfSIOs = 96;
        public const int maximumNumOfACRs = 128;
        public const int maximumNumOfACRsPrec = 64;
        public const int maximumNumOfCPs = 2048;
        public const int maximumNumOfMPs = 2048;
        public const int maximumNumOfAccessLevels = 32000;
        public const int maximumNumOfTrgrs = 8196;
        public const int maximumNumOfProcs = 8196;
        public const int maximumNumOfTZs = 255;
        public const int maximumNumOfHolidays = 255;
        public const int maximumNumOfMPGs = 128;
        public const int maximumNumOfElAlvl = 256;
        public const int maximumNumOfFloors = 128;
        public const int maximumNumOfTVs = 128;
        public const int maxScpID = 16383;
        public const int maxSioNumberReaders = 16;
        public const int maximumNumIPSGroups = 256;
        public const int maximumBioTemplateSize = 1024;
        public const int maximumApbAreas = 128;
        public const int maximumNumAssetGroups = 65535;
        public const int maximumOperatingModes = 8;

        public static readonly List<string> ControllerModelList = new List<string>
        {
            "scp2", "pw3k", "pro2200", "scpc", "pw3Kaes", "pro2200aes", "scpe",
            "scp2aes", "scpcaes", "pw5k", "scpeaes", "pw5kaes", "ep2500", "lp2500",
            "pw6k", "ep1502", "lp1502", "ep1501", "lp1501", "mpl_icd", "pim4001501",
            "pro32ic", "m5-ic", "kerinxt2", "kerinxt4", "mirs4", "mixl16", "ep4502",
            "lp4502", "kerinxt1", "rkarm", "msics", "aliro", "ssc", "acx4", "acx8", "x1100"
        };

        #endregion

        public enum ProductFamily
        {
            Mercury = 0,
            Honeywell = 1,
            Aero = 2
        }

        private int productFamily = 2;
        private bool processCommandOnly = true;
        private readonly List<Dictionary<int, Action<string[], int, bool, StringBuilder>>> commandMapList;
        private Dictionary<int, Action<string[], int, bool, StringBuilder>> activeCommandMap;

        public AeroCommandDecoder(ProductFamily family = ProductFamily.Aero)
        {
            this.productFamily = (int)family;
            this.commandMapList = new List<Dictionary<int, Action<string[], int, bool, StringBuilder>>>();
            this.loadCommandMap();
        }

        public void SetProductFamily(ProductFamily family)
        {
            this.productFamily = (int)family;
            if (this.commandMapList.Count > this.productFamily)
            {
                this.activeCommandMap = this.commandMapList[this.productFamily];
            }
        }

        /// <summary>
        /// Accepts an ASCII command string and returns the parsed output as string.
        /// </summary>
        public string DecodeCommand(string inputRaw)
        {
            if (string.IsNullOrWhiteSpace(inputRaw))
                return "Invalid input string\n";

            StringBuilder sb = new StringBuilder();
            string input = inputRaw.Trim();
            string text = RemoveDuplicateWhiteSpace(input);
            int length = text.Length;
            char[] separator = new char[1] { ' ' };
            string[] strArray = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            int num = strArray.Length;

            if (num == 1 && strArray[0].Length >= 2 && (strArray[0][0] == '5' && strArray[0][1] == 'A' || strArray[0][0] == '5' && strArray[0][1] == '3') && strArray[0] != "5304")
            {
                string str2 = "";
                for (int index = 0; index < length - 1; index += 2)
                {
                    str2 += $"{strArray[0][index]}{strArray[0][index + 1]} ";
                }
                strArray = str2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                num = strArray.Length;
            }

            if (num > 0)
            {
                try
                {
                    ProcessCommands(strArray, sb);
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"Error processing command: {ex.Message}");
                }
            }
            else
            {
                sb.AppendLine("Invalid input string");
            }

            return sb.ToString();
        }

        private void ProcessCommands(string[] words, StringBuilder sb)
        {
            try
            {
                this.processCommandOnly = true;
                int num1 = words.Length;
                if (num1 >= 3)
                {
                    if (words[2].Contains("enCc") || words[2].Contains("enNCc"))
                        this.processCommandOnly = false;
                }
                if (!this.processCommandOnly && num1 < 4 || this.processCommandOnly && num1 < 1)
                {
                    sb.AppendLine("Invalid input string");
                }
                else
                {
                    int cmdIndex = GetCmdIndex();
                    string word = words[cmdIndex];
                    try
                    {
                        int cmdKey = int.Parse(words[cmdIndex]);
                        if (this.activeCommandMap.ContainsKey(cmdKey))
                        {
                            this.activeCommandMap[cmdKey](words, num1, this.productFamily == 1, sb);
                        }
                        else
                        {
                            sb.AppendLine($"Command [{word}] Not Implemented for selected dialect.");
                        }
                    }
                    catch (KeyNotFoundException ex)
                    {
                        sb.AppendLine($"Command [{word}] Not Implemented! Exception Info: {ex.GetType()}");
                    }
                    catch (FormatException ex)
                    {
                        sb.AppendLine($"Parsing Error ({ex.GetType()}): {ex.Message} - Verify Command Parameters");
                    }
                    catch (Exception ex)
                    {
                        sb.AppendLine($"Unknown Error: {ex.GetType()}: {ex.Message}");
                    }
                }
            }
            catch (FormatException ex)
            {
                sb.AppendLine($"Error parsing input string: {ex.GetType()}: {ex.Message}");
            }
        }

        private int GetCmdIndex() => this.processCommandOnly ? 0 : 3;

        #region Helper Formatting Functions

        private void InsertLine(StringBuilder sb, string str)
        {
            sb.AppendLine(str);
        }

        private void InsertLine(StringBuilder sb, string labelStr, string separatorStr, string word)
        {
            if (!string.IsNullOrEmpty(word))
                sb.AppendLine(labelStr + separatorStr + word);
        }

        private void InsertLine(StringBuilder sb, string labelStr, string separatorStr, string[] words, int index)
        {
            if (index >= words.Length) return;
            sb.AppendLine(labelStr + separatorStr + words[index]);
        }

        private void InsertLineValidateRange(StringBuilder sb, string labelStr, string separatorStr, string[] words, int index, int minVal, int maxVal)
        {
            if (index >= words.Length) return;
            try
            {
                int num = int.Parse(words[index]);
                string str = labelStr + separatorStr + words[index];
                if (num < minVal || num > maxVal)
                    str += " [INVALID RANGE]";
                sb.AppendLine(str);
            }
            catch
            {
                sb.AppendLine($"{labelStr}{separatorStr}Invalid Parameter");
            }
        }

        private void InsertLine(StringBuilder sb, string labelStr, string separatorStr, string[] words, int index, int count)
        {
            if (index >= words.Length) return;
            string str = labelStr + separatorStr;
            for (int i = 0; i < count; ++i)
            {
                if (index + i < words.Length)
                    str += $"{words[index + i]} ";
            }
            sb.AppendLine(str);
        }

        private void InsertLineValidateRange(StringBuilder sb, string labelStr, string separatorStr, string[] words, int index, int count, int minVal, int maxVal)
        {
            if (index >= words.Length) return;
            string str = labelStr + separatorStr;
            for (int i = 0; i < count; ++i)
            {
                if (index + i < words.Length)
                {
                    int num = int.Parse(words[index + i]);
                    string err = (num < minVal || num > maxVal) ? "!" : "";
                    str += $"{words[index + i]}{err} ";
                }
            }
            sb.AppendLine(str);
        }

        private int InsertLineWithText(StringBuilder sb, string labelStr, string separatorStr, string[] words, int index)
        {
            int num1 = 0;
            string str = labelStr + separatorStr;
            bool flag = true;
            int num2 = 0;
            while (flag)
            {
                if (index + num1 < words.Length)
                {
                    str = str + words[index + num1] + " ";
                    int num3 = words[index + num1].Split('"').Length - 1;
                    num2 += num3;
                    ++num1;
                    if (num2 >= 2) flag = false;
                }
                else flag = false;
            }
            sb.AppendLine(str);
            return num1;
        }

        public string RemoveDuplicateWhiteSpace(string input)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string[] strArray = input.Split(new char[] { ' ', '\n', '\t', '\r', '\f', '\v' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in strArray)
                stringBuilder.AppendFormat("{0} ", item);
            return stringBuilder.ToString();
        }

        #endregion

        #region Command Map Setup

        private bool loadCommandMap()
        {
            var mapMercury = new Dictionary<int, Action<string[], int, bool, StringBuilder>>();
            var mapHoneywell = new Dictionary<int, Action<string[], int, bool, StringBuilder>>();
            var mapAero = new Dictionary<int, Action<string[], int, bool, StringBuilder>>();

            // Register Mercury Map (Family 0)
            mapMercury[1] = (w, n, h, sb) => ParseEnCcBatch(w, n, h, sb);
            mapMercury[11] = (w, n, h, sb) => ParseEnCcSystem(w, n, h, sb);
            mapMercury[12] = (w, n, h, sb) => ParseEnCcCreateChannel(w, n, h, sb);
            mapMercury[13] = (w, n, h, sb) => ParseEnCcCreateScp(w, n, h, sb);
            mapMercury[14] = (w, n, h, sb) => ParseEnCcDeleteChannel(w, n, h, sb);
            mapMercury[15] = (w, n, h, sb) => ParseEnCcDeleteScp(w, n, h, sb);
            mapMercury[16] = (w, n, h, sb) => ParseEnCcDaylight(w, n, h, sb);
            mapMercury[17] = (w, n, h, sb) => ParseEnCcPeerCertificate(w, n, h, sb);
            mapMercury[18] = (w, n, h, sb) => ParseEnCcIpClientDefaults(w, n, h, sb);
            mapMercury[19] = (w, n, h, sb) => ParseEnCcIpClientAssignment(w, n, h, sb);
            mapMercury[101] = (w, n, h, sb) => ParseEnCcIcvt(w, n, h, sb);
            mapMercury[102] = (w, n, h, sb) => ParseEnCcCfmt(w, n, h, sb);
            mapMercury[103] = (w, n, h, sb) => ParseEnCcTimezone(w, n, h, sb);
            mapMercury[104] = (w, n, h, sb) => ParseEnCcHoliday(w, n, h, sb);
            mapMercury[105] = (w, n, h, sb) => ParseEnCcAdbSpec(w, n, h, sb);
            mapMercury[106] = (w, n, h, sb) => ParseEnCcAdbCardFile(w, n, h, sb);
            mapMercury[107] = (w, n, h, sb) => ParseEnCcScp(w, n, h, sb);
            mapMercury[108] = (w, n, h, sb) => ParseEnCcMsp1(w, n, h, sb);
            mapMercury[109] = (w, n, h, sb) => ParseEnCcSio(w, n, h, sb);
            mapMercury[110] = (w, n, h, sb) => ParseEnCcInput(w, n, h, sb);
            mapMercury[111] = (w, n, h, sb) => ParseEnCcOutput(w, n, h, sb);
            mapMercury[112] = (w, n, h, sb) => ParseEnCcReader(w, n, h, sb);
            mapMercury[113] = (w, n, h, sb) => ParseEnCcMP(w, n, h, sb);
            mapMercury[114] = (w, n, h, sb) => ParseEnCcCP(w, n, h, sb);
            mapMercury[115] = (w, n, h, sb) => ParseEnCcACR(w, n, h, sb);
            mapMercury[116] = (w, n, h, sb) => ParseEnCcAlvl(w, n, h, sb);
            mapMercury[117] = (w, n, h, sb) => ParseEnCcTrgr(w, n, h, sb);
            mapMercury[118] = (w, n, h, sb) => ParseEnCcProc(w, n, h, sb);
            mapMercury[119] = (w, n, h, sb) => ParseEnCcActnRem(w, n, h, sb);
            mapMercury[120] = (w, n, h, sb) => ParseEnCcMpg(w, n, h, sb);
            mapMercury[121] = (w, n, h, sb) => ParseEnCcArea(w, n, h, sb);
            mapMercury[122] = (w, n, h, sb) => ParseEnCcRledSpc(w, n, h, sb);
            mapMercury[123] = (w, n, h, sb) => ParseEnCcRTxtSpc(w, n, h, sb);
            mapMercury[124] = (w, n, h, sb) => ParseEnCcAlvlSpc(w, n, h, sb);
            mapMercury[125] = (w, n, h, sb) => ParseEnCcPointName(w, n, h, sb);
            mapMercury[127] = (w, n, h, sb) => ParseEnCcSioAesControl(w, n, h, sb);
            mapMercury[128] = (w, n, h, sb) => ParseEnCcSioNetwork(w, n, h, sb);
            mapMercury[129] = (w, n, h, sb) => ParseEnCcLanguageCodePages(w, n, h, sb);
            mapMercury[130] = (w, n, h, sb) => ParseEnCcAcrExtendedLedDefault(w, n, h, sb);
            mapMercury[151] = (w, n, h, sb) => ParseEnCcAcrLogDeny(w, n, h, sb);
            mapMercury[203] = (w, n, h, sb) => ParseEnScpDown(w, n, h, sb);
            mapMercury[206] = (w, n, h, sb) => ParseEnCcFirmwareDown(w, n, h, sb);
            mapMercury[207] = (w, n, h, sb) => ParseEnCcAttachScp(w, n, h, sb);
            mapMercury[208] = (w, n, h, sb) => ParseEnCcDetachScp(w, n, h, sb);
            mapMercury[209] = (w, n, h, sb) => ParseEnCcConfigSave(w, n, h, sb);
            mapMercury[210] = (w, n, h, sb) => ParseEnCcConfigDelta(w, n, h, sb);
            mapMercury[211] = (w, n, h, sb) => ParseEnCcDualPortControl(w, n, h, sb);
            mapMercury[212] = (w, n, h, sb) => ParseEnCcAesControl(w, n, h, sb);
            mapMercury[213] = (w, n, h, sb) => ParseEnCcAesTest(w, n, h, sb);
            mapMercury[214] = (w, n, h, sb) => ParseEnCcPollMode(w, n, h, sb);
            mapMercury[215] = (w, n, h, sb) => ParseEnCcFileDownload(w, n, h, sb);
            mapMercury[217] = (w, n, h, sb) => ParseEnCcHexOutInternal(w, n, h, sb);
            mapMercury[218] = (w, n, h, sb) => ParseEnCcDeleteFile(w, n, h, sb);
            mapMercury[219] = (w, n, h, sb) => ParseEnCcGetFileInfo(w, n, h, sb);
            mapMercury[301] = (w, n, h, sb) => ParseSimpleCommand(w, n, h, sb);
            mapMercury[302] = (w, n, h, sb) => ParseEnCcTime(w, n, h, sb);
            mapMercury[303] = (w, n, h, sb) => ParseEnCcTranIndex(w, n, h, sb);
            mapMercury[304] = (w, n, h, sb) => ParseEnCcAdbCard304(w, n, h, sb);
            mapMercury[305] = (w, n, h, sb) => ParseEnCcCardDelete(w, n, h, sb);
            mapMercury[306] = (w, n, h, sb) => ParseEnCcMpMask(w, n, h, sb);
            mapMercury[307] = (w, n, h, sb) => ParseEnCcCpCtl(w, n, h, sb);
            mapMercury[308] = (w, n, h, sb) => ParseEnCcAcrMode(w, n, h, sb);
            mapMercury[309] = (w, n, h, sb) => ParseEnCcForcedOpenMask(w, n, h, sb);
            mapMercury[310] = (w, n, h, sb) => ParseEnCcHeldOpenMask(w, n, h, sb);
            mapMercury[311] = (w, n, h, sb) => ParseEnCcUnlock(w, n, h, sb);
            mapMercury[312] = (w, n, h, sb) => ParseEnCcProcedure(w, n, h, sb);
            mapMercury[313] = (w, n, h, sb) => ParseEnCcTVCommand(w, n, h, sb);
            mapMercury[314] = (w, n, h, sb) => ParseEnCcTzCommand(w, n, h, sb);
            mapMercury[315] = (w, n, h, sb) => ParseEnCcAcrLedMode(w, n, h, sb);
            mapMercury[316] = (w, n, h, sb) => ParseEnCcOemCode(w, n, h, sb);
            mapMercury[317] = (w, n, h, sb) => ParseEnCcPassword(w, n, h, sb);
            mapMercury[318] = (w, n, h, sb) => ParseEnCcScpID(w, n, h, sb);
            mapMercury[319] = (w, n, h, sb) => ParseEnCcApbFreePass(w, n, h, sb);
            mapMercury[320] = (w, n, h, sb) => ParseEnCcHexOut(w, n, h, sb);
            mapMercury[321] = (w, n, h, sb) => ParseEnCcMpgSet(w, n, h, sb);
            mapMercury[322] = (w, n, h, sb) => ParseEnCcAreaSet(w, n, h, sb);
            mapMercury[323] = (w, n, h, sb) => ParseEnCcUseLimit(w, n, h, sb);
            mapMercury[324] = (w, n, h, sb) => ParseEnCcScpOfflineTime(w, n, h, sb);
            mapMercury[325] = (w, n, h, sb) => ParseEnCcRLedTemp(w, n, h, sb);
            mapMercury[326] = (w, n, h, sb) => ParseEnCcLcdText(w, n, h, sb);
            mapMercury[327] = (w, n, h, sb) => ParseEnCcSioDc(w, n, h, sb);
            mapMercury[328] = (w, n, h, sb) => ParseEnCcSioHexLoad(w, n, h, sb);
            mapMercury[329] = (w, n, h, sb) => ParseEnCcHostResponse(w, n, h, sb);
            mapMercury[330] = (w, n, h, sb) => ParseEnCcNvArgSet(w, n, h, sb);
            mapMercury[331] = (w, n, h, sb) => ParseEnCcCardSim(w, n, h, sb);
            mapMercury[332] = (w, n, h, sb) => ParseEnCcIpsSet(w, n, h, sb);
            mapMercury[333] = (w, n, h, sb) => ParseEnCcDiag(w, n, h, sb);
            mapMercury[334] = (w, n, h, sb) => ParseEnCcTempAcrMode(w, n, h, sb);
            mapMercury[335] = (w, n, h, sb) => ParseEnCcOperatingMode(w, n, h, sb);
            mapMercury[336] = (w, n, h, sb) => ParseEnCcSioRdrHexLoad(w, n, h, sb);
            mapMercury[337] = (w, n, h, sb) => ParseEnCcAcrOsdpPassthrough(w, n, h, sb);
            mapMercury[338] = (w, n, h, sb) => ParseEnCcAcrOfflineAccessList(w, n, h, sb);
            mapMercury[339] = (w, n, h, sb) => ParseEnCcKeySim(w, n, h, sb);
            mapMercury[340] = (w, n, h, sb) => ParseEnCcOsdpReaderTransfer(w, n, h, sb);
            mapMercury[341] = (w, n, h, sb) => ParseEnCcControlReboot(w, n, h, sb);

            this.commandMapList.Insert(0, mapMercury);

            // Honeywell & Aero Maps
            this.commandMapList.Insert(1, mapHoneywell);
            this.commandMapList.Insert(2, mapAero);

            // Populate Aero Map defaults to match Mercury + Aero Overrides
            foreach (var kvp in mapMercury) mapAero[kvp.Key] = kvp.Value;
            foreach (var kvp in mapMercury) mapHoneywell[kvp.Key] = kvp.Value;

            this.activeCommandMap = this.commandMapList[this.productFamily];
            return false;
        }

        #endregion

        #region Parsers (100% Ported Logic)

        private void ParseEnCcBatch(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcBatch (0011)" : "enCcBatch (0001)");
            InsertLine(sb, "cfilename[128]", ": ", words, index);
        }

        private void ParseEnCcSystem(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int num1 = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcSystem (1001)" : "enCcSystem (011)");
            InsertLine(sb, "nPorts", ": ", words, num1);
            InsertLine(sb, "nScps", ": ", words, num1 + 1);
            InsertLineValidateRange(sb, "nTimezones", ": ", words, num1 + 2, 0, 255);
            InsertLine(sb, "nHolidays", ": ", words, num1 + 3);
            InsertLine(sb, "bDirectMode", ": ", words, num1 + 4);
            InsertLine(sb, "debug_rq", ": ", ParseDebugRq(words, num1 + 5));
            InsertLine(sb, "nDebugArg[4]", ": ", words, num1 + 6, 4);
        }

        private void ParseEnCcCreateChannel(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int num1 = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcCreateChannel (1001)" : "enCcCreateChannel (0012)");
            InsertLine(sb, "nChannelId", ": ", words, num1);
            int cType = int.Parse(words[num1 + 1]);
            InsertLine(sb, "cType", ": ", ParseCType(words, num1 + 1, true));
            InsertLine(sb, "cPort", ": ", ParseCPort(words, num1 + 2, cType));
            InsertLine(sb, "baud_rate", ": ", words, num1 + 3);
            InsertLine(sb, "timer1 (SCP reply timeout)", ": ", words, num1 + 4);
            InsertLine(sb, "timer2 (TCP/IP retry connect interval)", ": ", words, num1 + 5);
            InsertLine(sb, "cModemId[64]", ": ", words, num1 + 6);
            InsertLine(sb, "cRTSMode", ": ", ParseRtsMode(words, num1 + 7));
        }

        private void ParseEnCcCreateScp(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int num1 = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcCreateScpLn (2021)" : "enCcCreateScpLn (1013)");
            InsertLineValidateRange(sb, "nSCPId", ": ", words, num1, 0, 16383);
            InsertLine(sb, "address", ": ", ParseScpAddress(words, num1 + 1));
            InsertLine(sb, "nCommAccess", ": ", ParseCType(words, num1 + 2, false));
            InsertLine(sb, "e_max", ": ", words, num1 + 3);
            InsertLineValidateRange(sb, "poll_delay", ": ", words, num1 + 4, 0, 5000);
            InsertLine(sb, "cCommString[32]", ": ", words, num1 + 5);
            InsertLine(sb, "cPswdString[16]", ": ", words, num1 + 6);
            InsertLine(sb, "offline_time", ": ", words, num1 + 7);
        }

        private void ParseEnCcDeleteChannel(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            InsertLine(sb, honeywell ? "enNCcDeleteChannel (1012)" : "enCcDeleteChannel (0014)");
            InsertLine(sb, "nChannelId", ": ", words, GetCmdIndex() + 1);
        }

        private void ParseEnCcDeleteScp(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            InsertLine(sb, honeywell ? "enNCcDeleteScp (1022)" : "enCcDeleteScp (0015)");
            InsertLineValidateRange(sb, "nSCPid", ": ", words, GetCmdIndex() + 1, 0, 16383);
        }

        private void ParseEnCcDaylight(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, "enCcDaylight (0016)");
            InsertLine(sb, "nDstID", ": ", words, index);
            int nYear = int.Parse(words[index + 1]);
            int minVal = (nYear == 0) ? 0 : 1;
            InsertLine(sb, (nYear == 0) ? "nSYear: 0 - Clear All" : "nSYear", ": ", words, index + 1);
            InsertLineValidateRange(sb, "nSMonth", ": ", words, index + 2, minVal, 12);
            InsertLineValidateRange(sb, "nSDay", ": ", words, index + 3, minVal, 32);
            InsertLineValidateRange(sb, "nSHh", ": ", words, index + 4, 0, 23);
            InsertLineValidateRange(sb, "nSMm", ": ", words, index + 5, 0, 59);
            InsertLineValidateRange(sb, "nSSs", ": ", words, index + 6, 0, 59);
            InsertLine(sb, "nEYear", ": ", words, index + 7);
            InsertLineValidateRange(sb, "nEMonth", ": ", words, index + 8, minVal, 12);
            InsertLineValidateRange(sb, "nEDay", ": ", words, index + 9, minVal, 32);
            InsertLineValidateRange(sb, "nEHh", ": ", words, index + 10, 0, 23);
            InsertLineValidateRange(sb, "nEMm", ": ", words, index + 11, 0, 59);
            InsertLineValidateRange(sb, "nESs", ": ", words, index + 12, 0, 59);
        }

        private void ParseEnCcPeerCertificate(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, "enCcPeerCertificate (0017)");
            InsertLine(sb, "nEnable", ": ", words, index);
            InsertLine(sb, "cIssuer", ": ", words, index + 1);
        }

        private void ParseEnCcIpClientDefaults(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcIpClientDefaults (0018)" : "enCcIpClientDefaults (0018)");
            InsertLine(sb, "e_max", ": ", words, index);
            InsertLine(sb, "poll_delay", ": ", words, index + 1);
            InsertLine(sb, "offline_time", ": ", words, index + 2);
            InsertLine(sb, "provisioning_time", ": ", words, index + 3);
        }

        private void ParseEnCcIpClientAssignment(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcIpClientIDAassignment (0019)" : "enCcIpClientIDAassignment (0019)");
            InsertLine(sb, "assignmentFlags", ": ", ParseIpClientAssignFlags(words, index));
            InsertLine(sb, "lowScpID", ": ", words, index + 1);
            InsertLine(sb, "highScpID", ": ", words, index + 2);
        }

        private void ParseEnCcIcvt(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, "enCcIcvt (0101)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLine(sb, "number", ": ", words, index + 1);
            InsertLineValidateRange(sb, "pri_5pr", ": ", words, index + 2, 0, 2);
            InsertLineValidateRange(sb, "sts_5pr", ": ", words, index + 3, 0, 6);
        }

        private void ParseEnCcCfmt(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcCfmt (NA)" : "enCcCfmt (0102)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "number", ": ", words, index + 1, 0, 15);
            InsertLine(sb, "facility", ": ", words, index + 2);
            InsertLine(sb, "offset", ": ", words, index + 3);
            InsertLine(sb, "function_id", ": ", ParseCardFormatterFunction(words, index + 4));
        }

        private void ParseEnCcTimezone(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, "enCcScpTimezone (0103)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "number", ": ", words, index + 1, 0, 255);
            InsertLine(sb, "mode", ": ", ParseTimezoneMode(words, index + 2));
            int numIntervals = int.Parse(words[index + 3]);
            InsertLineValidateRange(sb, "intervals", ": ", words, index + 3, 0, 12);
            int curIndex = index + 4;
            for (int i = 0; i < Math.Min(numIntervals, 12); ++i)
            {
                InsertLine(sb, $"Interval: {i + 1}");
                InsertLine(sb, "i_days", ": ", ParseDayMask(words, curIndex));
                InsertLine(sb, "i_start", ": ", ParseStartEndTime(words, curIndex + 1));
                InsertLine(sb, "i_end", ": ", ParseStartEndTime(words, curIndex + 2));
                curIndex += 3;
            }
        }

        private void ParseEnCcHoliday(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, "enCcHoliday (0104)");
            InsertLine(sb, "lastModified", ": ", words, index);
            int year = int.Parse(words[index + 1]);
            InsertLine(sb, (year == 0) ? "year: 0 - Delete All Holidays" : "year", ": ", words, index + 1);
            int minVal = (year == 0) ? 0 : 1;
            InsertLineValidateRange(sb, "month", ": ", words, index + 2, minVal, 12);
            InsertLineValidateRange(sb, "day", ": ", words, index + 3, minVal, 31);
            InsertLine(sb, "extend", ": ", ParseHolidayExtendField(words, index + 4));
            InsertLine(sb, "type_mask", ": ", ParseHolidayTypeMask(words, index + 5));
        }

        private void ParseEnCcAdbSpec(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcAdbSpec (NA)" : "enCcAdbSpec (0105)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLine(sb, "nCards", ": ", words, index + 1);
            InsertLineValidateRange(sb, "nAlvl", ": ", words, index + 2, 0, 128);
            InsertLine(sb, "nPinDigits", ": ", ParsePinDigits(words, index + 3));
        }

        private void ParseEnCcAdbCardFile(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcAdbCardFile (1109)" : "enCcAdbCardFile (0106)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
            InsertLineWithText(sb, "file_name[100]", ": ", words, index + 2);
        }

        private void ParseEnCcScp(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, "enCcScp (0107)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "number", ": ", words, index + 1, 0, 16383);
            InsertLine(sb, "ser_num_low", ": ", words, index + 2);
            InsertLine(sb, "ser_num_high", ": ", words, index + 3);
            InsertLine(sb, "rev_major", ": ", words, index + 4);
            InsertLine(sb, "rev_minor", ": ", words, index + 5);
        }

        private void ParseEnCcMsp1(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcMsp1 (1201)" : "enCcMsp1 (0108)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
            InsertLine(sb, "msp1_number", ": ", words, index + 2);
            InsertLine(sb, "port_number", ": ", words, index + 3);
            InsertLine(sb, "baud_rate", ": ", words, index + 4);
        }

        private void ParseEnCcSio(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcSio (1202)" : "enCcSio (0109)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
            InsertLineValidateRange(sb, "sio_number", ": ", words, index + 2, 0, 95);
            InsertLine(sb, "model", ": ", ParseSioModel(words, index + 3, false));
        }

        private void ParseEnCcInput(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcInput (1203)" : "enCcInput (0110)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
            InsertLineValidateRange(sb, "sio_number", ": ", words, index + 2, 0, 95);
            InsertLine(sb, "input", ": ", words, index + 3);
            InsertLine(sb, "icvt_num", ": ", ParseIcvtNum(words, index + 4));
        }

        private void ParseEnCcOutput(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcOutput (1204)" : "enCcOutput (0111)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
            InsertLineValidateRange(sb, "sio_number", ": ", words, index + 2, 0, 95);
            InsertLine(sb, "output", ": ", words, index + 3);
            InsertLine(sb, "mode", ": ", ParseOutputDriveMode(words, index + 4));
        }

        private void ParseEnCcReader(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcReader (1205)" : "enCcReader (0112)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
            InsertLineValidateRange(sb, "sio_number", ": ", words, index + 2, 0, 95);
            InsertLineValidateRange(sb, "reader", ": ", words, index + 3, 0, 15);
            InsertLine(sb, "dt_fmt", ": ", ParseDtFmt(words, index + 4));
            InsertLine(sb, "keypad_mode", ": ", ParseKeypadMode(words, index + 5));
            InsertLine(sb, "led_drive_mode", ": ", ParseLedDriveMode(words, index + 6));
            InsertLine(sb, "osdp_flags", ": ", ParseOsdpFlags(words, index + 7));
        }

        private void ParseEnCcMP(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcMP (1211)" : "enCcMP (0113)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
            InsertLineValidateRange(sb, "mp_number", ": ", words, index + 2, 0, 2047);
        }

        private void ParseEnCcCP(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcCP (1212)" : "enCcCP (0114)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
            InsertLineValidateRange(sb, "cp_number", ": ", words, index + 2, 0, 2047);
        }

        private void ParseEnCcACR(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcACR (1213)" : "enCcACR (0115)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
            InsertLineValidateRange(sb, "acr_number", ": ", words, index + 2, 0, 127);
            InsertLine(sb, "access_cfg", ": ", ParseAccessCfg(words, index + 3));
        }

        private void ParseEnCcAlvl(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcAlvl (1214)" : "enCcAlvl (0116)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
            InsertLineValidateRange(sb, "alvl_number", ": ", words, index + 2, 0, 31999);
        }

        private void ParseEnCcTrgr(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcTrgr (1215)" : "enCcTrgr (0117)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
            InsertLine(sb, "trgr_number", ": ", words, index + 2);
            InsertLine(sb, "command", ": ", ParseTriggerCommand(words, index + 3));
        }

        private void ParseEnCcProc(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcProc (1216)" : "enCcProc (0118)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
            InsertLineValidateRange(sb, "proc_number", ": ", words, index + 2, 0, 8195);
        }

        private void ParseEnCcActnRem(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcActnRem (1217)" : "enCcActnRem (0119)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
        }

        private void ParseEnCcMpg(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcMpg (1218)" : "enCcMpg (0120)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
            InsertLineValidateRange(sb, "mpg_number", ": ", words, index + 2, 0, 127);
        }

        private void ParseEnCcArea(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcArea (1219)" : "enCcArea (0121)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
            InsertLineValidateRange(sb, "area_number", ": ", words, index + 2, 0, 127);
        }

        private void ParseEnCcRledSpc(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcRledSpc (1106)" : "enCcRledSpc (0122)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
            InsertLineValidateRange(sb, "led_mode", ": ", words, index + 2, 1, 3);
            InsertLine(sb, "rled_id", ": ", ParseRledId(words, index + 3));
        }

        private void ParseEnCcRTxtSpc(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcRTxtSpc (1229)" : "enCcRTxtSpc (0123)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLine(sb, "nMaxLines", ": ", words, index + 1);
            InsertLine(sb, "nLineIndex", ": ", ParseRTxtLineIndex(words, index + 3));
        }

        private void ParseEnCcAlvlSpc(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcAlvlSpc (1230)" : "enCcAlvlSpc (0124)");
            InsertLine(sb, "nScpNumber", ": ", words, index);
            InsertLineValidateRange(sb, "nAlvlNumber", ": ", words, index + 1, 0, 31999);
        }

        private void ParseEnCcPointName(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcPointName (1126)" : "enCcPointName (0125)");
            InsertLineValidateRange(sb, "nScpId", ": ", words, index, 0, 16383);
            InsertLine(sb, "nUseNames", ": ", ParseUseNames(words, index + 1));
            InsertLine(sb, "nPointType", ": ", ParsePointType(words, index + 2));
        }

        private void ParseEnCcSioAesControl(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcSioAesControl (1227)" : "enCcSioAesControl (0127)");
            InsertLineValidateRange(sb, "nScpId", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "sio_number", ": ", words, index + 1, 0, 95);
            InsertLine(sb, "nAesCommand", ": ", ParseSioAesCommand(words, index + 2));
        }

        private void ParseEnCcSioNetwork(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcSioNetwork (1228)" : "enCcSioNetwork (0128)");
            InsertLineValidateRange(sb, "nScpId", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "sio_number", ": ", words, index + 1, 0, 95);
            InsertLine(sb, "cIpAddr[22]", ": ", words, index + 2);
            InsertLine(sb, "cMacAddr[18]", ": ", words, index + 3);
            InsertLine(sb, "nMode", ": ", ParseSioNetworkMode(words, index + 4));
        }

        private void ParseEnCcLanguageCodePages(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcLanguageCodePages (1233)" : "enCcLanguageCodePages (0129)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
        }

        private void ParseEnCcAcrExtendedLedDefault(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcAcrExtendedLedDefault (1234)" : "enCcAcrExtendedLedDefault (0130)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "acr_number", ": ", words, index + 1, 0, 127);
        }

        private void ParseEnCcAcrLogDeny(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcAcrLogDeny (1221)" : "enCcAcrLogDeny (0151)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "acr_number", ": ", words, index + 1, 0, 127);
            InsertLine(sb, "nLpdFlags", ": ", ParseLpdFlags(words, index + 2));
        }

        private void ParseEnScpDown(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            InsertLine(sb, honeywell ? "enNScpDown (1002)" : "enScpDown (0203)");
            InsertLine(sb, "dummy", ": ", words, GetCmdIndex() + 1);
        }

        private void ParseEnCcFirmwareDown(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcFirmwareDown (1025)" : "enCcFirmwareDown (0206)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineWithText(sb, "file_name[200]", ": ", words, index + 1);
        }

        private void ParseEnCcAttachScp(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcAttachScp (1023)" : "enCcAttachScp (0207)");
            InsertLineValidateRange(sb, "nScpId", ": ", words, index, 0, 16383);
            InsertLine(sb, "nChannelId", ": ", words, index + 1);
        }

        private void ParseEnCcDetachScp(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcDetachScp (1024)" : "enCcDetachScp (0208)");
            InsertLineValidateRange(sb, "nSCPId", ": ", words, index, 0, 16383);
            InsertLine(sb, "nChannelId", ": ", words, index + 1);
        }

        private void ParseEnCcConfigSave(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcConfigSave (NA)" : "enCcConfigSave (0209)");
            int consumed = InsertLineWithText(sb, "file_name[100]", ": ", words, index);
            int cur = index + consumed;
            InsertLine(sb, "nMode", ": ", ParseSaveMode(words, cur));
            InsertLineValidateRange(sb, "nScpId", ": ", words, cur + 1, 0, 16383);
        }

        private void ParseEnCcConfigDelta(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            InsertLine(sb, honeywell ? "enNCcConfigDelta (NA)" : "enCcConfigDelta (0210)");
            InsertLineWithText(sb, "file_name[100]", ": ", words, GetCmdIndex() + 1);
        }

        private void ParseEnCcDualPortControl(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcDualPortControl (1026)" : "enCcDualPortControl (0211)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLine(sb, "nHcpDriver", ": ", ParseHcpDriver(words, index + 1));
            InsertLine(sb, "command", ": ", ParseDualPortControlCommand(words, index + 2));
        }

        private void ParseEnCcAesControl(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcAesControl (1027)" : "enCcAesControl (0212)");
            InsertLineValidateRange(sb, "nScpId", ": ", words, index, 0, 16383);
            InsertLine(sb, "nAesCommand", ": ", ParseAESCmnd(words, index + 1));
        }

        private void ParseEnCcAesTest(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcAesTest (1028)" : "enCcAesTest (0213)");
            InsertLineValidateRange(sb, "nScpId", ": ", words, index, 0, 16383);
            InsertLine(sb, "nAesTestCode", ": ", ParseAESTestCode(words, index + 1));
        }

        private void ParseEnCcPollMode(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcPollMode (1029)" : "enCcPollMode (0214)");
            InsertLineValidateRange(sb, "nScpId", ": ", words, index, 0, 16383);
            InsertLine(sb, "nPollMode", ": ", ParsePollMode(words, index + 1));
        }

        private void ParseEnCcFileDownload(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcFileDownload (1030)" : "enCcFileDownload (0215)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLine(sb, "file_type", ": ", ParseFileType(words, index + 1));
            int consumed = InsertLineWithText(sb, "src_file[MAX_PATH]", ": ", words, index + 2);
            InsertLineWithText(sb, "dest_file[MAX_PATH]", ": ", words, index + 2 + consumed);
        }

        private void ParseEnCcHexOutInternal(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcHexOutInternal (1032)" : "enCcHexOutInternal (0217)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLine(sb, "data_len", ": ", words, index + 1);
            InsertLine(sb, "data[2048]", ": ", words, index + 2);
        }

        private void ParseEnCcDeleteFile(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcDeleteFile (1033)" : "enCcDeleteFile (0218)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLine(sb, "file_type", ": ", ParseFileType(words, index + 1));
            InsertLine(sb, "file_name[100]", ": ", words, index + 2);
        }

        private void ParseEnCcGetFileInfo(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcGetFileInfo (1034)" : "enCcGetFileInfo (0219)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLine(sb, "file_type", ": ", ParseFileType(words, index + 1));
        }

        private void ParseSimpleCommand(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int cmdIndex = GetCmdIndex();
            int numCode = int.Parse(words[cmdIndex]);
            if (honeywell)
            {
                switch (numCode)
                {
                    case 1301: InsertLine(sb, "enNCcReset (1301)"); break;
                    case 1401: InsertLine(sb, "enNCcIDRequest (1401)"); break;
                    case 1402: InsertLine(sb, "enNCcTranSrq (1402)"); break;
                    case 1410: InsertLine(sb, "enNCcUtagRequest (1410)"); break;
                    default: InsertLine(sb, "UNKNOWN"); break;
                }
            }
            else
            {
                switch (numCode)
                {
                    case 301: InsertLine(sb, "enCcReset (0301)"); break;
                    case 401: InsertLine(sb, "enCcIDRequest (0401)"); break;
                    case 402: InsertLine(sb, "enCcTranSrq (0402)"); break;
                    case 410: InsertLine(sb, "enCcUtagRequest (0410)"); break;
                    default: InsertLine(sb, "UNKNOWN"); break;
                }
            }
            InsertLineValidateRange(sb, "scp_number", ": ", words, cmdIndex + 1, 0, 16383);
        }

        private void ParseEnCcTime(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcTime (1302)" : "enCcTime (302)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            if (numWords == 3)
                InsertLineValidateRange(sb, "custom_time", ": ", words, index + 1, 0, int.MaxValue);
        }

        private void ParseEnCcTranIndex(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcTranIndex (1303)" : "enCcTranIndex (0303)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLine(sb, "tran_index", ": ", ParseTranIndex(words, index + 1));
        }

        private void ParseEnCcAdbCard304(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, "enCcAdbCard (304)");
            InsertLine(sb, "lastModified", ": ", words, index);
            InsertLineValidateRange(sb, "scp_number", ": ", words, index + 1, 0, 16383);
            InsertLine(sb, "flags", ": ", ParseAdbcFlags(words, index + 2));
            InsertLine(sb, "card_number", ": ", words, index + 3);
        }

        private void ParseEnCcCardDelete(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcCardDelete (NA)" : "enCcCardDelete (0305)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLine(sb, "cardholder_id", ": ", words, index + 1);
        }

        private void ParseEnCcMpMask(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcMpMask (1306)" : "enCcMpMask (0306)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "mp_number", ": ", words, index + 1, 0, 2047);
            InsertLine(sb, "set_clear", ": ", ParseSetClear(words, index + 2));
        }

        private void ParseEnCcCpCtl(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcCpCtl (1307)" : "enCcCpCtl (0307)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "cp_number", ": ", words, index + 1, 0, 2047);
            InsertLine(sb, "command", ": ", ParseCpCtlCommand(words, index + 2));
        }

        private void ParseEnCcAcrMode(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcAcrMode (1308)" : "enCcAcrMode (0308)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "acr_number", ": ", words, index + 1, 0, 127);
            InsertLine(sb, "acr_mode", ": ", ParseAcrMode(words, index + 2));
        }

        private void ParseEnCcForcedOpenMask(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcForcedOpenMask (1309)" : "enCcForcedOpenMask (0309)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "acr_number", ": ", words, index + 1, 0, 127);
            InsertLine(sb, "set_clear", ": ", ParseSetClear(words, index + 2));
        }

        private void ParseEnCcHeldOpenMask(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcHeldOpenMask (1310)" : "enCcHeldOpenMask (0310)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "acr_number", ": ", words, index + 1, 0, 127);
            InsertLine(sb, "set_clear", ": ", ParseSetClear(words, index + 2));
        }

        private void ParseEnCcUnlock(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcUnlock (1311)" : "enCcUnlock (0311)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "acr_number", ": ", words, index + 1, 0, 127);
        }

        private void ParseEnCcProcedure(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcProcedure (1312)" : "enCcProcedure (0312)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "proc_number", ": ", words, index + 1, 0, 8195);
            InsertLine(sb, "command", ": ", ParseProcedureCommand(words, index + 2));
        }

        private void ParseEnCcTVCommand(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcTVCommand (1313)" : "enCcTVCommand (0313)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "tv_number", ": ", words, index + 1, 1, 127);
            InsertLine(sb, "set_clear", ": ", ParseSetClear(words, index + 2));
        }

        private void ParseEnCcTzCommand(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcTzCommand (1314)" : "enCcTzCommand (0314)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "tz_number", ": ", words, index + 1, 0, 255);
            InsertLine(sb, "command", ": ", ParseTzCommand(words, index + 2));
        }

        private void ParseEnCcAcrLedMode(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcAcrLedMode (1315)" : "enCcAcrLedMode (0315)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "acr_number", ": ", words, index + 1, 0, 127);
            InsertLineValidateRange(sb, "led_mode", ": ", words, index + 2, 1, 3);
        }

        private void ParseEnCcOemCode(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcOemCode (1316)" : "enCcOemCode (0316)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLine(sb, "oem_code", ": ", ParseOEMCodes(words, index + 1));
        }

        private void ParseEnCcPassword(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcPassword (1317)" : "enCcPassword (0317)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineWithText(sb, "password[16]", ": ", words, index + 1);
        }

        private void ParseEnCcScpID(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcScpID (1318)" : "enCcScpID (0318)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "scp_id", ": ", words, index + 1, 0, 16383);
        }

        private void ParseEnCcApbFreePass(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcApbFreePass (NA)" : "enCcApbFreePass (0319)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLine(sb, "cardholder_id", ": ", words, index + 1);
            InsertLineValidateRange(sb, "apb_area", ": ", words, index + 2, 0, 127);
        }

        private void ParseEnCcHexOut(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcHexOut (1320)" : "enCcHexOut (0320)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLine(sb, "baud", ": ", words, index + 1);
            InsertLine(sb, "port", ": ", words, index + 2);
            InsertLine(sb, "channel", ": ", words, index + 3);
            InsertLineWithText(sb, "data[128+1]", ": ", words, index + 4);
        }

        private void ParseEnCcMpgSet(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcMpgSet (1321)" : "enCcMpgSet (0321)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "mpg_number", ": ", words, index + 1, 0, 127);
            InsertLine(sb, "command", ": ", ParseMpgSetCommand(words, index + 2));
        }

        private void ParseEnCcAreaSet(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcAreaSet (1322)" : "enCcAreaSet (0322)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "area_number", ": ", words, index + 1, 0, 127);
            InsertLine(sb, "command", ": ", ParseAreaSetCommand(words, index + 2));
        }

        private void ParseEnCcUseLimit(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcUseLimit (NA)" : "enCcUseLimit (0323)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLine(sb, "cardholder_id", ": ", words, index + 1);
            InsertLineValidateRange(sb, "new_limit", ": ", words, index + 2, -1, 255);
        }

        private void ParseEnCcScpOfflineTime(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcScpOffLineTime (1324)" : "enCcScpOffLineTime (0324)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "offline_time", ": ", words, index + 1, 2000, 32767);
        }

        private void ParseEnCcRLedTemp(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcRLedTemp  (1325)" : "enCcRLedTemp  (0325)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "acr_number", ": ", words, index + 1, 0, 127);
            InsertLine(sb, "color_on", ": ", ParseLEDColor(words, index + 2));
            InsertLine(sb, "color_off", ": ", ParseLEDColor(words, index + 3));
        }

        private void ParseEnCcLcdText(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcLcdText  (1326)" : "enCcLcdText  (0326)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "term_number", ": ", words, index + 1, 0, 127);
            InsertLine(sb, "type", ": ", ParseLcdTextType(words, index + 2));
            InsertLineValidateRange(sb, "temp_time", ": ", words, index + 3, 0, 31);
            InsertLine(sb, "tone", ": ", ParseLcdTextTones(words, index + 4));
            InsertLineValidateRange(sb, "row", ": ", words, index + 6, 0, 1);
            InsertLineValidateRange(sb, "column", ": ", words, index + 7, 0, 15);
            InsertLineWithText(sb, "text[64]", ": ", words, index + 8);
        }

        private void ParseEnCcSioDc(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcSioDc (1327)" : "enCcSioDc (0327)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "sio_number", ": ", words, index + 1, 0, 95);
            InsertLine(sb, "cmnd_tag", ": ", words, index + 2);
            InsertLine(sb, "cmnd_id", ": ", words, index + 3);
        }

        private void ParseEnCcSioHexLoad(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcSioHexLoad (1328)" : "enCcSioHexLoad (328)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "sio_number", ": ", words, index + 1, 0, 95);
            InsertLineWithText(sb, "file_name[200]", ": ", words, index + 2);
        }

        private void ParseEnCcHostResponse(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcHostResponse (1329)" : "enCcHostResponse (0329)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "acr_number", ": ", words, index + 1, 0, 127);
            InsertLine(sb, "command", ": ", ParseHostResponseCommand(words, index + 2));
            InsertLine(sb, "cardholder_id", ": ", words, index + 3);
        }

        private void ParseEnCcNvArgSet(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcNvArgSet (1330)" : "enCcNvArgSet (0330)");
            InsertLineValidateRange(sb, "nScpId", ": ", words, index, 0, 16383);
            InsertLine(sb, "nNvArgType", ": ", ParseNvArgType(words, index + 1));
            InsertLineValidateRange(sb, "nSioNumber", ": ", words, index + 2, -1, 95);
        }

        private void ParseEnCcCardSim(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcCardSim (1331)" : "enCcCardSim (0331)");
            InsertLineValidateRange(sb, "nScp", ": ", words, index, 0, 16383);
            InsertLine(sb, "nCommand", ": ", ParseCardSimCommand(words, index + 1));
            InsertLineValidateRange(sb, "nAcr", ": ", words, index + 2, 0, 127);
        }

        private void ParseEnCcIpsSet(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcIpsSet (1332)" : "enCcIpsSet (0332)");
            InsertLineValidateRange(sb, "nScpId", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "nIpsNumber", ": ", words, index + 1, 0, 255);
            InsertLine(sb, "nCommand", ": ", ParseIpsSetCommand(words, index + 2));
        }

        private void ParseEnCcDiag(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcDiag (1333)" : "enCcDiag (0333)");
            InsertLineValidateRange(sb, "nScpId", ": ", words, index, 0, 16383);
            InsertLine(sb, "diag_code", ": ", ParseDiagCode(words, index + 1));
        }

        private void ParseEnCcTempAcrMode(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcTempAcrMode (1334)" : "enCcTempAcrMode (0334)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "acr_number", ": ", words, index + 1, 0, 127);
            InsertLine(sb, "acr_mode", ": ", ParseAcrMode(words, index + 2));
            InsertLine(sb, "time", ": ", ParseTempAcrModeTime(words, index + 3));
        }

        private void ParseEnCcOperatingMode(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcOperatingMode (1335)" : "enCcOperatingMode (0335)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "oper_mode", ": ", words, index + 1, 0, 7);
        }

        private void ParseEnCcSioRdrHexLoad(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcSioRdrHexLoad (1336)" : "enCcSioRdrHexLoad (0336)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "sio_number", ": ", words, index + 1, 0, 95);
            InsertLine(sb, "rdr_list", ": ", ParseSioRdrHexRdrListFlags(words, index + 2));
            InsertLine(sb, "target_type", ": ", ParseSioRdrHexTargetType(words, index + 3));
            InsertLine(sb, "file_name[200]", ": ", words, index + 4);
        }

        private void ParseEnCcAcrOsdpPassthrough(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcAcrOsdpPassthrough (1337)" : "enCcAcrOsdpPassthrough (0337)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "acr_number", ": ", words, index + 1, 0, 127);
            InsertLine(sb, "sequence_num", ": ", words, index + 2);
            InsertLine(sb, "reader_role", ": ", ParseOsdpPassThruReaderRole(words, index + 3));
            InsertLine(sb, "msg_type", ": ", ParseOsdpPassThruMsgType(words, index + 4));
        }

        private void ParseEnCcAcrOfflineAccessList(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcAcrOfflineAccessList (1338)" : "enCcAcrOfflineAccessList (0338)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "acr_number", ": ", words, index + 1, 0, 127);
            InsertLine(sb, "protocol", ": ", ParseOALProtocol(words, index + 2));
        }

        private void ParseEnCcKeySim(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcKeySim (1339)" : "enCcKeySim (0339)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "nAcr", ": ", words, index + 1, 0, 127);
            InsertLine(sb, "e_time", ": ", ParseTime(words, index + 2, 1970));
            InsertLine(sb, "keys[MAX_SIM_KEY_SIZE]", ": ", words, index + 3);
        }

        private void ParseEnCcOsdpReaderTransfer(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcOsdpReaderTransfer (1340)" : "enCcOsdpReaderTransfer (0340)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLine(sb, "acr_pri[16]", ": ", words, index + 1, 16);
            InsertLine(sb, "acr_alt[16]", ": ", words, index + 17, 16);
            InsertLine(sb, "src_file[100]", ": ", words, index + 33);
        }

        private void ParseEnCcControlReboot(string[] words, int numWords, bool honeywell, StringBuilder sb)
        {
            int index = GetCmdIndex() + 1;
            InsertLine(sb, honeywell ? "enNCcControlReboot (1341)" : "enCcControlReboot (0341)");
            InsertLineValidateRange(sb, "scp_number", ": ", words, index, 0, 16383);
            InsertLineValidateRange(sb, "sio_number", ": ", words, index + 1, 0, 32);
            InsertLineValidateRange(sb, "target_type", ": ", words, index + 2, 0, 2);
            InsertLineValidateRange(sb, "reset_type", ": ", words, index + 3, 0, 1);
        }

        #endregion

        #region Value Interpreters & Bitmask Parsers

        private string ParseDebugRq(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - Normal mode",
                1 => "1 - Dump Channel",
                2 => "2 - Dump SCP",
                3 => "3 - Enable/Disable Polls/Acks",
                _ => words[index] + " - Unknown value"
            };
        }

        private string ParseRtsMode(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - On",
                1 => "1 - Toggle",
                2 => "2 - Off",
                3 => "3 - CTS/RTS handshake",
                _ => words[index] + " (Unknown mode)"
            };
        }

        private string ParseCPort(string[] words, int index, int cType)
        {
            if (index >= words.Length) return "";
            if (cType >= 256) cType &= 255;
            string cport = words[index] + " ";
            switch (cType)
            {
                case 0: cport += " (physical port number)"; break;
                case 1: case 2: case 3: case 4: cport += " (not used)"; break;
                case 5: case 7: cport += " (tcp/ip port number)"; break;
            }
            return cport;
        }

        private string ParseCType(string[] words, int index, bool channelConfig)
        {
            if (index >= words.Length) return "";
            int num = int.Parse(words[index]);
            bool flag1 = false, flag2 = false, flag3 = false;
            if (num >= 256)
            {
                if (channelConfig)
                {
                    if ((num & 256) == 256) flag2 = true;
                    if ((num & 512) == 512) flag3 = true;
                }
                else flag1 = true;
            }
            string ctype = (num & 255) switch
            {
                0 => num + " - Serial",
                1 => num + " - Modem Dial Out",
                2 => num + " - Modem Dial In",
                3 => num + " - Modem Dial Out/In",
                4 => num + " - TCP/IP connect to remote",
                5 => num + " - accept single connection from remote",
                6 => num + " - Virtual IC internal connection",
                7 => num + " - Concurrent multiple inbound connections",
                9 => num + " - Direct SIO",
                10 => num + " - Direct SIO (Honeywell)",
                _ => num + " (Unknown cType)"
            };
            if (flag1) ctype += " (Disable Transactions)";
            if (flag2) ctype += " (TLS Required)";
            if (flag3) ctype += " (TLS Cert Verification)";
            return ctype;
        }

        private string ParseScpAddress(string[] words, int index)
        {
            if (index >= words.Length) return "";
            int num = int.Parse(words[index]);
            bool flag = num >= 256;
            int addr = num & 255;
            string res = addr.ToString();
            if (addr < 0 || addr > 7) res += " (InvalidAddress)";
            if (flag) res += " (Bilingual)";
            return res;
        }

        private string ParseIpClientAssignFlags(string[] words, int index)
        {
            if (index >= words.Length) return "";
            int num = int.Parse(words[index]);
            string res = $"{num} (0x{num:X4}) ";
            if ((num & 1) == 1) res += "DECREMENT ";
            if ((num & 2) == 2) res += "NO_REUSE ";
            if ((num & 4) == 4) res += "NO_HWID ";
            return res;
        }

        private string ParseCardFormatterFunction(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                1 => "1 (Wiegand)",
                2 => "2 (Mag)",
                3 => "3 (MTA)",
                _ => words[index] + " (Unknown Card Formatter Function)"
            };
        }

        private string ParseTimezoneMode(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - OFF",
                1 => "1 - ON",
                2 => "2 - SCAN",
                3 => "3 - One Time Event",
                4 => "4 - Scan-Always Honor DayOfWeek",
                5 => "5 - Scan-Day of Week and Holiday",
                _ => words[index] + " (Unknown mode)"
            };
        }

        private string ParseDayMask(string[] words, int index)
        {
            if (index >= words.Length) return "";
            int num = int.Parse(words[index]);
            string res = words[index] + " ";
            if ((num & 1) == 1) res += "Sunday ";
            if ((num & 2) == 2) res += "Monday ";
            if ((num & 4) == 4) res += "Tuesday ";
            if ((num & 8) == 8) res += "Wednesday ";
            if ((num & 16) == 16) res += "Thursday ";
            if ((num & 32) == 32) res += "Friday ";
            if ((num & 64) == 64) res += "Saturday ";
            if ((num & 256) == 256) res += "Holiday0 ";
            if ((num & 512) == 512) res += "Holiday1 ";
            if ((num & 1024) == 1024) res += "Holiday2 ";
            if ((num & 2048) == 2048) res += "Holiday3 ";
            return res;
        }

        private string ParseStartEndTime(string[] words, int index)
        {
            if (index >= words.Length) return "";
            int num1 = int.Parse(words[index]);
            int num2 = num1 / 60;
            int num3 = num1 % 60;
            return $"({num2:00}:{num3:00})";
        }

        private string ParseHolidayExtendField(string[] words, int index)
        {
            if (index >= words.Length) return "";
            int num = int.Parse(words[index]);
            return ((num & 128) != 128) ? words[index] : $"{num & 127} (Special Holiday)";
        }

        private string ParseHolidayTypeMask(string[] words, int index)
        {
            if (index >= words.Length) return "";
            int num = int.Parse(words[index]);
            string res = words[index] + " ";
            if (num == 0) res += "Delete Holiday";
            if ((num & 1) == 1) res += "Holiday0 ";
            if ((num & 2) == 2) res += "Holiday1 ";
            if ((num & 4) == 4) res += "Holiday2 ";
            if ((num & 8) == 8) res += "Holiday3 ";
            return res;
        }

        private string ParsePinDigits(string[] words, int index)
        {
            if (index >= words.Length) return "";
            int num1 = int.Parse(words[index]);
            int num2 = (num1 & 61440) / 4096;
            int num3 = (num1 & 3840) / 256;
            int num4 = (num1 & 240) / 16;
            int num5 = num1 & 15;
            string str = num2 switch { 0 => " (Disabled)", 1 => " (AddConstant)", 2 => " (AppendConstant)", _ => "" };
            return $"{num1} [duressMode: {num2}{str}, pinConstant: {num3}, cardIDSize: {num4}, nPinDigits: {num5}]";
        }

        private string ParseIcvtNum(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - Normally closed, No EOL",
                1 => "1 - Normally open, No EOL",
                2 => "2 - Standard EOL (1K Normal, 2K Active)",
                3 => "3 - Standard EOL (2K Normal, 1K Active)",
                128 => "128 - Custom EOL",
                129 => "129 - Custom EOL",
                _ => words[index] + " (Unknown value)"
            };
        }

        private string ParseOutputDriveMode(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - (ONLINE: Normal, OFFLINE: No Change)",
                1 => "1 - (ONLINE: Inverted, OFFLINE: No Change)",
                16 => "16 - (ONLINE: Normal, OFFLINE: Inactive)",
                17 => "17 - (ONLINE: Inverted, OFFLINE: Inactive)",
                32 => "32 - (ONLINE: Normal, OFFLINE: Active)",
                33 => "33 - (ONLINE: Inverted, OFFLINE: Active)",
                _ => words[index] + " (Unknown mode)"
            };
        }

        private string ParseDtFmt(string[] words, int index)
        {
            if (index >= words.Length) return "";
            int num = int.Parse(words[index]);
            string res = $"{num} (0x{num:X4}) ";
            if ((num & 1) == 1) res += "IDRDR_D1D0 ";
            if ((num & 2) == 2) res += "IDRDR_ZTRIM ";
            if ((num & 4) == 4) res += "IDRDR_T2FMT ";
            if ((num & 8) == 8) res += "IDRDR_BIDIR ";
            return res;
        }

        private string ParseKeypadMode(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - IDRDR_K_NONE",
                2 => "2 - IDRDR_K_HID (4-bit)",
                3 => "3 - IDRDR_K_INDALA",
                6 => "6 - IDRDR_K_4BIT_ALIVE_60",
                7 => "7 - IDRDR_K_8BIT_ALIVE_60",
                _ => words[index] + " (Unknown mode)"
            };
        }

        private string ParseLedDriveMode(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - NONE",
                1 => "1 - IDRDR_L_BICOLOR",
                7 => "7 - IDRDR_L_OSDP",
                _ => words[index] + " (Unknown mode)"
            };
        }

        private string ParseOsdpFlags(string[] words, int index)
        {
            if (index >= words.Length) return "";
            int num = int.Parse(words[index]);
            string res = $"{num} (0x{num:X4}) ";
            if (num == 0) return res + "Defaults for Reader Port";
            if ((num & 7) == 1) res += "BAUD_9600 ";
            if ((num & 7) == 2) res += "BAUD_19200 ";
            if ((num & 7) == 3) res += "BAUD_38400 ";
            if ((num & 7) == 4) res += "BAUD_115200 ";
            if ((num & 8) == 8) res += "NO_DISCOVER ";
            if ((num & 16) == 16) res += "DEBUG_TRACE ";
            if ((num & 128) == 128) res += "OSDP_SC ";
            return res;
        }

        private string ParseAccessCfg(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - ACR_A_SINGLE",
                1 => "1 - ACR_A_MASTER",
                2 => "2 - ACR_A_SLAVE",
                3 => "3 - ACR_A_TURNSTILE",
                4 => "4 - ACR_A_EL1",
                5 => "5 - ACR_A_EL2",
                _ => words[index] + " (Unknown mode)"
            };
        }

        private string ParseAcrMode(string[] words, int index)
        {
            if (index >= words.Length) return "";
            int num = int.Parse(words[index]);
            bool flag = (num & 128) == 128;
            if (flag) num &= 127;
            string res = num switch
            {
                0 => "0 - no change",
                1 => "1 - disable",
                2 => "2 - unlock",
                3 => "3 - locked",
                4 => "4 - facility code only",
                5 => "5 - card only",
                6 => "6 - pin only",
                7 => "7 - card and PIN",
                8 => "8 - card or PIN",
                _ => words[index] + " - UNKNOWN"
            };
            if (flag) res += " (Set Current Mode Flag Set)";
            return res;
        }

        private string ParseTriggerCommand(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                1 => "1 - (Abort a delayed procedure)",
                2 => "2 - (Execute actions with prefix 0)",
                3 => "3 - (Resume a delayed procedure)",
                _ => words[index] + " (Unknown command)"
            };
        }

        private string ParseRledId(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                1 => "1 - (disabled)",
                2 => "2 - (unlocked)",
                3 => "3 - (locked)",
                11 => "11 - (rled_deny)",
                12 => "12 - (rled_admit)",
                _ => words[index] + " (Unknown rled_id)"
            };
        }

        private string ParseRTxtLineIndex(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                1 => "1 - (rdiCLOCK)",
                2 => "2 - (rdiLOCKED)",
                3 => "3 - (rdiUNLOCKED)",
                4 => "4 - (rdiREADY)",
                5 => "5 - (rdiPINREQ)",
                6 => "6 - (rdiDENY)",
                7 => "7 - (rdiADMIT)",
                _ => words[index] + " (Unknown LineIndex)"
            };
        }

        private string ParseUseNames(string[] words, int index)
        {
            if (index >= words.Length) return "";
            int num = int.Parse(words[index]);
            string res = $"{num} (0x{num:X4}) ";
            if ((num & 1) == 1) res += "(MPs) ";
            if ((num & 2) == 2) res += "(CPs) ";
            if ((num & 4) == 4) res += "(ACRs) ";
            return res;
        }

        private string ParsePointType(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                1 => "1 - (MP)",
                2 => "2 - (CP)",
                3 => "3 - (ACR)",
                _ => words[index] + " (Unknown type)"
            };
        }

        private string ParseSioAesCommand(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - AESCMND_SIO_OFF",
                1 => "1 - AESCMND_SIO_SET_MK",
                2 => "2 - AESCMND_SIO_ENA_MK",
                _ => words[index] + " (Unknown mode)"
            };
        }

        private string ParseSioNetworkMode(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - Mercury DHCP",
                1 => "1 - DHCP",
                2 => "2 - Static",
                _ => words[index] + " (Unknown nMode)"
            };
        }

        private string ParseLpdFlags(string[] words, int index)
        {
            if (index >= words.Length) return "";
            int num = int.Parse(words[index]);
            string res = $"{num} (0x{num:X4}) ";
            if ((num & 1) == 1) res += "(LDF_PIN) ";
            if ((num & 2) == 2) res += "(LDF_CIPHER) ";
            if ((num & 4) == 4) res += "(LDF_CAP) ";
            return res;
        }

        private string ParseSaveMode(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - Save all settings (system and all Scp's)",
                1 => "1 - Save ONLY the system settings",
                2 => "2 - Save only the settings for the Scp specified",
                _ => words[index] + " (Unknown mode)"
            };
        }

        private string ParseHcpDriver(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - (Primary) ",
                1 => "1 - (Alternate)",
                _ => words[index] + " (Unknown HcpDriver)"
            };
        }

        private string ParseDualPortControlCommand(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                4 => "4 (save the controller configuration - no cardholders)",
                5 => "5 (save the controllers database)",
                127 => "127 (clear card database)",
                _ => words[index] + " (Unknown command)"
            };
        }

        private string ParseAESCmnd(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - AESCMND_OFF (disable encryption)",
                1 => "1 - AESCMND_SET_MK1 (Load Master Key 1)",
                2 => "2 - AESCMND_SET_MK2 (Load Master Key 2)",
                _ => words[index] + " - UNKNOWN"
            };
        }

        private string ParseAESTestCode(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                1 => "1 - AES_T_ECB_E",
                2 => "2 - AES_T_ECB_D",
                3 => "3 - AES_T_CBC_E",
                _ => words[index] + " - UNKNOWN"
            };
        }

        private string ParsePollMode(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - DPOLL_ENABLE",
                1 => "1 - DPOLL_DISABLE",
                2 => "2 - DPOLL_ALTERNATE",
                _ => words[index] + " (Unknown mode)"
            };
        }

        private string ParseFileType(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - Certificate",
                1 => "1 - User Defined",
                2 => "2 - License",
                _ => words[index] + " (Unknown FileType)"
            };
        }

        private string ParseTranIndex(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                -2 => "-2 (enable reporting from last_reported)",
                -1 => "-1 (disable reporting)",
                _ => words[index]
            };
        }

        private string ParseAdbcFlags(string[] words, int index)
        {
            if (index >= words.Length) return "";
            int num = int.Parse(words[index]);
            string res = $"{num} (0x{num:X4}) ";
            if ((num & 1) == 1) res += "ADBC_ACTIVE ";
            if ((num & 2) == 2) res += "ADBC_1FREE ";
            if ((num & 4) == 4) res += "ADBC_VIP ";
            return res;
        }

        private string ParseSetClear(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - Clear Mask (generate alarm)",
                1 => "1 - Set Mask (suppress alarm generation)",
                _ => words[index] + " - UNKNOWN"
            };
        }

        private string ParseCpCtlCommand(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                1 => "1 - Off",
                2 => "2 - On",
                3 => "3 - Single Pulse",
                4 => "4 - Repeating Pulse",
                _ => words[index] + " (Unknown command)"
            };
        }

        private string ParseProcedureCommand(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                1 => "1 - Abort a delayed procedure",
                2 => "2 - Execute (prefix 0 actions)",
                3 => "3 - Resume a delayed procedure",
                _ => words[index] + " (Unknown ProcedureCommand)"
            };
        }

        private string ParseTzCommand(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                1 => "1 - TmpClr",
                2 => "2 - TmpSet",
                3 => "3 - OvrClr",
                4 => "4 - OvrSet",
                _ => words[index] + " (Unknown command)"
            };
        }

        private string ParseOEMCodes(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return ParseOEMCodes(int.Parse(words[index]));
        }

        private string ParseOEMCodes(int value)
        {
            return value switch
            {
                0 => value + " (default)",
                1 => value + " - Mercury Security",
                1024 => value + " - Lenel Systems International, Inc.",
                8192 => value + " - Honeywell",
                12288 => value + " - AMAG",
                _ => value + " - Unknown OEM code"
            };
        }

        private string ParseMpgSetCommand(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                1 => "1 - access",
                2 => "2 - override",
                3 => "3 - force arm",
                4 => "4 - arm",
                _ => words[index] + " (Unknown command)"
            };
        }

        private string ParseAreaSetCommand(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                1 => "1 - Disable Access",
                2 => "2 - Enable Access",
                3 => "3 - Set occupancy count (standard)",
                _ => words[index] + " (Unknown command)"
            };
        }

        private string ParseLEDColor(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - (off)",
                1 => "1 - (red)",
                2 => "2 - (green)",
                3 => "3 - (amber)",
                _ => words[index] + " (Unknown color)"
            };
        }

        private string ParseLcdTextType(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                1 => "1 - Permanent",
                2 => "2 - Temp",
                _ => words[index] + " (Unknown type)"
            };
        }

        private string ParseLcdTextTones(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - None",
                1 => "1 - Steady",
                _ => words[index] + " (Unknown tone)"
            };
        }

        private string ParseHostResponseCommand(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - Deny Access",
                1 => "1 - Allow Access",
                _ => words[index] + " (Unknown command)"
            };
        }

        private string ParseNvArgType(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                11 => "11 - Factory settings",
                13 => "13 - SCP oem code",
                33 => "33 - SIO oem code",
                _ => words[index] + " (Unknown type)"
            };
        }

        private string ParseCardSimCommand(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return (int.Parse(words[index]) == 1) ? "1 - (Process this as card data input)" : words[index] + " (Unknown nCommand)";
        }

        private string ParseIpsSetCommand(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                1 => "1 (Disarm)",
                2 => "2 (Arm)",
                _ => words[index] + " (Unknown IpsSetCommand)"
            };
        }

        private string ParseDiagCode(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - ForceException",
                1 => "1 - BulkErase",
                _ => words[index] + " (Unknown code)"
            };
        }

        private string ParseTempAcrModeTime(string[] words, int index)
        {
            if (index >= words.Length) return "";
            int num1 = int.Parse(words[index]);
            int num2 = (num1 & 49152) >> 14;
            int num3 = num1 & 16383;
            return num2 switch
            {
                0 => num3.ToString() + " minutes (standard)",
                2 => num3.ToString() + " (Indefinite)",
                3 => num3.ToString() + " seconds (standard)",
                _ => $"{num3} UNKNOWN Mode {num2}"
            };
        }

        private string ParseSioRdrHexRdrListFlags(string[] words, int index)
        {
            if (index >= words.Length) return "";
            int num = int.Parse(words[index]);
            string res = $"{num} (0x{num:X4}) ";
            if ((num & 1) == 1) res += "RDR-1 ";
            if ((num & 2) == 2) res += "RDR-2 ";
            return res;
        }

        private string ParseSioRdrHexTargetType(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0x00 MG(K)/MS(K) Reader",
                1 => "0x01 KP/PR(K)/SM(K)/MT(K)/MI(K)/FMK Reader",
                _ => words[index] + " (Unknown Target Type)"
            };
        }

        private string ParseOsdpPassThruReaderRole(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - primary",
                1 => "1 - alternate",
                _ => words[index] + " - UNKNOWN"
            };
        }

        private string ParseOsdpPassThruMsgType(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return int.Parse(words[index]) switch
            {
                0 => "0 - osdp_MFG",
                1 => "1 - osdp_XWR",
                _ => words[index] + " - UNKNOWN"
            };
        }

        private string ParseOALProtocol(string[] words, int index)
        {
            if (index >= words.Length) return "";
            return (int.Parse(words[index]) == 9) ? "9 - SimonsVoss" : words[index] + " - UNKNOWN";
        }

        private string ParseTime(string[] words, int index, int baseYear)
        {
            if (index >= words.Length) return "";
            if (words[index] != "-1")
            {
                uint num = uint.Parse(words[index]);
                string time = num.ToString() + " ";
                if (num > 0U)
                {
                    DateTime dateTime = new DateTime(baseYear, 1, 1).AddSeconds((double)num);
                    time += dateTime.ToString(CultureInfo.InvariantCulture);
                }
                return time;
            }
            return words[index];
        }

        private string ParseSioModel(string[] words, int index, bool hexValue)
        {
            if (index >= words.Length) return "";
            int model = !hexValue ? int.Parse(words[index]) : int.Parse(words[index], NumberStyles.HexNumber);
            return model switch
            {
                -1 => "-1 - (Unspecified model)",
                80 => "80 - MR50",
                81 => "81 - MR16IN",
                82 => "82 - MR16OUT",
                84 => "84 - MR52",
                _ => model.ToString() + " (Unknown model)"
            };
        }

        #endregion
    }
}