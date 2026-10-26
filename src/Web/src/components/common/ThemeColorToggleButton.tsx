import { useEffect, useRef, useState } from "react";
import { useTheme } from "../../context/ThemeContext";
import { COLOR_PALETTES } from "../../context/ThemePalettes";

export const ThemeColorToggleButton: React.FC = () => {
  const { colorPalette, setColorPalette } = useTheme();
  const [isOpen, setIsOpen] = useState(false);
  const containerRef = useRef<HTMLDivElement>(null);
  const triggerRef = useRef<HTMLButtonElement>(null);
  const selectedPalette =
    COLOR_PALETTES.find((palette) => palette.id === colorPalette) ??
    COLOR_PALETTES[0];

  useEffect(() => {
    if (!isOpen) return;

    const handlePointerDown = (event: PointerEvent) => {
      if (
        event.target instanceof Node &&
        !containerRef.current?.contains(event.target)
      ) {
        setIsOpen(false);
      }
    };
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === "Escape") {
        setIsOpen(false);
        triggerRef.current?.focus();
      }
    };

    document.addEventListener("pointerdown", handlePointerDown);
    document.addEventListener("keydown", handleKeyDown);
    return () => {
      document.removeEventListener("pointerdown", handlePointerDown);
      document.removeEventListener("keydown", handleKeyDown);
    };
  }, [isOpen]);

  return (
    <div ref={containerRef} className="relative">
      <button
        ref={triggerRef}
        type="button"
        title="Choose color theme"
        aria-label={`Choose color theme. Current theme: ${selectedPalette.name}`}
        aria-haspopup="dialog"
        aria-expanded={isOpen}
        aria-controls="color-theme-menu"
        onClick={() => setIsOpen((open) => !open)}
        className="relative flex h-11 w-11 items-center justify-center rounded-lg border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] text-gray-500 shadow-theme-xs transition-colors hover:border-brand-200 hover:text-brand-500"
      >
        <svg
          width="20"
          height="20"
          viewBox="0 0 20 20"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
          aria-hidden="true"
        >
          <path
            d="M10 2.08337C5.62775 2.08337 2.08337 5.62775 2.08337 10C2.08337 14.3723 5.62775 17.9167 10 17.9167C11.3807 17.9167 12.5 16.7974 12.5 15.4167V15.0693C12.5 14.3374 13.0874 13.75 13.8193 13.75H14.1667C16.2378 13.75 17.9167 12.0711 17.9167 10C17.9167 5.62775 14.3723 2.08337 10 2.08337Z"
            stroke="currentColor"
            strokeWidth="1.5"
          />
          <circle cx="6.25" cy="9.16671" r="1.04167" fill="currentColor" />
          <circle cx="9.58325" cy="6.66671" r="1.04167" fill="currentColor" />
          <circle cx="13.3333" cy="8.33329" r="1.04167" fill="currentColor" />
        </svg>
        <span
          className="absolute right-1 top-1 h-3 w-3 rounded-full border border-white"
          style={{ backgroundColor: selectedPalette.color }}
        />
      </button>

      {isOpen && (
        <div
          id="color-theme-menu"
          role="dialog"
          aria-label="Choose a color theme"
          className="absolute right-0 top-full z-99999 mt-2 w-72 rounded-xl border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] p-3 shadow-theme-lg"
        >
          <div className="mb-2">
            <p className="text-sm font-semibold text-gray-800 dark:text-white/90">
              Color theme
            </p>
            <p className="mt-0.5 text-xs text-gray-500 dark:text-gray-400">
              Matching colors update across the app.
            </p>
          </div>
          <div className="max-h-[min(70vh,28rem)] space-y-1 overflow-y-auto">
            {COLOR_PALETTES.map((palette) => {
              const isSelected = palette.id === colorPalette;

              return (
                <button
                  key={palette.id}
                  type="button"
                  aria-label={`Use ${palette.name} color theme`}
                  aria-pressed={isSelected}
                  onClick={() => {
                    setColorPalette(palette.id);
                    setIsOpen(false);
                  }}
                  className={`flex w-full items-center gap-3 rounded-lg border px-2.5 py-2 text-left transition-colors ${
                    isSelected
                      ? "border-brand-200 bg-brand-50 dark:border-brand-700 dark:bg-brand-950/40"
                      : "border-transparent hover:bg-gray-50 dark:hover:bg-white/5"
                  }`}
                >
                  <div
                    className="flex h-7 w-10 shrink-0 overflow-hidden rounded-md border border-black/5"
                    aria-hidden="true"
                  >
                    <span
                      className="w-1/3"
                      style={{
                        backgroundColor: `color-mix(in srgb, ${palette.color} 18%, white)`
                      }}
                    />
                    <span
                      className="w-1/3"
                      style={{ backgroundColor: palette.color }}
                    />
                    <span
                      className="w-1/3"
                      style={{
                        backgroundColor: `color-mix(in srgb, ${palette.color} 72%, black)`
                      }}
                    />
                  </div>
                  <span className="flex-1 text-sm font-medium text-gray-700 dark:text-gray-200">
                    {palette.name}
                  </span>
                  {isSelected && (
                    <span
                      className="h-2 w-2 rounded-full"
                      style={{ backgroundColor: palette.color }}
                    />
                  )}
                </button>
              );
            })}
          </div>
        </div>
      )}
    </div>
  );
};
