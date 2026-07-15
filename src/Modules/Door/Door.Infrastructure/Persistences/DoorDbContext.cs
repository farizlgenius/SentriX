using Door.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Door.Infrastructure.Persistences;

public sealed class DoorDbContext(DbContextOptions<DoorDbContext> options) : DbContext(options)
{
       public const string Schema = "door";
      public DbSet<Doors> Doors { get; set; }
      public DbSet<ReaderMode> ReaderModes {get; set;}
      public DbSet<StrikeMode> StrikeModes {get; set;}
      public DbSet<ApbMode> ApbModes {get; set;}
      public DbSet<DoorMode> DoorModes {get ;set;}
      public DbSet<AccessControlFlag> AccessControlFlags {get; set;}
      public DbSet<SpareFlag> SpareFlags {get; set;}
      public DbSet<OsdpBaudrate> OsdpBaudrates {get; set;}

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            base.OnModelCreating(modelBuilder);

            // ⭐ Module schema
            modelBuilder.HasDefaultSchema(Schema);

            // Make default datetime now
            var isSqlServer = Database.ProviderName == "Microsoft.EntityFrameworkCore.SqlServer";
            var isPostgres = Database.ProviderName == "Npgsql.EntityFrameworkCore.PostgreSQL";

            string utcNowSql;
            string guidSql;

            if (isSqlServer)
            {
                  utcNowSql = "GETUTCDATE()";
                  guidSql = "NEWSEQUENTIALID()"; // or NEWID()
            }
            else if (isPostgres)
            {
                  utcNowSql = "NOW() AT TIME ZONE 'UTC'";
                  guidSql = "gen_random_uuid()";
            }
            else
            {
                  throw new Exception("Unsupported database provider");
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                  if (typeof(BaseDbEntity).IsAssignableFrom(entityType.ClrType))
                  {
                        modelBuilder.Entity(entityType.ClrType)
                              .Property(nameof(BaseDbEntity.created_at))
                              .HasDefaultValueSql(utcNowSql)
                              .ValueGeneratedOnAdd();

                        modelBuilder.Entity(entityType.ClrType)
                              .Property(nameof(BaseDbEntity.updated_at))
                              .HasDefaultValueSql(utcNowSql)
                              .ValueGeneratedOnAdd();

                        modelBuilder.Entity(entityType.ClrType)
                              .Property(nameof(BaseDbEntity.guid))
                              .HasDefaultValueSql(guidSql)
                              .ValueGeneratedOnAdd();
                  }
            }

            modelBuilder.Entity<ReaderMode>()
            .HasData(
                  new ReaderMode
                  {
                        id=1,
                        label="Single",
                        value=0,
                        description="Single reader, controlling the door"
                        
                  },
                  new ReaderMode
                  {
                        id=2,
                        label="Dual",
                        value=1,
                        description="Paired readers, Primary - this reader controls the door"
                  },
                  new ReaderMode
                  {
                        id=3,
                        label="Turnstile",
                        value=3,
                        description="Turnstile Reader"
                  },
                  new ReaderMode
                  {
                        id=4,
                        label="Elevator No Floor",
                        value=4,
                        description="Elevator, no floor select feedback *"
                  },
                  new ReaderMode
                  {
                        id=5,
                        label="Elevator with Floor",
                        value=5,
                        description="Elevator with floor select feedback *"
                  }
            );


            modelBuilder.Entity<StrikeMode>()
            .HasData(
                  new StrikeMode
                  {
                        id=1,
                        label="No Change",
                        value=0,
                        description="Do not use! This would allow the strike to stay active for the entire strike time allowing the door to be opened multiple times."
                        
                  },
                  new StrikeMode
                  {
                        id=2,
                        label="Deactivate on open",
                        value=1,
                        description="Deactivate strike when door opens."
                  },
                  new StrikeMode
                  {
                        id=3,
                        label="Deactivate on close",
                        value=2,
                        description="Deactivate strike on door close or strike_t_max expires."
                  },
                  new StrikeMode
                  {
                        id=4,
                        label="Tailgate",
                        value=16,
                        description="Used with ACR_S_OPEN or ACR_S_CLOSE, to select tailgate mode: pulse (strk_sio:strk_number+1) relay for each user expected to enter."
                  }
            );

            modelBuilder.Entity<ApbMode>()
            .HasData(
                  new ApbMode
                  {
                        id=1,
                        label="No Apb",
                        value=0,
                        description="Do not check or alter anti-passback location. No anti-passback rules."
                  },
                  new ApbMode
                  {
                        id=2,
                        label="Soft",
                        value=1,
                        description="Soft anti-passback: Accept any new location, change the user’s location to current reader, and generate an anti-passback violation for an invalid entry."
                  },
                  new ApbMode
                  {
                        id=3,
                        label="Hard",
                        value=2,
                        description="Hard anti-passback: Check user location, if a valid entry is made, change user’s location to new location. If an invalid entry is attempted, do not grant access."
                  },
                  new ApbMode
                  {
                        id=4,
                        label="Reader-based Last Valid (s)",
                        value=3,
                        description="Reader-based anti-passback using the ACR’s last valid user. Verify it’s not the same user within the time parameter specified within apb_delay."
                  },
                  new ApbMode
                  {
                        id=5,
                        label="Reader-based Access History (s)",
                        value=4,
                        description="Reader-based anti-passback using the access history from the cardholder database: Check user’s last ACR used, checks for same reader within a specified time (apb_delay). This requires the bSupportTimeApb flag be set in Command 1105: Access Database Specification."
                  },
                  new ApbMode
                  {
                        id=6,
                        label="Area-based (s)",
                        value=5,
                        description="Area based anti-passback: Check user’s current location, if it does not match the expected location then check the delay time (apb_delay). Change user’s location on entry. This requires the bSupportTimeApb flag be set in Command 1105: Access Database Specification."
                  },
                  new ApbMode
                  {
                        id=7,
                        label="Reader-based Last Valid (m)",
                        value=6,
                        description="Reader-based anti-passback using the ACR’s last valid user. Verify it’s not the same user within the time parameter specified within apb_delay."
                  },
                  new ApbMode
                  {
                        id=8,
                        label="Reader-based Access History (s)",
                        value=7,
                         description="Reader-based anti-passback using the access history from the cardholder database: Check user’s last ACR used, checks for same reader within a specified time (apb_delay). This requires the bSupportTimeApb flag be set in Command 1105: Access Database Specification."
                  },
                  new ApbMode
                  {
                        id=9,
                        label="Area-based (m)",
                        value=8,
                        description="Area based anti-passback: Check user’s current location, if it does not match the expected location then check the delay time (apb_delay). Change user’s location on entry. This requires the bSupportTimeApb flag be set in Command 1105: Access Database Specification."
                  }
            );

            modelBuilder.Entity<DoorMode>()
            .HasData(
                  new DoorMode
                  {
                        id=1,
                        label="Disable",
                        value=1,
                        description="Disable the ACR, no REX"
                  },
                  new DoorMode
                  {
                        id=2,
                        label="Unlock",
                        value=2,
                        description="Unlock (unlimited access)"
                  },
                  new DoorMode
                  {
                        id=3,
                        label="Lock",
                        value=3,
                        description="Locked (no access,REX active)"
                  },
                  new DoorMode
                  {
                        id=4,
                        label="FAC Only",
                        value=4,
                        description="Facility code only"
                  },
                  new DoorMode
                  {
                        id=5,
                        label="Card Only",
                        value=5,
                        description="Card Only"
                  },
                  new DoorMode
                  {
                        id=6,
                        label="PIN Only",
                        value=6,
                        description="PIN Only"
                  },
                  new DoorMode
                  {
                        id=7,
                        label="Card and PIN",
                        value=7,
                        description="Card and PIN required"
                  },
                  new DoorMode
                  {
                        id=8,
                        label="Card or PIN",
                        value=8,
                        description="Card or PIN required"
                  }
            );

            modelBuilder.Entity<AccessControlFlag>()
            .HasData(
                  new AccessControlFlag
                  {
                        id=1,
                        label="Decrement Use Limit",
                        value=0x0001,
                        description="Decrement use limits on access"
                  },
                  new AccessControlFlag
                  {
                        id=2,
                        label="Require use limit",
                        value=0x0002,
                        description="Require use limit to be non-zero"
                  },
                   new AccessControlFlag
                  {
                        id=3,
                        label="Deny duress",
                        value=0x0004,
                        description="Set to deny a duress request. The default behavior is to grant access under duress and log event. "
                  },
                  new AccessControlFlag
                  {
                        id=4,
                        label="Not wait door open",
                        value=0x0008,
                        description="Do not wait for door to open. Assume that the door was used and log all access requests as used as soon as the request is granted."
                  },
                  new AccessControlFlag
                  {
                        id=5,
                        label="Quiet REX",
                        value=0x0010,
                        description="Do not pulse the door strike on REX cycle. Used for “quiet” exit."
                  },
                  new AccessControlFlag
                  {
                        id=6,
                        label="Filter door transaction",
                        value=0x0020,
                        description="Filter Change-of-state Door transactions. This flag is normally set,unless detailed door sequence notifications are required."
                  },
                  new AccessControlFlag
                  {
                        id=7,
                        label="2 Card require",
                        value=0x0040,
                        description="Require two-card control at this reader."
                  },
                  new AccessControlFlag
                  {
                        id=8,
                        label="Require host confirm",
                        value=0x0400,
                        description="If online, check with HOST before GRANTING access."
                  },
                  new AccessControlFlag
                  {
                        id=9,
                        label="Always grant if offline",
                        value=0x0800,
                        description="If HOST is not available (offline or timeout) proceed with GRANT."
                  },
                  new AccessControlFlag
                  {
                        id=10,
                        label="Cipher mode",
                        value=0x1000,
                        description="Enable cipher mode (if user command fits a card format then use it as card). Allows user to enter digits through the keypad as card number."
                  },
                  new AccessControlFlag
                  {
                        id=11,
                        label="Log early",
                        value=0x4000,
                        description="If set, log access grant transaction right away, then log used/not-used. This feature disabled when the ACR_F_ALLUSED (0x0008) access control flag is set."
                  },
                   new AccessControlFlag
                  {
                        id=12,
                        label="Wait pattern",
                        value=0x8000,
                        description="If set, show “wait” pattern on “card not in file” instead of “denied” response. See Command 122: Reader LED/Buzzer Function Specs “wait” state."
                  }
            );

            modelBuilder.Entity<SpareFlag>()
            .HasData(
                  new SpareFlag
                  {
                        id=1,
                        label="No extend held timer",
                        value=0x0001,
                        description="On a new access grant, do not resume the extended door held open timer"
                  },
                  new SpareFlag
                  {
                        id=2,
                        label="Force card before PIN",
                        value=0x0002,
                        description="Card and PIN reader mode: Do not accept PIN followed by CARD. Forces CARD to be read first."
                  },
                  new SpareFlag
                  {
                        id=3,
                        label="Door Forced Filter",
                        value=0x0008,
                        description="Enable “Door Forced Open Filter”. Opening door within 3 seconds of door closed will not report a door forced open."
                  },
                  new SpareFlag
                  {
                        id=4,
                        label="No request",
                        value=0x0010,
                        description="Do not process any access request. Reports all access requests as “Access Denied, Door Locked”."
                  },
                  new SpareFlag
                  {
                        id=5,
                        label="Shunt relay",
                        value=0x0020,
                        description="Relay #(strike_rly+1) becomes the 'shunt relay'. On door unlocked, the shunt relay is activated 5 ms before the strike relay. The shunt relay is deactivated 1 second after the door is closed or the held open timer expires. The dc_held field must be greater than 1 for the shunt relay to function correctly."
                  },
                  new SpareFlag
                  {
                        id=6,
                        label="Output Selection Tracking",
                        value=0x0040,
                        description="Enables “output selection tracking” feature when reader is configured for elevator type 1 and the reader is also in Card and PIN mode. Instead of entering a PIN code at the reader, the floor/output number would be entered instead."
                  },
                  new SpareFlag
                  {
                        id=7,
                        label="Link mode",
                        value=0x0080,
                        description="Enables “output selection tracking” feature when reader is configured for elevator type 1 and the reader is also in Card and PIN mode. Instead of entering a PIN code at the reader, the floor/output number would be entered instead."
                  },
                  new SpareFlag
                  {
                        id=8,
                        label="Double Card",
                        value=0x0100,
                        description="Flag that enables the ability to use the double card functionality at this ACR. Presenting a valid card that has rights at the ACR twice within 5 seconds will generate a double card transaction."
                  },
                  new SpareFlag
                  {
                        id=9,
                        label="Override Credential",
                        value=0x0400,
                        description="Flag that allows for override credentials to gain access to this ACR even when in locked state. Override credentials are configured using Free Form Field type FFRM_FLD_ACCESSFLGS."
                  },
                  new SpareFlag
                  {
                        id=10,
                        label="Disable Elevator Floor",
                        value=0x0800,
                        description="Flag indicating if this ACR allows the disabling of elevator floors via the offline_mode field. Applies only to Type 1 and Type 2 elevators."
                  },
                  new SpareFlag
                  {
                        id=11,
                        label="Link mode Alt",
                        value=0x1000,
                        description="Flag that indicates if ACR is in linking mode for alternate reader, acr_mode = 32 will start linking mode and acr_mode = 33 can abort linking mode or once reader is linked or timeout reached this flag will clear."
                  },
                  new SpareFlag
                  {
                        id=12,
                        label="Extend REX",
                        value=0x2000,
                        description="lag to enable extending REX 'grant time' while REX input is active"
                  },
                  new SpareFlag
                  {
                        id=13,
                        label="Controller Bypass",
                        value=0x4000,
                        description="ACR_F_HOST_CBG must also be enabled for this flag to take effect. When both flags are active, the controller bypasses its local database check and for a grant decision. The host can respond with a grant or deny, which will be processed by the controller. If the host does not respond in time, the process times out, and the controller performs a secondary check using the local controller database. During a timeout, if the card is present in the local controller database and valid for the ACR/time, a grant is locally issued by the controller. Otherwisea deny is issued. This mode works with PIN codes if the ACR is configured into the Card and PIN reader mode."
                  },
                  new SpareFlag
                  {
                        id=14,
                        label="Early REX",
                        value=0x8000,
                        description="Flag to enable generating a transaction at the start of the REX cycle."
                  }
            );


            modelBuilder.Entity<OsdpBaudrate>()
            .HasData(
                  new OsdpBaudrate
                  {
                        id=1,
                        label="9600",
                        value=9600
                  },
                  new OsdpBaudrate
                  {
                        id=2,
                        label="19200",
                        value=19200
                  },
                  new OsdpBaudrate
                  {
                        id=3,
                        label="38400",
                        value=38400
                  },
                  new OsdpBaudrate
                  {
                        id=4,
                        label="115200",
                        value=115200
                  },
                  new OsdpBaudrate
                  {
                        id=5,
                        label="57600",
                        value=57600
                  },
                  new OsdpBaudrate
                  {
                        id=6,
                        label="230400",
                        value=230400
                  }
            );
           

      }
}