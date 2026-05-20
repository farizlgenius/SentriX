import { useAuth } from "./context/AuthContext";
import { useLoading } from "./context/LoadingContext";


export const AppBootstrap = ({ children }: { children: React.ReactNode }) => {
  const { isAuthReady } = useAuth();
  const { Loading } = useLoading();

  if (!isAuthReady) {
    return (
      <Loading/>
    );
  }

  return <>{children}</>;
};