import React, { PropsWithChildren, useState } from "react";
import Button from "../../components/ui/button/Button";
import { GroupForm } from "../../components/form/user/GroupForm";
import { CredentialForm } from "../../components/form/user/CredentialForm";
import { PersonalInformationForm } from "../../components/form/user/PersonalInformationForm";
import { UserSettingForm } from "../../components/form/user/UserSettingForm";
import { UserDto } from "../../model/User/UserDto";
import { FormProp, FormType } from "../../model/Form/FormProp";
import StepProgress from "../../components/form/StepProgress";
import { UserOperatorForm } from "../../components/form/user/UserOperatorForm";
import { LocationForm } from "../../components/form/user/LocationForm";

interface UserFormProps extends FormProp<UserDto> {
  image: File | undefined;
  setImage: React.Dispatch<React.SetStateAction<File | undefined>>;
}

enum UserFormStep {
  Personal,
  Operator,
  Location,
  AccessLevel,
  Credential,
  Setting,
}

const userFormSteps = [
  {
    step: UserFormStep.Personal,
    title: "Personal Information",
    detail: "Identity, contact and company details.",
  },
  {
    step: UserFormStep.Operator,
    title: "Operator",
    detail: "Assign operator access for this user.",
  },
  {
    step: UserFormStep.Location,
    title: "Locations",
    detail: "Assign locations access for this user.",
  },
  {
    step: UserFormStep.AccessLevel,
    title: "Access Group",
    detail: "Assign access groups for this user.",
  },
  {
    step: UserFormStep.Credential,
    title: "Credentials",
    detail: "Manage cards and activation dates.",
  },
  {
    step: UserFormStep.Setting,
    title: "Settings",
    detail: "Set cardholder behavior flags",
  },
];

const UserForm: React.FC<PropsWithChildren<UserFormProps>> = ({
  dto,
  setDto,
  handleClick,
  image,
  setImage,
  type,
}) => {
  const [activeStep, setActiveStep] = useState<number>(UserFormStep.Personal);
  const currentStepIndex = userFormSteps.findIndex(
    (x) => x.step === activeStep,
  );
  const currentStep = userFormSteps[currentStepIndex];
  const isFirstStep = currentStepIndex === 0;
  const isLastStep = currentStepIndex === userFormSteps.length - 1;

  const goToStep = (stepIndex: number) => {
    if (stepIndex < 0 || stepIndex >= userFormSteps.length) return;
    setActiveStep(userFormSteps[stepIndex].step);
  };

  return (
    <div className="flex flex-col gap-5 p-5 border border-gray-200 rounded-2xl dark:border-gray-800 lg:p-6">
      <StepProgress
        steps={userFormSteps.map((step) => ({
          key: step.step,
          title: step.title,
          detail: step.detail,
        }))}
        activeIndex={currentStepIndex}
        onStepClick={goToStep}
      />

      <div className="rounded-xl border border-gray-200 p-4 dark:border-gray-800 lg:p-6">
        <div className="mb-4">
          <h3 className="text-lg font-semibold text-gray-900 dark:text-white">
            {currentStep?.title}
          </h3>
          <p className="text-sm text-gray-500 dark:text-gray-400">
            {currentStep?.detail}
          </p>
        </div>

        {activeStep === UserFormStep.Personal && (
          <PersonalInformationForm
            type={type}
            dto={dto}
            setDto={setDto}
            handleClick={handleClick}
            image={image}
            setImage={setImage}
          />
        )}

        {activeStep == UserFormStep.Operator && (
          <UserOperatorForm type={type} setDto={setDto} dto={dto} />
        )}

        {activeStep == UserFormStep.Location && (
          <LocationForm type={type} setDto={setDto} dto={dto} />
        )}

        {activeStep === UserFormStep.AccessLevel && (
          <GroupForm
            type={type}
            dto={dto}
            setDto={setDto}
            handleClick={handleClick}
          />
        )}

        {activeStep === UserFormStep.Credential && (
          <CredentialForm
            type={type}
            dto={dto}
            setDto={setDto}
            handleClick={handleClick}
          />
        )}

        {activeStep === UserFormStep.Setting && (
          <UserSettingForm
            type={type}
            dto={dto}
            setDto={setDto}
            handleClick={handleClick}
          />
        )}

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
    </div>
  );
};

export default UserForm;
