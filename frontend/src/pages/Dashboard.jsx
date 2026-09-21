import { useNavigate } from "react-router-dom";

function Dashboard() {
    const navigate = useNavigate();

    function handleLogout() {
        localStorage.removeItem("token");
        navigate("/login", { replace: true });
    }

    return (
        <div className="dashboard">

            <header className="dashboard-header">
                <div>
                    <h1>IceKit Sentinel</h1>
                    <p>Defensive Cybersecurity Toolkit</p>
                </div>

                <button
                    className="logout-button"
                    onClick={handleLogout}
                >
                    Logout
                </button>
            </header>

            <main className="dashboard-content">

                <section className="dashboard-intro">
                    <h2>Security Dashboard</h2>

                    <p>
                        Analyze, verify and monitor common
                        cybersecurity security controls.
                    </p>
                </section>

                <section className="tool-grid">

                    <div className="tool-card">
                        <div className="tool-icon">🔐</div>

                        <h3>Password Security Analyzer</h3>

                        <p>
                            Analyze the strength and security
                            characteristics of a password.
                        </p>

                        <button
                            onClick={() =>
                                navigate(
                                    "/tools/password-analyzer"
                                )
                            }
                        >
                            Open Tool
                        </button>
                    </div>

                    <div className="tool-card">
                        <div className="tool-icon">🔑</div>

                        <h3>Password Generator</h3>

                        <p>
                            Generate secure random passwords
                            using cryptographically secure
                            randomness.
                        </p>

                        <button
                            onClick={() =>
                                navigate(
                                    "/tools/password-generator"
                                )
                            }
                        >
                            Open Tool
                        </button>
                    </div>

                    <div className="tool-card">
                        <div className="tool-icon">#</div>

                        <h3>Hashing</h3>

                        <p>
                            Generate SHA-256 and SHA-512
                            cryptographic hashes.
                        </p>

                        <button
                            onClick={() =>
                                navigate("/tools/hashing")
                            }
                        >
                            Open Tool
                        </button>
                    </div>

                    <div className="tool-card">
                        <div className="tool-icon">✓</div>

                        <h3>Hash Verification</h3>

                        <p>
                            Verify whether an input matches
                            an expected cryptographic hash.
                        </p>

                        <button
                            onClick={() =>
                                navigate(
                                    "/tools/hash-verification"
                                )
                            }
                        >
                            Open Tool
                        </button>
                    </div>

                    <div className="tool-card">
                        <div className="tool-icon">🛡</div>

                        <h3>File Integrity</h3>

                        <p>
                            Register file fingerprints and
                            detect changes using hashes.
                        </p>

                        <button
                            onClick={() =>
                                navigate(
                                    "/tools/file-integrity"
                                )
                            }
                        >
                            Open Tool
                        </button>
                    </div>

                    <div className="tool-card">
                        <div className="tool-icon">🌐</div>

                        <h3>Security Headers</h3>

                        <p>
                            Analyze HTTP response headers
                            for common security controls.
                        </p>

                        <button
                            onClick={() =>
                                navigate(
                                    "/tools/security-headers"
                                )
                            }
                        >
                            Open Tool
                        </button>
                    </div>

                </section>

            </main>
        </div>
    );
}

export default Dashboard;