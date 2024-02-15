import { createContext, h } from 'preact';
import { useContext, useState, useEffect } from 'preact/hooks';
import { useLanguage } from './LanguageContext';
import { fetchTexts } from '../utils/api';
import { useAppConfig } from './ConfigContext';

const TextsContext = createContext({});

export const TextsProvider = ({ children }) => {
    const { language } = useLanguage();
    const [texts, setTexts] = useState({});
    const config = useAppConfig();

    useEffect(() => {
        if (config) {
            fetchTexts(language, config).then(setTexts).catch(error => console.error("Failed to fetch texts:", error));
        }
    }, [language, config]); // Dependency on language ensures fetch on both initial render and language change
    

    return (
        <TextsContext.Provider value={texts}>
            {children}
        </TextsContext.Provider>
    );
};

export const useTexts = () => useContext(TextsContext);
