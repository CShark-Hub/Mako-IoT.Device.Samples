import { useTexts } from '../components/TextContext';

const useLocalize = () => {
  const texts = useTexts();

  const localize = (key: string) => {
    return texts[key] || key;
  };

  return localize;
};

export default useLocalize;
