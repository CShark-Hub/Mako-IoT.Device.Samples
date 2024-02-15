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
        fetchTexts(language, config).then(setTexts);
    }, [language]);

    return (
        <TextsContext.Provider value={texts}>
            {children}
        </TextsContext.Provider>
    );
};

export const useTexts = () => useContext(TextsContext);
