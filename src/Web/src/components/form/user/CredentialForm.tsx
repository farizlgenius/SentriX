import { PropsWithChildren, useEffect, useState } from "react";
import Label from "../Label";
import { FormProp, FormType } from "../../../model/Form/FormProp";
import { UserDto } from "../../../model/User/UserDto";
import Button from "../../ui/button/Button";
import Input from "../input/InputField";
import { useLocation } from "../../../context/LocationContext";
import { Options } from "../../../model/Options";
import SignalRService from "../../../services/SignalRService";
import { ScanCardStatus } from "../../../model/User/ScanCardStatus";
import { CredentialEndpoint } from "../../../endpoint/CredentialEndpoint";
import { send } from "../../../api/api";
import { DeviceEndpoint } from "../../../endpoint/DeviceEndpoint";
import { DeviceDto } from "../../../model/Device/DeviceDto";
import { DoorEndpoint } from "../../../endpoint/DoorEndpoint";
import { DoorDto } from "../../../model/Door/DoorDto";
import { ScanCardDto } from "../../../model/User/ScanCard";
import { AddIcon, CardIcon, ScanIcon, TrashBinIcon } from "../../../icons";
import { FormActions, FormField, FormSection } from "../template/FormTemplate";
import { CardDto } from "../../../model/User/CardDto";
import Modals from "../../../pages/UiElements/Modals";
import Select from "../Select";
import Spinner from "../../../pages/UiElements/Spinner";
import { Vendor } from "../../../enum/Vendor";

const emptyCard: CardDto = { bits: 26, fac: 0, cardNumber: 0 };
const maxCards = 10;

export const CredentialForm: React.FC<PropsWithChildren<FormProp<UserDto>>> = ({
  dto,
  setDto,
  type,
}) => {
  const { locationGuid: locationId } = useLocation();
  const [cardModal, setCardModal] = useState(false);
  const [scanModal, setScanModal] = useState(false);
  const [controllerOptions, setControllerOptions] = useState<Options[]>([]);
  const [doorOptions, setDoorOptions] = useState<Options[]>([]);
  const [scanning, setScanning] = useState(false);
  const [scanData, setScanData] = useState<ScanCardDto>({
    deviceId: -1,
    doorId: -1,
  });
  const [card, setCard] = useState<CardDto>(emptyCard);
  const [selectedCardNumber, setSelectedCardNumber] = useState<number | null>(
    null,
  );
  const readOnly = type === FormType.INFO;

  const updateValue = (key: "licensePlate" | "qrCode" | "pin", value: string) =>
    setDto((previous) => {
      if (key === "licensePlate")
        return {
          ...previous,
          licensePlate: { ...previous.licensePlate, licensePlate: value },
        };
      if (key === "qrCode")
        return { ...previous, qrCode: { ...previous.qrCode, qrCode: value } };
      return { ...previous, pin: { ...previous.pin, pin: value } };
    });

  const addCard = () => {
    if (
      !card.cardNumber ||
      card.bits <= 0 ||
      dto.cards.length >= maxCards ||
      dto.cards.some((item) => item.cardNumber === card.cardNumber)
    )
      return;
    setDto((previous) => ({ ...previous, cards: [...previous.cards, card] }));
    setCard(emptyCard);
    setCardModal(false);
  };
  const removeSelected = () => {
    if (selectedCardNumber === null) return;
    setDto((previous) => ({
      ...previous,
      cards: previous.cards.filter(
        (item) => item.cardNumber !== selectedCardNumber,
      ),
    }));
    setSelectedCardNumber(null);
  };
  const fetchDoors = async (deviceId: number) => {
    const response = await send.get(
      DoorEndpoint.GET_ACR_BY_DEVICE_ID(deviceId),
    );
    if (response?.data?.data)
      setDoorOptions(
        response.data.data.map((door: DoorDto) => ({
          value: door.id,
          label: door.name,
          isTaken: false,
        })),
      );
  };
  useEffect(() => {
    const fetchControllers = async () => {
      const response = await send.get(
        DeviceEndpoint.GET(locationId, Vendor.aero),
      );
      if (response?.data?.data)
        setControllerOptions(
          response.data.data.map((device: DeviceDto) => ({
            value: device.mac,
            label: device.name,
            description: device.ip,
            isTaken: false,
          })),
        );
    };
    fetchControllers();
  }, []);
  const startScan = async () => {
    const connection = SignalRService.getConnection();
    if (!connection) return;
    connection.on("CRED.STATUS", (status: ScanCardStatus) => {
      setCard({
        bits: status.formatNumber || 26,
        fac: status.fac,
        cardNumber: status.cardId,
      });
      setScanning(false);
      setScanModal(false);
      setCardModal(true);
    });
    try {
      setScanning(true);
      await send.post(CredentialEndpoint.POST_SCAN, scanData);
    } catch {
      setScanning(false);
    }
  };
  const selectScanDevice = (
    value: string,
    event: React.ChangeEvent<HTMLSelectElement>,
  ) => {
    if (event.currentTarget.name === "scpId") {
      const deviceId = Number(value);
      setScanData((previous) => ({ ...previous, deviceId, doorId: -1 }));
      setDoorOptions([]);
      fetchDoors(deviceId);
    } else setScanData((previous) => ({ ...previous, doorId: Number(value) }));
  };
  const Tile = ({
    label,
    hint,
    icon,
    children,
  }: PropsWithChildren<{
    label: string;
    hint: string;
    icon: React.ReactNode;
  }>) => (
    <div className="rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] p-4 shadow-theme-xs transition hover:border-brand-200 dark:hover:border-brand-500/50">
      <div className="mb-4 flex items-start gap-3">
        <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-brand-50 text-brand-500 dark:bg-brand-500/15">
          {icon}
        </div>
        <div>
          <h4 className="font-semibold text-gray-900 dark:text-white">
            {label}
          </h4>
          <p className="mt-0.5 text-xs leading-5 text-gray-500 dark:text-gray-400">
            {hint}
          </p>
        </div>
      </div>
      {children}
    </div>
  );

  return (
    <>
      {cardModal && (
        <Modals
          header="Add access card"
          handleClickWithEvent={(event) =>
            event.currentTarget.name === "close" && setCardModal(false)
          }
          body={
            <>
              <p className="mb-5 text-sm text-gray-500 dark:text-gray-400">
                Enter the card details manually or scan a card from a connected
                reader.
              </p>
              <div className="grid gap-4 sm:grid-cols-2">
                <FormField>
                  <Label>Card format (bits)</Label>
                  <Input
                    name="bits"
                    type="number"
                    value={card.bits}
                    onChange={(event) =>
                      setCard((previous) => ({
                        ...previous,
                        bits: Number(event.target.value),
                      }))
                    }
                  />
                </FormField>
                <FormField>
                  <Label>Facility code</Label>
                  <Input
                    name="fac"
                    type="number"
                    value={card.fac}
                    onChange={(event) =>
                      setCard((previous) => ({
                        ...previous,
                        fac: Number(event.target.value),
                      }))
                    }
                  />
                </FormField>
                <FormField className="sm:col-span-2">
                  <Label>Card number</Label>
                  <Input
                    name="cardNumber"
                    type="number"
                    placeholder="e.g. 123456"
                    value={card.cardNumber || ""}
                    onChange={(event) =>
                      setCard((previous) => ({
                        ...previous,
                        cardNumber: Number(event.target.value),
                      }))
                    }
                  />
                </FormField>
              </div>
              <FormActions
                typeLabel="Create"
                submitName="add"
                cancelName="close"
                submitLabel="Add card"
                onSubmit={addCard}
                onCancel={() => setCardModal(false)}
                altrBtn={
                  <Button
                    size="sm"
                    variant="outline"
                    startIcon={<ScanIcon className="h-4 w-4" />}
                    onClick={() => {
                      setCardModal(false);
                      setScanModal(true);
                    }}
                  >
                    Scan card
                  </Button>
                }
              />
            </>
          }
        />
      )}
      {scanModal && (
        <Modals
          header="Scan access card"
          handleClickWithEvent={(event) =>
            event.currentTarget.name === "close" && setScanModal(false)
          }
          body={
            <>
              <p className="mb-5 text-sm text-gray-500 dark:text-gray-400">
                Choose a controller and reader, then present the card to the
                reader.
              </p>
              <div className="grid gap-4">
                <FormField>
                  <Label>Select controller</Label>
                  <Select
                    isString
                    name="scpId"
                    options={controllerOptions}
                    placeholder="Choose a controller"
                    onChangeWithEvent={selectScanDevice}
                    defaultValue={scanData.deviceId}
                  />
                </FormField>
                <FormField>
                  <Label>Select reader</Label>
                  <Select
                    isString={false}
                    name="doorId"
                    options={doorOptions}
                    placeholder="Choose a reader"
                    onChangeWithEvent={selectScanDevice}
                    defaultValue={scanData.doorId}
                  />
                </FormField>
              </div>
              <FormActions
                typeLabel="Create"
                submitName="scan"
                submitLabel={scanning ? "Waiting for card…" : "Start scan"}
                disabled={scanning}
                onSubmit={startScan}
                onCancel={() => setScanModal(false)}
                altrBtn={scanning ? <Spinner /> : undefined}
              />
            </>
          }
        />
      )}
      <FormSection
        overall="Credentials"
        title="Access credentials"
        description="Add the ways this person can identify themselves at your entry points."
      >
        <div className="grid gap-5 lg:grid-cols-2">
          <Tile
            label="License plate"
            hint="For vehicle and parking access"
            icon={<span className="text-sm font-bold">LP</span>}
          >
            <Label htmlFor="licensePlate">Plate number</Label>
            <Input
              disabled={readOnly}
              id="licensePlate"
              name="licensePlate"
              value={dto.licensePlate.licensePlate}
              onChange={(event) =>
                updateValue("licensePlate", event.target.value.toUpperCase())
              }
              placeholder="e.g. 1AB-2345"
            />
          </Tile>
          <Tile
            label="QR code"
            hint="For mobile or printed visitor passes"
            icon={<ScanIcon className="h-5 w-5" />}
          >
            <Label htmlFor="qrCode">QR code value</Label>
            <Input
              disabled={readOnly}
              id="qrCode"
              name="qrCode"
              value={dto.qrCode.qrCode}
              onChange={(event) => updateValue("qrCode", event.target.value)}
              placeholder="Enter QR code value"
            />
          </Tile>
          <Tile
            label="PIN"
            hint="Optional keypad credential"
            icon={<span className="text-sm font-bold">#</span>}
          >
            <Label htmlFor="pin">Personal identification number</Label>
            <Input
              disabled={readOnly}
              id="pin"
              name="pin"
              type="number"
              value={dto.pin.pin}
              onChange={(event) => updateValue("pin", event.target.value)}
              placeholder="e.g. 1234"
            />
          </Tile>
          <div className="flex flex-col justify-between rounded-2xl border border-brand-100 bg-brand-50/50 p-4 dark:border-brand-500/20 dark:bg-brand-500/5">
            <div className="flex items-start gap-3">
              <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-brand-500 text-white">
                <CardIcon className="h-5 w-5" />
              </div>
              <div>
                <h4 className="font-semibold text-gray-900 dark:text-white">
                  Access cards
                </h4>
                <p className="mt-0.5 text-xs leading-5 text-gray-500 dark:text-gray-400">
                  Physical card collection for this person.
                </p>
              </div>
            </div>
            <div className="mt-4 flex items-center justify-between">
              <span className="text-sm font-medium text-brand-700 dark:text-brand-300">
                {dto.cards.length} of {maxCards} cards added
              </span>
              <Button
                disabled={readOnly || dto.cards.length >= maxCards}
                size="sm"
                startIcon={<AddIcon className="h-4 w-4" />}
                onClick={() => setCardModal(true)}
              >
                Add card
              </Button>
            </div>
          </div>
        </div>
        <div className="mt-5 rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] p-4 sm:p-5">
          <div className="mb-4 flex flex-wrap items-center justify-between gap-3">
            <div>
              <h4 className="font-semibold text-gray-900 dark:text-white">
                Card collection
              </h4>
              <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
                Select a card to manage it.
              </p>
            </div>
            <Button
              disabled={readOnly || selectedCardNumber === null}
              size="sm"
              variant="outline"
              startIcon={<TrashBinIcon className="h-4 w-4" />}
              onClick={removeSelected}
            >
              Remove selected
            </Button>
          </div>
          {dto.cards.length === 0 ? (
            <div className="flex min-h-40 flex-col items-center justify-center rounded-xl border border-dashed border-gray-300 px-4 text-center dark:border-gray-700">
              <div className="mb-3 flex h-11 w-11 items-center justify-center rounded-full bg-gray-100 text-gray-500 dark:bg-gray-800">
                <CardIcon className="h-5 w-5" />
              </div>
              <p className="font-medium text-gray-800 dark:text-white">
                No access cards yet
              </p>
              <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
                Add a card manually or scan one from a reader.
              </p>
            </div>
          ) : (
            <div className="grid gap-3 md:grid-cols-2 xl:grid-cols-3">
              {dto.cards.map((item) => {
                const selected = selectedCardNumber === item.cardNumber;
                return (
                  <button
                    type="button"
                    key={`${item.fac}-${item.cardNumber}`}
                    onClick={() => setSelectedCardNumber(item.cardNumber)}
                    className={`rounded-xl border p-4 text-left transition ${selected ? "border-brand-500 bg-brand-50 ring-2 ring-brand-500/15 dark:bg-brand-500/10" : "border-[var(--app-panel-border)] hover:border-brand-200 dark:hover:border-brand-500/50"}`}
                  >
                    <div className="flex items-start justify-between">
                      <div className="flex h-9 w-9 items-center justify-center rounded-lg bg-gray-100 text-gray-600 dark:bg-gray-800 dark:text-gray-300">
                        <CardIcon className="h-5 w-5" />
                      </div>
                      {selected && (
                        <span className="rounded-full bg-brand-500 px-2 py-0.5 text-xs font-semibold text-white">
                          Selected
                        </span>
                      )}
                    </div>
                    <p className="mt-4 text-lg font-semibold text-gray-900 dark:text-white">
                      {item.cardNumber}
                    </p>
                    <div className="mt-3 flex gap-2 text-xs text-gray-500 dark:text-gray-400">
                      <span className="rounded-md bg-gray-100 px-2 py-1 dark:bg-gray-800">
                        {item.bits}-bit
                      </span>
                      <span className="rounded-md bg-gray-100 px-2 py-1 dark:bg-gray-800">
                        Facility {item.fac}
                      </span>
                    </div>
                  </button>
                );
              })}
            </div>
          )}
        </div>
      </FormSection>
    </>
  );
};
