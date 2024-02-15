import { createContext, FunctionComponent  } from 'preact';
import { useState, useEffect, useContext } from 'preact/hooks';

interface AppConfig {
  backendUrl: string;
  supportedLanguages: string[];
}

const ConfigContext = createContext<AppConfig | null>(null);

export const useAppConfig = () => useContext(ConfigContext);

export const ConfigProvider: FunctionComponent = ({ children }) => {
  const [config, setConfig] = useState<AppConfig | null>(null);

  useEffect(() => {
    const loadConfig = async () => {
      try {
        const response = await fetch("/appconfig.json");
        if (!response.ok) {
          throw new Error(`Failed to load configuration: ${response.statusText}`);
        }
        const configData: AppConfig = await response.json();
        setConfig(configData);
      } catch (error) {
        console.error("Config load error:", error);
        // Optionally handle the error, e.g., by setting an error state
      }
    };

    loadConfig();
  }, []);


  return (
    <ConfigContext.Provider value={config}>
      {children}
    </ConfigContext.Provider>
  );
};

