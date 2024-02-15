import { createContext, h, FunctionComponent } from 'preact';
import { useState, useContext, useEffect } from 'preact/hooks';
import { useAppConfig } from './ConfigContext';

interface LanguageContextType {
  language: string;
  switchLanguage: (lang: string) => void;
}

const LanguageContext = createContext<LanguageContextType>({
  language: 'en', // A default value, will be updated dynamically
  switchLanguage: () => {}, // Placeholder function
});

export const useLanguage = () => useContext(LanguageContext);

export const LanguageProvider: FunctionComponent = ({ children }) => {
  const [language, setLanguage] = useState<string>('en');
  const appConfig = useAppConfig(); // Access the app configuration

  useEffect(() => {
    // Dynamically set the default language based on the browser's setting or fallback to English
    // Check if the appConfig and supportedLanguages are loaded
    if (appConfig && appConfig.supportedLanguages) {
      const defaultLanguage = navigator.language.split('-')[0];
      setLanguage(appConfig.supportedLanguages.includes(defaultLanguage) ? defaultLanguage : 'en');
    }
  }, [appConfig]); // Re-run this effect when appConfig changes

  const switchLanguage = (lang: string) => {
    if (appConfig && appConfig.supportedLanguages && appConfig.supportedLanguages.includes(lang)) {
      setLanguage(lang);
    } else {
      console.warn(`The language '${lang}' is not supported.`);
    }
  };

  return (
    <LanguageContext.Provider value={{ language, switchLanguage }}>
      {children}
    </LanguageContext.Provider>
  );
};

