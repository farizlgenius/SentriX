import React from "react";
import Button from "../../ui/button/Button";

interface FormShellProps {
  eyebrow?: string;
  title: string;
  description?: string;
  children: React.ReactNode;
  className?: string;
}

interface FormSectionProps {
  title?: string;
  description?: string;
  children: React.ReactNode;
  className?: string;
}

interface FormActionsProps {
  typeLabel: "Create" | "Update";
  disabled?: boolean;
  submitName: string;
  cancelName?: string;
  onSubmit?: (e: React.MouseEvent<HTMLButtonElement>) => void;
  onCancel?: (e: React.MouseEvent<HTMLButtonElement>) => void;
  submitLabel?: string;
  cancelLabel?: string;
}

// export const FormShell: React.FC<FormShellProps> = ({
//   eyebrow = "Form",
//   title,
//   description,
//   children,
//   className = "",
// }) => {
//   return (
//     <div className={`rounded-[28px] border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] p-6 shadow-theme-xs lg:p-8 ${className}`}>
//       <div className="mb-6 rounded-[24px] bg-[linear-gradient(180deg,rgba(59,130,246,0.10),rgba(255,255,255,0))] p-5 dark:bg-[linear-gradient(180deg,rgba(59,130,246,0.18),rgba(17,24,39,0))]">
//         <p className="text-xs font-semibold uppercase tracking-[0.24em] text-brand-500">{eyebrow}</p>
//         <h2 className="mt-2 text-2xl font-semibold text-gray-900 dark:text-white">{title}</h2>
//         {description && (
//           <p className="mt-2 max-w-2xl text-sm text-gray-500 dark:text-gray-400">{description}</p>
//         )}
//       </div>
//       <div className="space-y-6">{children}</div>
//     </div>
//   );
// };

export const FormSection: React.FC<FormSectionProps> = ({
  title,
  description,
  children,
  className = "",
}) => {
  return (
    <section className={`rounded-[24px] border border-[var(--app-panel-border)] bg-[var(--app-panel-muted)]/30 p-5 lg:p-6 ${className}`}>
      {(title || description) && (
        <div className="mb-5">
          {title && <h3 className="text-lg font-semibold text-gray-900 dark:text-white">{title}</h3>}
          {description && <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">{description}</p>}
        </div>
      )}
      {children}
    </section>
  );
};

export const FormGrid: React.FC<{ children: React.ReactNode; className?: string }> = ({ children, className = "" }) => {
  return <div className={`grid gap-5 md:grid-cols-2 ${className}`}>{children}</div>;
};

export const FormField: React.FC<{ children: React.ReactNode; className?: string }> = ({ children, className = "" }) => {
  return <div className={`space-y-1 ${className}`}>{children}</div>;
};

export const FormActions: React.FC<FormActionsProps> = ({
  typeLabel,
  disabled = false,
  submitName,
  cancelName = "cancel",
  onSubmit,
  onCancel,
  submitLabel,
  cancelLabel = "Cancel",
}) => {
  return (
    <div className="mt-5 flex flex-wrap justify-end gap-3">
      <Button
        disabled={disabled}
        onClickWithEvent={onSubmit}
        name={submitName}
        size="sm"
      >
        {submitLabel ?? typeLabel}
      </Button>
      <Button
        variant="outline"
        onClickWithEvent={onCancel}
        name={cancelName}
        size="sm"
      >
        {cancelLabel}
      </Button>
    </div>
  );
};
