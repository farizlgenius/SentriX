import React from "react";

export interface StepProgressItem {
  key: string | number;
  title: string;
  detail?: string;
  icon?: React.ReactNode;
}

interface StepProgressProps {
  steps: StepProgressItem[];
  activeIndex: number;
  onStepClick: (index: number) => void;
}

const StepProgress: React.FC<StepProgressProps> = ({ steps, activeIndex, onStepClick }) => {
  const safeActiveIndex = Math.max(0, Math.min(activeIndex, steps.length - 1));
  const progressRatio = steps.length > 1 ? safeActiveIndex / (steps.length - 1) : 0;
  const minWidth = Math.max(steps.length * 112, 360);

  return (
    <div className="overflow-x-auto pb-2">
      <div
        className="relative rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] px-8 py-5 shadow-theme-sm"
        style={{ minWidth }}
      >
        <div className="absolute left-16 right-16 top-9 h-px bg-gray-200 dark:bg-gray-700" />
        <div
          className="absolute left-16 top-9 h-px bg-brand-500 transition-all duration-300 dark:bg-brand-400"
          style={{
            width: safeActiveIndex === 0 ? "0%" : `calc((100% - 8rem) * ${progressRatio})`
          }}
        />
        <div className="relative grid" style={{ gridTemplateColumns: `repeat(${steps.length}, minmax(96px, 1fr))` }}>
          {steps.map((step, index) => {
            const isComplete = index < safeActiveIndex;
            const isActive = index === safeActiveIndex;
            const isReached = isComplete || isActive;

            return (
              <button
                key={step.key}
                type="button"
                onClick={() => onStepClick(index)}
                className="group flex min-h-[74px] flex-col items-center gap-2 text-center outline-none"
              >
                <span
                  className={`relative z-10 flex h-9 w-9 items-center justify-center rounded-xl border-4 border-[var(--app-panel-bg)] text-xs font-semibold shadow-theme-sm transition-all ${
                    isReached
                      ? "bg-brand-500 text-white ring-4 ring-brand-100 dark:bg-brand-400 dark:ring-brand-900"
                      : "bg-gray-50 text-gray-400 ring-1 ring-gray-200 group-hover:text-brand-500 dark:bg-gray-800 dark:text-gray-500 dark:ring-gray-700"
                  }`}
                >
                  {isComplete ? (
                    <svg className="h-4 w-4" viewBox="0 0 16 16" fill="none" aria-hidden="true">
                      <path d="M12.7 4.7 6.6 10.8 3.3 7.5" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
                    </svg>
                  ) : step.icon ? (
                    <span className="flex h-4 w-4 items-center justify-center [&_svg]:h-4 [&_svg]:w-4">
                      {step.icon}
                    </span>
                  ) : (
                    <span className="h-2 w-2 rounded-sm bg-current" />
                  )}
                </span>
                <span className={`text-xs font-semibold leading-4 ${isReached ? "text-gray-900 dark:text-white" : "text-gray-500 dark:text-gray-400"}`}>
                  Step {index + 1}
                </span>
                <span className="max-w-[112px] truncate text-[11px] leading-4 text-gray-500 dark:text-gray-400">
                  {step.detail || step.title}
                </span>
              </button>
            );
          })}
        </div>
      </div>
    </div>
  );
};

export default StepProgress;
