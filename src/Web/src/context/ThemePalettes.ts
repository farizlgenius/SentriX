export const COLOR_PALETTES = [
  { id: "soft-beach", name: "Soft Beach", color: "#d9773c" },
  { id: "deep-blue-tan", name: "Deep Blue + Tan", color: "#183153" },
  { id: "modern-violet", name: "Modern Violet", color: "#6d5ef8" },
  { id: "forest-sage", name: "Forest Sage", color: "#2a6b57" },
  { id: "rose-coral", name: "Rose Coral", color: "#d94b70" },
  { id: "neon-sunset", name: "Neon Sunset", color: "#ff6b35" },
  { id: "ivory-slate", name: "Ivory Slate", color: "#4b5563" },
  { id: "electric-teal", name: "Electric Teal", color: "#0f9d8a" },
  { id: "velvet-plum", name: "Velvet Plum", color: "#6b2d6d" },
  { id: "warm-sand", name: "Warm Sand", color: "#c8894b" },
  { id: "midnight-azure", name: "Midnight Azure", color: "#1e3a8a" },
  { id: "coral-citrus", name: "Coral Citrus", color: "#f97316" }
] as const;

export type ColorPaletteId = (typeof COLOR_PALETTES)[number]["id"];
