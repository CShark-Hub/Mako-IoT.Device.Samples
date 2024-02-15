import { useState, useEffect} from 'preact/hooks';
import { useLocation } from 'preact-iso';
import LanguageSelector from './Form/LanguageSelector';
import useLocalize from '../utils/useLocalize ';

export function Header() {
    const { url } = useLocation();
    const [isOpen, setIsOpen] = useState(false);

    const toggleNavbar = () => setIsOpen(!isOpen);
    const localize = useLocalize();

    useEffect(() => {
        setIsOpen(false);
    }, [url]);

    return (
        <header className="mb-4">
            <nav className="navbar navbar-expand-lg navbar-light bg-light">
                <div className="container-fluid">
                    <a className="navbar-brand" href="/">{localize('header.title')}</a>
                    <button className="navbar-toggler" type="button" onClick={toggleNavbar} aria-controls="navbarNav" aria-expanded={isOpen} aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className={`collapse navbar-collapse ${isOpen ? 'show' : ''}`} id="navbarNav">
                        <ul className="navbar-nav">
                            <li className="nav-item">
                                <a className={`nav-link ${url === '/configuration' ? 'active' : ''}`} href="/configuration">{localize('header.configuration')}</a>
                            </li>
                            <li className="nav-item">
                                <a className={`nav-link ${url === '/certificates' ? 'active' : ''}`} href="/certificates">{localize('header.certificates')}</a>
                            </li>
                            <li className="nav-item">
                                <a className={`nav-link ${url === '/exit' ? 'active' : ''}`} href="/exit">{localize('header.exit')}</a>
                            </li>
                        </ul>
                    </div>
                    <LanguageSelector />
                </div>
            </nav>
            <div id="alert-container"></div>
        </header>
    );
}
