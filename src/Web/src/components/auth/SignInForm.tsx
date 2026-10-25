import { PropsWithChildren, useState } from "react";
import { Link } from "react-router";
import { EyeCloseIcon, EyeIcon } from "../../icons";
import Label from "../form/Label";
import Input from "../form/input/InputField";
import Button from "../ui/button/Button";
import { LoginDto } from "../../model/Auth/LoginDto";

interface SignInFormProp {
  setDto: React.Dispatch<React.SetStateAction<LoginDto>>;
  handleClick: (e: React.MouseEvent<HTMLButtonElement>) => void;
}

export const SignInForm: React.FC<PropsWithChildren<SignInFormProp>> = ({ setDto, handleClick }) => {
  const [showPassword, setShowPassword] = useState(false);
  return (
    <div className="flex flex-col flex-1">
      <div className="flex w-full flex-col justify-center">
        <div>
          <div className="mb-7">
            <p className="mb-2 text-xs font-semibold uppercase tracking-[0.18em] text-brand-600 dark:text-brand-400">
              Welcome back
            </p>
            <h1 className="mb-2 text-2xl font-semibold text-gray-800 dark:text-white/90 sm:text-3xl">
              Sign in to SentriX
            </h1>
            <p className="text-sm leading-6 text-gray-500 dark:text-gray-400">
              Enter your credentials to access your workspace.
            </p>
          </div>
          <div>

            <div>
              <div className="space-y-5">
                <div>
                  <Label>
                    Username <span className="text-error-500">*</span>
                  </Label>
                  <Input placeholder="Enter your username" onChange={(e) => setDto(prev => ({ ...prev, username: e.target.value }))} />
                </div>
                <div>
                  <Label>
                    Password <span className="text-error-500">*</span>
                  </Label>
                  <div className="relative">
                    <Input
                      type={showPassword ? "text" : "password"}
                      placeholder="Enter your password"
                      onChange={(e) => setDto(prev => ({ ...prev, password: e.target.value }))}
                    />
                    <button
                      type="button"
                      onClick={() => setShowPassword(!showPassword)}
                      aria-label={showPassword ? "Hide password" : "Show password"}
                      className="absolute right-4 top-1/2 z-30 -translate-y-1/2 cursor-pointer"
                    >
                      {showPassword ? (
                        <EyeIcon className="fill-gray-500 dark:fill-gray-400 size-5" />
                      ) : (
                        <EyeCloseIcon className="fill-gray-500 dark:fill-gray-400 size-5" />
                      )}
                    </button>
                  </div>
                </div>
                <div className="flex items-center justify-between">
                  <Link
                    to="/reset-password"
                    className="text-sm text-brand-500 hover:text-brand-600 dark:text-brand-400"
                  >
                    Forgot password?
                  </Link>
                </div>
                <div>
                  <Button name="login" onClickWithEvent={handleClick} className="w-full rounded-xl" size="md">
                    Sign in
                  </Button>
                </div>
                <p className="pt-1 text-center text-sm text-gray-500 dark:text-gray-400">
                  Need an account?{" "}
                  <Link
                    to="/register"
                    className="font-semibold text-brand-600 hover:text-brand-700 dark:text-brand-400 dark:hover:text-brand-300"
                  >
                    Register
                  </Link>
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
  