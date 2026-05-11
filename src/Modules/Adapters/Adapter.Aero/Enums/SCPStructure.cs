namespace Adapter.Aero.Enums;

public enum SCPStructure
{
    SCPSID_TRAN = 1,        // Transactions
    SCPSID_TZ = 2,          // Time zones
    SCPSID_HOL = 3,         // Holidays
    SCPSID_MSP1 = 4,        // Msp1 ports (SIO drivers)
    SCPSID_SIO = 5,         // SIOs
    SCPSID_MP = 6,          // Monitor points
    SCPSID_CP = 7,          // Control points
    SCPSID_ACR = 8,         // Access control readers
    SCPSID_ALVL = 9,        // Access levels
    SCPSID_TRIG = 10,       // Triggers
    SCPSID_PROC = 11,       // Procedures
    SCPSID_MPG = 12,        // Monitor point groups
    SCPSID_AREA = 13,       // Access areas
    SCPSID_EAL = 14,        // Elevator access levels
    SCPSID_CRDB = 15,       // Cardholder database

    SCPSID_FLASH = 20,      // FLASH specs
    SCPSID_BSQN = 21,       // Build sequence number
    SCPSID_SAVE_STAT = 22,  // Flash save status
    SCPSID_MAB1_FREE = 23,  // Memory alloc block 1 free
    SCPSID_MAB2_FREE = 24,  // Memory alloc block 2 free
    SCPSID_ARQ_BUFFER = 26, // Access request buffers
    SCPSID_PART_FREE_CNT = 27, // Partition memory free info
    SCPSID_LOGIN_STANDARD = 33, // Web logins - standard
    SCPSID_FILE_SYSTEM = 35 // File system version information
}