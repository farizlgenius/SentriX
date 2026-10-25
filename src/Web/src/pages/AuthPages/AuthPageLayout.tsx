import React from "react";
import { Link } from "react-router";
import GridShape from "../../components/common/GridShape";
import ThemeTogglerTwo from "../../components/common/ThemeTogglerTwo";

export default function AuthLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div className="relative isolate min-h-screen overflow-hidden bg-brand-950 px-4 py-10 sm:px-6">
      <div className="pointer-events-none absolute inset-0 bg-[radial-gradient(ellipse_at_top,_var(--color-brand-800),_transparent_58%)]" />
      <div className="pointer-events-none absolute inset-0 opacity-20">
        <GridShape />
      </div>

      <main className="relative z-10 flex min-h-[calc(100vh-5rem)] flex-col items-center justify-center">
        <Link to="/" className="mb-7 block" aria-label="SentriX home">
          <img
            width={231}
            height={48}
            src="/images/logo/auth-logo.svg"
            alt="SentriX"
            className="h-10 w-auto"
          />
        </Link>
        <div className="w-full max-w-md rounded-3xl border border-white/40 bg-white p-6 shadow-2xl shadow-black/30 dark:border-white/10 dark:bg-gray-900 sm:p-10">
          {children}
        </div>
        <p className="mt-6 text-center text-xs text-white/60">
          Secure access to your SentriX workspace
        </p>
      </main>

      <div className="fixed bottom-5 right-5 z-50">
        <ThemeTogglerTwo />
      </div>
    </div>
  );
}
