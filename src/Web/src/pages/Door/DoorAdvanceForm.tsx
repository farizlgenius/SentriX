import { PropsWithChildren, useEffect, useState } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import Select from "../../components/form/Select";
import { Vendor } from "../../enum/Vendor";
import { DoorType } from "../../enum/DoorType";
import { Options } from "../../model/Options";
import { AeroDoorMetadata } from "../../model/Door/AeroDoorMetadata";
import { DoorMode } from "../../enum/DoorMode";
import Switch from "../../components/form/switch/Switch";



const DoorAdvanceForm: React.FC<PropsWithChildren<FormProp<DoorDto>>> = ({
      dto,
      type,
      handleChange,
      setDto,
      setIsNext
}) => {
      const isReadOnly = type == FormType.INFO;
      //const [deviceOption,setDeviceOption] = useState<Options[]>([]);

      // NEW: Automatically validate whenever name or deviceGuid changes
      useEffect(() => {
            if (setIsNext) {
                  // .trim() ensures spaces aren't counted as valid input
                  const isValid = dto.name.trim() !== "" && dto.deviceGuid.trim() !== "";
                  setIsNext(isValid);
            }
      }, [dto.name, dto.deviceGuid, setIsNext]);


      return (
            <div className="grid grid-cols-2 gap-5">
                  <FormField>
                        <Label>Default Mode</Label>
                        <Select
                              disabled={isReadOnly}
                              name="default"
                              options={
                                    [
                                          {
                                                label: DoorMode[DoorMode.Disabled],
                                                value: DoorMode.Disabled,
                                          },
                                          {
                                                label: DoorMode[DoorMode.Unlocked],
                                                value: DoorMode.Unlocked,
                                          },
                                          {
                                                label: DoorMode[DoorMode.Locked],
                                                value: DoorMode.Locked,
                                          },
                                          {
                                                label: DoorMode[DoorMode.Facility],
                                                value: DoorMode.Facility,
                                          },
                                          {
                                                label: DoorMode[DoorMode.Card],
                                                value: DoorMode.Card,
                                          },
                                          {
                                                label: DoorMode[DoorMode.CardPIN],
                                                value: DoorMode.CardPIN,
                                          },
                                          {
                                                label: DoorMode[DoorMode.CardOrPIN],
                                                value: DoorMode.CardOrPIN,
                                          },

                                    ]}
                              onChange={(e) => {
                                    setDto(prev => ({
                                          ...prev,
                                          metadata: {
                                                ...(prev.metadata as AeroDoorMetadata),
                                                defaultMode: Number(e.target.value)
                                          }
                                    }))

                              }

                              }
                              defaultValue={(dto.metadata as AeroDoorMetadata).defaultMode ?? DoorMode.Card}
                        />
                  </FormField>
                  <FormField>
                        <Label>Offline Mode</Label>
                        <Select
                              disabled={isReadOnly}
                              name="offline"
                              options={
                                    [
                                          {
                                                label: DoorMode[DoorMode.Disabled],
                                                value: DoorMode.Disabled,
                                          },
                                          {
                                                label: DoorMode[DoorMode.Unlocked],
                                                value: DoorMode.Unlocked,
                                          },
                                          {
                                                label: DoorMode[DoorMode.Locked],
                                                value: DoorMode.Locked,
                                          },
                                          {
                                                label: DoorMode[DoorMode.Facility],
                                                value: DoorMode.Facility,
                                          },
                                          {
                                                label: DoorMode[DoorMode.Card],
                                                value: DoorMode.Card,
                                          },
                                          {
                                                label: DoorMode[DoorMode.CardPIN],
                                                value: DoorMode.CardPIN,
                                          },
                                          {
                                                label: DoorMode[DoorMode.CardOrPIN],
                                                value: DoorMode.CardOrPIN,
                                          },

                                    ]}
                              onChange={(e) => {
                                    setDto(prev => ({
                                          ...prev,
                                          metadata: {
                                                ...(prev.metadata as AeroDoorMetadata),
                                                offlineMode: Number(e.target.value)
                                          }
                                    }))

                              }

                              }
                              defaultValue={(dto.metadata as AeroDoorMetadata).offlineMode ?? DoorMode.Unlocked}
                        />
                  </FormField>
                   <FormField>
                        <Label>LED Mode</Label>
                        <Select
                              disabled={isReadOnly}
                              name="led"
                              options={
                                    [
                                          
                                          {
                                                label: "No Change",
                                                value: 0,
                                          },
                                          {
                                                label: "Table 1",
                                                value: 1,
                                          },
                                          {
                                                label: "Table 2",
                                                value: 2,
                                          },
                                          {
                                                label: "Table 3",
                                                value: 3,
                                          }
                                    ]}
                              onChange={(e) => {
                                    setDto(prev => ({
                                          ...prev,
                                          metadata: {
                                                ...(prev.metadata as AeroDoorMetadata),
                                                defaultLedMode: Number(e.target.value)
                                          }
                                    }))

                              }

                              }
                              defaultValue={(dto.metadata as AeroDoorMetadata).defaultLedMode ?? 0}
                        />
                  </FormField>
                  <FormField className="col-span-2">
                        <div className="grid grid-cols-2 gap-5">
                              <Switch label={"Double Swipe Trigger"} 
                              onChange={(checked) => {
                                    setDto((prev) => ({
                                          ...prev,
                                          metadata:{
                                                ...(prev.metadata as AeroDoorMetadata),
                                                doubleCard:checked
                                          }
                                    }))
                              }}
                              defaultChecked={(dto.metadata as AeroDoorMetadata).doubleCard ?? false}
                              />
                              <Switch label={"Elevator Floor Select"}
                              onChange={(checked) => {
                                    setDto((prev) => ({
                                          ...prev,
                                          metadata:{
                                                ...(prev.metadata as AeroDoorMetadata),
                                                outputSelectionTracking:checked
                                          }
                                    }))
                              }}
                              defaultChecked={(dto.metadata as AeroDoorMetadata).outputSelectionTracking ?? false}
                              />
                              <Switch label={"Locked mode override"}
                              onChange={(checked) => {
                                    setDto((prev) => ({
                                          ...prev,
                                          metadata:{
                                                ...(prev.metadata as AeroDoorMetadata),
                                                lockedOverrid:checked
                                          }
                                    }))
                              }}
                              defaultChecked={(dto.metadata as AeroDoorMetadata).lockedOverrid ?? false}
                              />
                              <Switch label={"Decrease Use Limit"}
                              onChange={(checked) => {
                                    setDto((prev) => ({
                                          ...prev,
                                          metadata:{
                                                ...(prev.metadata as AeroDoorMetadata),
                                                decreaseUseLimit:checked
                                          }
                                    }))
                              }}
                              defaultChecked={(dto.metadata as AeroDoorMetadata).decreaseUseLimit ?? false}
                              />
                              <Switch label={"Require 1 Use Limit"}
                              onChange={(checked) => {
                                    setDto((prev) => ({
                                          ...prev,
                                          metadata:{
                                                ...(prev.metadata as AeroDoorMetadata),
                                                requireUseLimit:checked
                                          }
                                    }))
                              }}
                              defaultChecked={(dto.metadata as AeroDoorMetadata).requireUseLimit ?? false}
                              />
                              <Switch label={"Denied Duress"}
                              onChange={(checked) => {
                                    setDto((prev) => ({
                                          ...prev,
                                          metadata:{
                                                ...(prev.metadata as AeroDoorMetadata),
                                                deniedDuress:checked
                                          }
                                    }))
                              }}
                              defaultChecked={(dto.metadata as AeroDoorMetadata).deniedDuress ?? false}
                              />
                              <Switch label={"Quiet REX"}
                              onChange={(checked) => {
                                    setDto((prev) => ({
                                          ...prev,
                                          metadata:{
                                                ...(prev.metadata as AeroDoorMetadata),
                                                quietRex:checked
                                          }
                                    }))
                              }}
                              defaultChecked={(dto.metadata as AeroDoorMetadata).quietRex ?? false}
                              />
                              <Switch label={"Filter Status"}
                              onChange={(checked) => {
                                    setDto((prev) => ({
                                          ...prev,
                                          metadata:{
                                                ...(prev.metadata as AeroDoorMetadata),
                                                filterStatus:checked
                                          }
                                    }))
                              }}
                              defaultChecked={(dto.metadata as AeroDoorMetadata).filterStatus ?? false}
                              />
                              <Switch label={"Double Card Access"}
                              onChange={(checked) => {
                                    setDto((prev) => ({
                                          ...prev,
                                          metadata:{
                                                ...(prev.metadata as AeroDoorMetadata),
                                                doubleCardAccess:checked
                                          }
                                    }))
                              }}
                              defaultChecked={(dto.metadata as AeroDoorMetadata).doubleCardAccess ?? false}
                              />
                              <Switch label={"Host Permission"}
                              onChange={(checked) => {
                                    setDto((prev) => ({
                                          ...prev,
                                          metadata:{
                                                ...(prev.metadata as AeroDoorMetadata),
                                                hostPermission:checked
                                          }
                                    }))
                              }}
                              defaultChecked={(dto.metadata as AeroDoorMetadata).hostPermission ?? false}
                              />
                              <Switch label={"Host Offline Grant"}
                              onChange={(checked) => {
                                    setDto((prev) => ({
                                          ...prev,
                                          metadata:{
                                                ...(prev.metadata as AeroDoorMetadata),
                                                hostOfflineGrant:checked
                                          }
                                    }))
                              }}
                              defaultChecked={(dto.metadata as AeroDoorMetadata).hostOfflineGrant ?? false}
                              />
                        </div>
                  </FormField>
                  {/* <FormField>
        <Label>Device</Label>
        <Select
          isString={true}
          disabled={isReadOnly}
          name={"deviceGuid"}
          options={deviceOption}
          onChange={(e) => {
            setDto(prev => ({
              ...prev,
              deviceGuid: e.target.value
            }))
            handleChange(e)
         
          }
          }
          defaultValue={dto.deviceGuid}
        />
      </FormField> */}
            </div>
      );
};

export default DoorAdvanceForm;
