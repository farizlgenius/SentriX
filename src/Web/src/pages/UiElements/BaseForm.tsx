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
}

export const BaseForm: React.FC<PropsWithChildren<FormProp>> = ({
  tabContent,
  type,
  handleClick,
  header = "",
  desc = "",
  layout = undefined,
}) => {
  const [activeTab, setActiveTab] = useState<string>(tabContent[0].label);
  const currentStepIndex = Math.max(
    0,
    tabContent.findIndex((tab) => tab.label === activeTab),
  );

  // const currentStep = tabContent[currentStepIndex];
  const isFirstStep = currentStepIndex === 0;
  const isLastStep = currentStepIndex === tabContent.length - 1;

  const goToStep = (stepIndex: number) => {
    if (stepIndex < 0 || stepIndex >= tabContent.length) return;
    setActiveTab(tabContent[stepIndex].label);
  };

  return (
    <div className="rounded-[32px] border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] p-6 shadow-theme-xs lg:p-8">
      <div className="mb-6 rounded-[24px] bg-[linear-gradient(180deg,rgba(59,130,246,0.10),rgba(255,255,255,0))] p-5 dark:bg-[linear-gradient(180deg,rgba(59,130,246,0.18),rgba(17,24,39,0))]">
        <p className="text-xs font-semibold uppercase tracking-[0.24em] text-brand-500">
          SentriX
        </p>
        <h2 className="mt-2 text-2xl font-semibold text-gray-900 dark:text-white">
          {header}
        </h2>
        <p className="mt-2 text-sm text-gray-500 dark:text-gray-400">{desc}</p>
      </div>

      {tabContent.length != 0 && (
        <StepProgress
          steps={tabContent.map((tab) => ({
            key: tab.label,
            title: tab.label,
            detail: tab.label,
            icon: tab.icon,
          }))}
          activeIndex={currentStepIndex}
          onStepClick={goToStep}
        />
      )}
      <div className={layout ? "grid grid-cols-5 gap-5" : ""}>
        {layout && (
          <FormSection
            overall="Door layout detail"
            title="Door Component"
            description="Diagram for show dooe component selected each component to setting."
            className="col-span-3"
          >
            {layout}
          </FormSection>
        )}

        {tabContent.map((a: FormContent, i: number) => {
          return (
            <>
              {activeTab == a.label && (
                <FormSection
                  overall={a.label}
                  title={a.title}
                  description={a.description}
                  key={i}
                  className="col-span-2 flex flex-col"
                >
                  <div className="flex flex-col h-full justify-between">
                    <div className="text-sm text-gray-500 dark:text-gray-400">
                      {a.content}
                    </div>
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
                      <div className="flex gap-3">
                        <Button
                          variant="danger"
                          onClickWithEvent={handleClick}
                          name="close"
                          className="min-w-[120px]"
                          size="sm"
                        >
                          Cancel
                        </Button>
                        {isLastStep ? (
                          <Button
                            disabled={type == FormType.INFO}
                            onClickWithEvent={handleClick}
                            name={type == FormType.UPDATE ? "update" : "create"}
                            className="min-w-[120px]"
                            size="sm"
                          >
                            {type == FormType.UPDATE ? "Update" : "Create"}
                          </Button>
                        ) : (
                          <Button
                            onClick={() => goToStep(currentStepIndex + 1)}
                            className="min-w-[120px]"
                            size="sm"
                          >
                            Next
                          </Button>
                        )}
                      </div>
                    </div>
                  </div>
                </FormSection>
              )}
            </>
          );
        })}
      </div>
    </div>
  );

 
};
