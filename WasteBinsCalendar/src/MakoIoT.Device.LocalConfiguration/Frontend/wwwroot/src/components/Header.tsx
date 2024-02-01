import { useState, useEffect} from 'preact/hooks';
import { useLocation } from 'preact-iso';

export function Header() {
    const { url } = useLocation();
    const [isOpen, setIsOpen] = useState(false);

    // Toggle the navbar
    const toggleNavbar = () => setIsOpen(!isOpen);

    // Effect to close the navbar when the URL changes
    useEffect(() => {
        setIsOpen(false); // Close the navbar
    }, [url]); // Depend on the URL, so this runs whenever the URL changes

    return (
        <header className="mb-4">
            <nav className="navbar navbar-expand-lg navbar-light bg-light">
                <div className="container-fluid">
                    <a className="navbar-brand" href="/">Waste Bins Calendar</a>
                    <button className="navbar-toggler" type="button" onClick={toggleNavbar} aria-controls="navbarNav" aria-expanded={isOpen} aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className={`collapse navbar-collapse ${isOpen ? 'show' : ''}`} id="navbarNav">
                        <ul className="navbar-nav">
                            <li className="nav-item">
                                <a className={`nav-link ${url === '/configuration' ? 'active' : ''}`} href="/configuration">Configuration</a>
                            </li>
                            <li className="nav-item">
                                <a className={`nav-link ${url === '/certificates' ? 'active' : ''}`} href="/certificates">Certificates</a>
                            </li>
                            <li className="nav-item">
                                <a className={`nav-link ${url === '/exit' ? 'active' : ''}`} href="/exit">Exit</a>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>
            <div id="alert-container"></div>
        </header>
    );
}
