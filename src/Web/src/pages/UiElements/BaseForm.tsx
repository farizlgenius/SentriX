import React, { JSX, PropsWithChildren, useState } from "react";
import { FormContent } from "../../model/Form/FormContent";
import StepProgress from "../../components/form/StepProgress";
import Button from "../../components/ui/button/Button";
import { FormType } from "../../model/Form/FormProp";
import { FormSection } from "../../components/form/template/FormTemplate";

interface FormProp {
  tabContent: FormContent[];
  header?: string;
  desc?: string;
  type: FormType;
  handleClick?: (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
  layout?: JSX.Element | undefined;
  isNext?:boolean;
  activeTab?: string;
  onActiveTabChange?: (tab: string) => void;
  validateSteps?: boolean;
  submitOnEveryStep?: boolean;
  canSubmit?: boolean;
  submitHint?: string;
}

export const BaseForm: React.FC<PropsWithChildren<FormProp>> = ({
  tabContent,
  type,
  handleClick,
  header = "",
  desc = "",
  layout = undefined,
  isNext=true,
  activeTab: controlledActiveTab,
  onActiveTabChange,
  validateSteps = true,
  submitOnEveryStep = false,
  canSubmit = true,
  submitHint,
}) => {
  const [internalActiveTab, setInternalActiveTab] = useState<string>(
    tabContent[0]?.label ?? "",
  );
  const activeTab = tabContent.some((tab) => tab.label === controlledActiveTab)
    ? controlledActiveTab!
    : tabContent.some((tab) => tab.label === internalActiveTab)
      ? internalActiveTab
      : tabContent[0]?.label ?? "";
  const currentStepIndex = Math.max(
    0,
    tabContent.findIndex((tab) => tab.label === activeTab),
  );

  // const currentStep = tabContent[currentStepIndex];
  const isFirstStep = currentStepIndex === 0;
  const isLastStep = currentStepIndex === tabContent.length - 1;

  const goToStep = (stepIndex: number) => {
    if (stepIndex < 0 || stepIndex >= tabContent.length) return;
    const nextTab = tabContent[stepIndex].label;
    if (controlledActiveTab === undefined) {
      setInternalActiveTab(nextTab);
    }
    onActiveTabChange?.(nextTab);
  };

  return (
    <>
      {(header || desc) && (
        <div className="mb-5 border-b border-[var(--app-panel-border)] px-1 pb-5">
          <p className="text-xs font-semibold uppercase tracking-[0.2em] text-brand-500">
            SentriX
          </p>
          {header && (
            <h2 className="mt-1 text-2xl font-semibold text-gray-900 dark:text-white">
              {header}
            </h2>
          )}
          {desc && (
            <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
              {desc}
            </p>
          )}
        </div>
      )}

      {tabContent.length > 1  && (
        <StepProgress
          steps={tabContent.map((tab) => ({
            key: tab.label,
            title: tab.label,
            detail: tab.label,
            icon: tab.icon,
          }))}
          activeIndex={currentStepIndex}
          onStepClick={goToStep}
          className="mb-4"
        />
      )}
      <div className={layout ? "grid grid-cols-1 gap-5 xl:grid-cols-5" : ""}>
        {layout && (
          <FormSection
            overall="Door layout detail"
            title="Door component layout"
            description="Click a component to edit it. Green means configured; blue marks your selection."
            className="xl:col-span-3"
          >
            {layout}
          </FormSection>
        )}

        {tabContent.map((a: FormContent, i: number) => {
          return (
            <React.Fragment key={i}>
              {activeTab == a.label && (
                 <div className="flex flex-col h-full justify-between xl:col-span-2">
                    <div className="text-sm text-gray-500 dark:text-gray-400">
                      {a.content}
                    </div>
                    <div>
                      <div className="mt-6 flex w-full items-center justify-between gap-3">
                      <div>
                        {!isFirstStep && (
                          <Button
                            variant="outline"
                            onClick={() => goToStep(currentStepIndex - 1)}
                            className="min-w-[120px]"
                            size="sm"
                          >
                            Back
                          </Button>
                        )}
                      </div>
                      <div className="flex flex-wrap justify-end gap-3">
                        <Button
                          onClickWithEvent={handleClick}
                          name="close"
                          className="min-w-[120px]"
                          size="sm"
                        >
                          Cancel
                        </Button>
                        {!isLastStep && (
                          <Button
                            disabled={validateSteps && !isNext}
                            onClick={() => {
                              goToStep(currentStepIndex + 1)
                            }}
                            className="min-w-[120px]"
                            size="sm"
                          >
                            Next
                          </Button>
                        )}
                        {(isLastStep || submitOnEveryStep) && (
                          <Button
                            disabled={type == FormType.INFO || !canSubmit}
                            onClickWithEvent={handleClick}
                            name={type == FormType.UPDATE ? "update" : "create"}
                            className="min-w-[120px]"
                            size="sm"
                          >
                            {type == FormType.UPDATE ? "Update" : type == FormType.INFO ? "Info" : "Create"}
                          </Button>
                        )}
                      </div>
                    </div>
                    {submitHint && (
                      <p className="mt-3 text-xs text-gray-500 dark:text-gray-400">
                        {submitHint}
                      </p>
                    )}

                    </div>
                    
                  </div>
              )}
            </React.Fragment>
          );
        })}
      </div>
    </>
  );

 
};
