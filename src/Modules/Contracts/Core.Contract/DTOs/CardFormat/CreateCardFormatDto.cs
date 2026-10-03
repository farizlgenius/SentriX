using SharedKernel.Domain;


namespace Core.Contract.DTOs.CardFormat;


public sealed record CreateCardFormatDto(
      string Name,
      short Fac,
      short Bits,
      short EvenParityLen,
      short EvenParityLoc,
      short OddParityLen,
      short OddParityLoc,
      short FacLen,
      short FacLoc,
      short CardNoLen,
      short CardNoLoc,
      short IssueCodeLen,
      short IssueCodeLoc,
      Guid LocationGuid
);