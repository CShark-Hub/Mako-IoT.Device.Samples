import { h } from 'preact';
import { useLocation } from 'preact-iso';

export function Header() {
    const { url } = useLocation();

    return (
        <header className="mb-4">
            <nav className="navbar navbar-expand-lg navbar-light bg-light">
                <div className="container-fluid">
                    <a className="navbar-brand" href="/">Waste Bins Calendar</a>
                    <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="collapse navbar-collapse" id="navbarNav">
                        <ul className="navbar-nav">
                            <li className="nav-item">
                                <a className={`nav-link ${url === '/' ? 'active' : ''}`} href="/">Home</a>
                            </li>
                            <li className="nav-item">
                                <a className={`nav-link ${url === '/config' ? 'active' : ''}`} href="/config">Configuration</a>
                            </li>
                            <li className="nav-item">
                                <a className={`nav-link ${url === '/exit' ? 'active' : ''}`} href="/exit">Exit</a>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>
        </header>
    );
}
