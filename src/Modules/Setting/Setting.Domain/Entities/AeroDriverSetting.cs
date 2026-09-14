namespace Setting.Domain.Entities;

public sealed class AeroDriverSetting : BaseDomain
{
  public int cType { get; set; }
  public int cPort { get; set; }
  public int nMsp1Port { get; set; }
  public int nTransaction { get; set; }
  public int nSio { get; set; }
  public int nMp { get; set; }
  public int nCp { get; set; }
  public int nAcr { get; set; }
  public int nAlvl { get; set; }
  public int nTrgr { get; set; }
  public int nProc { get; set; }
  public int GmtOffset { get; set; }
  public bool IsDaylightSaving { get; set; }
  public int nTz { get; set; }
  public int nHol { get; set; }
  public int nMpg { get; set; }
  public int nTranLimit { get; set; }
  public int nCards { get; set; }
  public int nAlvlPerCard { get; set; }
  public int PinDuressMode { get; set; }
  public int DuressConstDigit { get; set; }
  public int CardIdSize { get; set; }
  public int PinDigit { get; set; }
  public int IssueCodeBit { get; set; }
  public bool ApbLocation { get; set; }
  public int StoreActDate { get; set; }
  public int StoreDeactDate { get; set; }
  public int UsedLimit { get; set; }
  public int EscortTimeout { get; set; }
  public int MultiCardTimeout { get; set; }

  public AeroDriverSetting(
    int cType,
    int cPort,
    int nMsp1Port,
    int nTransaction,
    int nSio,
    int nMp,
    int nCp,
    int nAcr,
    int nAlvl,
    int nTrgr,
    int nProc,
    bool IsDaylightSaving,
    int nTz,
    int nHol,
    int nMpg,
    int nTranLimit,
    int nCards,
    int nAlvlPerCard,
    int PinDuressMode,
    int DuressConstDigit,
    int CardIdSize,
    int PinDigit,
    int IssueCodeBit,
    bool ApbLocation,
    int StoreActDate,
    int StoreDeactDate,
    int UsedLimit,
    int EscortTimeout,
    int MultiCardTimeout
  ) : base(Guid.NewGuid())
  {
    this.cType = cType;
    this.cPort = cPort;
    this.nMsp1Port = nMsp1Port;
    this.nTransaction = nTransaction;
    this.nSio = nSio;
    this.nMp = nMp;
    this.nCp = nCp;
    this.nAcr = nAcr;
    this.nAlvl = nAlvl;
    this.nTrgr = nTrgr;
    this.nProc = nProc;
    this.IsDaylightSaving = IsDaylightSaving;
    this.nTz = nTz;
    this.nHol = nHol;
    this.nMpg = nMpg;
    this.nTranLimit = nTranLimit;
    this.nCards = nCards;
    this.nAlvlPerCard = nAlvlPerCard;
    this.PinDuressMode = PinDuressMode;
    this.DuressConstDigit = DuressConstDigit;
    this.CardIdSize = CardIdSize;
    this.PinDigit = PinDigit;
    this.IssueCodeBit = IssueCodeBit;
    this.ApbLocation = ApbLocation;
    this.StoreActDate = StoreActDate;
    this.StoreDeactDate = StoreDeactDate;
    this.UsedLimit = UsedLimit;
    this.EscortTimeout = EscortTimeout;
    this.MultiCardTimeout = MultiCardTimeout;
  }

  public AeroDriverSetting(
    Guid guid
  ) : base(guid)
  {
    this.cType = cType;
    this.cPort = cPort;
    this.nMsp1Port = nMsp1Port;
    this.nTransaction = nTransaction;
    this.nSio = nSio;
    this.nMp = nMp;
    this.nCp = nCp;
    this.nAcr = nAcr;
    this.nAlvl = nAlvl;
    this.nTrgr = nTrgr;
    this.nProc = nProc;
    this.IsDaylightSaving = IsDaylightSaving;
    this.nTz = nTz;
    this.nHol = nHol;
    this.nMpg = nMpg;
    this.nTranLimit = nTranLimit;
    this.nCards = nCards;
    this.nAlvlPerCard = nAlvlPerCard;
    this.PinDuressMode = PinDuressMode;
    this.DuressConstDigit = DuressConstDigit;
    this.CardIdSize = CardIdSize;
    this.PinDigit = PinDigit;
    this.IssueCodeBit = IssueCodeBit;
    this.ApbLocation = ApbLocation;
    this.StoreActDate = StoreActDate;
    this.StoreDeactDate = StoreDeactDate;
    this.UsedLimit = UsedLimit;
    this.EscortTimeout = EscortTimeout;
    this.MultiCardTimeout = MultiCardTimeout;
  }
}