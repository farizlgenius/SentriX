import { PropsWithChildren, useEffect, useState } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import { AlertLevelDto } from "../../model/Alert/AlertLevelDto";


const PRESET_COLORS = [
      "#3B82F6", // Blue
      "#6366F1", // Indigo
      "#8B5CF6", // Purple
      "#EC4899", // Pink
      "#EF4444", // Red
      "#10B981", // Emerald
      "#F59E0B", // Amber
];



const AlertLevelForm: React.FC<PropsWithChildren<FormProp<AlertLevelDto>>> = ({
      dto,
      type,
      setDto,
}) => {
      const isReadOnly = type == FormType.INFO;

      return (
            <div className="grid grid-cols-2 gap-5">
                  <FormField>
                        <Label>Name</Label>
                        <Input type="text" disabled={isReadOnly}
                              placeholder="Action name"
                              onChange={(e) => {
                                    setDto(prev => ({
                                          ...prev,
                                          name: e.target.value
                                    }))
                              }}
                              value={dto.name}
                        />
                  </FormField>
                  <FormField>
                        <Label>Color</Label>
                        <div className="flex flex-col gap-2.5">

                              {/* Main Input Bar */}
                              <div className="flex items-center gap-3 rounded-lg border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] p-2 shadow-sm transition-all focus-within:border-brand-500 focus-within:ring-2 focus-within:ring-brand-500/20">
                                    {/* Visual Color Preview Trigger */}
                                    <label className="relative flex h-9 w-9 shrink-0 cursor-pointer items-center justify-center rounded-md border border-gray-200 shadow-inner dark:border-gray-700">
                                          <span
                                                className="h-full w-full rounded-md transition-transform hover:scale-105"
                                                style={{ backgroundColor: dto.colorCode }}
                                          />
                                          <input
                                                type="color"
                                                value={dto.colorCode}
                                                onChange={(e) => setDto(prev => ({
                                                      ...prev,
                                                      colorCode: e.target.value
                                                }))}
                                                className="absolute inset-0 cursor-pointer opacity-0"
                                                aria-label="Pick custom color"
                                          />
                                    </label>

                                    {/* Hex Text Input */}
                                    <input
                                          type="text"
                                          value={dto.colorCode}
                                          onChange={(e) => setDto(prev => ({
                                                ...prev,
                                                colorCode: e.target.value
                                          }))}
                                          placeholder="#000000"
                                          className="w-full bg-transparent font-mono text-sm uppercase text-gray-800 focus:outline-none dark:text-gray-100"
                                    />
                              </div>

                              {/* Quick Select Color Palette */}
                              <div className="flex items-center gap-2 pt-1">
                                    <span className="text-xs text-gray-500 dark:text-gray-400">Presets:</span>
                                    <div className="flex items-center gap-1.5">
                                          {PRESET_COLORS.map((color) => {
                                                const isSelected = dto.colorCode.toLowerCase() === color.toLowerCase();
                                                return (
                                                      <button
                                                            key={color}
                                                            type="button"
                                                            onClick={() => setDto(prev => ({
                                                                  ...prev,
                                                                  colorCode: color
                                                            }))}
                                                            style={{ backgroundColor: color }}
                                                            className={`h-6 w-6 rounded-full transition-all hover:scale-110 focus:outline-none focus:ring-2 focus:ring-offset-1 ${isSelected
                                                                  ? "ring-2 ring-brand-500 ring-offset-2 scale-110"
                                                                  : "hover:shadow-md"
                                                                  }`}
                                                            title={color}
                                                            aria-label={`Select color ${color}`}
                                                      />
                                                );
                                          })}
                                    </div>
                              </div>
                        </div>
                  </FormField>





            </div>
      );
};

export default AlertLevelForm;
