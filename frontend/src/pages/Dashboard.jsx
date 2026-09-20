import { useNavigate } from "react-router-dom";
function Dashboard() {
    const navigate = useNavigate();
    return (
        <div>
            <h1>IceKit Sentinel</h1>

            <h2>Security Dashboard</h2>

            <p>
                Welcome to your security toolkit.
            </p>

            <div>
                <h3>Password Security Analyzer</h3>
                <p>
                    Analyze the strength of a password.
                </p>
                <button onClick={() => navigate('/tools/password-analyzer')}>
                    Open Tool
                </button>
            </div>

            <div>
                <h3>Password Generator</h3>
                <p>
                    Generate a secure random password.
                </p>
                <button onClick={() => navigate('/tools/password-generator')}>
                    Open Tool
                </button>
            </div>

            <div>
                <h3>Hashing</h3>
                <p>
                    Generate cryptographic hashes.
                </p>
                <button onClick={() => navigate('/tools/hashing')}>
                    Open Tool
                </button>
            </div>

            <div>
                <h3>Hash Verification</h3>
                <p>
                    Verify if a given hash matches the generated hash.
                </p>
                <button onClick={() => navigate('/tools/hash-verification')}>
                    Open Tool
                </button>
            </div>

            <div>
                <h3>File Integrity Check</h3>
                <p>
                    Verify the integrity of a file by comparing its hash.
                </p>
                <button onClick={() => navigate('/tools/file-integrity')}>
                    Open Tool
                </button>
            </div>

            <div>
                <h3>Security Headers Checker</h3>
                <p>
                    Analyze a website's HTTP response headers for commonly recommended security headers.
                </p>
                <button onClick={() => navigate('/tools/security-headers')}>
                    Open Tool
                </button>
            </div>
        </div>
    );
}

export default Dashboard;