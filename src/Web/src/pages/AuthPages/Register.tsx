import { Link } from "react-router";
import AuthLayout from "./AuthPageLayout";

export default function Register() {
  return (
    <AuthLayout>
      <div className="w-full">
        <p className="mb-2 text-xs font-semibold uppercase tracking-[0.18em] text-brand-600 dark:text-brand-400">
          Account access
        </p>
        <h1 className="mb-3 text-2xl font-semibold text-gray-800 dark:text-white/90 sm:text-3xl">
          Request an account
        </h1>
        <p className="mb-6 text-sm leading-6 text-gray-500 dark:text-gray-400">
          New accounts are created by your SentriX administrator. Contact your
          administrator to request access to this workspace.
        </p>
        <div className="rounded-xl border border-brand-200 bg-brand-50 p-4 text-sm leading-6 text-brand-900 dark:border-brand-900 dark:bg-brand-950/50 dark:text-brand-100">
          Self-service registration is not available for this workspace.
        </div>
        <Link
          to="/login"
          className="mt-6 inline-flex w-full items-center justify-center rounded-xl bg-brand-500 px-5 py-3 text-sm font-semibold text-white transition hover:bg-brand-600"
        >
          Back to sign in
        </Link>
      </div>
    </AuthLayout>
  );
}
