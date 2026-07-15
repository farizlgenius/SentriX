namespace Adapter.Amico.Enums;

public enum AccessEvent
{
    InvalidReader = 1,
    InvalidIdentificationRuleParameters = 2,
    NotIdentified = 3,
    PendingIdentification = 4,
    IdentificationTimeExpired = 5,
    AccessDenied = 6,
    AccessGranted = 7,
    PendingAccess = 8,
    UserIsNotAdministrator = 9,
    NonIdentifiedAccess = 10,
    AccessViaPushbuttonSwitch = 11,
    AccessThroughWebInterface = 12,
    NoResponse = 13
}