import { useAuth } from "./context/AuthContext";

export const AppBootstrap = ({ children }: { children: React.ReactNode }) => {
  const { isAuthReady } = useAuth();

  if (!isAuthReady) {
    return <div>Bootstrapping app...</div>;
  }

  return <>{children}</>;
};