"use client";

import type React from "react";
import { createContext, useState, useContext, useEffect } from "react";
import { COLOR_PALETTES, type ColorPaletteId } from "./ThemePalettes";

type Theme = "light" | "dark";

type ThemeContextType = {
  theme: Theme;
  accentColor: string;
  colorPalette: ColorPaletteId;
  toggleTheme: () => void;
  setColorPalette: (palette: ColorPaletteId) => void;
};

const ThemeContext = createContext<ThemeContextType | undefined>(undefined);

const DEFAULT_COLOR_PALETTE: ColorPaletteId = "soft-beach";

const normalizeHexColor = (color: string): string => {
  const value = color.trim();
  const shortHex = /^#([0-9a-fA-F]{3})$/;
  const fullHex = /^#([0-9a-fA-F]{6})$/;

  if (shortHex.test(value)) {
    return `#${value
      .slice(1)
      .split("")
      .map((char) => `${char}${char}`)
      .join("")
      .toLowerCase()}`;
  }

  if (fullHex.test(value)) {
    return value.toLowerCase();
  }

  return COLOR_PALETTES[0].color;
};

const getPalette = (id: ColorPaletteId) =>
  COLOR_PALETTES.find((palette) => palette.id === id) ?? COLOR_PALETTES[0];

const hexToRgb = (hex: string) => {
  const normalized = normalizeHexColor(hex).slice(1);
  return {
    r: Number.parseInt(normalized.slice(0, 2), 16),
    g: Number.parseInt(normalized.slice(2, 4), 16),
    b: Number.parseInt(normalized.slice(4, 6), 16)
  };
};

const getClosestPalette = (color: string): ColorPaletteId => {
  const input = hexToRgb(color);
  return COLOR_PALETTES.reduce((closest, palette) => {
    const candidate = hexToRgb(palette.color);
    const distance =
      (input.r - candidate.r) ** 2 +
      (input.g - candidate.g) ** 2 +
      (input.b - candidate.b) ** 2;
    const closestColor = hexToRgb(closest.color);
    const closestDistance =
      (input.r - closestColor.r) ** 2 +
      (input.g - closestColor.g) ** 2 +
      (input.b - closestColor.b) ** 2;

    return distance < closestDistance ? palette : closest;
  }).id;
};

const mixRgb = (
  from: { r: number; g: number; b: number },
  to: { r: number; g: number; b: number },
  weight: number
) => ({
  r: Math.round(from.r * (1 - weight) + to.r * weight),
  g: Math.round(from.g * (1 - weight) + to.g * weight),
  b: Math.round(from.b * (1 - weight) + to.b * weight)
});

const rgbToHex = ({ r, g, b }: { r: number; g: number; b: number }) => {
  const clamp = (n: number) => Math.max(0, Math.min(255, n));
  return `#${[clamp(r), clamp(g), clamp(b)]
    .map((c) => c.toString(16).padStart(2, "0"))
    .join("")}`;
};

const applyAccentTheme = (accentColor: string, theme: Theme) => {
  const root = document.documentElement;
  const base = hexToRgb(accentColor);
  const white = { r: 255, g: 255, b: 255 };
  const black = { r: 0, g: 0, b: 0 };
  const lightSurface = { r: 247, g: 248, b: 250 };
  const darkSurface = { r: 11, g: 15, b: 20 };
  const isDark = theme === "dark";

  const vars: Record<string, string> = {
    "--color-brand-25": rgbToHex(mixRgb(base, white, 0.95)),
    "--color-brand-50": rgbToHex(mixRgb(base, white, 0.9)),
    "--color-brand-100": rgbToHex(mixRgb(base, white, 0.75)),
    "--color-brand-200": rgbToHex(mixRgb(base, white, 0.6)),
    "--color-brand-300": rgbToHex(mixRgb(base, white, 0.4)),
    "--color-brand-400": rgbToHex(mixRgb(base, white, 0.2)),
    "--color-brand-500": rgbToHex(base),
    "--color-brand-600": rgbToHex(mixRgb(base, black, 0.12)),
    "--color-brand-700": rgbToHex(mixRgb(base, black, 0.24)),
    "--color-brand-800": rgbToHex(mixRgb(base, black, 0.38)),
    "--color-brand-900": rgbToHex(mixRgb(base, black, 0.5)),
    "--color-brand-950": rgbToHex(mixRgb(base, black, 0.68)),
    "--shadow-focus-ring": `0px 0px 0px 4px rgba(${base.r}, ${base.g}, ${base.b}, 0.16)`,
    "--app-shell-bg": rgbToHex(
      mixRgb(isDark ? darkSurface : lightSurface, base, isDark ? 0.04 : 0.045)
    )
  };

  const surfaceProperties = [
    "--app-shell-elevated",
    "--app-panel-bg",
    "--app-panel-muted",
    "--app-panel-border",
    "--app-header-bg",
    "--app-sidebar-bg"
  ];

  if (isDark) {
    surfaceProperties.forEach((property) => root.style.removeProperty(property));
  } else {
    const headerRgb = mixRgb(white, base, 0.02);
    const elevatedRgb = mixRgb(white, base, 0.025);
    const sidebarTop = mixRgb(white, base, 0.025);
    const sidebarBottom = mixRgb(lightSurface, base, 0.08);

    Object.assign(vars, {
      "--app-shell-elevated": `rgba(${elevatedRgb.r}, ${elevatedRgb.g}, ${elevatedRgb.b}, 0.88)`,
      "--app-panel-bg": rgbToHex(mixRgb(white, base, 0.018)),
      "--app-panel-muted": rgbToHex(mixRgb(lightSurface, base, 0.055)),
      "--app-panel-border": rgbToHex(
        mixRgb({ r: 213, g: 219, b: 227 }, base, 0.1)
      ),
      "--app-header-bg": `rgba(${headerRgb.r}, ${headerRgb.g}, ${headerRgb.b}, 0.9)`,
      "--app-sidebar-bg": `linear-gradient(180deg, ${rgbToHex(sidebarTop)} 0%, ${rgbToHex(sidebarBottom)} 100%)`
    });
  }

  Object.entries(vars).forEach(([key, value]) => {
    root.style.setProperty(key, value);
  });
};

export const ThemeProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [theme, setTheme] = useState<Theme>("light");
  const [colorPalette, setColorPaletteState] =
    useState<ColorPaletteId>(DEFAULT_COLOR_PALETTE);
  const [isInitialized, setIsInitialized] = useState(false);
  const accentColor = getPalette(colorPalette).color;

  useEffect(() => {
    const savedTheme = localStorage.getItem("theme") as Theme | null;
    const savedPalette = localStorage.getItem("colorPalette");
    const savedAccentColor = localStorage.getItem("accentColor");
    const initialTheme = savedTheme || "light";
    const isKnownPalette = COLOR_PALETTES.some(
      (palette) => palette.id === savedPalette
    );
    const initialPalette = isKnownPalette
      ? (savedPalette as ColorPaletteId)
      : savedAccentColor
        ? getClosestPalette(savedAccentColor)
        : DEFAULT_COLOR_PALETTE;

    setTheme(initialTheme);
    setColorPaletteState(initialPalette);
    applyAccentTheme(getPalette(initialPalette).color, initialTheme);
    setIsInitialized(true);
  }, []);

  useEffect(() => {
    if (isInitialized) {
      localStorage.setItem("theme", theme);
      if (theme === "dark") {
        document.documentElement.classList.add("dark");
      } else {
        document.documentElement.classList.remove("dark");
      }
    }
  }, [theme, isInitialized]);

  useEffect(() => {
    if (!isInitialized) return;
    localStorage.setItem("colorPalette", colorPalette);
    applyAccentTheme(accentColor, theme);
  }, [accentColor, colorPalette, isInitialized, theme]);

  const toggleTheme = () => {
    setTheme((prevTheme) => (prevTheme === "light" ? "dark" : "light"));
  };

  const setColorPalette = (palette: ColorPaletteId) =>
    setColorPaletteState(palette);

  return (
    <ThemeContext.Provider
      value={{ theme, accentColor, colorPalette, toggleTheme, setColorPalette }}
    >
      {children}
    </ThemeContext.Provider>
  );
};

export const useTheme = () => {
  const context = useContext(ThemeContext);
  if (context === undefined) {
    throw new Error("useTheme must be used within a ThemeProvider");
  }
  return context;
};
