import { h } from 'preact';
import { useLanguage } from '../LanguageContext';

const LanguageSelector = () => {
  const { language, switchLanguage } = useLanguage();

  const handleLanguageChange = (event: Event) => {
    const target = event.target as HTMLSelectElement;
    switchLanguage(target.value);
  };

  return (
    <select value={language} onChange={handleLanguageChange}>
      <option value="en">English</option>
      <option value="pl">Polski</option>
    </select>
  );
};

export default LanguageSelector;
