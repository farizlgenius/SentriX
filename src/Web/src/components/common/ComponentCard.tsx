interface ComponentCardProps {
  title?: string;
  children: React.ReactNode;
  className?: string; // Additional custom classes for styling
  desc?: string; // Description text
}

const ComponentCard: React.FC<ComponentCardProps> = ({
  title,
  children,
  className = "",
  desc = "",
}) => {
  return (
    <div
      className={`console-card overflow-hidden rounded-[28px] border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] shadow-theme-xs ${className}`}
    >
      <div className="border-b border-[var(--app-panel-border)] bg-[linear-gradient(180deg,rgba(59,130,246,0.08),rgba(255,255,255,0))] px-6 py-5 dark:bg-[linear-gradient(180deg,rgba(59,130,246,0.12),rgba(17,24,39,0))]">
        <h3 className="text-lg font-semibold text-gray-800 dark:text-white/90">
          {title}
        </h3>
        {desc && (
          <p className="mt-1.5 max-w-2xl text-sm text-gray-500 dark:text-gray-400">
            {desc}
          </p>
        )}
      </div>

      <div className="p-5 sm:p-6 lg:p-8">
        <div className="space-y-6">{children}</div>
      </div>
    </div>
  );
};

export default ComponentCard;
